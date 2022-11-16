using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gahoAni : MonoBehaviour
{
    public float distance = 0;
    public List<Transform> objs;
    // Update is called once per frame

    void Update()
    {
        float angle = Mathf.PI * 2 / objs.Count;
        
        for(int i = 0; i < objs.Count; i++){
            objs[i].localPosition = new Vector3(distance * Mathf.Sin(angle * i), distance * Mathf.Cos(angle * i), 0);
        }
    }
}
