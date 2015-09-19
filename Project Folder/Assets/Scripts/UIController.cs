using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIController : MonoBehaviour {


    public static UIController UICont;
    private GameControl control;
    public GameObject camera;

    public bool inTransition;

    public GameObject scene1;
    public GameObject scene2;
    public GameObject scene3;
    public GameObject scene4;
    public GameObject scene5;
    public GameObject caughtFishScene;

    public Text fishName;
    public Text fishWeight;
    public Image fishIcon;

    public GameObject castButton;

    private bool removeUI;

    private NetworkView nView;

    void Awake()
    {
        UICont = this;
    }


    void Start () {
        control = GameControl.control;
        scene1.transform.position = scene1.GetComponent<UI_Scene>().startPos;

        inTransition = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Network.isClient && !removeUI) { 
            control.currentState = GameControl.State.game;
        removeUI = true;
            startGame();
        showGameScreen();
        }
        if ( !removeUI && control.currentState == GameControl.State.game)
        {
            
        }
     /*  if (! showCastBtn && control.currentState == GameControl.State.game)
        {
            showCastBtn = true;
            iTween.MoveTo(castButton, iTween.Hash("position", castButtonPosOff, "time", 0.5f, "easeType", iTween.EaseType.easeOutSine));
        }*/
    }

    private bool buttonDown;
    public void CastGauge()
    {
        if (!inTransition) { 
            if (nView == null)
        {
            nView = GameObject.Find("NetworkView_TestObject(Clone)").GetComponent<NetworkView>();
        }

            Debug.Log("Sending RPC castGauge");
            nView.RPC("castGauge", RPCMode.All);
            buttonDown = true;
        }
    }

    private bool showCastBtn;
    Vector3 castButtonPosOn;
    Vector3 castButtonPosOff;
    public void Cast()
    {
        if (!inTransition) { 
        if (nView == null)
        {
            nView = GameObject.Find("NetworkView_TestObject(Clone)").GetComponent<NetworkView>();
        }

        Debug.Log("Sending RPC CAST");
        buttonDown = false;
        nView.RPC("cast", RPCMode.All);

        castButtonPosOn = castButton.transform.position;
        castButtonPosOff = new Vector3(castButtonPosOn.x, castButtonPosOn.y + 1000, castButtonPosOn.z);
        iTween.MoveTo(castButton, iTween.Hash("position", castButtonPosOff, "time", 0.5f, "easeType", iTween.EaseType.easeInSine));
        showCastBtn = false;
        }
    }

    public void showCastBtnF()
    {
            showCastBtn = true;
            iTween.MoveTo(castButton, iTween.Hash("position", castButtonPosOn, "time", 0.5f, "easeType", iTween.EaseType.easeOutSine));
    }

    public void showStartScreen()
    {
        control.currentState = GameControl.State.connect;
        scene1.GetComponent<UI_Scene>().Show();
        scene2.GetComponent<UI_Scene>().Hide();
        scene3.GetComponent<UI_Scene>().Hide();
    }

    public void showGameScreen()
    {
        scene1.GetComponent<UI_Scene>().Hide();
        scene2.GetComponent<UI_Scene>().Show();
        scene3.GetComponent<UI_Scene>().Hide();
    }

    public void showFishLog()
    {
        control.currentState = GameControl.State.fishlog;
        scene1.GetComponent<UI_Scene>().Hide();
        scene2.GetComponent<UI_Scene>().Hide();
        scene3.GetComponent<UI_Scene>().Show();
    }

    private bool showingScoreScreen;
    public void showScoreScreen()
    {
        if (!showingScoreScreen) {
            showingScoreScreen = true;
           // control.currentState = GameControl.State.fishlog;
            scene1.GetComponent<UI_Scene>().Hide();
            scene2.GetComponent<UI_Scene>().Hide();
            scene3.GetComponent<UI_Scene>().Hide();
            scene4.GetComponent<UI_Scene>().Show();

        }
    }

    private bool showingRestartScreen;
    public void showRestartScreen()
    {
        if (!inTransition)
        {
            if (!showingScoreScreen)
            {

                //enableHighScore

                showingRestartScreen = true;
                //  control.currentState = GameControl.State.fishlog;
                scene1.GetComponent<UI_Scene>().Hide();
                scene2.GetComponent<UI_Scene>().Hide();
                scene3.GetComponent<UI_Scene>().Hide();
                scene4.GetComponent<UI_Scene>().Hide();
                scene5.GetComponent<UI_Scene>().Show();
                if (nView == null)
                {
                    nView = GameObject.Find("NetworkView_TestObject(Clone)").GetComponent<NetworkView>();
                }

                Debug.Log("going to high score");
                nView.RPC("enableHighScore", RPCMode.All);
            }
        }
    }

    /*void OnGUI()
    {
        if (GUI.Button(new Rect(30, 30, 200, 30), "1"))
        {
            scene1.GetComponent<UI_Scene>().Show();
            scene2.GetComponent<UI_Scene>().Hide();
            scene3.GetComponent<UI_Scene>().Hide();
        }

        if (GUI.Button(new Rect(30, 60, 200, 30), "2"))
        {
            scene1.GetComponent<UI_Scene>().Hide();
            scene2.GetComponent<UI_Scene>().Show();
            scene3.GetComponent<UI_Scene>().Hide();
        }

        if (GUI.Button(new Rect(30, 90, 200, 30), "3"))
        {
            scene1.GetComponent<UI_Scene>().Hide();
            scene2.GetComponent<UI_Scene>().Hide();
            scene3.GetComponent<UI_Scene>().Show();
        }
    }*/
    public void setFishName(string name, float weight)
    {
        fishName.text = name;
        fishWeight.text = "" + weight;

        string fishIconPath = "Prefabs/Fish/Icons/" + name;
        //string stringToLoad = fishIconPath.Substring(0, fishIconPath.Length - 1);

        Texture2D tex = Resources.Load<Texture2D>(fishIconPath);

        if (tex != null)
            fishIcon.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        else
            Debug.Log("Error loading fish icon: " + name);

        caughtFishScene.GetComponent<CaughtFishUI>().Show();
    }

    public void startGame()
    {
        Vector3 startPos = new Vector3(0, 0, 0);
        iTween.RotateTo(camera, iTween.Hash("rotation", startPos, "time", 1.5f, "easeType", iTween.EaseType.easeOutSine));
    }

    private bool gameIsOver;
    public void gameOver()
    { 
        if (!gameIsOver) {
            gameIsOver = true;
            scene1.GetComponent<UI_Scene>().Hide();
            scene2.GetComponent<UI_Scene>().Hide();
            scene3.GetComponent<UI_Scene>().Hide();
            scene4.GetComponent<UI_Scene>().Show();
            Vector3 startPos = new Vector3(-80,0,0);
            iTween.RotateTo(camera,iTween.Hash("rotation", startPos, "time", 1.5f, "easeType", iTween.EaseType.easeOutSine));
            // iTween.MoveTo(camera, iTween.Hash("rotation", startPos, "time", 1.5f, "easeType", iTween.EaseType.easeOutSine));
        }
    }

    public void giveUp()
    {
        if (!inTransition)
        {
            if (nView == null)
            {
                nView = GameObject.Find("NetworkView_TestObject(Clone)").GetComponent<NetworkView>();
            }
            nView.RPC("giveUp", RPCMode.All);
            gameOver();
            Debug.Log("giving up");
        }
    }

    public void restartGame()
    {
        Network.Disconnect();
        Application.LoadLevel(Application.loadedLevel);
    }


}
