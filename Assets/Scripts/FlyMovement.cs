using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    public float speed = 3f; // Fly's movement speed
    public float slowSpeed; // Fly's slow speed when flying to target
    public float moveRadius = 2f; // Movement radius
    public float floatAmplitude = 0.1f; // Amplitude of the floating effect
    public float floatSpeed = 2f; // Speed of the floating effect

    protected Vector2 originPosition;
    protected Vector2 targetPosition;  // 修改为protected
    public Vector2 fixedTargetPosition = new Vector2(0f, 0f); // The fixed target position to fly towards

    private float changeDirectionTime = 1f; // Time interval for changing direction
    protected float timer = 0f;  // 修改为protected

    protected bool isFlyingToTarget = false;  // 已修改为protected

    void Start()
    {
        originPosition = transform.position;
        targetPosition = originPosition;
        slowSpeed = speed * 0.5f;
    }

    void Update()
    {
        if (isFlyingToTarget)
        {
            transform.position = Vector2.MoveTowards(transform.position, fixedTargetPosition, Time.deltaTime * slowSpeed);
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LampLight"))
        {
            isFlyingToTarget = true;
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
