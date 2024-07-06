using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMoveCharacter : MonoBehaviour
{
    public Character character;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "InteractTile"&& collision.gameObject.tag != "FightTile")
        {
            character.DontGo();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "InteractTile" && collision.gameObject.tag != "FightTile")
        {
            character.DontGo();
        }
    }


}
