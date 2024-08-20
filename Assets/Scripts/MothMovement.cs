using UnityEngine;

public class MothMovement : FlyMovement
{
    public float damageToLight = 20f; // Moth's damage to light
    public float moveOutSpeed = 2f; // Speed at which the moth moves out of bounds
    public float maxInactiveTime = 5f; // Maximum time before the moth moves out of bounds
    public Camera mainCamera; // Reference to the main camera

    private float inactiveTime = 0f; // Time the moth has been inactive

    protected new void Start()
    {
        base.Start();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found. Please assign a Camera in the inspector.");
            }
        }

        // Ensure AudioSource component is attached
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found. Please assign it in the inspector.");
            }
        }
    }

    void Update()
    {
        if (isFlyingToTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, fixedTargetPosition, Time.deltaTime * slowSpeed);

            if (Vector2.Distance(transform.position, fixedTargetPosition) <= destroyRadius)
            {
                if (lampController != null)
                {
                    lampController.DecreaseBattery(damageToLight); // Decrease battery by damageToLight
                }
                Destroy(gameObject);
            }

            inactiveTime = 0f;
        }
        else
        {
            inactiveTime += Time.deltaTime;

            if (inactiveTime > maxInactiveTime)
            {
                Vector3 direction = (transform.position - (Vector3)fixedTargetPosition).normalized;
                transform.position += direction * moveOutSpeed * Time.deltaTime;

                if (IsOutOfBounds(transform.position))
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                timer += Time.deltaTime;
                changeDirectionTime = 1f + Random.Range(0.5f, 1.5f);

                if (timer > changeDirectionTime)
                {
                    Vector2 randomDirection = new Vector2(
                        Random.Range(-1f, 1f),
                        Random.Range(-1f, 1f)
                    ).normalized;

                    targetPosition = originPosition + randomDirection * moveRadius;

                    timer = 0f;
                }

                transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
            }
        }
    }

    bool IsOutOfBounds(Vector3 position)
    {
        if (mainCamera == null)
            return false;

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(position);
        return viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LampLight"))
        {
            isFlyingToTarget = true;

            // Play audio with random pitch
            if (audioSource != null && triggerEnterClip != null)
            {
                audioSource.pitch = Random.Range(pitchMin, pitchMax);
                audioSource.PlayOneShot(triggerEnterClip);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LampLight") && isFlyingToTarget)
        {
            originPosition = transform.position;
            isFlyingToTarget = false;
            timer = 0f;
            targetPosition = originPosition + new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized * moveRadius;
        }
    }
}
