using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMoveMonster : MonoBehaviour
{
    public MonsterMovement character;
    public BattleZone character2;
    private void Start()
    {
        character2 = GameObject.Find("BattleZoneManager").GetComponent<BattleZone>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "InteractTile")
        {
            character.DontGo();
            character2.DontGoE();

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "InteractTile")
        {
            character.DontGo();
            character2.DontGoE();

        }
    }


}
