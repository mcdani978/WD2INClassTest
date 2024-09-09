using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float turnSpeed = 100f;
    [SerializeField] private int health = 5;
    private float currentSpeed;
    private int score = 0;
    private bool isGameOver = false;

    // scoreText is a regular Text, gameOverText and healthText are TextMeshProUGUI
    public Text scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI healthText;
    public AudioClip coinSound;
    private AudioSource audioSource;

    void Start()
    {
        currentSpeed = moveSpeed;
        UpdateScoreText(); // Initialize the score display
        UpdateHealthText(); // Initialize the health display
        gameOverText.gameObject.SetActive(false); // Hide the Game Over text at the start

        // Initialize the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isGameOver)
        {
            // Braking function when space is pressed
            if (Input.GetKey(KeyCode.Space))
            {
                currentSpeed = 0f; // Brake the vehicle
            }
            else
            {
                // Vehicle movement
                float moveDirection = Input.GetAxis("Vertical");

                // Also check if 'R' is pressed for reversing
                if (Input.GetKey(KeyCode.R))
                {
                    moveDirection = -1f; // Set reverse
                }

                // Set current speed based on the move direction (forward or reverse)
                currentSpeed = moveSpeed * moveDirection;

                // Vehicle rotation
                float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

                // Translate the vehicle based on current speed
                transform.Translate(0, 0, currentSpeed * Time.deltaTime);
                transform.Rotate(0, turn, 0);
            }
        }
        else if (Input.anyKeyDown) // Detect any key press to restart game
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            currentSpeed = 0f; // Stop vehicle on collision with obstacle
            health--;
            UpdateHealthText(); // Update health display whenever health decreases

            if (health <= 0)
            {
                GameOver(); // Trigger game over if health is zero
            }
        }
        else if (collision.gameObject.CompareTag("Powerup"))
        {
            StartCoroutine(SpeedBoost(30f, 2f)); // Speed boost to 30 for 2 seconds
            Destroy(collision.gameObject); // Remove power-up from the scene
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            currentSpeed = moveSpeed; // Restore normal speed after leaving the obstacle
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            score++; // Increase score for collectible
            Destroy(other.gameObject); // Remove collectible from the scene
            UpdateScoreText(); // Update the score display
            PlayCoinSound(); // Play the coin sound effect
        }
        else if (other.CompareTag("Powerup"))
        {
            StartCoroutine(SpeedBoost(30f, 2f)); // Speed boost to 30 for 2 seconds
            Destroy(other.gameObject); // Remove power-up from the scene
        }
    }

    // Method to play the coin sound effect
    private void PlayCoinSound()
    {
        if (coinSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(coinSound);
        }
    }

    // speed boost
    private IEnumerator SpeedBoost(float boostSpeed, float duration)
    {
        currentSpeed = boostSpeed;
        yield return new WaitForSeconds(duration);
        currentSpeed = moveSpeed; // Reset the speed back to normal after the duration
    }

    // Method to update the score display
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Method to update the health display
    private void UpdateHealthText()
    {
        healthText.text = "Health: " + health; // Update the health UI text
    }

    // Game Over method
    private void GameOver()
    {
        isGameOver = true;
        currentSpeed = 0f; // Stop the car
        gameOverText.gameObject.SetActive(true); // Show Game Over text
    }
}