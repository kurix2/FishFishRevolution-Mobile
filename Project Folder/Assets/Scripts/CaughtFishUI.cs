using UnityEngine;
using System.Collections;

public class CaughtFishUI : MonoBehaviour {

    private NetworkView nView;

    public GameObject container;
    public Vector3 startPos;
    private Vector3 offscreenPos;
    private bool showingUI;

    // Use this for initialization
    void Start () {
        startPos = container.transform.position;
        offscreenPos = new Vector3(container.transform.position.x, container.transform.position.y + 1000, container.transform.position.z);
        container.transform.position = offscreenPos;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show()
    {
        showingUI = true;
        iTween.MoveTo(container, iTween.Hash("position", startPos, "time", 0.5f, "easeType", iTween.EaseType.easeOutSine));
    }

    public void Hide()
    {
        showingUI = false;
        iTween.MoveTo(container, iTween.Hash("position", offscreenPos, "time", 0.5f, "easeType", iTween.EaseType.easeInSine));
    }

    public void keepFish()
    {
        if (nView == null)
        {
            nView = GameObject.Find("NetworkView_TestObject(Clone)").GetComponent<NetworkView>();
        }

        nView.RPC("keepFish", RPCMode.All);
        Hide();

    }

    public void releaseFish()
    {
        if (nView == null)
        {
            nView = GameObject.Find("NetworkView_TestObject(Clone)").GetComponent<NetworkView>();
        }

        nView.RPC("releaseFish", RPCMode.All);
        Hide();

    }



}

