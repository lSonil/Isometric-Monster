using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerControler : MonoBehaviour,ISavable
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject exclamation;
    [SerializeField] public TeamMonster[] monstersInParty;
    [SerializeField] public GameObject monsterBody;
    [SerializeField] public bool battled;

    public IEnumerator TriggerTrainerBattle()
    {
       
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5F);
        exclamation.SetActive(false);


        StartCoroutine(DialogManager.Instace.ShowDialog(dialog));
        BattleZone.enemyTrainer = gameObject;
        BattleZone.battleStart = true;       

    }

    public object CaptureState()
    {
        return battled;
    }

    public void RestoreState(object state)
    {
        battled = (bool)state;
    }
}
