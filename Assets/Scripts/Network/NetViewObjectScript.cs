using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetViewObjectScript : MonoBehaviour {

    public Vector3 acceRotationVector;
    public Quaternion reelRotationVector;
	public float speed = 0.1f;
	NetworkView nView;
    private bool buttonDown;

    private bool caughtFish;

    void Start () {
        nView = GetComponent<NetworkView>();
        if (!nView.isMine)
        {
            //enabled = false;
        }
	}

    GUIStyle guiStyle;
    private float rotAngle = 90;
    private Vector2 pivotPoint;
    public void OnGUI()
    {
       if (Network.isClient)
         {
             if (GUI.RepeatButton(new Rect(Screen.width / 3 - 100, Screen.height / 3, 100, 50), "Cast"))
             {
                 Debug.Log("Sending RPC castGauge");
                 nView.RPC("castGauge", RPCMode.All);
                 buttonDown = true;
             }
             else if (buttonDown && Event.current.type == EventType.repaint)
             {
                 Debug.Log("Sending RPC CAST");
                 buttonDown = false;
                 nView.RPC("cast", RPCMode.All);
             }

             if (caughtFish)
             {
                 if (GUI.RepeatButton(new Rect(Screen.width / 3 - 100, Screen.height / 3 + 100, 150, 100), "Keep"))
                 {
                     nView.RPC("keepFish", RPCMode.All);
                     caughtFish = false;
                 }

                 if (GUI.RepeatButton(new Rect(Screen.width / 3 + 120, Screen.height / 3 + 100, 150, 100), "Let go"))
                 {
                     nView.RPC("releaseFish", RPCMode.All);
                     caughtFish = false;
                 }
             }

             if (showRestartButton)
             {
                 if (GUI.RepeatButton(new Rect(Screen.width / 3 + 120, Screen.height / 3 + 100, 150, 100), "Restart Game"))
                 {
                     //nView.RPC("releaseFish", RPCMode.All);
                     Network.Disconnect();
                     Application.LoadLevel(Application.loadedLevel);
                 }
             }
      
        return;
        }
    }


    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 acceRot = acceRotationVector;
            stream.Serialize(ref acceRot);
            Quaternion reelRot = reelRotationVector;
            stream.Serialize(ref reelRot);
        }

        else
        {
            // Retrieving data from server
        }
    }

    [RPC]
    public void vibrate() {
        Debug.Log("Vibrate");
        Handheld.Vibrate();
    }

    [RPC]
    public void castGauge()
    {
        Debug.Log("Gauge!");
    }

	[RPC]
	public void cast() {
		Debug.Log("Cast!");
	}

	[RPC]
	public void reel() {
		Debug.Log("Reel!");
	}

    [RPC]
    public void reeling(float angle)
    {
        Debug.Log("Reeling?");
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    [RPC]
    public void keepFish()
    {
        Debug.Log("Adding fish to List!");
    }

    [RPC]
    public void releaseFish()
    {
        Debug.Log("Releasomg fish back in the ocean!");
    }

    [RPC]
    public void enableCaughtUI()
    {
        Debug.Log("Enabling keep or release ui elements");
        caughtFish = true;
    }

    private bool showRestartButton;
    [RPC]
    public void enableRestartButton()
    {
        showRestartButton = true;
    }


	void Update () 
    {
        reelRotationVector = transform.rotation;
        acceRotationVector = new Vector3(Input.acceleration.z, Input.acceleration.y, -Input.acceleration.x);
    }
}
