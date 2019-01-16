using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AllowPickup : MonoBehaviour {
    public bool PlayerInRange { get; set; }


    private InputManager _input;


    private void Start()
    {
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CheckIfPlayerCanPickUp(other.gameObject);
            PlayerInRange = true;
        }
        else
            PlayerInRange = false;
    }

    private void CheckIfPlayerCanPickUp(GameObject player)
    {
        PlayerBehaviour.States state = player.GetComponent<PlayerBehaviour>().State;

        //Allow player to pick up object
        if (Input.GetButtonDown(_input.A) && state != PlayerBehaviour.States.WalkingWithPickup)
        {
            player.GetComponent<PlayerBehaviour>().PickUpObject(this.gameObject);
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

}
