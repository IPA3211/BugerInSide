using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class randomFire : MonoBehaviour
{
    public float min, max, loopTime;
    Light2D light;
    float sumTime;

    // Start is called before the first frame update
    void Start()
    {
        light = gameObject.GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(loopTime < sumTime){
            DOTween.To(() => light.pointLightOuterRadius, x => {
                light.pointLightOuterRadius = x;
            }, Random.Range(min, max), 0.5f).SetEase(Ease.InBounce);
            sumTime = 0;
        }

        sumTime += Time.deltaTime;
    }
}
