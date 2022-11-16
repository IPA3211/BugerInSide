using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSystem : MonoBehaviour
{
    public static FocusSystem instance;
    private GameObject inst_induceObj;
    private GameObject lastMinimap;
    private Color lastObjMinimapColor;
    public GameObject induceObj;
    public GameObject focusedObj;
    public Color focusColor;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null){
            instance = this;
        }
    }

    public void SetFocus(GameObject other, float yOffset){
        if(inst_induceObj == null){
            inst_induceObj = Instantiate(induceObj);
        }
        focusedObj = other;
        inst_induceObj.SetActive(true);
        inst_induceObj.transform.SetParent(other.transform);
        inst_induceObj.transform.localPosition = new Vector3(0, yOffset, 0);
        Transform [] children;

        if(other.transform.parent != null){
            children = other.transform.parent.GetComponentsInChildren<Transform>();
        }
        else{
            children = other.GetComponentsInChildren<Transform>();
        }
        

        foreach(var item in children){
            if(item.CompareTag("Minimap")){
                lastMinimap = item.gameObject;
                lastObjMinimapColor = item.GetComponent<SpriteRenderer>().color;
                item.transform.localScale = new Vector3(3f, 3f, 3f);
                item.GetComponent<SpriteRenderer>().sortingLayerName = "Fg";
                item.GetComponent<SpriteRenderer>().color = focusColor;
                break;
            }
        }
    }

    public void resetFocus(){
        focusedObj = null;
        if(lastMinimap != null){
            lastMinimap.transform.localScale = new Vector3(2f, 2f, 2f);
            lastMinimap.GetComponent<SpriteRenderer>().color = lastObjMinimapColor;
            lastMinimap.GetComponent<SpriteRenderer>().sortingLayerName = "Bg";
        }
        if(inst_induceObj != null){
            inst_induceObj.SetActive(false);
        }
    }

    public void changeFocus(GameObject other, float yOffset = 2){
        resetFocus();
        SetFocus(other, yOffset);
    }
}
