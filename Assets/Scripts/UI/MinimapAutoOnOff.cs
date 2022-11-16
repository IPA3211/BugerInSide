using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapAutoOnOff : MonoBehaviour
{
    public GameObject minimap;
    // Update is called once per frame
    void Update()
    {
        if(!PlayerStatics.isCanMove()){
            minimap.SetActive(true);
        }
        else{
            minimap.SetActive(false);
        }
    }
}
