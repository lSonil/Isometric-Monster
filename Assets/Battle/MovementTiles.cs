using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTiles : MonoBehaviour
{
    public int i;
    public int j;
    public int pointsCost;

    int layers;
    // Start is called before the first frame update
    void Start()
    {
        layers = GameObject.Find("GameManager").GetComponent<BattleZone>().layers;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = layers;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "PlayerMonster")
        {
            Destroy(gameObject);
        }
        
    }
}
