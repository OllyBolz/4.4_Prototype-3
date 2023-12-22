using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab0;
    public GameObject obstaclePrefab1;
    public GameObject obstaclePrefab2;

    private PlayerController playerControllerScript;

    private Vector3 spawnPos = new Vector3(15, 0.5f, 0f); //I changed it from [Vector3(25, 0, 0)] to [Vector3(7f, 1f, 0.7f)].

    private float startDelay = 5f;
    private float repeatRate = 5f;

    private bool inputCheck = false;

    private int spawnNo;

    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        spawnPos = new Vector3(15f + Random.Range(0f, 15f), 0.5f, 0f);

        if ((Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.LeftArrow)) && !inputCheck) 
        {
            InvokeRepeating("SpawnObstacle", 0f, repeatRate / (playerControllerScript.moveLeftModifier * playerControllerScript.moveLeftModifier)); // Squared it for spice.
            inputCheck = true;
        }
        else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && inputCheck)
        {
            InvokeRepeating("SpawnObstacle", 0f, repeatRate);
            inputCheck = false;
        }
    }

    void SpawnObstacle()
    {
        if (!playerControllerScript.gameOver && !playerControllerScript.gameOver)//4.4
        {
            int prefabPicker = Random.Range(0, 3);
            if (prefabPicker == 0)
            {
                Instantiate(obstaclePrefab0, spawnPos, Quaternion.Euler(0, 90, 90));
            }
            else if (prefabPicker == 1)
            {
                Instantiate(obstaclePrefab1, spawnPos, Quaternion.Euler(0, 0, 0));
            }
            else if (prefabPicker == 2)
            {
                Instantiate(obstaclePrefab2, spawnPos, Quaternion.Euler(0, 0, 0));
            }
        }
    }
}
