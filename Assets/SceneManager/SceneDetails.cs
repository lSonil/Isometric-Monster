using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;
    [SerializeField] List<SceneDetails> allScenes;
    bool dont=false;
    public bool IsLoaded { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (CharacterMovement.connectedScenes != null)
            {
                var prevConnectedScenes = CharacterMovement.connectedScenes;
                foreach (var scene in prevConnectedScenes)
                {
                    if (!connectedScenes.Contains(scene) && scene != this)
                        scene.UnloadScene();
                }
            }
        
            LoadScene();
            foreach (var scene in connectedScenes)
            {
                scene.LoadScene();
            }
            CharacterMovement.connectedScenes = connectedScenes;
        }
    }

    public void LoadScene()
    {
        if (!IsLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            IsLoaded = true;
        }
    }
    public void UnloadScene()
    {
        if (IsLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            IsLoaded = false;
        }
    }
}
