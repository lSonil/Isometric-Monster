using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
public class CharacterMovement : MonoBehaviour,ISavable
{
    [Header("Scenes")]
    public static List<SceneDetails> connectedScenes;
    public PlayerTeam playerTeam;
    public TeamMonster[] monsterDex;
    public MonsterAttacks[] moveDex;

    [Header("Directions")]
    public float dH;
    public float dV;
    public Sprite[] face;
    public GameObject chekInteractable;
    public bool interactable;
    public GameObject objectToInteract;
    public GameObject dialogBox;
    [Header("Movement")]
    [SerializeField] public float speed = 0.25f;
    [SerializeField] public GameObject layerMap;
    [SerializeField] public bool usingStairs;
    public static bool cantMove;
    public Vector3 targetPosition;

    Vector3 startPosition;

    public int posItIs;
    public int posCantBe;

    public bool moving;
    [SerializeField] float slowSpeed;
    [SerializeField] float fastSpeed;
    [SerializeField] float snapDistance;


    [Header("UI")]
    [SerializeField] public bool inMenu;
    [SerializeField] public GameObject skillsPanel;

    public GameObject pauseMenu;
    public GameObject[] otherMenusT;
    public GameObject[] otherMenusF;

    public GameObject[] firstButton;
    public GameObject pauseFirstButton, inventoryFirstButton, teamFirstButton;  
    private void Start()  

    {

        pauseMenu.SetActive(false);
    }
    void Update()
    {
           layerMap = GameObject.Find("ColidersLayers");


        if (layerMap!=null&&BattleZone.battleStart == false && BattleZone.battleOn == false && DialogManager.dialogOn == false && cantMove == false)
        {
            for (int i = 0; i < layerMap.transform.childCount; i++)
            {
                if (i == transform.GetComponent<SpriteRenderer>().sortingOrder - 1)
                {
                    layerMap.transform.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    layerMap.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            //movement

            if (moving)
            {

                if (Vector2.Distance(startPosition, transform.position) > snapDistance)
                {
                    transform.position = targetPosition;
                    moving = false;

                }
                transform.position += (targetPosition - startPosition) * speed * Time.deltaTime;
                return;
            }
            else
            {
                transform.position = targetPosition;
            }
            if (inMenu == false)
            {
                if (Input.GetKey(KeyCode.W) && posCantBe != 1)
                {

                    if (gameObject.GetComponent<SpriteRenderer>().sprite == face[1])
                    {
                        targetPosition = transform.position + new Vector3(dH, dV, 0);
                        startPosition = transform.position;
                        chekInteractable.transform.position = transform.position + new Vector3(dH, dV - 0.2f, 0);
                        moving = true;
                    }
                    else
                    {
                        posItIs = 1; posCantBe = 5;
                        chekInteractable.transform.position = transform.position + new Vector3(dH, dV - 0.2f, 0);
                        gameObject.GetComponent<SpriteRenderer>().sprite = face[1];
                        return;
                    }

                }
                else if (Input.GetKey(KeyCode.S) && posCantBe != 3)
                {

                    if (gameObject.GetComponent<SpriteRenderer>().sprite == face[3])
                    {
                        chekInteractable.transform.position = transform.position + new Vector3(-dH + 0.1f, -dV - 0.1f, 0);
                        targetPosition = transform.position + new Vector3(-dH, -dV, 0);
                        startPosition = transform.position;
                        moving = true;
                    }
                    else
                    {
                        posItIs = 3; posCantBe = 5;
                        chekInteractable.transform.position = transform.position + new Vector3(-dH + 0.1f, -dV - 0.1f, 0);
                        gameObject.GetComponent<SpriteRenderer>().sprite = face[3];
                        return;
                    }

                }
                else if (Input.GetKey(KeyCode.A) && posCantBe != 0)
                {

                    if (gameObject.GetComponent<SpriteRenderer>().sprite == face[0])
                    {
                        chekInteractable.transform.position = transform.position + new Vector3(-dH, dV - 0.2f, 0);
                        targetPosition = transform.position + new Vector3(-dH, dV, 0);
                        startPosition = transform.position;
                        moving = true;
                    }
                    else
                    {
                        posItIs = 0;
                        posCantBe = 5;
                        chekInteractable.transform.position = transform.position + new Vector3(-dH, dV - 0.2f, 0);
                        gameObject.GetComponent<SpriteRenderer>().sprite = face[0];
                        return;
                    }

                }

                else if (Input.GetKey(KeyCode.D) && posCantBe != 2)
                {

                    if (gameObject.GetComponent<SpriteRenderer>().sprite == face[2])
                    {
                        targetPosition = transform.position + new Vector3(dH, -dV, 0);
                        chekInteractable.transform.position = transform.position + new Vector3(dH - 0.1f, -dV - 0.1f, 0);
                        startPosition = transform.position;
                        moving = true;
                    }
                    else
                    {
                        posItIs = 2; posCantBe = 5;

                        chekInteractable.transform.position = transform.position + new Vector3(dH - 0.1f, -dV - 0.1f, 0);
                        gameObject.GetComponent<SpriteRenderer>().sprite = face[2];
                        return;
                    }

                }
            }
            //UI
            if (Input.GetKeyDown(KeyCode.P) && inMenu == false)
            {
                ShowPaused();
            }
            if (Input.GetKeyDown(KeyCode.X) && inMenu == true)
            {
                ExitPanels();
            }

            if (Input.GetKeyDown(KeyCode.Z) && interactable == true && objectToInteract.GetComponent<Character>().moving == false)
            {
                objectToInteract.GetComponent<Character>().directionSpeaking = posItIs;
                objectToInteract.GetComponent<Character>().isSpeaking = true;
                objectToInteract.GetComponent<Interaction>().Interact();
                return;
            }
            if (dialogBox.activeInHierarchy == false && objectToInteract != null)
            {
                objectToInteract.GetComponent<Character>().isSpeaking = false;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = fastSpeed;
            }
            else
            {
                speed = slowSpeed;

            }
        }
        if (BattleZone.battleOn == false && DialogManager.dialogOn == true)
        {
            DialogManager.Instace.HandleUpdate();
        }

    }

    public void ShowPaused()
    {

        if (inMenu==false)
        {
            inMenu = true;
            pauseMenu.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);

            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
        else
        {
            inMenu = false ;

            for (int i = 0; i < otherMenusF.Length; i++)
            {
                otherMenusF[i].SetActive(false);

            }
            for (int i = 0; i < otherMenusT.Length; i++)
            {
                otherMenusT[i].SetActive(true);

            }
            pauseMenu.SetActive(false);

        }
    }
    public void ExitPanels()
    {
        for (int i = 0; i < otherMenusF.Length; i++)
        {
            otherMenusF[i].SetActive(false);

        }
        for (int i = 0; i < otherMenusT.Length; i++)
        {
            otherMenusT[i].SetActive(true);

        }
        pauseMenu.SetActive(true);
        if (skillsPanel != null)
        {
            Destroy(skillsPanel);
        }
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }
    public void SetMenuToInventory()
    {

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(inventoryFirstButton);
    }
    public void SetMenuToTeam()
    {

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(teamFirstButton);
    }
    public void DontGo()
    {
        posCantBe = posItIs;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "FightTile"&& inMenu == false)
        {
            if (collision.GetComponentInParent<TrainerControler>().battled == false)
            {
                transform.position = targetPosition;
                cantMove = true;
                collision.GetComponentInParent<Character>().isSpeaking = true;
                collision.GetComponentInParent<Character>().transform.position = collision.GetComponentInParent<Character>().targetPosition;
                StartCoroutine(collision.GetComponentInParent<TrainerControler>().TriggerTrainerBattle());
            }
        }


    }

    public object CaptureState()
    {
        var savedData = new PlayerSaveData
        {
            position = new float[] { transform.position.x, transform.position.y },
            PartyLength = playerTeam.myTeam.Count,
        };
        for (int i = 0; i < playerTeam.myTeam.Count; i++)
        {
            savedData.TeamMonster.Add(playerTeam.myTeam[i].myMonster.name);
            savedData.MonsterAttacks0.Add(playerTeam.myTeam[i].move[0].name);
            savedData.MonsterAttacks1.Add(playerTeam.myTeam[i].move[1].name);
            savedData.MonsterAttacks2.Add(playerTeam.myTeam[i].move[2].name);
            savedData.MonsterAttacks3.Add(playerTeam.myTeam[i].move[3].name);

            savedData.MonsterNickname.Add(playerTeam.myTeam[i].monsterNickname);
            savedData.InParty.Add(playerTeam.myTeam[i].inParty);
            savedData.MaxHp.Add(playerTeam.myTeam[i].maxHp);
            savedData.CurrentHp.Add(playerTeam.myTeam[i].currentHp);
            savedData.Attack.Add(playerTeam.myTeam[i].attack);
            savedData.Defense.Add(playerTeam.myTeam[i].defense);
            savedData.SpAttack.Add(playerTeam.myTeam[i].spAttack);
            savedData.SpDefence.Add(playerTeam.myTeam[i].spDefence);
            savedData.Speed.Add(playerTeam.myTeam[i].speed);
            savedData.Pp.Add(playerTeam.myTeam[i].pp);

            savedData.Lvl.Add(playerTeam.myTeam[i].lvl);
            savedData.ExpNeeded.Add(playerTeam.myTeam[i].expNeeded);
            savedData.Exp.Add(playerTeam.myTeam[i].exp);
            savedData.SkillPoints.Add(playerTeam.myTeam[i].skillPoints);  
            savedData.Condition.Add(playerTeam.myTeam[i].monsterCondition);

        }

        return savedData;
    }

    public void RestoreState(object state)
    {
        var savedData = (PlayerSaveData)state;
        var pos = savedData.position;
        transform.position = new Vector3(pos[0], pos[1]); 
        targetPosition = new Vector3(pos[0], pos[1]);

        playerTeam.myTeam = new List<Monster>();
        for (int i = 0; i < savedData.PartyLength; i++)
        {
            playerTeam.myTeam.Add(new Monster());
        }
        for (int i = 0; i < playerTeam.myTeam.Count; i++)
        {
            for (int j = 0; j < monsterDex.Length; j++)
            {
                if (savedData.TeamMonster[i] == monsterDex[j].name) 
                {
                    playerTeam.myTeam[i].myMonster = monsterDex[j];
                }
            }
            playerTeam.myTeam[i].move = new MonsterAttacks[4];

            for (int j = 0; j < moveDex.Length; j++)
            {
                if (savedData.MonsterAttacks0[i] == moveDex[j].name)
                {
                    playerTeam.myTeam[i].move[0] = moveDex[j];
                }
            }
            for (int j = 0; j < moveDex.Length; j++)
            {
                if (savedData.MonsterAttacks1[i] == moveDex[j].name)
                {
                    playerTeam.myTeam[i].move[1] = moveDex[j];
                }
            }
            for (int j = 0; j < moveDex.Length; j++)
            {
                if (savedData.MonsterAttacks2[i] == moveDex[j].name)
                {
                    playerTeam.myTeam[i].move[2] = moveDex[j];
                }
            }
            for (int j = 0; j < moveDex.Length; j++)
            {
                if (savedData.MonsterAttacks3[i] == moveDex[j].name)
                {
                    playerTeam.myTeam[i].move[3] = moveDex[j];
                }
            }
            playerTeam.myTeam[i].monsterNickname = savedData.MonsterNickname[i];
            playerTeam.myTeam[i].monsterCondition = savedData.Condition[i];
            playerTeam.myTeam[i].inParty = savedData.InParty[i];
            playerTeam.myTeam[i].maxHp = savedData.MaxHp[i];
            playerTeam.myTeam[i].currentHp = savedData.CurrentHp[i];
            playerTeam.myTeam[i].attack = savedData.Attack[i];
            playerTeam.myTeam[i].defense = savedData.Defense[i];
            playerTeam.myTeam[i].spAttack = savedData.SpAttack[i];
            playerTeam.myTeam[i].spDefence = savedData.SpDefence[i];
            playerTeam.myTeam[i].speed = savedData.Speed[i];
            playerTeam.myTeam[i].pp = savedData.Pp[i];
            playerTeam.myTeam[i].lvl = savedData.Lvl[i];
            playerTeam.myTeam[i].expNeeded = savedData.ExpNeeded[i];
            playerTeam.myTeam[i].exp = savedData.Exp[i];
            playerTeam.myTeam[i].skillPoints = savedData.SkillPoints[i];
        }

    }
}
[System.Serializable]

public class PlayerSaveData
{
    public float[] position;
    public int PartyLength;
    [SerializeField] public List<string> TeamMonster = new List<string>();
    [SerializeField] public List<string> MonsterAttacks0 = new List<string>();
    [SerializeField] public List<string> MonsterAttacks1 = new List<string>();
    [SerializeField] public List<string> MonsterAttacks2 = new List<string>();
    [SerializeField] public List<string> MonsterAttacks3 = new List<string>();

    [SerializeField] public List<string> MonsterNickname = new List<string>();
    [SerializeField] public List<bool> InParty = new List<bool>();
    [SerializeField] public List<float> MaxHp = new List<float>();
    [SerializeField] public List<float> CurrentHp = new List<float>();
    [SerializeField] public List<float> Attack = new List<float>();
    [SerializeField] public List<float> Defense = new List<float>();
    [SerializeField] public List<float> SpAttack = new List<float>();
    [SerializeField] public List<float> SpDefence = new List<float>();
    [SerializeField] public List<float> Speed = new List<float>();
    [SerializeField] public List<int[]> Pp = new List<int[]>();
    [SerializeField] public List<int> Lvl = new List<int>();
    [SerializeField] public List<Condition> Condition = new List<Condition>();

    [SerializeField] public List<float> ExpNeeded = new List<float>();
    [SerializeField] public List<float> Exp = new List<float>();
    [SerializeField] public List<int[]> SkillPoints = new List<int[]>();

}





