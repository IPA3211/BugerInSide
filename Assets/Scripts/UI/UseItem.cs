using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public List<ItemData> items;
    public void use(){
        foreach (var item in items)
        {
            if(GameSystem.instance._ItemsInInventory.ContainsKey(item))
                GameSystem.instance.useItem(item, 100);
        }
    }
}
