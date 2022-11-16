using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFocusSetter : MonoBehaviour
{
    private MenuCtrl mc;
    public GameObject UI2Focus;
    // Start is called before the first frame update
    void Start()
    {
        mc = GetComponentInParent<MenuCtrl>();
    }

    public void changeFocus(){
        GetComponent<MenuBtn>().isClicked = true;
    }
}
