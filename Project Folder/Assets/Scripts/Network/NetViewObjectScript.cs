using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetViewObjectScript : MonoBehaviour {

    public GameObject rodRotationObj;

    public Vector3 acceRotationVector;
    public Quaternion reelRotationVector;
	public float speed = 0.1f;
	NetworkView nView;
    private bool buttonDown;

    public Button castBtn;

    private bool caughtFish;

    void Start () {
        nView = GetComponent<NetworkView>();
        if (!nView.isMine)
        {
            //enabled = false;
        }
	}

    GUIStyle guiStyle;

    private Vector2 pivotPoint;
    public void OnGUI()
    {
       if (Network.isClient)
         {/*
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
             }*/

             if (caughtFish)
             {
                /* if (GUI.RepeatButton(new Rect(Screen.width / 3 - 100, Screen.height / 3 + 100, 150, 100), "Keep"))
                 {
                     nView.RPC("keepFish", RPCMode.All);
                     caughtFish = false;
                 }

                 if (GUI.RepeatButton(new Rect(Screen.width / 3 + 120, Screen.height / 3 + 100, 150, 100), "Let go"))
                 {
                     nView.RPC("releaseFish", RPCMode.All);
                     caughtFish = false;
                 }*/
            }
             /*
            if (showRestartButton)
             {
                 if (GUI.RepeatButton(new Rect(Screen.width / 3 + 120, Screen.height / 3 + 100, 150, 100), "Restart Game"))
                 {
                     //nView.RPC("releaseFish", RPCMode.All);
                     Network.Disconnect();
                     Application.LoadLevel(Application.loadedLevel);
                 }
             }*/
      
        return;
        }
    }


    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
             Vector3 acceRot = acceRotationVector;
            stream.Serialize(ref acceRot);
          //  Quaternion acceRot = rodRotationObj.transform.rotation;
          //  stream.Serialize(ref acceRot);
            Quaternion reelRot = reelRotationVector;
            stream.Serialize(ref reelRot);
        }

        else
        {
            // Retrieving data from server
        }
    }

    [RPC]
    public void showCastButton()
    {
        Debug.Log("Showing Cast Button");
        UIController.UICont.showCastBtnF();
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
    public void enableCaughtUI(string name, float weight)
    {
        Debug.Log("Enabling keep or release ui elements");
        UIController.UICont.setFishName(name, weight);
        caughtFish = true;
    }

    [RPC]
    public void GetGameState()
    {
        Debug.Log("Requesting Current Game State");
    }

    [RPC]
    public void RetrieveGameState(string s)
    {
        Debug.Log("Getting Current Game State");
        Debug.Log("State: "+ s);
    }

    [RPC]
    public void giveUp()
    {
        // gives up, game ends
    }

    [RPC]
    public void showEndScreen()
    {
        UIController.UICont.gameOver();
        // shows endscreen on phone
    }

    private bool showRestartButton;
    [RPC]
    public void enableRestartButton()
    {
        showRestartButton = true;
    }

    [RPC]
    public void enableHighScore()
    {
        // enable the highscore button on phone
    }


    [RPC]
    public void inTransition()
    {
        // tells phone theres a animation transit going
        UIController.UICont.inTransition = true;
        Debug.Log("in transition");
    }

    [RPC]
    public void transitionOver()
    {
        // tells phone theres a animation transit is over
        UIController.UICont.inTransition = false;
        Debug.Log("transition over");
    }

    void Update () 
    {
        reelRotationVector = transform.rotation;
        acceRotationVector = new Vector3(Input.acceleration.z, Input.acceleration.y, -Input.acceleration.x);
    }
}
