using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject player = null;

    [SerializeField]
    float speed1 = 1.0f;
    [SerializeField]
    float speed2 = 50.0f;
    [SerializeField]
    float innerBuffer = 0.1f;
    [SerializeField]

    float outerBuffer = 1.5f;
    bool moving;
    public Vector3 offset1;
    public Vector3 offset2;

    public bool cameraScript;
    public bool arenaScript;

    void Start()
    {

    }
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null|| GameObject.FindGameObjectWithTag("PlayerMonster") != null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null) 
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
            else
            if(GameObject.FindGameObjectWithTag("PlayerMonster") != null)
            {
                player = GameObject.FindGameObjectWithTag("PlayerMonster");
            }
                
            if (cameraScript)
                if (player.activeInHierarchy == true)
                {
                    Vector3 heading = player.transform.position - transform.position;
                    float distance = heading.magnitude;
                    Vector3 direction = heading / distance;
                    if (distance > outerBuffer)
                        moving = true;
                    if (moving)
                        if (distance > innerBuffer)
                            transform.position += direction * Time.deltaTime * speed1 * Mathf.Max(distance);
                        else
                        {
                            transform.position = player.transform.position;
                            moving = false;
                        }

                    gameObject.GetComponent<Camera>().orthographic = true;

                    gameObject.GetComponent<Camera>().orthographicSize = 6;
                    gameObject.GetComponent<Camera>().nearClipPlane = 0;

                }
                else
                {
                    Vector3 heading = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position - transform.position;
                    float distance = heading.magnitude;
                    Vector3 direction = heading / distance;
                    if (distance > outerBuffer)
                        moving = true;
                    if (moving)
                        if (distance > innerBuffer)
                            transform.position += direction * Time.deltaTime * speed2 * Mathf.Max(distance);
                        else
                        {
                            transform.position = GameObject.FindGameObjectWithTag("PlayerMonster").transform.position;
                            moving = false;
                        }

                    gameObject.GetComponent<Camera>().orthographic = true;

                    gameObject.GetComponent<Camera>().orthographicSize = 5;
                    gameObject.GetComponent<Camera>().nearClipPlane = 0;


                }

            if (arenaScript)
            {
                if (BattleZone.battleOn == false)
                {
                    transform.GetComponent<Collider2D>().enabled = false;
                    transform.GetComponent<Renderer>().enabled = false;

                    transform.position = player.transform.position;
                }
                else
                {
                    transform.GetComponent<Collider2D>().enabled = true;

                    transform.GetComponent<Renderer>().enabled = true;

                }
            }
        }
    }
}
