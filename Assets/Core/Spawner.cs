using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject monsterBody;
    public TeamMonster[] monsterToSpawn;
    public int[] lvlMax;
    public int[] lvlMin;

    public int chances;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            int chance = Random.RandomRange(0, chances);
            if (chance == 0)
            {
                GameObject child = Instantiate(monsterBody);
                int i = Random.RandomRange(0, monsterToSpawn.Length);
                child.GetComponent<MonsterMovement>().monster=monsterToSpawn[i];
                child.GetComponent<MonsterMovement>().lvl= Random.RandomRange(lvlMax[i], lvlMin[i]);
                child.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
                child.transform.parent = gameObject.transform;
                child.transform.position = gameObject.transform.position;
            }
        }
    }
}
