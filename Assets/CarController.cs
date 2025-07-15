using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed to restart the scene

public class CarController : MonoBehaviour
{
    [Header("Track Settings")]
    public List<Transform> waypoints; // List of all waypoints that define the track
    private int currentWaypointIndex = 0;

    [Header("Car Speed")]
    public float normalSpeed = 20f;
    public float slowSpeed = 8f;

    [Header("Braking")]
    public KeyCode brakeKey = KeyCode.Space;

    private float currentSpeed;
    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component attached to this car
        rb = GetComponent<Rigidbody>();
        
        // Set the initial speed
        currentSpeed = normalSpeed;

        // Make sure there are waypoints to follow
        if (waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned to the car controller!");
            this.enabled = false; // Disable the script if there's no path
        }
    }

    void Update()
    {
        // --- Player Input for Braking ---
        // Check if the player is pressing the brake key
        if (Input.GetKey(brakeKey))
        {
            currentSpeed = slowSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }
    }

    void FixedUpdate()
    {
        // --- Autopilot Logic ---
        // Ensure we haven't finished the track
        if (currentWaypointIndex >= waypoints.Count)
        {
            // We've reached the end of the path
            rb.velocity = Vector3.zero; // Stop the car
            Debug.Log("Race Finished!");
            return;
        }

        // Get the target waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Calculate the direction to the target
        Vector3 directionToTarget = (targetWaypoint.position - transform.position).normalized;

        // Apply force/velocity to move the car forward
        // We only want to move on the X and Z axes, not fly up.
        Vector3 movementVelocity = new Vector3(directionToTarget.x, 0, directionToTarget.z) * currentSpeed;
        
        // Keep the car's current vertical velocity (so it stays on the ground)
        movementVelocity.y = rb.velocity.y;
        
        rb.velocity = movementVelocity;
        
        // Make the car look towards the direction it's moving
        if (rb.velocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z));
        }


        // Check if we are close enough to the waypoint to move to the next one
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 3f)
        {
            currentWaypointIndex++;
        }
    }

    // --- Collision Detection ---
    void OnCollisionEnter(Collision collision)
    {
        // Check if we hit an object tagged as "Obstacle"
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("CRASH! You hit an obstacle.");
            // You can add game over logic here
            // For example, restart the level:
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}