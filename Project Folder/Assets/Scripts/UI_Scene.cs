using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Scene : MonoBehaviour {

 

    public GameObject container;
    public Vector3 startPos; 
    private Vector3 offscreenPos;
    private bool showingUI;
    private Rect imgRect;
    public string sceneName;

    public int offset;

    void Start()
    {
        startPos = container.transform.position;
        offscreenPos = new Vector3(container.transform.position.x + offset, container.transform.position.y, container.transform.position.z);
        container.transform.position = offscreenPos;
    }


    void Update()
    {

    }


    public void Show()
    {
        container.transform.position = new Vector3(startPos.x - offset, startPos.y, startPos.z);
        showingUI = true;
        iTween.MoveTo(container, iTween.Hash("position", startPos, "time", 0.5f, "easeType", iTween.EaseType.easeOutSine));
    }

    public void Hide()
    {
        showingUI = false;
        iTween.MoveTo(container, iTween.Hash("position", offscreenPos, "time", 0.5f, "easeType", iTween.EaseType.easeInSine));
    }




}
