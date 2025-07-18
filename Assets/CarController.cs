using UnityEngine;

public class CarController : MonoBehaviour
{
    // A list of points for the car to follow.
    public Transform[] waypoints;

    // The speed at which the car moves.
    public float normalSpeed = 20.0f; // CHANGED: Renamed to normalSpeed
    public float slowSpeed = 5.0f;   // NEW: The speed when spacebar is pressed

    // This will keep track of which waypoint we are currently moving towards.
    private int currentWaypointIndex = 0;

    void Update()
    {
        // First, check if we have any waypoints in our list. If not, do nothing.
        if (waypoints.Length == 0)
        {
            return;
        }

        // --- NEW: LOGIC FOR CHANGING SPEED ---
        // Create a temporary variable to hold the current speed.
        float currentSpeed = normalSpeed;

        // Check if the spacebar is being held down.
        if (Input.GetKey(KeyCode.Space))
        {
            // If it is, use the slow speed.
            currentSpeed = slowSpeed;
        }
        // ------------------------------------

        // Get the current target waypoint from our list.
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Move the car towards the target waypoint using the 'currentSpeed'.
        // CHANGED: We now use our new 'currentSpeed' variable here.
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, currentSpeed * Time.deltaTime);

        // Optional: Make the car look towards the point it's moving to.
        transform.LookAt(targetWaypoint.position);

        // Check if the car has reached the target waypoint.
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }
}