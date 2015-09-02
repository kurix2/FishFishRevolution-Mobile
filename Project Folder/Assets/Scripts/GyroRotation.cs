using UnityEngine;
using System.Collections;

public class GyroRotation : MonoBehaviour {

  
   // public GameObject theRod;
   // public GameObject testRod;
    // Use this for initialization
    void Start()
    {
        Input.gyro.enabled = true;
        //Debug.Log(Input.gyro.enabled);
        Input.gyro.updateInterval = 0.01f;

        parentOffset = new Vector3(90, 0, 90);
        Quaternion currentGyro = Input.gyro.attitude;
        thisOffset = new Vector3(0, 90, 0);

    }

    // Update is called once per frame
    public Vector3 parentOffset;
    public Vector3 thisOffset;

    void Update()
    {

        transform.parent.transform.eulerAngles = parentOffset;
        Quaternion currentGyro = Input.gyro.attitude;
        transform.localRotation = currentGyro * Quaternion.Euler(thisOffset);

       // Quaternion sendRotation = theRod.transform.rotation;
        //testRod.transform.rotation = sendRotation;
    }
}
