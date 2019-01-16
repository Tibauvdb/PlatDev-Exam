using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AllowPickup))]
public class TrophyBehaviour : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            Destroy(this.gameObject);
    }
}
