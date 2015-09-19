using UnityEngine;
using System.Collections;

public class RetrieveFromPC : MonoBehaviour {

   // private NetworkView nView;
    private FishManager fishM;

	// Use this for initialization
	void Start () {
     //   nView = GetComponent<NetworkView>();
        fishM = FishManager.fishmanager;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [RPC]
    private void SendList(string f)
    {
        Debug.Log("string: " + f);
        string[] lines = f.Split("\n"[0]);

        for (int i = 0; i < lines.Length -1; i++)
        {
            string[] row = lines[i].Split(","[0]);
            Debug.Log("row: " + row);
            string name = row[0];
            float weight = float.Parse(row[1]);

            Debug.Log("name: " + name + " Weight: " + weight);
            fishM.caughtFish.Add(new Fish(name, weight));
        }
        fishM.AddToFishLog();
    }
}
