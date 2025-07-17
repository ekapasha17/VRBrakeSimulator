using UnityEngine;

public class CarController : MonoBehaviour
{
    // A list of points for the car to follow.
    // 'public' means we can drag-and-drop our points into this list in the Unity Editor.
    public Transform[] waypoints;

    // The speed at which the car moves.
    public float speed = 10.0f;

    // This will keep track of which waypoint we are currently moving towards.
    private int currentWaypointIndex = 0;

    // The Update function is called by Unity once every frame.
    void Update()
    {
        // First, check if we have any waypoints in our list. If not, do nothing.
        if (waypoints.Length == 0)
        {
            return;
        }

        // Get the current target waypoint from our list.
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Move the car towards the target waypoint.
        // Vector3.MoveTowards calculates a position between our current position and the target.
        // Time.deltaTime makes the movement smooth and independent of the frame rate.
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Optional: Make the car look towards the point it's moving to.
        transform.LookAt(targetWaypoint.position);

        // Check if the car has reached the target waypoint.
        // We check if the distance is very small, because it might never be exactly zero.
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // If we've reached the waypoint, update the index to aim for the next one.
            currentWaypointIndex++;

            // If we've reached the last waypoint, loop back to the first one (index 0).
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }
    }
}