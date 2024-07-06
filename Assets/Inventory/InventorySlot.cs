using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [Header("UI stuff to change")]
    [SerializeField] public TextMeshProUGUI itemNumberText;
    [SerializeField] public Image itemImage;


    [Header("Variable from item")]

    public InventoryItem thisItem;
    public InventoryManager thisManager;
    public EquipedItemManager thatManger;

    public void Setup(InventoryItem newItem, InventoryManager newManager)
    {
        thisItem = newItem;
        thisManager = newManager;
        if(thisItem)
        {
            itemImage.sprite = thisItem.itemImage;

            itemNumberText.text = "" + thisItem.numberHeld;
        }
    }

    public void ClickedOn()
    {
        thisManager.SetupDescriptionAndButton(thisItem.itemDescription,thisItem.itemImage,
            thisItem.usable, thisItem.dicardable, thisItem);
    }
}
