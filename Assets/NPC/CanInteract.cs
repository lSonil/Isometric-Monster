using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanInteract : MonoBehaviour
{
    public CharacterMovement character;
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Interactable")
        {
            character.interactable = true;
            character.objectToInteract = collision.gameObject;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag != "Stair" && collision.gameObject.tag != "FightTile" && collision.gameObject.tag != "Portal")
        {
            character.DontGo();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Interactable")
        {
            character.interactable = false;
            character.objectToInteract = null;
        }

    }

}
