﻿using System.Collections;
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
        if(other.gameObject.tag == "Player")
        {
            if (Input.GetButtonDown(_input.E))
            {
                Debug.Log("Picked Up Object");
                other.gameObject.GetComponent<PlayerBehaviour>().PickUpObject(this.gameObject);
            }
        }
    }
}
