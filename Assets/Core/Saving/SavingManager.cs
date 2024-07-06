using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingManager : MonoBehaviour
{

    public void Save()
    {
        SavingSystem.i.Save("Slot1");
    }
    public void Load()
    {
        SavingSystem.i.Load("Slot1");
    }
}
