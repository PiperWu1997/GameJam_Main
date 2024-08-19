using UnityEngine;
using TMPro;

public class PhantomBugMovement : FlyMovement
{
    protected new void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("LampLight"))
        {
            isFlyingToTarget = true;
        }
        
        if (!gameManager.hasPhantomBugInstructionShownOnceInScene)
        {
            instructor.ShowInstructions();
            gameManager.hasPhantomBugInstructionShownOnceInScene = true;
        } 
    }
}