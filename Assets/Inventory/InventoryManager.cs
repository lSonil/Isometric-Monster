using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventoryManager : MonoBehaviour
{

    [Header("Inventory Information")]
    public PlayerInventory playerInventory;
    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject discardButton;
    public InventoryItem currentItem;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject decoyButton;


    public void SetTextAndButton(string description, bool buttonActive)
    {
        descriptionText.text = description;
        if (buttonActive)
        {
            optionPanel.SetActive(true);

        }else
        {
            optionPanel.SetActive(false);
        }

    }

    public void MakeInventorySlots()
    {
        if (playerInventory)
        {
            for (int i = 0; i < playerInventory.myInventory.Count; i++)
            {
                if (playerInventory.myInventory[i].numberHeld > 0 ||
                    playerInventory.myInventory[i].itemName == "Bottle")
                {
                    GameObject temp =
                        Instantiate(blankInventorySlot, inventoryPanel.transform.position, Quaternion.identity);

                    temp.transform.SetParent(inventoryPanel.transform);

                    InventorySlot newSlot = temp.GetComponent<InventorySlot>();
                    if (newSlot)
                    {
                        newSlot.Setup(playerInventory.myInventory[i], this);
                    }
                }
            }
        }
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        ClearInventorySlots();
        MakeInventorySlots();
        SetTextAndButton("", false);
    }

    public void SetupDescriptionAndButton(string newDescriptionString,Sprite itemSprite,
        bool isButtonUsable,  bool isButtonDiscardable, InventoryItem newItem)
    {
        optionPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(useButton);
        currentItem = newItem;
        descriptionText.text = newDescriptionString;
        useButton.SetActive(isButtonUsable);
        discardButton.SetActive(isButtonDiscardable);
        itemImage.sprite = itemSprite;
    }
    public void CancelButtonPressd()
    {
        optionPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(decoyButton);
    }
    void ClearInventorySlots()
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
    }

    public void UseButtonPressd()
    {
        if (currentItem)
        {
            currentItem.Use();
            ClearInventorySlots();
            MakeInventorySlots();
            if (currentItem.numberHeld == 0)
            {
                SetTextAndButton("", false);
            }
        }
    }

    public void DiscardButtonPressd()
    {
        if (currentItem)
        {
            currentItem.Discard();
            ClearInventorySlots();
            MakeInventorySlots();
            if (currentItem.numberHeld == 0)
            {
                SetTextAndButton("", false);
            }
        }
    }

}
