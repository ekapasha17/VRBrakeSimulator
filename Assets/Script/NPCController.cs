using UnityEngine;

public class NPCPedestrian : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public Vector3 walkDirection = Vector3.forward; // Direction NPC walks on sidewalk

    [Header("Detection Settings")]
    public float detectionRange = 20f;
    public float detectionCrashRange = 5f;
    public LayerMask playerCarLayer = -1; // What layers to detect (Player/Car)

    [Header("Road Crossing")]
    public Transform roadCrossPoint; // Point to cross to (other side of road)
    public float crossingDistance = 1f; // How close to get to crossing point

    [Header("Debug")]
    public bool showGizmos = true;

    // Private variables
    private Vector3 originalPosition;
    private bool isCrossing = false;
    private bool hasReachedCrossing = false;
    private bool isCrashing = false;
    private Rigidbody rb;
    private Animator animator;

    [SerializeField] Transform car;

    // States
    private enum NPCState
    {
        Walking,
        CrossingRoad,
        ReturningToSidewalk
    }
    private NPCState currentState = NPCState.Walking;

    AudioManager audioManager;

    void Awake()
    {
        // Find the AudioManager in the scene.
        audioManager = GameObject.FindGameObjectsWithTag("Audio")[0].GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Optional
        originalPosition = transform.position;

        // Normalize walk direction
        walkDirection = walkDirection.normalized;
    }

    void Update()
    {
        switch (currentState)
        {
            case NPCState.Walking:
                HandleWalking();
                break;
            case NPCState.CrossingRoad:
                HandleCrossing();
                break;
            case NPCState.ReturningToSidewalk:
                HandleReturning();
                break;
        }

        DetectCarAlmostCrush();

    }

    void HandleWalking()
    {
        // Move in walk direction
        Vector3 movement = walkDirection * walkSpeed * Time.deltaTime;
        transform.position += movement;

        // Rotate to face movement direction
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

        // Check for player/car in range
        if (DetectPlayerOrCar())
        {
            StartCrossing();
        }
    }

    void HandleCrossing()
    {
        if (roadCrossPoint == null)
        {
            return;
        }

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }

        // Move towards crossing point
        Vector3 targetDirection = (roadCrossPoint.position - transform.position).normalized;
        Vector3 movement = targetDirection * runSpeed * Time.deltaTime;
        transform.position += movement;

        // Rotate to face crossing direction
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

        // Check if reached crossing point
        float distanceToCrossing = Vector3.Distance(transform.position, roadCrossPoint.position);
        if (distanceToCrossing <= crossingDistance)
        {
            currentState = NPCState.ReturningToSidewalk;
            hasReachedCrossing = true;
        }
    }

    void HandleReturning()
    {
        // Continue walking in original direction on the other side
        Vector3 movement = walkDirection * walkSpeed * Time.deltaTime;
        transform.position += movement;

        // Rotate to face movement direction
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

        // Optional: You can add logic to return to normal walking state
        // after some time or distance
    }

    bool DetectPlayerOrCar()
    {
        // Use sphere cast to detect player/car in range
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRange, playerCarLayer);

        foreach (Collider col in detectedObjects)
        {
            if (col.CompareTag("Player") || col.CompareTag("Car"))
            {
                isCrashing = true;
                return true;
            }
        }
        return false;
    }

    void DetectCarAlmostCrush()
    {
        // Use sphere cast to detect player/car in range
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionCrashRange, playerCarLayer);

        foreach (Collider col in detectedObjects)
        {
            // Check if detected object has Player or Car tag
            if (col.CompareTag("Player") || col.CompareTag("Car"))
            {

                // Smooth rotation towards target
                Vector3 targetDirection = (car.position - transform.position).normalized;

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

                animator.SetBool("isFalling", true);
                walkSpeed = 0f;

            }
        }
    }

    void StartCrossing()
    {
        if (roadCrossPoint == null)
        {
            return;
        }

        isCrossing = true;
        currentState = NPCState.CrossingRoad;

    }

    // Optional: Reset NPC to original behavior
    public void ResetNPC()
    {
        transform.position = originalPosition;
        isCrossing = false;
        hasReachedCrossing = false;
        currentState = NPCState.Walking;
    }

    // Gizmos for debugging in Scene view
    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        // Detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.DrawWireSphere(transform.position, detectionCrashRange);

        // Walk direction
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, walkDirection * 3f);

        // Line to crossing point
        if (roadCrossPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, roadCrossPoint.position);

            // Crossing point indicator
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(roadCrossPoint.position, crossingDistance);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {

            audioManager.PlaySFX(audioManager.womanScream);
            walkSpeed = 0f;
        }
    }
}