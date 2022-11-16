using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipBtn : MonoBehaviour
{
    public Image image;
    public List<ST_PuzzleDisplay> puzzles;
    public List<Sprite> sprites;
    public int index = 0;
    public float delay = 120f;
    private float sumTime = 0f;

    public void Update(){        
        sumTime += Time.deltaTime;

        if(sumTime < delay){
            transform.localScale = new Vector3(0, 0, 0);
        }
        else{
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void onClickBtn(){
        puzzles[index].Complete = true;
        delay /= 2f;
    }

    public void clear(){
        sumTime = 0;
        index++;
        image.sprite = sprites[index];
    }
}
