using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveContainer : MonoBehaviour
{
    public Vector3 offset;
    public GameObject passiveUI;
    private GameObject target;
    void Update()
    {
        if(target != null)
            this.transform.position = Camera.main.WorldToScreenPoint(target.transform.position + offset);
    }

    public void setTarget(GameObject other){
        target = other;
        gameObject.SetActive(true);
    }

    public void addPassive(PassiveData data){
        var obj = Instantiate(passiveUI, gameObject.transform);
        var pUI = obj.GetComponent<PassiveImage>();
        pUI.setData(data);
    }

    public void resetContainer(){
        if(gameObject.transform.childCount != 0){
            var btns = gameObject.GetComponentsInChildren<Transform>();

            for(int i = 1; i < btns.Length; i++){
                Destroy(btns[i].gameObject);
            }

            gameObject.transform.DetachChildren();
        }
        
        gameObject.SetActive(false);
    }
}
