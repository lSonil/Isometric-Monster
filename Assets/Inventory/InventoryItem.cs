using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName="Inventory/Items")]
public class InventoryItem : ScriptableObject
{
    [Header("Inventory Info")]

    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public int numberHeld;
    public bool unique;
    public bool usable;
    public bool equipable;
    public bool dicardable;
    public bool forBattle;

    public UnityEvent useEvent;

    public void ActivEvent()
    {
    }
    public void Use()
    {
        useEvent.Invoke();
        if(forBattle==true&&BattleZone.battleOn==true)
        DecreaseAmount(1);
    }

    public void Discard()
    {
        numberHeld = 0;
    }

    public void DecreaseAmount(int amountToDecrease)
    {
        numberHeld -= amountToDecrease;
        if(numberHeld<0)
        {
            numberHeld = 0;
        }
    }

    public void deActiv()
    {
        ActivEvent();
    }

}
