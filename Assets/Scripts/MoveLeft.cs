using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private Rigidbody thisRb;
    private Animator playerAnim;

    private float speed = 7500f;
    private float leftBound = -20f;

    private GameObject player;
    private PlayerController playerControllerScript;

    public bool isBarrel = false;
    private float barrelSpin;

    private bool scoreSwitch = true;

    void Start()
    {
        player = GameObject.Find("Player");
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        playerAnim = GetComponent<Animator>();  
    }

    void Update() //I changed some things to get rolling barrels :)
    {
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            return;
        }

        if (transform.position.x <= player.transform.position.x && gameObject.CompareTag("Obstacle") && scoreSwitch)
        {
            playerControllerScript.AddScore();
            scoreSwitch = false;
        }

        if (thisRb != null)
        {
            if (!playerControllerScript.startOfGame && !playerControllerScript.gameOver)
            {
                if (isBarrel)
                {
                    barrelSpin += -90f * Time.deltaTime;
                    thisRb.velocity = Vector3.left * Time.deltaTime * speed * playerControllerScript.moveLeftModifier * 1.75f; //faster barrels to spice things up
                    thisRb.rotation = Quaternion.Euler(barrelSpin, 90f, 90f);
                    return;
                }
                else
                {
                    thisRb.velocity = Vector3.left * Time.deltaTime * speed * playerControllerScript.moveLeftModifier;
                    return;
                }
            }
            else if (playerControllerScript.startOfGame || playerControllerScript.gameOver)
            {
                if (isBarrel)
                {
                    thisRb.velocity = Vector3.left * Time.deltaTime * speed * 0.75f; //Barrels still move because I opted out of a complete pause for them to move according to their relative motion.
                    thisRb.rotation = Quaternion.Euler(0f, 90f, 90f);
                    return;
                }
                else
                {
                    thisRb.velocity = Vector3.zero;
                    return;
                }
            }
        }
        else
        {
            thisRb = GetComponent<Rigidbody>();
        }
    }
}
