using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPlayer : MonoBehaviour {
    public bool AllowRespawn { get; set; }
    private float _respawnTimer = 0;
	// Update is called once per frame
	void Update () {
        if(AllowRespawn)
            AutoRespawn();

        ManualRespawn();
	}

    private void ManualRespawn()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadScene();
        }
    }

    private void AutoRespawn()
    {
        _respawnTimer += Time.deltaTime;

        if (_respawnTimer >= 5)
        {
            LoadScene();
            Reset();
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    private void Reset()
    {
        _respawnTimer = 0;
        AllowRespawn = false;
    }
}
