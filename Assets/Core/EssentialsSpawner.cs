using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsSpawner : MonoBehaviour
{
    [SerializeField] GameObject essentials;
    private void Awake()
    {
        var exist = FindObjectsOfType<Essentials>();
        if(exist.Length==0)
        {
            Instantiate(essentials, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
