using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusPointer : MonoBehaviour
{
    public Transform focusPointerObj;
    public float scaleMaxDistance;
    public float scaleMinDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FocusSystem.instance.focusedObj == null){
            focusPointerObj.gameObject.SetActive(false);
        }
        else{
            focusPointerObj.gameObject.SetActive(true);
            var dir = FocusSystem.instance.focusedObj.transform.position - transform.position;
            focusPointerObj.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f); 
            var dis = Vector2.Distance(transform.position, FocusSystem.instance.focusedObj.transform.position);
            if(dis < scaleMaxDistance){
                focusPointerObj.transform.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, (dis - scaleMinDistance) / (scaleMaxDistance - scaleMinDistance));
            }
            else{
                focusPointerObj.transform.localScale = Vector2.one;
            }
            
        }
    }
}
