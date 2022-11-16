using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFocus : MonoBehaviour
{
    public float yoffset = 2;
    public void changeFocus(){
        FocusSystem.instance.changeFocus(gameObject, yoffset);
    }

    public void resetFocus(){
        FocusSystem.instance.resetFocus();
    }
}
