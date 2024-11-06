using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip crashSound;
    private AudioSource playerAudio;
    public float moveSpeed = 50.0f;
    public float horizontalInput;
    public float xRange = 10.0f;
    public GameManager gameManager;    // Reference to GameManager

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        // Ensure constraints are cleared at the start
        playerRb.constraints = RigidbodyConstraints.None;

        // Find the GameManager instance in the scene
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (gameOver) return; // Stop all movement if the game is over

        // Restrict movement to within xRange
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("obstacle") || collision.gameObject.CompareTag("walls"))
        {
            gameOver = true;
            Debug.Log("Game Over!");

            // Trigger game over effects
            explosionParticle.Play();
            Debug.Log("Playing explosion particle");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);

            // Freeze the car to stop all movement
            playerRb.constraints = RigidbodyConstraints.FreezeAll;

            // Set the velocity and angular velocity to zero to ensure the car stops completely
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;

            // Notify GameManager to stop spawning obstacles
            if (gameManager != null)
            {
                Debug.Log("Calling EndGame() in GameManager");
                gameManager.EndGame(); // End the game and stop spawning obstacles
            }

        }
    }
}
