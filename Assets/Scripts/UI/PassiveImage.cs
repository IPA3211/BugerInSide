using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveImage : MonoBehaviour
{
    public float loopTime = 1;
    public Image image;
    private PassiveData _data;
    private float _sumTime = 0;
    private Color startColor;
    private Color endColor;

    public void Update(){
        if(_data != null){
            if(_data.duration <= 1){
                image.color = Color.Lerp(endColor, startColor, Mathf.PingPong(Time.realtimeSinceStartup, loopTime));
            }
            else{
                image.color = Color.white;
            }

            if(_data.duration == 0){
                Destroy(gameObject);
            }
        }
    }
    public void setData(PassiveData data){
        _data = data;
        image.sprite = data.describeImage;
        startColor = image.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
    }
}
