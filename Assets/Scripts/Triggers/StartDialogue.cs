using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialouges
{
    [HideInInspector]
    public string _name;
    public string _dialogue;
    public List<UnityEvent> _onEnd; 
}
public class StartDialogue : MonoBehaviour
{
    public int startNum = 0;
    public List<Dialouges> dialouges;
    public void playDialogue(int num){
        try
        {
            if(num == -1){
                DialogueCtrl.instance.startDialogue(dialouges[startNum]._dialogue, dialouges[startNum]._onEnd);
            }
            else
                DialogueCtrl.instance.startDialogue(dialouges[num]._dialogue, dialouges[num]._onEnd);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public void changeDialogNum(int num){
        startNum = num;
    }
}
