using UnityEngine;
using TMPro;

public class FlyMovement : MonoBehaviour
{
    public float speed = 3f; // Fly's movement speed
    public float slowSpeed; // Fly's slow speed when flying to target
    public float moveRadius = 2f; // Movement radius
    public float floatAmplitude = 0.1f; // Amplitude of the floating effect
    public float floatSpeed = 2f; // Speed of the floating effect
    public int scoreValue = 1; // Value to add to score when a fly is collected

    protected Vector2 originPosition; // Changed to protected
    protected Vector2 targetPosition; // Changed to protected
    public Vector2 fixedTargetPosition = new Vector2(0f, 0f); // The fixed target position to fly towards

    protected float changeDirectionTime = 1f; // Changed to protected
    protected float timer = 0f; // Changed to protected
    protected bool isFlyingToTarget = false; // Changed to protected

    public float destroyRadius = 0.5f; // Radius to detect if near the center

    protected LampController lampController; // Reference to the LampController script
    private ScoreManager scoreManager; // Reference to the ScoreManager script
    protected GameManager gameManager;
    protected Instructor instructor;
    protected Transform scaleGameObject;


    protected virtual void Start()
    {
        originPosition = transform.position;
        targetPosition = originPosition;
        slowSpeed = speed * 0.5f;
        instructor = GetComponent<Instructor>();

        // Find the LampController component in the scene
        lampController = FindObjectOfType<LampController>();
        if (lampController == null)
        {
            Debug.LogError("LampController not found in the scene.");
        }

        // Find the ScoreManager component in the scene
        scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found in the scene.");
        }

        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }

        foreach (Transform child in transform)
        {
            // 检查子物体的名称是否包含 "scale"
            if (child.name.Contains("scale"))
            {
                scaleGameObject = child;
            }
        }
    }

    private void Update()
    {
        if (isFlyingToTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, fixedTargetPosition, Time.deltaTime * slowSpeed);

            if (Vector2.Distance(transform.position, fixedTargetPosition) <= destroyRadius)
            {
                if (lampController != null)
                {
                    lampController.IncreaseBattery(5f); // Increase battery by a specified amount
                }

                if (scoreManager != null)
                {
                    scoreManager.AddScore(scoreValue); // Add score
                }

                Destroy(gameObject); // Destroy the fly object
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

        float floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localScale = new Vector3(0.3f, 0.3f + 0.3f * floatOffset, 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LampLight"))
        {
            isFlyingToTarget = true;
        }

        if (!gameManager.hasFlyInstructionShownOnceInScene)
        {
            instructor.ShowInstructions();
            gameManager.hasFlyInstructionShownOnceInScene = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
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