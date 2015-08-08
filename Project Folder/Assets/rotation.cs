using UnityEngine;
using System.Collections;

public class rotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    float speed = 100;
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}
