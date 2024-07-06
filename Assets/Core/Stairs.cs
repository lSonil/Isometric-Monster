using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public GameObject bot;
    public GameObject top;
    GameObject player;
    public bool check;
    // Start is called before the first frame update
    void Start()
    {
        bot.GetComponent<Renderer>().enabled = false;
        top.GetComponent<Renderer>().enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position == top.transform.position && check==false)
        {
            player.gameObject.GetComponent<CharacterMovement>().moving = true;
            player.gameObject.GetComponent<CharacterMovement>().targetPosition = bot.transform.position;
            check = true;
            player.transform.GetComponent<SpriteRenderer>().sortingOrder--;

        }
        if (player.transform.position == bot.transform.position && check == false)
        {
            player.gameObject.GetComponent<CharacterMovement>().moving = true;
            player.gameObject.GetComponent<CharacterMovement>().targetPosition = top.transform.position;
            check = true;
            player.transform.GetComponent<SpriteRenderer>().sortingOrder++;
        }
        if(check==true)
            if(Vector2.Distance(player.transform.position, bot.transform.position) > 0.5f && Vector2.Distance(player.transform.position, top.transform.position)>0.5f)
                check = false;
    }

}
