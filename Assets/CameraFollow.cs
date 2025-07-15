using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target to Follow")]
    public Transform target; // The car's transform

    [Header("Camera Settings")]
    public float smoothSpeed = 10f; // How quickly the camera catches up
    public Vector3 offset; // The camera's position relative to the car (e.g., behind and above)

    // LateUpdate is called after all Update functions have been called.
    // This is the best place to move a camera to avoid jittery movement.
    void LateUpdate()
    {
        // Make sure we have a target to follow
        if (target == null)
        {
            Debug.LogWarning("Camera Follow script has no target!");
            return;
        }

        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;
        
        // Smoothly move the camera from its current position to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        
        // Apply the new position
        transform.position = smoothedPosition;

        // Make the camera always look at the target (the car)
        transform.LookAt(target);
    }
}