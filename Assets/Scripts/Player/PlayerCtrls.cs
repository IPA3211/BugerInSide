using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCtrls : MonoBehaviour
{
    public GameObject clicker;
    
    void Update(){
        if(PlayerStatics.isCanMove()){
            return;
        }
        bool summit = Input.GetButtonDown("Submit");

        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        if(!((xRaw == 0) && (yRaw == 0)))
            clicker.transform.localPosition = new Vector2(xRaw, yRaw).normalized + new Vector2(0, 0.25f);

        if(summit){
            ClickTrigger trigger = clicker.GetComponent<ClickerCtrl>()._otherTrigger;
            if(trigger != null){
                trigger.click();
            }
        }
    }

}
