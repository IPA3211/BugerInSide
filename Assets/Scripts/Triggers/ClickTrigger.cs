using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickTrigger : MonoBehaviour
{
    public UnityEvent onClick;

    public void click(){
        if(!PlayerStatics.isCanMove())
            onClick.Invoke();
    }
}