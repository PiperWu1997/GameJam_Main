
using UnityEngine;

public class BeetleMovement : MonoBehaviour
{
    public float speed = 3f; // Beetle's movement speed
    public float moveRadius = 2f; // Movement radius
    public float zigzagAmplitude = 0.5f; // Amplitude of the zigzag pattern
    public float zigzagFrequency = 2f; // Frequency of the zigzag pattern

    public float destroyRadius = 0.5f; // Radius to detect if near the center

    private Vector2 targetPosition; // The target position to fly towards
    private Vector2 originPosition;
    private LampController lampController; // Reference to the LampController script

    private float exposureTime = 0f; // Time the beetle has been exposed to the laser
    private bool isExposedToLaser = false; // Is the beetle exposed to the laser?

    void Start()
    {
        // Find the LampController component in the scene
        lampController = FindObjectOfType<LampController>();
        if (lampController == null)
        {
            Debug.LogError("LampController not found in the scene.");
        }

        // Set the target position to the lamp's position
        targetPosition = lampController.transform.position;
        originPosition = transform.position;
    }

    void Update()
    {
        // Move the beetle in a zigzag pattern toward the lamp
        float zigzagOffset = Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude;
        Vector2 direction = (targetPosition - originPosition).normalized;
        Vector2 zigzagDirection = direction + new Vector2(-direction.y, direction.x) * zigzagOffset;

        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + zigzagDirection, Time.deltaTime * speed);

        if (isExposedToLaser)
        {
            exposureTime += Time.deltaTime;
            if (exposureTime >= 0.2f)
            {
                DestroyBeetle(true); // Destroy beetle and increase battery
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LampLight"))
        {
            // Exposed to laser
            isExposedToLaser = true;
        }
        else if (other.CompareTag("Lamp"))
        {
            // Collision with lamp
            DestroyBeetle(false); // Destroy beetle and decrease battery
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LampLight"))
        {
            // Exit laser
            isExposedToLaser = false;
            exposureTime = 0f;
        }
    }

    private void DestroyBeetle(bool increaseBattery)
    {
        if (increaseBattery)
        {
            lampController.IncreaseBattery(5f); // Increase battery by 5
        }
        else
        {
            lampController.DecreaseBattery(5f); // Decrease battery by 5
        }

        Destroy(gameObject); // Destroy the beetle object
    }
}
