using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerwithPlayer : MonoBehaviour
{
   
    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectWithTag("Player")!=null)
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sortingOrder;
    else
            this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<SpriteRenderer>().sortingOrder;
    }
}
