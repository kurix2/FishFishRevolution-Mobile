using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	//Not being used for now
	public Button connectButton;
	public InputField ipInput;
    public InputField portInput;
    public Toggle wifiToggle;
    
	
	// Default Wifi connection
	public string ipString = "127.0.0.1";
	public string portString = "8888";

    private GameControl gameControl;
	
	// Select buttons
	public int selGridInt = 0;
	public string[] selStrings = new string[] { "Master", "Wifi", "Bluetooth" };

    private bool showFishLog;

	public NetworkView nView;

    private bool buttonDown;

	void Start()
	{
        gameControl = GameControl.control;
        string  prefIP = PlayerPrefs.GetString("savedIP");
        if (prefIP.Length == 0)
            ipInput.text = "127.0.0.1";
        else
            ipInput.text = prefIP;

        int prefPort = PlayerPrefs.GetInt("savedPort");
        Debug.Log(prefPort);
        if (prefPort == 0)
            portInput.text = "8888";
        else
            portInput.text = "" + prefPort;



        Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}
	
	
	// For testing only, MasterServer is not reliable
	HostData[] hostData;
	public IEnumerator RefresHostList()
	{
		Debug.Log("Fetching serverlist");
		MasterServer.RequestHostList("JustAnotherFishingGame5468");
		
		float timeEnd = Time.time + 4.0f;
		
		while (Time.time < timeEnd)
		{
			hostData = MasterServer.PollHostList();
			yield return new WaitForEndOfFrame();
		}
		
		if (hostData == null || hostData.Length == 0)
			Debug.Log("No servers");
		else
			Debug.Log(hostData.Length + " has been found");
	}
	
	
	GUIStyle guiStyle;
	private float rotAngle = 90;
	private Vector2 pivotPoint;
	public void OnGUI()
	{

        // Debugging bool, removes UI completely
        bool noui = true;
        if (noui)
            return;

		if (Network.isClient) {
            //if (GUI.RepeatButton(new Rect(Screen.width / 3 - 100, Screen.height / 3, 100, 50), "Cast"))
            //{
            //    Debug.Log("Sending RPC castGauge");
            //    nView.RPC("cast", RPCMode.All);
            //    buttonDown = true;


            //}
            //else if (buttonDown && Event.current.type == EventType.repaint)
            //{
            //    buttonDown = false;
            //    nView.RPC("cast", RPCMode.All);
            //}

  
			return;
		}
		
		guiStyle = new GUIStyle(GUI.skin.button); 
		guiStyle.fontSize = 26;
		guiStyle.padding.bottom = 5;
		//  guiStyle.fixedHeight = 200;
		// guiStyle.fixedWidth = 200;
		
		pivotPoint = new Vector2(Screen.width / 2 + 100, Screen.height / 2);
		GUIUtility.RotateAroundPivot(rotAngle, pivotPoint);
		
		
		if (!showFishLog) { 
		    GUILayout.BeginVertical("Box");
		    selGridInt = GUI.SelectionGrid(new Rect(Screen.width / 2, Screen.height / 2 - 200, 200, 300), selGridInt, selStrings, 1,guiStyle);
		    //selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 1, guiStyle);
		
		    if (selStrings[selGridInt] == "Master")
			    if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 105, 200, 100), "Refresh List",guiStyle))
		    {
			    StartCoroutine("RefresHostList");
		    }
		
		    if (selStrings[selGridInt] == "Wifi")
		    {
			    hostData = null;
			    ipString = GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2 +105, 200, 45), ipString,guiStyle);
			    portString = GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2 + 155, 200, 45), portString, guiStyle);
			    if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 210, 200f, 100f), "Connect",guiStyle))
			    {
				    Debug.Log("Trying to connect to server");
				    string ip = ipString;
				    int port = int.Parse(portString);
				    Network.Connect(ip, port);
			    }
		    }
		
		    if (selStrings[selGridInt] == "Bluetooth")
		    {
			    // Todo: Add bluetooth
		    }

            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 350, 200, 100), "Fish Log", guiStyle))
            {
                GameControl.control.currentState = GameControl.State.fishlog;
                showFishLog = true;
            }
        }

        if (showFishLog)
        {
            if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 350, 200, 100), "< Back", guiStyle))
            {
                showFishLog = false;
                GameControl.control.currentState = GameControl.State.connect;
            }

            // Clears the fish log, for debugging
            if (GUI.Button(new Rect(Screen.width / 2 +  220, Screen.height / 2 + 350, 120, 100), "X Clear", guiStyle))
            {
                FishManager.fishmanager.fishLog.Clear();
            }

        }

        if (hostData != null)
		{
			for (int i = 0; i < hostData.Length; i++)
			{   //                  Screen.height / 2 + 215, 200f, 100f
				if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 210 + (105 * i), 200, 100), hostData[i].gameName,guiStyle))
				{
					Debug.Log("Trying to connect to server");
					Network.Connect(hostData[i]);                 
				}
			}
		}
	}
	
	public void connectToServer()
    {
        Debug.Log("Trying to connect to server");
        string ip = ipInput.text;
        Debug.Log(ipInput.text);
        int port = int.Parse(portInput.text); //int.Parse(portString);


        PlayerPrefs.SetString("savedIP",ip);
        PlayerPrefs.SetInt("savedPort", port);

        Network.Connect(ip, port);

    }
	
	private void SpawnPlayer()
	{
		Debug.Log("Spawning Reel");
		Network.Instantiate(Resources.Load("Prefabs/NetworkView_TestObject"), new Vector3(0f, 1.2f, 0f), Quaternion.identity, 0);
	}
	
	
	void OnConnectedToServer() 
	{
		Debug.Log("Connected to server");
		SpawnPlayer();
	}
	
	void OnDisconnectedFromServer()
	{

		Network.RemoveRPCs(Network.player);
		Network.DestroyPlayerObjects(Network.player);
        Application.LoadLevel(Application.loadedLevel);

    }
	
	
	
	void OnApplicationQuit() 
	{
		Network.Disconnect(500); 
	}
	
	
	void Update () 
	{
		
	}
}