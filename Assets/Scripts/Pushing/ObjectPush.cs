using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPush : MonoBehaviour {
    public float PushForce; //Base 10.5 F

    private void Update()
    {
        Push();
    }
    public void Push()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(0, 0, PushForce);
    }
}
