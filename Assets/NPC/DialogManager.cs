using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] int letterPerSecond;
    int curentLine = 0;
    Dialog dialog;
    bool isTyping;
    public static bool dialogOn;
    public static DialogManager Instace { get; private set; }
    private void Awake()
    {
        Instace = this;
    }
    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        dialogOn = true;
        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }
    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isTyping == false)
        {
            curentLine++;
            if (curentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[curentLine]));
            }
            else
            {

                dialogOn = false;
                dialogBox.SetActive(false);
                curentLine = 0;
            }
        }
        
    }
    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text +=letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
        isTyping = false;

    }
}
