using UnityEngine;
using UnityEngine.UI;

public class LampController : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D lampLight;  // Reference to Light2D component
    public float maxIntensity = 5f;  // Maximum intensity of the light
    public float minIntensity = 0f;  // Minimum intensity of the light
    public float maxRange = 5f;  // Maximum range of the light
    public float minRange = 0f;  // Minimum range of the light
    public float intensityIncreaseRate = 1f;  // Rate of intensity increase
    public float rangeIncreaseRate = 1f;  // Rate of range increase
    public float smoothTime = 0.5f;  // Smooth time for decreasing intensity and range
    public float rotationSmoothTime = 0.1f;  // Smooth time for light rotation
    public float batteryConsumptionRate = 10f;  // Rate of battery consumption
    public float maxBattery = 100f;  // Maximum battery level
    public Slider batterySlider;  // Reference to the battery slider
    public float batteryIncreaseAmount = 20f;  // Amount of battery increase

    private float currentIntensity;  // Current intensity
    public float currentRange;  // Current range
    private float currentBattery;  // Current battery level
    private float intensityVelocity = 0f;  // Intensity smoothing velocity
    private float rangeVelocity = 0f;  // Range smoothing velocity
    private Vector3 currentVelocity = Vector3.zero;  // Rotation smoothing velocity

    void Start()
    {
        currentBattery = maxBattery;  // Initialize battery to max value
        batterySlider.maxValue = maxBattery;  // Set the max value of the slider
        batterySlider.value = currentBattery;  // Initialize the slider value
    }

    void Update()
    {
        if (Input.GetMouseButton(0))  // While holding down the left mouse button
        {
            // Increase light intensity and range
            currentIntensity = Mathf.Clamp(currentIntensity + intensityIncreaseRate * Time.deltaTime, minIntensity, maxIntensity);
            currentRange = Mathf.Clamp(currentRange + rangeIncreaseRate * Time.deltaTime, minRange, maxRange);

            // Calculate battery consumption based on intensity
            float batteryConsumption = batteryConsumptionRate * currentIntensity * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery - batteryConsumption, 0, maxBattery);

            // Update the battery slider
            batterySlider.value = currentBattery;
        }
        else
        {
            // Smoothly decrease light intensity and range when mouse is not held down
            currentIntensity = Mathf.SmoothDamp(currentIntensity, minIntensity, ref intensityVelocity, smoothTime);
            currentRange = Mathf.SmoothDamp(currentRange, minRange, ref rangeVelocity, smoothTime);
        }

        // Apply light intensity and range
        lampLight.intensity = currentIntensity;
        lampLight.pointLightOuterRadius = currentRange;

        // Control light direction
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Smoothly rotate the light
        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle - 90, ref currentVelocity.z, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0, 0, smoothedAngle);
    }

    public void DecreaseBattery(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery - amount, 0, maxBattery);
        batterySlider.value = currentBattery;

        // Handle battery depletion logic if needed
        if (currentBattery <= 0)
        {
            // For example, turn off the light
            lampLight.enabled = false;
        }
    }

    public void IncreaseBattery(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery + amount, 0, maxBattery);
        batterySlider.value = currentBattery;
    }
}
