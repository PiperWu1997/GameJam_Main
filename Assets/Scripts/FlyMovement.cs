using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    public float speed = 3f;
    public float slowSpeed;
    public float moveRadius = 2f;
    public float floatAmplitude = 0.1f;
    public float floatSpeed = 2f;
    public int scoreValue = 1;

    protected Vector2 originPosition;
    protected Vector2 targetPosition;
    public Vector2 fixedTargetPosition = new Vector2(0f, 0f);

    protected float changeDirectionTime = 1f;
    protected float timer = 0f;
    protected bool isFlyingToTarget = false;

    public float destroyRadius = 0.5f;

    protected LampController lampController;
    private ScoreManager scoreManager;
    protected GameManager gameManager;
    protected Instructor instructor;
    protected Transform scaleGameObject;

    public AudioSource audioSource;
    public AudioClip triggerEnterClip;
    public float pitchMin = 0.9f;
    public float pitchMax = 1.1f;

    protected virtual void Start()
    {
        originPosition = transform.position;
        targetPosition = originPosition;
        slowSpeed = speed * 0.5f;
        instructor = GetComponent<Instructor>();

        lampController = FindObjectOfType<LampController>();
        if (lampController == null)
        {
            Debug.LogError("LampController not found in the scene.");
        }

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
            if (child.name.Contains("scale"))
            {
                scaleGameObject = child;
            }
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component not found.");
            }
        }
    }

    protected virtual void Update()
    {
        if (isFlyingToTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, fixedTargetPosition, Time.deltaTime * slowSpeed);

            if (!gameObject.name.Contains("Phantom") && Vector2.Distance(transform.position, fixedTargetPosition) <= destroyRadius)
            {
                if (lampController != null)
                {
                    lampController.IncreaseBattery(5f);
                }

                if (scoreManager != null)
                {
                    scoreManager.AddScore(scoreValue);
                }

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

        float floatOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localScale = new Vector3(0.3f, 0.3f + 0.3f * floatOffset, 0.3f);
    }

    protected void PlaySoundWithRandomPitch()
    {
        if (audioSource != null && triggerEnterClip != null)
        {
            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.PlayOneShot(triggerEnterClip);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LampLight"))
        {
            isFlyingToTarget = true;
            PlaySoundWithRandomPitch();
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
