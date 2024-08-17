using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    // Speed of the fly's movement
    public float speed = 3f; // Fly's movement speed

    // Radius within which the fly can move
    public float moveRadius = 2f; // Movement radius

    // Amplitude of the floating effect
    public float floatAmplitude = 0.1f; // Amplitude of the floating effect

    // Speed of the floating effect
    public float floatSpeed = 2f; // Speed of the floating effect

    // The original position of the fly
    private Vector2 originPosition;
    private Vector2 targetPosition;

    // Timer for changing direction
    private float changeDirectionTime = 1f; // Time interval for changing direction
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Record the fly's initial position
        originPosition = transform.position;
        // Initialize the target position
        targetPosition = originPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;
        changeDirectionTime = 1f + Random.Range(0.5f, 1.5f);
        
        // If the timer exceeds the change direction interval, calculate a new target position
        if (timer > changeDirectionTime)
        {
            // Generate a random direction (on the top-down 2D plane)
            Vector2 randomDirection = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;

            // Calculate the new target position
            targetPosition = originPosition + randomDirection * moveRadius;

            // Reset the timer
            timer = 0f;
        }

        // Move the fly towards the target position
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * speed);

        // Add a floating effect
        float floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localScale = new Vector3(0.3f, 0.3f + 0.3f * floatOffset, 0.3f);
    }
}