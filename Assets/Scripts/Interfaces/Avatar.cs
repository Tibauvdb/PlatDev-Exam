using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviour,LimitVelocity<Vector3,float>{

    public Vector3 LimitXZVel(Vector3 velocity,float maxVelocity)
    {
        Vector3 yVel = Vector3.Scale(velocity, Vector3.up);
        Vector3 xzVel = Vector3.Scale(velocity, new Vector3(1, 0, 1));

        xzVel = Vector3.ClampMagnitude(xzVel, maxVelocity);

        velocity = xzVel + yVel;
        return velocity;
    }
}
