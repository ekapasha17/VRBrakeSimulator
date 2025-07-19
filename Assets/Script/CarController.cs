using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform[] waypoints;


    public float normalSpeed = 20.0f;
    public float slowSpeed = 5.0f;

    private int currentWaypointIndex = 0;
    public float steerSpeed = 1f;
    public float arrivalDistance = 0.1f;
    private Rigidbody rb;

    AudioManager audioManager;

    public Canvas successCanvas;
    public Canvas failedCanvas;

    void Awake()
    {
        // Find the AudioManager in the scene.
        audioManager = GameObject.FindGameObjectsWithTag("Audio")[0].GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        successCanvas.enabled = false;
        failedCanvas.enabled = false;
    }

    void Update()
    {

        if (waypoints.Length == 0)
        {
            return;
        }

        // --- NEW: LOGIC FOR CHANGING SPEED ---
        float currentSpeed = normalSpeed;

        if (audioManager.sfxSource.clip != audioManager.carMovingSlowly && normalSpeed != 0f)
        {
            audioManager.PlaySFX(audioManager.carMovingSlowly, true);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            currentSpeed = slowSpeed;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            normalSpeed = 0f;
            if (audioManager != null && audioManager.explosion != null)
            {
                audioManager.PlaySFX(audioManager.engineIdle, true);
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            normalSpeed = 10f;
        }

        // Smooth rotation towards target
        Vector3 targetDirection = (waypoints[currentWaypointIndex].position - transform.position).normalized;


        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, steerSpeed * Time.deltaTime);

        // Move towards the target waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, currentSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            if (currentWaypointIndex == waypoints.Length - 1)
            {
                // Reached the last waypoint, show success canvas
                successCanvas.enabled = true;
                if (audioManager != null && audioManager.engineStart != null)
                {
                    normalSpeed = 0f;
                    audioManager.sfxSource.Stop();
                }
            }
            else
            {
                // Move to the next waypoint
                currentWaypointIndex++;
            }

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            normalSpeed = 0f;
            if (audioManager != null && audioManager.explosion != null)
            {
                failedCanvas.enabled = true;
                audioManager.PlaySFX(audioManager.explosion);
            }
        }
    }
}