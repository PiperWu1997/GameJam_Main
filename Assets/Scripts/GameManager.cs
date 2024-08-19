using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool hasFlyInstructionShownOnceInScene;
    public bool hasMothInstructionShownOnceInScene;
    public bool hasPhantomBugInstructionShownOnceInScene;
    // Start is called before the first frame update
    void Start()
    {
        hasFlyInstructionShownOnceInScene = false;
        hasMothInstructionShownOnceInScene = false;
        hasPhantomBugInstructionShownOnceInScene = false;
    }

}
