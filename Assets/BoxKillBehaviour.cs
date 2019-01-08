using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxKillBehaviour : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AI")
        {
            //if the an AI stept into the box' trigger && a player is pushing it

        }
    }
}
