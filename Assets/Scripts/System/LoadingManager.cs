using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    public void Start(){
        GameSystem.instance.gameObject.GetComponent<GameSaveManager>().load();
    }
}
