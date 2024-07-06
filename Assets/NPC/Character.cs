using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour,ISavable
{
    [Header("Directions")]

    public float dH; 
    public float dV;
    public Sprite[] face;

    [Header("Movement")]


    [SerializeField] public Vector3 targetPosition;
    [SerializeField] public float step;
    public bool moving;

    public GameObject checkMovable;

    public int posItIs;
    public int posCantBe;


    [Header("Patern")]
    public bool random;
    [SerializeField] public int[] posibbleDirections;
    public int direction;
    public float speed;
    public float actualSpeed;
    public bool canMove = true;
    int spot=0;
    public bool isSpeaking = false;
    public int directionSpeaking;
    // Start is called before the first frame update
    void Start()
    {
        actualSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            if (BattleZone.battleStart == false && BattleZone.battleOn == false && this.gameObject.GetComponent<SpriteRenderer>().sortingOrder == GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sortingOrder && isSpeaking == false)
        {
            if (Time.timeScale != 0f)
            {
                if (actualSpeed <= 0)
                {
                    ChooseDirection();
                    actualSpeed = speed;
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
        }
        if (isSpeaking == true)
        {
            if (directionSpeaking == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = face[2];
            }
            else
                if (directionSpeaking == 1)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = face[3];
            }
            else
                if (directionSpeaking == 2)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = face[0];
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = face[1];
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

    public object CaptureState()
    {
        var savedData = new NPCSaveData
        {
            position = new float[] { transform.position.x, transform.position.y },
            Spot = spot,
        };
        return savedData;
    }

    public void RestoreState(object state)
    {
        var savedData = (NPCSaveData)state;
        var pos = savedData.position;

        transform.position = new Vector3(pos[0],pos[1]);
        targetPosition = new Vector3(pos[0], pos[1]);

        spot = savedData.Spot;
    }
}
[System.Serializable]
public class NPCSaveData
{
    public float[] position;
    public int Spot;
}