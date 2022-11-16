using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveItem : MonoBehaviour
{
    public List<ItemData> items;
    
    public void give(){
        foreach (var item in items)
        {
            GameSystem.instance.giveItem(item, 1);
        }
    }
}
