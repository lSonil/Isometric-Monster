using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedItemManager : MonoBehaviour
{

    [Header("Inventory Information")]
    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject equipeItemPanel;


    public void MakeInventorySlots()
    {

                GameObject temp =
                    Instantiate(blankInventorySlot, equipeItemPanel.transform.position, Quaternion.identity);

                temp.transform.SetParent(equipeItemPanel.transform);

                InventorySlot newSlot = temp.GetComponent<InventorySlot>();

            
        
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        ClearInventorySlots();
        MakeInventorySlots();
    }

    void ClearInventorySlots()
    {
        for (int i = 0; i < equipeItemPanel.transform.childCount; i++)
        {
            Destroy(equipeItemPanel.transform.GetChild(i).gameObject);
        }
    }


}
