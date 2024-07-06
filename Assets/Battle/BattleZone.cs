using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BattleZone : MonoBehaviour
{
    [Header("Trainer")]

    public static GameObject enemyTrainer;
    public int enemyMonsterInParty=0;
    public  GameObject monsterBod;
    public Monster newMonsterBody;

    [Header("Directions")]
    public float dH;
    public float dV;
    public TeamManager team;
    public GameObject monster;
    public GameObject player;
    public GameObject battlePanel;
    public GameObject playerInfo;
    public GameObject enemyInfo;
    public GameObject monsterBody;

    [Header("Battle")]
    public static bool battleOn = false;
    public static bool battleStart = false;
    public GameObject container;
    public GameObject containerBody;

    public float statusBonus=1;
    public bool tryToCatch;
    public bool caughtIs;

    [Header("Attacks")]
    public Button attackButton;
    public TextMeshProUGUI[] attack;
    public TextMeshProUGUI[] type;
    public TextMeshProUGUI[] pp;
    public GameObject[] attackSlot;
    public Button[] buttons;
    public GameObject placeHolderTile;
    public int attackNumber;
    public GameObject[] attackPosition;
    public bool enemyAttacked = false;
    public GameObject selectableMonster;

    [Header("Move")]
    public int layers;
    public int monsterOutNumber;
    public int posItIsP;
    public int posCantBeP;
    public int posItIsE;
    public int posCantBeE;
    public GameObject arrow;
    public GameObject arrowBody;
    public Vector3 arroPosE;
    public GameObject checkMovableP;
    public GameObject checkMovableE;

    [Header("UI")]
    [SerializeField] bool mustChange;

    public GameObject[] otherMenus;

    public GameObject battleFirstButton, attackFirstButton, inventoryFirstButton, teamFirstButton;
    [Header("Info")]
    [SerializeField] public TextMeshProUGUI monsterName;
    [SerializeField] public Image fillImage;
    [SerializeField] public Slider slider;
    [SerializeField] public TextMeshProUGUI hpValue;
    [SerializeField] public Slider expSlider;
    [SerializeField] public TextMeshProUGUI expValue;
    [SerializeField] public bool checkPlayerCondition = false;
    [SerializeField] public bool checkEnemyCondition = false;
    [SerializeField] public Image enemyCondition;
    [SerializeField] public Image playerCondition;
    [SerializeField] public Sprite[] conditions;
    [SerializeField] public TextMeshProUGUI lvl;

    [Header("Enemy")]
    public TextMeshProUGUI enemyMonsterName;
    public Slider enemySlider;
    public TextMeshProUGUI enemyHpValue;
    public static GameObject enemy;
    public int monsterMoveUsed = 0;
    public TextMeshProUGUI enemyLvl;



    [Header("Text")]
    [SerializeField] GameObject dialogBox;
    [SerializeField] BattleDialogBox dialogBoxText;

    [Header("Boosts")]
    public float attackBP;
    public float defenceBP;
    public float spDefenceBP;
    public float spAttackBP;
    public float speedBP;
    public float attackBE;
    public float defenceBE;
    public float spDefenceBE;
    public float spAttackBE;
    public float speedBE;



    [Header("Movement Player")]
    [SerializeField] float speed = 0.25f;
    public Vector3 targetPosition;
    Vector3 startPosition;

    public bool moving;
    [SerializeField] float slowSnapDistance;
    [SerializeField] float snapDistance;



    [Header("Movement Enemy")]
    [SerializeField] Vector3 targetPositionE;
    public bool movingE;
    [SerializeField] private Vector3 lastPositionE;
    public int directionE;
    public float speedE;
    public float actualSpeed;
    public bool canMoveE;


    [Header("Conditions Player")]

    public float poisonClock;
    float actualPoisonClock;
    public float burnClock;
    float actualBurnClock;
    public float sleepClock;
    float actualSleepClock;
    public float iceClock;
    float actualIceClock;
    public float cursClock;
    float actualCursClock;

    [Header("Conditions Enemy")]
    public float poisonClockE;
    float actualPoisonClockE;
    public float burnClockE;
    float actualBurnClockE;
    public float sleepClockE;
    float actualSleepClockE;
    public float iceClockE;
    float actualIceClockE;
    public float cursClockE;
    float actualCursClockE;
    // Update is called once per frame
    private void Start()
    {
        poisonClockE = poisonClock;
        burnClockE = burnClock;
        sleepClockE = sleepClock;
        iceClockE = iceClock;
        cursClockE = cursClock;
    }
    void Update()
    {
        if (battleStart == true)
        {
            if (enemyTrainer == null)
            {
                dialogBox.SetActive(true);
                dialogBoxText.SetDialog($"A wild {enemy.GetComponent<MonsterMovement>().name} attacked!");

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    dialogBox.SetActive(false);
                    bool found = false;
                    for (int i = 0; i < team.playerTeam.myTeam.Count; i++)
                    {
                        if (team.playerTeam.myTeam[i].currentHp > 0 && found == false)
                        {
                            found = true;
                            BattleStart(i);
                        }
                    }
                    enemyInfo.SetActive(true);
                    playerInfo.SetActive(true);

                    checkMovableE = enemy.transform.GetChild(0).gameObject;

                    targetPosition = player.transform.position;
                    player.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(null);

                    EventSystem.current.SetSelectedGameObject(battleFirstButton);
                    battleOn = true;

                }
            }
            else
            {
                if (DialogManager.dialogOn == false)
                {
                    EnemyTrainerSeUp();

                    enemyInfo.SetActive(true);
                    playerInfo.SetActive(true);


                    SetUpEnemy();
                    SetUpPlayer();
                    dialogBox.SetActive(false);
                    bool found = false;
                    for (int i = 0; i < team.playerTeam.myTeam.Count; i++)
                    {
                        if (team.playerTeam.myTeam[i].currentHp > 0 && found == false)
                        {
                            found = true;
                            BattleStart(i);
                        }
                    }

                    checkMovableP = GameObject.FindGameObjectWithTag("PlayerMonster").transform.GetChild(0).gameObject;
                    checkMovableE = enemy.transform.GetChild(0).gameObject;

                    targetPosition = player.transform.position;
                    enemyTrainer.SetActive(false);
                    player.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(battleFirstButton);

                    battleOn = true;
                }

            }
        }
        if (battleOn == true)
        {
            if (team.playerTeam.myTeam[monsterOutNumber].currentHp < 0)
                team.playerTeam.myTeam[monsterOutNumber].currentHp = 0;
            bool allDown = true;
            for (int i = 0; i < team.playerTeam.myTeam.Count; i++)
            {
                if (team.playerTeam.myTeam[i].currentHp > 0)
                {
                    allDown = false;
                }

            }
            if (selectableMonster.transform.childCount != 0)
                for (int i = 0; i < team.playerTeam.myTeam.Count; i++)
                {
                   // if (team.playerTeam.myMonster[i].currentHp > 0)
                 //   {
                 //       ButtonRefreshed(selectableMonster.transform.GetChild(i).GetComponent<Button>());
                 //   }
                 //   else
                 ///   {
                  //      ButtonUsed(selectableMonster.transform.GetChild(i).GetComponent<Button>());
                 //   }
                }


            if (allDown == true)
            {
                battlePanel.SetActive(false);



                dialogBox.SetActive(true);
                dialogBoxText.SetDialog($"ALL of your monsters are down!You lost");
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    playerInfo.SetActive(false);
                    enemyInfo.SetActive(false);
                    Run();
                    return;
                }
                return;

            }
            if(caughtIs == true)
            {
                battlePanel.SetActive(false);
                dialogBox.SetActive(true);
                dialogBoxText.SetDialog($"You caught a wild {enemy.GetComponent<MonsterMovement>().name}, neet!");
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    playerInfo.SetActive(false);
                    enemyInfo.SetActive(false);

                    Run();
                    return;
                }
                return;
            }
            if (enemy.GetComponent<MonsterMovement>().currentHp <= 0)
            {
                if (enemyTrainer == null)
                {
                    battlePanel.SetActive(false);
                    dialogBox.SetActive(true);
                    dialogBoxText.SetDialog($"The wild {enemy.GetComponent<MonsterMovement>().name} fainted!You won!");
                    if (Input.GetKeyDown(KeyCode.Z))
                    {                   
                        team.playerTeam.myTeam[monsterOutNumber].exp = team.playerTeam.myTeam[monsterOutNumber].exp + (10 / enemy.GetComponent<MonsterMovement>().monster.catchRate);
                        if(team.playerTeam.myTeam[monsterOutNumber].exp> team.playerTeam.myTeam[monsterOutNumber].expNeeded)
                        {
                            team.playerTeam.myTeam[monsterOutNumber].expNeeded = team.playerTeam.myTeam[monsterOutNumber].exp - team.playerTeam.myTeam[monsterOutNumber].expNeeded;
                            team.playerTeam.myTeam[monsterOutNumber].lvl++;
                        }

                        playerInfo.SetActive(false);
                        enemyInfo.SetActive(false);

                        Run();
                        return;
                    }
                    return;
                }
                else

                {
                    enemyMonsterInParty++;
                    if (enemyMonsterInParty < enemyTrainer.GetComponent<TrainerControler>().monstersInParty.Length)
                    {
                        team.playerTeam.myTeam[monsterOutNumber].exp = team.playerTeam.myTeam[monsterOutNumber].exp + (10 / enemy.GetComponent<MonsterMovement>().monster.catchRate);
                        if (team.playerTeam.myTeam[monsterOutNumber].exp > team.playerTeam.myTeam[monsterOutNumber].expNeeded)
                        {
                            team.playerTeam.myTeam[monsterOutNumber].exp = team.playerTeam.myTeam[monsterOutNumber].exp - team.playerTeam.myTeam[monsterOutNumber].expNeeded;
                            team.playerTeam.myTeam[monsterOutNumber].lvl++;
                        }
                        EnemyTrainerSeUp();
                    }
                    else
                    {
                        battlePanel.SetActive(false);
                        dialogBox.SetActive(true);
                        dialogBoxText.SetDialog($"The trainer  ran out of monsters!!");
                        if (Input.GetKeyDown(KeyCode.Z))
                        {
                            team.playerTeam.myTeam[monsterOutNumber].exp = team.playerTeam.myTeam[monsterOutNumber].exp + (10 / enemy.GetComponent<MonsterMovement>().monster.catchRate);
                            if (team.playerTeam.myTeam[monsterOutNumber].exp > team.playerTeam.myTeam[monsterOutNumber].expNeeded)
                            {
                                team.playerTeam.myTeam[monsterOutNumber].exp = team.playerTeam.myTeam[monsterOutNumber].exp - team.playerTeam.myTeam[monsterOutNumber].expNeeded;
                                team.playerTeam.myTeam[monsterOutNumber].lvl++;
                            }
                            playerInfo.SetActive(false);
                            enemyInfo.SetActive(false);

                            Run();
                            return;
                        }
                        return;
                    }
                }
            }
            if (team.playerTeam.myTeam[monsterOutNumber].currentHp == 0)
            {
                if (mustChange == false)
                {

                    ShowMenu();
                    battlePanel.SetActive(false);
                    otherMenus[2].SetActive(true);
                    SetBattleToTeam();
                    mustChange = true;
                    return;

                }
                else
                {
                    return;
                }
            }



            SetUpEnemy();
            SetUpPlayer();
            ButtonsPPCheck();



            if (tryToCatch == false)
            {

                if (Input.GetKeyDown(KeyCode.X) && dialogBox.activeInHierarchy == false)

                {
                    ShowMenu();
                }
                if (Input.GetKeyDown(KeyCode.Z) && Time.timeScale == 1f && dialogBox.activeInHierarchy == false && team.playerTeam.myTeam[monsterOutNumber].monsterCondition != Condition.Asleep && team.playerTeam.myTeam[monsterOutNumber].monsterCondition != Condition.Frozen)

                {
                    if (attackNumber != 5)
                    {
                        arrowBody = Instantiate(arrow);
                        arrowBody.transform.position = checkMovableP.transform.position;
                        team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].theMove.Invoke();
                        team.playerTeam.myTeam[monsterOutNumber].pp[attackNumber]--;
                        Destroy(arrowBody);
                        if (team.playerTeam.myTeam[monsterOutNumber].pp[attackNumber] <= 0)
                        {
                            for (int i = 3; i >= 0; i--)
                            {
                                if (team.playerTeam.myTeam[monsterOutNumber].pp[attackNumber] > 0)
                                {
                                    attackNumber = i;
                                    return;
                                }

                            }
                        }
                    }
                }



                if (Time.deltaTime != 0f && dialogBox.activeInHierarchy == false)
                {
                    if (enemy.GetComponent<MonsterMovement>().condition == Condition.None)
                    {
                        statusBonus = 1f;
                    }

                    if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Poison)
                    {
                        if (actualPoisonClock <= 0)
                        {
                            float d = 5 * ((float)5 / (team.playerTeam.myTeam[monsterOutNumber].spDefence + team.playerTeam.myTeam[monsterOutNumber].spDefence + spDefenceBP)) + 2;
                            int theDamage = Mathf.FloorToInt(d);
                            team.playerTeam.myTeam[monsterOutNumber].currentHp = team.playerTeam.myTeam[monsterOutNumber].currentHp - theDamage;
                            actualPoisonClock = poisonClock;

                        }
                        else
                        {
                            actualPoisonClock -= Time.deltaTime;
                        }
                    }
                    if (enemy.GetComponent<MonsterMovement>().condition == Condition.Poison)
                    {
                        if (actualPoisonClockE <= 0)
                        {
                            float d = 5 * ((float)5 / (enemy.GetComponent<MonsterMovement>().spDefence + spDefenceBE)) + 2;
                            int theDamage = Mathf.FloorToInt(d);
                            enemy.GetComponent<MonsterMovement>().currentHp = enemy.GetComponent<MonsterMovement>().currentHp - theDamage;
                            actualPoisonClockE = poisonClockE;

                        }
                        else
                        {
                            actualPoisonClockE -= Time.deltaTime;
                        }
                    }
                    if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Burn)
                    {
                        if (actualBurnClock <= 0)
                        {
                            float d = 5 * ((float)5 / (team.playerTeam.myTeam[monsterOutNumber].spAttack + team.playerTeam.myTeam[monsterOutNumber].spAttack + spAttackBP)) + 2;
                            int theDamage = Mathf.FloorToInt(d);
                            team.playerTeam.myTeam[monsterOutNumber].currentHp = team.playerTeam.myTeam[monsterOutNumber].currentHp - theDamage;
                            actualBurnClock = burnClock;

                        }
                        else
                        {
                            actualBurnClock -= Time.deltaTime;
                        }
                    }
                    if (enemy.GetComponent<MonsterMovement>().condition == Condition.Burn)
                    {
                        if (actualBurnClockE <= 0)
                        {
                            float d = 5 * ((float)5 / (enemy.GetComponent<MonsterMovement>().spAttack + spAttackBE)) + 2;
                            int theDamage = Mathf.FloorToInt(d);
                            enemy.GetComponent<MonsterMovement>().currentHp = enemy.GetComponent<MonsterMovement>().currentHp - theDamage;
                            actualBurnClockE = burnClockE;

                        }
                        else
                        {
                            actualBurnClockE -= Time.deltaTime;
                        }
                    }
                    if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Cursed)
                    {
                        if (actualCursClock <= 0)
                        {
                            team.playerTeam.myTeam[monsterOutNumber].currentHp = team.playerTeam.myTeam[monsterOutNumber].currentHp - ((team.playerTeam.myTeam[monsterOutNumber].maxHp + team.playerTeam.myTeam[monsterOutNumber].myMonster.hp) / 4);
                            actualCursClock = burnClock;

                        }
                        else
                        {
                            actualCursClock -= Time.deltaTime;
                        }
                    }
                    if (enemy.GetComponent<MonsterMovement>().condition == Condition.Cursed)
                    {
                        if (actualCursClockE <= 0)
                        {
                            enemy.GetComponent<MonsterMovement>().currentHp = enemy.GetComponent<MonsterMovement>().currentHp - (enemy.GetComponent<MonsterMovement>().maxHp / 4);
                            actualCursClockE = burnClockE;

                        }
                        else
                        {
                            actualCursClockE -= Time.deltaTime;
                        }
                    }
                    if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Asleep)
                    {
                        if (actualSleepClock <= 0)
                        {
                            int i = Random.Range(0, 3);
                            if (i == 0)
                                team.playerTeam.myTeam[monsterOutNumber].monsterCondition = Condition.None;
                            actualSleepClock = sleepClock;

                        }
                        else
                        {
                            actualSleepClock -= Time.deltaTime;
                        }
                    }
                    if (enemy.GetComponent<MonsterMovement>().condition == Condition.Asleep)
                    {
                        if (actualSleepClockE <= 0)
                        {
                            int i = Random.Range(0, 3);
                            if (i == 0)
                                enemy.GetComponent<MonsterMovement>().condition = Condition.None;
                            actualSleepClockE = sleepClockE;

                        }
                        else
                        {
                            actualSleepClockE -= Time.deltaTime;
                        }
                    }
                    if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Frozen)
                    {
                        if (actualIceClock <= 0)
                        {
                            int i = Random.Range(0, 3);
                            if (i == 0)
                                team.playerTeam.myTeam[monsterOutNumber].monsterCondition = Condition.None;
                            actualIceClock = iceClock;

                        }
                        else
                        {
                            actualIceClock -= Time.deltaTime;
                        }
                    }
                    if (enemy.GetComponent<MonsterMovement>().condition == Condition.Frozen)
                    {
                        if (actualIceClockE <= 0)
                        {
                            int i = Random.Range(0, 3);
                            if (i == 0)
                                enemy.GetComponent<MonsterMovement>().condition = Condition.None;
                            actualIceClockE = iceClockE;

                        }
                        else
                        {
                            actualIceClockE -= Time.deltaTime;
                        }
                    }






                    if (enemy.GetComponent<MonsterMovement>().condition != Condition.Asleep && enemy.GetComponent<MonsterMovement>().condition != Condition.Frozen)
                    {
                        if (actualSpeed <= 0)
                        {
                            ChooseDirection();
                            actualSpeed = 1 / enemy.GetComponent<MonsterMovement>().speed;
                            canMoveE = true;
                        }
                        else
                        {
                            actualSpeed -= Time.deltaTime;
                        }

                        if (movingE == false && canMoveE == true)
                        {
                            if (enemy.GetComponent<MonsterMovement>().posibbleDirections[directionE] == 0 && posCantBeE != 0)
                            {
                                if (enemy.GetComponent<SpriteRenderer>().sprite == enemy.GetComponent<MonsterMovement>().face[0])
                                {
                                    targetPositionE = enemy.transform.position + new Vector3(-dH, dV, 0);
                                    checkMovableE.transform.position = enemy.transform.position + new Vector3(-dH, dV - 0.2f, 0);
                                    movingE = true; canMoveE = false; return;
                                }
                                else
                                {
                                    posItIsE = 0; posCantBeE = 5;
                                    checkMovableE.transform.position = enemy.transform.position + new Vector3(-dH, dV - 0.2f, 0);
                                    enemy.GetComponent<SpriteRenderer>().sprite = enemy.GetComponent<MonsterMovement>().face[0];
                                    return;
                                }
                            }

                            if (enemy.GetComponent<MonsterMovement>().posibbleDirections[directionE] == 2 && posCantBeE != 2)
                            {
                                if (enemy.GetComponent<SpriteRenderer>().sprite == enemy.GetComponent<MonsterMovement>().face[2])
                                {
                                    targetPositionE = enemy.transform.position + new Vector3(dH, -dV, 0);
                                    checkMovableE.transform.position = enemy.transform.position + new Vector3(dH - 0.1f, -dV - 0.1f, 0);

                                    movingE = true; canMoveE = false; return;
                                }
                                else
                                {
                                    posItIsE = 2; posCantBeE = 5;
                                    checkMovableE.transform.position = enemy.transform.position + new Vector3(dH - 0.1f, -dV - 0.1f, 0);
                                    enemy.GetComponent<SpriteRenderer>().sprite = enemy.GetComponent<MonsterMovement>().face[2];
                                    return;
                                }
                            }

                            if (enemy.GetComponent<MonsterMovement>().posibbleDirections[directionE] == 1 && posCantBeE != 1)
                            {
                                if (enemy.GetComponent<SpriteRenderer>().sprite == enemy.GetComponent<MonsterMovement>().face[1])
                                {
                                    targetPositionE = enemy.transform.position + new Vector3(dH, dV, 0);
                                    checkMovableE.transform.position = enemy.transform.position + new Vector3(dH, dV - 0.2f, 0);

                                    movingE = true; canMoveE = false; return;
                                }
                                else
                                {
                                    posItIsE = 1; posCantBeE = 5;
                                    checkMovableE.transform.position = enemy.transform.position + new Vector3(dH, dV - 0.2f, 0);
                                    enemy.GetComponent<SpriteRenderer>().sprite = enemy.GetComponent<MonsterMovement>().face[1];
                                    return;
                                }
                            }

                            if (enemy.GetComponent<MonsterMovement>().posibbleDirections[directionE] == 3 && posCantBeE != 3)
                            {
                                if (enemy.GetComponent<SpriteRenderer>().sprite == enemy.GetComponent<MonsterMovement>().face[3])
                                {
                                    targetPositionE = enemy.transform.position + new Vector3(-dH, -dV, 0);
                                    checkMovableE.transform.position = enemy.transform.position + new Vector3(-dH + 0.1f, -dV - 0.1f, 0);
                                    movingE = true; canMoveE = false; return;
                                }
                                else
                                {
                                    posItIsE = 3; posCantBeE = 5;
                                    checkMovableE.transform.position = enemy.transform.position + new Vector3(-dH + 0.1f, -dV - 0.1f, 0);
                                    enemy.GetComponent<SpriteRenderer>().sprite = enemy.GetComponent<MonsterMovement>().face[3];
                                    return;
                                }
                            }

                        }


                        if (movingE == true)
                        {

                            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPositionE, enemy.GetComponent<MonsterMovement>().step);

                            if (targetPositionE == enemy.transform.position)
                            {
                                movingE = false;

                            }
                        }

                    }








                    if (GameObject.FindGameObjectWithTag("PlayerMonster") != null && Time.timeScale == 1f)
                    {
                        if (moving)
                        {

                            if (Vector2.Distance(startPosition, transform.position) > snapDistance)
                            {
                                GameObject.FindGameObjectWithTag("PlayerMonster").transform.position = targetPosition;
                                moving = false;

                            }
                            GameObject.FindGameObjectWithTag("PlayerMonster").transform.position += (targetPosition - startPosition) * speed * Time.deltaTime;
                            return;
                        }
                        else
                        {
                            GameObject.FindGameObjectWithTag("PlayerMonster").transform.position = targetPosition;
                        }

                        if (Input.GetKey(KeyCode.W) && posCantBeP != 1)
                        {

                            if (GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<SpriteRenderer>().sprite == team.playerTeam.myTeam[monsterOutNumber].myMonster.face[1])
                            {
                                targetPosition = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(dH, dV, 0);
                                startPosition = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position;
                                checkMovableP.transform.position = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(dH, dV - 0.2f, 0);
                                moving = true;
                            }
                            else
                            {
                                posItIsP = 1; posCantBeP = 5;
                                checkMovableP.transform.position = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(dH, dV - 0.2f, 0);
                                GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<SpriteRenderer>().sprite = team.playerTeam.myTeam[monsterOutNumber].myMonster.face[1];
                                return;
                            }

                        }
                        else if (Input.GetKey(KeyCode.S) && posCantBeP != 3)
                        {

                            if (GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<SpriteRenderer>().sprite == team.playerTeam.myTeam[monsterOutNumber].myMonster.face[3])
                            {
                                checkMovableP.transform.position = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(-dH + 0.1f, -dV - 0.1f, 0);
                                targetPosition = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(-dH, -dV, 0);
                                startPosition = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position;
                                moving = true;
                            }
                            else
                            {
                                posItIsP = 3; posCantBeP = 5;
                                checkMovableP.transform.position = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(-dH + 0.1f, -dV - 0.1f, 0);
                                GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<SpriteRenderer>().sprite = team.playerTeam.myTeam[monsterOutNumber].myMonster.face[3];
                            }

                        }
                        else if (Input.GetKey(KeyCode.A) && posCantBeP != 0)
                        {

                            if (GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<SpriteRenderer>().sprite == team.playerTeam.myTeam[monsterOutNumber].myMonster.face[0])
                            {
                                checkMovableP.transform.position = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(-dH, dV - 0.2f, 0);
                                targetPosition = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(-dH, dV, 0);
                                startPosition = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position;
                                moving = true;
                            }
                            else
                            {
                                posItIsP = 0;
                                posCantBeP = 5;
                                checkMovableP.transform.position = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(-dH, dV - 0.2f, 0);
                                GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<SpriteRenderer>().sprite = team.playerTeam.myTeam[monsterOutNumber].myMonster.face[0];
                                return;
                            }

                        }

                        else if (Input.GetKey(KeyCode.D) && posCantBeP != 2)
                        {

                            if (GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<SpriteRenderer>().sprite == team.playerTeam.myTeam[monsterOutNumber].myMonster.face[2])
                            {
                                targetPosition = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(dH, -dV, 0);
                                checkMovableP.transform.position = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(dH - 0.1f, -dV - 0.1f, 0);
                                startPosition = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position;
                                moving = true;
                            }
                            else
                            {
                                posItIsP = 2; posCantBeP = 5;

                                checkMovableP.transform.position = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position + new Vector3(dH - 0.1f, -dV - 0.1f, 0);
                                GameObject.FindGameObjectWithTag("PlayerMonster").GetComponent<SpriteRenderer>().sprite = team.playerTeam.myTeam[monsterOutNumber].myMonster.face[2];
                                return;
                            }

                        }
                    }

                }

            }
            else
            {
                if (containerBody == null)
                {
                    containerBody = Instantiate(container);
                    containerBody.transform.position = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position;

                }
                else
                {
                    containerBody.transform.position += (enemy.transform.position - containerBody.transform.position) * 8f * Time.deltaTime;
                    if (Vector2.Distance(enemy.transform.position, containerBody.transform.position) < 0.3)
                    {
                        containerBody.transform.position = enemy.transform.position;

                    }
                    if(containerBody.transform.position == enemy.transform.position)
                    {
                        float caught = (3 * enemy.GetComponent<MonsterMovement>().maxHp - 2 * enemy.GetComponent<MonsterMovement>().currentHp) *Random.Range(0,10) * statusBonus / 3 * enemy.GetComponent<MonsterMovement>().maxHp* enemy.GetComponent<MonsterMovement>().monster.catchRate;
                        if (caught >=255)
                        {
                            caughtIs = true;
                            team.playerTeam.AddMonster(enemy.GetComponent<MonsterMovement>().lvl,enemy.GetComponent<MonsterMovement>().monster, newMonsterBody,
                                 enemy.GetComponent<MonsterMovement>().name, enemy.GetComponent<MonsterMovement>().maxHp, enemy.GetComponent<MonsterMovement>().currentHp, enemy.GetComponent<MonsterMovement>().attack, enemy.GetComponent<MonsterMovement>().defense, enemy.GetComponent<MonsterMovement>().spAttack, enemy.GetComponent<MonsterMovement>().spDefence, enemy.GetComponent<MonsterMovement>().speed, enemy.GetComponent<MonsterMovement>().condition,
                                 enemy.GetComponent<MonsterMovement>().moves[0].pp, enemy.GetComponent<MonsterMovement>().moves[1].pp, enemy.GetComponent<MonsterMovement>().moves[2].pp, enemy.GetComponent<MonsterMovement>().moves[3].pp,
                                 enemy.GetComponent<MonsterMovement>().moves[0], enemy.GetComponent<MonsterMovement>().moves[1], enemy.GetComponent<MonsterMovement>().moves[2], enemy.GetComponent<MonsterMovement>().moves[3]);;

                        }
                       
                            Destroy(containerBody);              
                            tryToCatch = false;
                            containerBody = null;
                            container = null;
                    }
                }

            }

        }



    }
    public void EnemyTrainerSeUp()
    {


        if (enemyMonsterInParty == 0)
        {
            GameObject child = Instantiate(monsterBod);
            child.GetComponent<MonsterMovement>().monster = enemyTrainer.GetComponent<TrainerControler>().monstersInParty[0];
            child.GetComponent<MonsterMovement>().AtStart();
            child.GetComponent<SpriteRenderer>().sortingOrder = enemyTrainer.GetComponent<SpriteRenderer>().sortingOrder;
            child.transform.position = enemyTrainer.gameObject.transform.position;
            enemy = child;
        }
        else
        {
            GameObject child = Instantiate(monsterBod);
            child.GetComponent<MonsterMovement>().monster = enemyTrainer.GetComponent<TrainerControler>().monstersInParty[enemyMonsterInParty];
            child.GetComponent<MonsterMovement>().AtStart();
            child.GetComponent<SpriteRenderer>().sortingOrder = enemyTrainer.GetComponent<SpriteRenderer>().sortingOrder;
            child.transform.position = enemy.gameObject.transform.position;
            Destroy(enemy);
            enemy = child;
            checkMovableE = enemy.transform.GetChild(0).gameObject;


        }
    }
    public void ChooseDirection()
    {
        directionE = Random.Range(0, 3);
    }
    public void ShowMenu()
    {
        if (otherMenus[1].activeInHierarchy == false && otherMenus[2].activeInHierarchy == false && otherMenus[0].activeInHierarchy == false)
        {
            if (Time.timeScale == 1f)
            {
                battlePanel.SetActive(true);

                Time.timeScale = 0f;
                EventSystem.current.SetSelectedGameObject(null);

                EventSystem.current.SetSelectedGameObject(buttons[0].gameObject); return;
                return;

            }
            else
            {
                battlePanel.SetActive(false);
                Time.timeScale = 1f;
                return;
            }
        }
        else
        {
            ExitPanels();
        }
    }
    public void BattleStart(int number)
    {

        monsterOutNumber = number;

        if (GameObject.FindGameObjectWithTag("PlayerMonster") == null)
        {
            monsterBody = Instantiate(monster);

            monsterBody.transform.position = player.transform.position;
            monsterBody.GetComponent<SpriteRenderer>().sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder;

        }
        else
        {
            Vector2 position = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position;
            Destroy(GameObject.FindGameObjectWithTag("PlayerMonster"));
            monsterBody = Instantiate(monster);
            monsterBody.GetComponent<SpriteRenderer>().sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder;
            monsterBody.transform.position = position;

        }

        monsterBody.GetComponent<SpriteRenderer>().sprite = team.playerTeam.myTeam[monsterOutNumber].myMonster.monsterImage;
        if (team.playerTeam.myTeam[monsterOutNumber].currentHp > (team.playerTeam.myTeam[monsterOutNumber].maxHp + team.playerTeam.myTeam[monsterOutNumber].myMonster.hp))
            team.playerTeam.myTeam[monsterOutNumber].currentHp = team.playerTeam.myTeam[monsterOutNumber].maxHp + team.playerTeam.myTeam[monsterOutNumber].myMonster.hp;
        team.playerTeam.myTeam[monsterOutNumber].expNeeded = team.playerTeam.myTeam[monsterOutNumber].expNeeded;
        for (int i = 0; i < team.playerTeam.myTeam[monsterOutNumber].move.Length; i++)
        {
            attack[i].text = team.playerTeam.myTeam[monsterOutNumber].move[i].attackName;
            type[i].text = team.playerTeam.myTeam[monsterOutNumber].move[i].type.ToString();
            if (team.playerTeam.myTeam[monsterOutNumber].pp[i] > team.playerTeam.myTeam[monsterOutNumber].move[i].pp)
                team.playerTeam.myTeam[monsterOutNumber].pp[i] = team.playerTeam.myTeam[monsterOutNumber].move[i].pp;
            pp[i].text = team.playerTeam.myTeam[monsterOutNumber].pp[i].ToString() + "/" + team.playerTeam.myTeam[monsterOutNumber].move[i].pp.ToString();
        }
        ButtonsPPCheck();
        for (int i = 0; i < 4 - team.playerTeam.myTeam[number].move.Length; i++)
        {
            attackSlot[3 - i].SetActive(false);
        }
        layers = player.GetComponent<SpriteRenderer>().sortingOrder;
        if (battleStart != true)
        {
            battlePanel.SetActive(false);
            Time.timeScale = 1f;
        }
        checkMovableP = monsterBody.transform.GetChild(0).gameObject;

        battleStart = false; mustChange = false;
    }
    public void ButtonsPPCheck()
    {

        for (int i = 0; i < 4; i++)
        {
            if (team.playerTeam.myTeam[monsterOutNumber].pp[i] <= 0)
            { ButtonUsed(attackSlot[i].GetComponent<Button>()); }
            else
            { ButtonRefreshed(attackSlot[i].GetComponent<Button>()); }
        }
    }
    public void Run()
    {
        Time.timeScale = 1f;
        Destroy(enemy);
        Destroy(monsterBody);

        BattleFinish();
    }


    public void ButtonUsed(Button buttonUsed)
    {
        buttonUsed.interactable = false;
    }
    public void ButtonRefreshed(Button button)
    {
        button.interactable = true;
    }
    public void BattleFinish()
    {
        CharacterMovement.cantMove = false;
        caughtIs = false;
            enemyMonsterInParty = 0;
        if (enemyTrainer != null)
        {
            enemyTrainer.SetActive(true);   
            enemyTrainer.GetComponent<TrainerControler>().battled = true;

            enemyTrainer = null;    
        }

        enemy = null;


        battleOn = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            ButtonRefreshed(buttons[i]);
        }
        battlePanel.SetActive(false);
        playerInfo.SetActive(false);
        enemyInfo.SetActive(false);
        dialogBox.SetActive(false);
        Destroy(arrowBody);
        movingE = false;
        moving = false; 


        player.SetActive(true);
    }
    public void ExitPanels()
    {
        for (int i = 0; i < otherMenus.Length; i++)
        {
            otherMenus[i].SetActive(false);

        }
        battlePanel.SetActive(true);

        Destroy(arrowBody);

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(battleFirstButton);

    }
    public void SetBattleToAttack()
    {

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(attackFirstButton);
    }
    public void SetBattleToInventory()
    {

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(inventoryFirstButton);
    }
    public void SetBattleToTeam()
    {

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(teamFirstButton);
    }


    public void SetUpEnemy()
    {

        enemyMonsterName.text = enemy.GetComponent<MonsterMovement>().name;
        float fillValue = enemy.GetComponent<MonsterMovement>().currentHp / enemy.GetComponent<MonsterMovement>().maxHp;
        enemySlider.GetComponent<Slider>().value = fillValue;
        enemyHpValue.text = enemy.GetComponent<MonsterMovement>().currentHp + "/" + enemy.GetComponent<MonsterMovement>().maxHp;
        enemyLvl.text = enemy.GetComponent<MonsterMovement>().lvl.ToString();
        if (enemy.GetComponent<MonsterMovement>().condition == Condition.None)
        {
            enemyCondition.sprite = conditions[0];
        }
        else
            if (enemy.GetComponent<MonsterMovement>().condition == Condition.Asleep)
        {
            enemyCondition.sprite = conditions[1];
        }
        else
            if (enemy.GetComponent<MonsterMovement>().condition == Condition.Burn)
        {
            enemyCondition.sprite = conditions[2];
        }
        else
            if (enemy.GetComponent<MonsterMovement>().condition == Condition.Confused)
        {
            enemyCondition.sprite = conditions[3];
        }
        else
            if (enemy.GetComponent<MonsterMovement>().condition == Condition.Cursed)
        {
            enemyCondition.sprite = conditions[4];
        }
        else
            if (enemy.GetComponent<MonsterMovement>().condition == Condition.Frozen)
        {
            enemyCondition.sprite = conditions[5];
        }
        else
            if (enemy.GetComponent<MonsterMovement>().condition == Condition.Paralyzed)
        {
            enemyCondition.sprite = conditions[6];
        }
        else
            if (enemy.GetComponent<MonsterMovement>().condition == Condition.Poison)
        {
            enemyCondition.sprite = conditions[7];
        }
    }
    public void SetUpPlayer()
    {
        for (int i = 0; i < team.playerTeam.myTeam[monsterOutNumber].move.Length; i++)
        {
            if (team.playerTeam.myTeam[monsterOutNumber].pp[i] > team.playerTeam.myTeam[monsterOutNumber].move[i].pp)
                team.playerTeam.myTeam[monsterOutNumber].pp[i] = team.playerTeam.myTeam[monsterOutNumber].move[i].pp;
            pp[i].text = team.playerTeam.myTeam[monsterOutNumber].pp[i].ToString() + "/" + team.playerTeam.myTeam[monsterOutNumber].move[i].pp.ToString();
        }
        monsterName.text = team.playerTeam.myTeam[monsterOutNumber].monsterNickname;
        float fillValueHp = team.playerTeam.myTeam[monsterOutNumber].currentHp /( team.playerTeam.myTeam[monsterOutNumber].maxHp+ team.playerTeam.myTeam[monsterOutNumber].myMonster.hp);
        slider.GetComponent<Slider>().value = fillValueHp;
        float fillValueExp = team.playerTeam.myTeam[monsterOutNumber].exp / (team.playerTeam.myTeam[monsterOutNumber].expNeeded);
        expSlider.GetComponent<Slider>().value = fillValueExp;
        hpValue.text = team.playerTeam.myTeam[monsterOutNumber].currentHp + "/" + (team.playerTeam.myTeam[monsterOutNumber].maxHp + team.playerTeam.myTeam[monsterOutNumber].myMonster.hp);
        expValue.text = team.playerTeam.myTeam[monsterOutNumber].exp + "/" + (team.playerTeam.myTeam[monsterOutNumber].expNeeded);
        lvl.text = team.playerTeam.myTeam[monsterOutNumber].lvl.ToString();
        if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.None)
        {
            playerCondition.sprite = conditions[0];
        }
        else
            if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Asleep)
        {
            statusBonus = 2f;

            playerCondition.sprite = conditions[1];
        }
        else
            if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Burn)
        {
            statusBonus = 1.5f;

            playerCondition.sprite = conditions[2];
        }
        else
            if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Confused)
        {
            statusBonus = 1.5f;

            playerCondition.sprite = conditions[3];
        }
        else
            if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Cursed)
        {
            statusBonus = 2.5f;

            playerCondition.sprite = conditions[4];
        }
        else
            if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Frozen)
        {
            statusBonus = 2f;

            playerCondition.sprite = conditions[5];
        }
        else
            if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Paralyzed)
        {
            statusBonus = 1.5f;

            playerCondition.sprite = conditions[6];
        }
        else
            if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Poison)
        {
            statusBonus = 1.5f;

            playerCondition.sprite = conditions[7];
        }

    }
    public void ChooseDirection(int points)
    {
        attackNumber = points;
    }
    public void DontGoP()
    {
        posCantBeP = posItIsP;
    }
    public void DontGoE()
    {
        posCantBeE = posItIsE;
    }
    public void TakeDamage(GameObject target, float damage, bool isEnemy)
    {
        float critical = 1f;
        if (Random.value * 100f <= 6.25f)
            critical = 2f;
        if (isEnemy == true)
        {

            float type = TypeChart.GetEffectiveness(team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].type, target.GetComponent<MonsterMovement>().monster.type1) * TypeChart.GetEffectiveness(team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].type, target.GetComponent<MonsterMovement>().monster.type2);
            float modifier = Random.Range(0.85f, 1f) * type * critical;
            if (team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].isSpecial == true)
            {
                float d = damage * ((float)(team.playerTeam.myTeam[monsterOutNumber].spAttack+team.playerTeam.myTeam[monsterOutNumber].spAttack + spAttackBP) / (target.GetComponent<MonsterMovement>().spDefence + spDefenceBE)) + 2;
                int theDamage = Mathf.FloorToInt(d * modifier);
                target.GetComponent<MonsterMovement>().currentHp = target.GetComponent<MonsterMovement>().currentHp - theDamage;
            }
            else
            {
                float d = damage * ((float)(team.playerTeam.myTeam[monsterOutNumber].attack+team.playerTeam.myTeam[monsterOutNumber].attack + attackBP) / (target.GetComponent<MonsterMovement>().defense + defenceBE)) + 2;
                int theDamage = Mathf.FloorToInt(d * modifier);
                target.GetComponent<MonsterMovement>().currentHp = target.GetComponent<MonsterMovement>().currentHp - theDamage;
            }
            if (target.GetComponent<MonsterMovement>().condition == Condition.Frozen && (team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].type == Type.Fire || team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].type == Type.Fighting || team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].type == Type.Rock || team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].type == Type.Steel))
            {
                target.GetComponent<MonsterMovement>().condition = Condition.None;
                dialogBoxText.SetDialog($"{target.GetComponent<MonsterMovement>().name} defroze!");

            }

        }
        else
        {

            float type = TypeChart.GetEffectiveness(enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].type, team.playerTeam.myTeam[monsterOutNumber].myMonster.type1) * TypeChart.GetEffectiveness(enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].type, team.playerTeam.myTeam[monsterOutNumber].myMonster.type2);
            float modifier = Random.Range(0.85f, 1f) * type * critical;
            if (enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].isSpecial == true)
            {
                float d = damage * ((float)(enemy.GetComponent<MonsterMovement>().spAttack + spAttackBE) / (team.playerTeam.myTeam[monsterOutNumber].spDefence + team.playerTeam.myTeam[monsterOutNumber].spDefence + spDefenceBP)) + 2;
                int theDamage = Mathf.FloorToInt(d * modifier);
                team.playerTeam.myTeam[monsterOutNumber].currentHp = team.playerTeam.myTeam[monsterOutNumber].currentHp - theDamage;
            }
            else
            {
                float d = damage * ((float)(enemy.GetComponent<MonsterMovement>().attack + attackBP) / (team.playerTeam.myTeam[monsterOutNumber].defense + team.playerTeam.myTeam[monsterOutNumber].defense + defenceBP)) + 2;
                int theDamage = Mathf.FloorToInt(d * modifier);
                team.playerTeam.myTeam[monsterOutNumber].currentHp = team.playerTeam.myTeam[monsterOutNumber].currentHp - theDamage;
            }
            if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.Frozen && (enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].type == Type.Fire || enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].type == Type.Fighting || enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].type == Type.Rock || enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].type == Type.Steel))
            {
                team.playerTeam.myTeam[monsterOutNumber].monsterCondition = Condition.None;
                dialogBoxText.SetDialog($"{team.playerTeam.myTeam[monsterOutNumber].monsterNickname} defroze!");

            }


        }
    }
    public void ConditionChange(GameObject target, Condition condition, bool isEnemy)
    {
        if (isEnemy == true)
        {
            if (target.GetComponent<MonsterMovement>().condition == Condition.None)
                target.GetComponent<MonsterMovement>().condition = condition;

        }
        else
        {
            if (team.playerTeam.myTeam[monsterOutNumber].monsterCondition == Condition.None)
                team.playerTeam.myTeam[monsterOutNumber].monsterCondition = condition;

        }


    }
    public void LowerIncrease(GameObject target, float ammount, bool isEnemy)
    {

        if (isEnemy == true)
        {


            if (team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].attack == true)
            {
                attackBE = attackBP - attackBP * ammount / 100;

            }
            else if (team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].defence == true)
            {

                defenceBE = defenceBE - defenceBE * ammount / 100;

            }
            else if (team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].spAttack == true)
            {

                spAttackBE = spAttackBE - spAttackBE * ammount / 100;

            }
            else if (team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].spDefence == true)
            {

                spDefenceBE = spDefenceBE - spDefenceBE * ammount / 100;

            }
            else if (team.playerTeam.myTeam[monsterOutNumber].move[attackNumber].speed == true)
            {

                speedBE = speedBE - speedBE * ammount / 100;

            }


        }
        else
        {

            if (enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].attack == true)
            {
                attackBP = attackBP - attackBP * ammount / 100;
            }
            else if (enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].defence == true)
            {

                defenceBP = defenceBP - defenceBP * ammount / 100;

            }
            else if (enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].spAttack == true)
            {

                spAttackBP = spAttackBP - spAttackBP * ammount / 100;

            }
            else if (enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].spDefence == true)
            {

                spDefenceBP = spDefenceBP - spDefenceBP * ammount / 100;

            }
            else if (enemy.GetComponent<MonsterMovement>().moves[monsterMoveUsed].speed == true)
            {

                speedBP = speedBP - speedBP * ammount / 100;

            }



        }

    }
}
