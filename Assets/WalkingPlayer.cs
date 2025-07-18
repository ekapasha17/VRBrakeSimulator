using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 movement;

    AudioManager audioManager;

    private void OnCollisionEnter(Collision collision)
    {
        Console.WriteLine("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            audioManager.PlaySFX(audioManager.explosion);
        }
    }

    void Awake()
    {
        audioManager = GameObject.FindGameObjectsWithTag("Audio")[0].GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Start Engine Sound
        if (Input.GetKeyDown(KeyCode.E))
        {
            audioManager.PlaySFX(audioManager.engineStart);
            // Play idle sound after 100ms starting engine
            Invoke(nameof(PlayIdleSound), 1f);

        }

        // Play moving sound when moving
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (audioManager.sfxSource.clip != audioManager.carMovingSlowly)
            {
                audioManager.PlaySFX(audioManager.carMovingSlowly, true);
            }
        }
        else
        {
            if (audioManager.sfxSource.clip == audioManager.carMovingSlowly)
            {
                this.PlayIdleSound();
            }
        }

        // Get WASD input
        float moveX = Input.GetAxisRaw("Horizontal"); // A, D
        float moveZ = Input.GetAxisRaw("Vertical");   // W, S

        // Movement direction
        movement = new Vector3(moveX, 0f, moveZ).normalized;
    }

    private object PlayIdleSound()
    {
        audioManager.PlaySFX(audioManager.engineIdle, true);
        return null;
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
