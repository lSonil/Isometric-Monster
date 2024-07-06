using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capture : MonoBehaviour
{
    public void Update()
    {
        
    }
    public void CaptureMonster()
    {
        if (BattleZone.battleOn == true&& BattleZone.enemyTrainer == null)
        {
            GameObject.Find("BattleZoneManager").GetComponent<BattleZone>().ShowMenu();
            GameObject.Find("BattleZoneManager").GetComponent<BattleZone>().ShowMenu();
            GameObject.Find("BattleZoneManager").GetComponent<BattleZone>().container = gameObject;
            GameObject.Find("BattleZoneManager").GetComponent<BattleZone>().tryToCatch = true;



        }
        else
        {
            Debug.Log("YOU CANT AAAAA");
        }

    }
}
