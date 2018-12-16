using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AllowPickup : MonoBehaviour {

    private InputManager _input;

    private void Start()
    {
        _input = GameObject.Find("GameManager").GetComponent<InputManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerBehaviour.States state = other.gameObject.GetComponent<PlayerBehaviour>().State;
            if (Input.GetButtonDown(_input.E) && other.gameObject.GetComponent<PlayerBehaviour>().State != PlayerBehaviour.States.WalkingWithPickup)
            {
                //Debug.Log("Picked Up Object");
                other.gameObject.GetComponent<PlayerBehaviour>().PickUpObject(this.gameObject);
            }
            //If player presses trigger and is walkingwithPickup -> activate throw script and deactivate this one
            if(state == PlayerBehaviour.States.WalkingWithPickup)
            {
                this.gameObject.transform.parent = other.transform;
                this.gameObject.GetComponent<ObjectThrow>().enabled = true;
                this.gameObject.GetComponent<AllowPickup>().enabled = false;
            }
        }
    }
}
