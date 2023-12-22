using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private GameObject player; //4.4
    private Rigidbody playerRb;
    private Animator playerAnim;

    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    private AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    private float defaultAnimSpeed;

    public bool startOfGame = true; //New boolean. 4.4
    private float startXPos = -10f; // Starting Position 4.4
    private float startSpeed = 10f;

    private float jumpForce = 40f;
    private float dJumpMultiplier = 0.8f;
    private float gravityModifier = 10f;

    public float moveLeftModifier;

    public bool isOnGround = true;
    public bool dJump = false;

    public bool gameOver = false;
    private GameObject gOText;
    private TextMeshProUGUI scoreText;

    public int scoreTotal = 0;

    void Start()
    {
        startOfGame = true; //4.4
        gameOver = false;

        player = GameObject.Find("Player");
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        dirtParticle = GameObject.Find("/Player/FX_DirtSplatter").GetComponent<ParticleSystem>();
        explosionParticle = GameObject.Find("/Player/FX_Explosion_Smoke").GetComponent<ParticleSystem>();

        playerAudio = GetComponent<AudioSource>();

        Physics.gravity *= gravityModifier;

        defaultAnimSpeed = playerAnim.speed;

        gOText = GameObject.Find("/Canvas/Game Over");
        gOText.SetActive(false);

        scoreText = GameObject.Find("/Canvas/Score").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (startOfGame)//4.4
        {
            if (player.transform.position.x < startXPos)
            {
                player.transform.position = player.transform.position + new Vector3(startSpeed, 0, 0) * (Time.deltaTime / 3f);
            }
            else if (player.transform.position.x > startXPos)
            {
                dirtParticle.Play();

                playerAnim.SetFloat("Speed_f", 1f);

                startOfGame = false;
            }
            return;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && isOnGround && !startOfGame && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            dJump = true;
            playerAnim.SetTrigger("Jump_trig");

            playerAudio.PlayOneShot(jumpSound, 1f);

            dirtParticle.Stop();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isOnGround && dJump && !startOfGame && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce * dJumpMultiplier, ForceMode.Impulse);
            dJump = false;
            playerAnim.SetTrigger("Jump_trig");

            playerAudio.PlayOneShot(jumpSound, 1f);
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveLeftModifier = 2f;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveLeftModifier = 0.75f;
        }
        else
        {
            moveLeftModifier = 1f;
        }

        if (playerAnim.speed != defaultAnimSpeed * moveLeftModifier)
        {
            playerAnim.speed = defaultAnimSpeed * moveLeftModifier;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dJump = false;
            if (!startOfGame && !gameOver) //Bit of bug fixing. It turns out the dust particles play if the player hits the ground after game over, before this change.
            {
                dirtParticle.Play();
            }
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over");
            gameOver = true;
            gOText.SetActive(true);
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);

            playerAudio.PlayOneShot(crashSound, 1f);

            dirtParticle.Stop();
            explosionParticle.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
        Destroy(other.gameObject);
    }

    public void AddScore()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            scoreTotal += 125;
        }
        else
        {
            scoreTotal += 100;
        }
        scoreText.text = scoreTotal.ToString();
        Debug.Log(scoreTotal.ToString());
    }
}
