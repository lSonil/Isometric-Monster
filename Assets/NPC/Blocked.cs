using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocked : MonoBehaviour
{
    public bool back = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        back = true;
    }
}
