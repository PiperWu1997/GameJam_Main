using UnityEngine;

public class PhantomBugMovement : FlyMovement
{
    public int scoreWhenDestroyed = 5;
    public int batteryDecreasedWhenHitLamp = 5;

    protected new void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LampLight"))
        {
            isFlyingToTarget = true;
        }
        if (other.CompareTag("Lamp"))
        {
            Debug.LogWarning($"PhantomBug collided with Lamp! {batteryDecreasedWhenHitLamp}");
            lampController.DecreaseBattery(batteryDecreasedWhenHitLamp);
            Destroy(gameObject);

            PlaySoundWithRandomPitch();
        }
        if (!gameManager.hasPhantomBugInstructionShownOnceInScene)
        {
            instructor.ShowInstructions();
            gameManager.hasPhantomBugInstructionShownOnceInScene = true;
        }
    }
}
