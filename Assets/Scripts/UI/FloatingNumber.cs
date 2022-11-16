using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingNumber : MonoBehaviour
{
    public float min = 200, max = 5000;
    public float xPower = 2, yPower = 5;
    public float lifeTime = 2f;
    public Color minC, maxC, healColor;
    Rigidbody2D rigidbody;
    TMP_Text text;

    public void init(int num){
        float ratio = 0;
        GetComponent<Renderer>().sortingLayerName = "Fg Design";
        text = GetComponent<TMP_Text>();
        rigidbody = GetComponent<Rigidbody2D>();
        
        if(Mathf.Abs(num) < min){
            ratio = 0;
        }
        else if(Mathf.Abs(num) > max){
            ratio = 1;
        }
        else{
            ratio = Mathf.Abs((float)num / (float)max);
        }

        transform.localScale = Vector2.Lerp(new Vector2(1,1), new Vector2(1.5f,1.5f), ratio);
        if(num > 0){
            text.color = healColor;
            rigidbody.AddForce(new Vector2(0,Random.Range(1, 1) * yPower), ForceMode2D.Impulse);
        }
        else{
            text.color = Color.Lerp(minC, maxC, ratio);
            rigidbody.AddForce(new Vector2(Random.Range(-1f, 1f) * xPower,Random.Range(0.9f, 1) * yPower), ForceMode2D.Impulse);
        }
        text.text = Mathf.Abs(num) + "";
        text.DOFade(0, lifeTime);
        Debug.Log((float)num / (float)max);
        Destroy(gameObject, lifeTime);
    }
}
