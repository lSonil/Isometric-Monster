using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] int sceneToLoad ;
    [SerializeField] Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(SwitchScenes());
        }

    }
    IEnumerator SwitchScenes()
    {
 
        GameObject.Find("Player").transform.position = spawnPoint.position;

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

    }

}
