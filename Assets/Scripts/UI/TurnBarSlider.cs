using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnBarSlider : MonoBehaviour
{
    public Image image;
    public Vector2 startPos = new Vector2(-800, -50), endPos = new Vector2(800, -50);
    public bool flip;
    float lastRatio = 0;

    public void Update(){
        if(flip){
            gameObject.transform.localScale = new Vector3(1, -1, 1);
            image.transform.localScale = new Vector3(1, -1, 1);
        }
        else{
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            image.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void setPosition(float max, float cur){
        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startPos, endPos, cur / max);
    }

    public void setData(CharacterData data){
        image.sprite = data.describeImage;
    }


}
