using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneSystem : MonoBehaviour
{
    public void cutSceneStart(){
        GameSystem.instance.cutSceneStart();
    }
    public void cutSceneEnd(){
        GameSystem.instance.cutSceneEnd();
    }
    public void SaveGame(){
        GameSaveManager.instance.save();
    }
}
