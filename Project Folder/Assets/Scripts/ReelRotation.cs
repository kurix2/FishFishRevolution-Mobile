using UnityEngine;
using System.Collections;

public class ReelRotation : MonoBehaviour {

    bool mouseDown;

    Vector3 v3Pos;
    float fAngle;
    public float offSet;

    public float oldRot;
    public float newRot;
    public float speed;

    public NetworkView nView;

    void Update()
    {
        if (mouseDown)
        {

            v3Pos = Input.mousePosition;
            v3Pos.z = (transform.position.z - Camera.main.transform.position.z);
            v3Pos = Camera.main.ScreenToWorldPoint(v3Pos);
            v3Pos = v3Pos - transform.position;
            fAngle = Mathf.Atan2(v3Pos.y, v3Pos.x) * Mathf.Rad2Deg;
            if (fAngle < 0.0f) fAngle += 360.0f;

            float currentAngle = transform.rotation.eulerAngles.z;
            oldRot = currentAngle;
            if (fAngle + offSet < transform.rotation.eulerAngles.z)
                currentAngle -= 360;
            if (fAngle >= currentAngle)
            {
                transform.rotation = Quaternion.AngleAxis(fAngle, Vector3.forward);
                newRot = transform.rotation.eulerAngles.z;
                speed = newRot - oldRot;
                nView.RPC("reeling", RPCMode.All, fAngle);
               // speed = 0;
            }
        }

    }

    [RPC]
    public void reeling(float angle)
    {
        Debug.Log("Reeling");
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnMouseDown()
    {
        mouseDown = true;
    }

    void OnMouseUp()
    {
        mouseDown = false;
    }

 
}
