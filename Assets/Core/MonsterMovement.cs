using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [Header("Directions")]

    public float dH; 
    public float dV;

    [Header("Movement")]
    public float stopDistance;
    public Transform player;

    [SerializeField] Vector3 targetPosition;
    [SerializeField] public float step;

    bool moving;



    [Header("Patern")]
    public bool random;
    [SerializeField] public int[] posibbleDirections;
    public int direction;
    public float actualSpeed;
    public bool canMove = true;
    int spot = 0;


    public GameObject checkMovable;
    public int posItIs;
    public int posCantBe;
    [Header("Monster")]

    [SerializeField] public TeamMonster monster;

    [SerializeField] public string name;

    [SerializeField] public float maxHp;
    [SerializeField] public float currentHp;
    [SerializeField] public float attack;

    [SerializeField] public float defense;

    [SerializeField] public float spAttack;

    [SerializeField] public float spDefence;

    [SerializeField] public float speed;
    [SerializeField] public Condition condition;
    [SerializeField] public Sprite[] face;
    [SerializeField] public MonsterAttacks[] moves;
    [SerializeField] public int lvl;

    // Start is called before the first frame update
    void Start()
    {
        AtStart();

    }
    public void AtStart()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = monster.monsterImage;
        name = monster.monsterName;
        maxHp = monster.hp;
        currentHp = monster.hp;
        attack = monster.attack;
        defense = monster.defense;
        spAttack = monster.spAttack;
        spDefence = monster.spDefence;
        speed = monster.speed;
        if (GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        face[0] = monster.face[0];
        face[1] = monster.face[1];
        face[2] = monster.face[2];
        face[3] = monster.face[3];

    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            if (BattleZone.battleStart == false && BattleZone.battleOn == false && this.gameObject.GetComponent<SpriteRenderer>().sortingOrder == GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sortingOrder)
            {
                if (Time.timeScale != 0f)
                {
                    if (actualSpeed <= 0)
                    {
                        ChooseDirection();
                        actualSpeed = 1 / speed;
                        canMove = true;
                    }
                    else
                    {
                        actualSpeed -= Time.deltaTime;
                    }

                    if (moving == false && canMove == true)
                    {
                        if (posibbleDirections[direction] == 0 && posCantBe != 0)
                        {
                            if (gameObject.GetComponent<SpriteRenderer>().sprite == face[0])
                            {
                                targetPosition = transform.position + new Vector3(-dH, dV, 0);
                                checkMovable.transform.position = transform.position + new Vector3(-dH, dV - 0.2f, 0);
                                moving = true; canMove = false; return;
                            }
                            else
                            {
                                posItIs = 0; posCantBe = 5;
                                checkMovable.transform.position = transform.position + new Vector3(-dH, dV - 0.2f, 0);
                                gameObject.GetComponent<SpriteRenderer>().sprite = face[0];
                                return;
                            }
                        }

                        if (posibbleDirections[direction] == 2 && posCantBe != 2)
                        {
                            if (gameObject.GetComponent<SpriteRenderer>().sprite == face[2])
                            {
                                targetPosition = transform.position + new Vector3(dH, -dV, 0);
                                checkMovable.transform.position = transform.position + new Vector3(dH - 0.1f, -dV - 0.1f, 0);

                                moving = true; canMove = false; return;
                            }
                            else
                            {
                                posItIs = 2; posCantBe = 5;
                                checkMovable.transform.position = transform.position + new Vector3(dH - 0.1f, -dV - 0.1f, 0);
                                gameObject.GetComponent<SpriteRenderer>().sprite = face[2];
                                return;
                            }
                        }

                        if (posibbleDirections[direction] == 1 && posCantBe != 1)
                        {
                            if (gameObject.GetComponent<SpriteRenderer>().sprite == face[1])
                            {
                                targetPosition = transform.position + new Vector3(dH, dV, 0);
                                checkMovable.transform.position = transform.position + new Vector3(dH, dV - 0.2f, 0);

                                moving = true; canMove = false; return;
                            }
                            else
                            {
                                posItIs = 1; posCantBe = 5;
                                checkMovable.transform.position = transform.position + new Vector3(dH, dV - 0.2f, 0);
                                gameObject.GetComponent<SpriteRenderer>().sprite = face[1];
                                return;
                            }
                        }

                        if (posibbleDirections[direction] == 3 && posCantBe != 3)
                        {
                            if (gameObject.GetComponent<SpriteRenderer>().sprite == face[3])
                            {
                                targetPosition = transform.position + new Vector3(-dH, -dV, 0);
                                checkMovable.transform.position = transform.position + new Vector3(-dH + 0.1f, -dV - 0.1f, 0);
                                moving = true; canMove = false; return;
                            }
                            else
                            {
                                posItIs = 3; posCantBe = 5;
                                checkMovable.transform.position = transform.position + new Vector3(-dH + 0.1f, -dV - 0.1f, 0);
                                gameObject.GetComponent<SpriteRenderer>().sprite = face[3];
                                return;
                            }
                        }

                    }


                    if (moving == true)
                    {

                        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                        if (targetPosition == transform.position)
                        {
                            moving = false;

                        }
                    }

                }



                //battle
                if (player.gameObject.GetComponent<CharacterMovement>().moving == false && moving == false && player.gameObject.GetComponent<CharacterMovement>().inMenu == false)
                    if (Vector3.Distance(transform.position, player.position) < stopDistance)
                    {
                        BattleZone.battleStart = true;
                        BattleZone.enemy = gameObject;
                    }



            }
        }
    }
    public void DontGo()
    {
        posCantBe = posItIs;
    }
    public void ChooseDirection()
    {
        if (random == true)
        {
            direction = Random.Range(0, posibbleDirections.Length);
        }
        else
        {
            direction = spot;
            spot++;
            if (spot == posibbleDirections.Length)
                spot = 0;

        }
    }
}
   



