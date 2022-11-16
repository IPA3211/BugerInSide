using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowSkillInfo : ShowMenuInfo
{   
    public GameObject content;
    public GameObject skillBtn;
    public TMP_Text describe;

    public override void setUniqueData(CharacterData data)
    {
        if(content.transform.childCount != 0){
            var btns = content.GetComponentsInChildren<Transform>();

            for(int i = 1; i < btns.Length; i++){
                Destroy(btns[i].gameObject);
            }

            content.transform.DetachChildren();
        }

        foreach(var skill in data.skills){
            var newObj = Instantiate(skillBtn, content.transform);
            var mbd = newObj.GetComponent<MenuBtnDynamic>();
            mbd.text.text = skill.title;
            mbd.onFocusOneShot.AddListener((o) => {
                describe.text = skill.title + "\n"
                    + skill.describe;
            });
            mbd.onClickBtn.AddListener((o) => {
                if(data.isSupporter){
                    data.useSkillNum = data.skills.IndexOf(skill);
                }
            });
        }
        
        content.GetComponent<MenuObj>().renewBtns();
    }
}
