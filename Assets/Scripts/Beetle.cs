using UnityEngine;

public class Beetle : MonoBehaviour
{
    private float laserExposureTime = 0f; // Keep this private

    private LampController lampController;

    void Start()
    {
        lampController = FindObjectOfType<LampController>();
        if (lampController == null)
        {
            Debug.LogError("LampController not found in the scene.");
        }
    }

    public void IncreaseLaserExposure(float amount)
    {
        laserExposureTime += amount;
        Debug.Log("Beetle exposure increased by: " + amount + ", total exposure: " + laserExposureTime);
    }

    public bool IsExposedToLaser(float threshold)
    {
        bool isExposed = laserExposureTime >= threshold;
        Debug.Log("Beetle exposed to laser: " + isExposed);
        return isExposed;
    }

    public void DestroyBeetle(bool increaseBattery)
    {
        if (lampController != null)
        {
            if (increaseBattery)
            {
                Debug.Log("Destroying beetle and increasing battery");
                lampController.IncreaseBattery(5f);
            }
            else
            {
                Debug.Log("Destroying beetle and decreasing battery");
                lampController.DecreaseBattery(5f);
            }
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LampLight"))
        {
            Debug.Log("Beetle exposed to laser");
            IncreaseLaserExposure(Time.deltaTime);
        }
        else if (collision.CompareTag("Lamp"))
        {
            Debug.Log("Beetle collided with lamp");
            DestroyBeetle(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("LampLight"))
        {
            Debug.Log("Beetle exited laser");
        }
    }
}
