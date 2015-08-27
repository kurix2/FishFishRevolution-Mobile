using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FishManager : MonoBehaviour
{

    public static FishManager fishmanager;

    public TextAsset FishToAdd;

    // List of all Fish in the game
    public List<Fish> fishList = new List<Fish>();

    // List of Fish caught this round
    public List<Fish> caughtFish = new List<Fish>();

    // List of all Fish ever caught
    public List<Fish> fishLog = new List<Fish>();

    public GUISkin customSkin;

    // Sprite for caught fish counter
    public Texture2D fishCountSprite;
    public Texture2D splashSprite;

    //private float fishOffsetX = 175f;
    //private float fishOffsetY = 135f;
     
    Fish hookedFish;

    void Awake()
    {
        if (fishmanager == null)
            fishmanager = this;
    }

    void Start()
    {
        ReadFishList();
        GameControl.control.Load();
    }

   
    public void OnGUI()
    {
        GUI.skin = customSkin;

        float rotAngle = 90;
        Vector2 pivotPoint = new Vector2(Screen.width / 2, Screen.height / 2);
        GUIUtility.RotateAroundPivot(rotAngle, pivotPoint);
        
        if (GameControl.control.currentState == GameControl.State.fishlog)
        {
            GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height - 700, 250, 45), "Welcome to the Fish Log");

            for (int i = 0; i < fishList.Count; i++)
            {
                if (fishLog.Any(obj => obj.getName() == fishList[i].getName()))
                {
                    GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height - 600 + (i * 170), 250, 45), fishList[i].getName());
                    GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height - 510 + (i * 170), 250, 45), "Weight: " + GetHeaviestOfType(fishList[i].getName()));
                    GUI.Label(new Rect(Screen.width / 2 - 250 , Screen.height - 570 + (i * 170), splashSprite.width, splashSprite.height), splashSprite);
                    // if (GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 210, 200f, 100f), "Connect"))

                }

                else
                { 
                    GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height - 600 + (i * 170), 250, 45), "???");
                    // Add Greyed Icons here
                }      
                               
            }
        }
    }

    // Puts the fish listed in the CSV into a List
    private void ReadFishList()
    {
        string[] lines = FishToAdd.text.Split("\n"[0]);

        // Loop starts at 1, 0 is only for headers
        for (int i = 1; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(","[0]);

            int id = int.Parse(row[0]);
            string name = row[1];
            string prefab = row[2];
            int rarity = int.Parse(row[3]);
            float minWeight = float.Parse(row[4]);
            float maxWeight = float.Parse(row[5]);
            int movePattern = int.Parse(row[6]);
            float speed = float.Parse(row[7]);
            float endurance = float.Parse(row[8]);
            string icon = row[9];

            fishList.Add(new Fish(id, name, prefab, rarity, minWeight, maxWeight, movePattern, speed, endurance, icon));

            // Debugging stuff, delete at release
            // Fish aFish = new Fish(id, name, prefab, rarity, minWeight, maxWeight, movePattern, speed, endurance, icon);
            // aFish.randomize();
            // fishLog.Add(aFish);

        }
    }

    // 


    // Adds all newly caught fish into the fish log
    public void AddToFishLog()
    {
        foreach (Fish fish in caughtFish)
        {
            fishLog.Add(fish);
            Debug.Log("Adding: " + fish);
        }

        GameControl.control.Save();
        caughtFish.Clear();

    }

    // Returns the heaviest fish caught depending on name
    private float GetHeaviestOfType(string name)
    {
        float heaviest = 0f;

        foreach (Fish fish in fishLog)
        {
            if (fish.getName() == name && fish.getWeight() > heaviest)
            {
                heaviest = fish.getWeight();
            }
        }

        return heaviest;

    }

}
