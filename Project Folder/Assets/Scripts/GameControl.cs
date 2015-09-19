using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// Handles Saving and Loading
// and States

public class GameControl : MonoBehaviour {

    public static GameControl control;

    public enum State { connect, fishlog, game }
    public State currentState;

	void Awake () {
        if (control == null)
            control = this;
	}

    void Start()
    {
        currentState = State.connect;
    }

    public void Save()
    {
        List<Fish> fishLog = FishManager.fishmanager.fishLog;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/fishLog.dat");

        bf.Serialize(file, fishLog);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/fishLog.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/fishLog.dat", FileMode.Open);
            FishManager.fishmanager.fishLog = (List<Fish>)bf.Deserialize(file);
            file.Close();
        }
    }
}
