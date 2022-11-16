using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class ToonScene : MonoBehaviour
{
    float startDialogueDelay = 0f;
    public GameObject toonCutHolder;
    public GameObject zObject;
    public List<ToonAniCallbacks> cuts;
    public int index = 0;
    public UnityEvent onEnd;
    public static bool isEnded = false;
    
    void Start(){
        cuts = new List<ToonAniCallbacks>(toonCutHolder.GetComponentsInChildren<ToonAniCallbacks>());
        foreach (var item in cuts)
        {
            item.gameObject.SetActive(false);
        }
        cuts[0].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(isEnded){
            zObject.SetActive(true);
        }
        else{
            zObject.SetActive(false);
        }
        
        if(Input.GetButtonDown("Submit") && isEnded){
            index++;
            if(index >= cuts.Count){
                onEnd.Invoke();
                return;
            }
            cuts[index - 1].gameObject.SetActive(false);
            cuts[index].gameObject.SetActive(true);
            isEnded = false;
        }
    }

    public void endScene(){
        Destroy(SoundPlayer.instance.gameObject);
    }
}
