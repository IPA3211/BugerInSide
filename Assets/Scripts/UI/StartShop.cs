using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartShop : MonoBehaviour
{
    public List<ItemData> items;
    public List<EquipData> equips;
    
    public void startShop(){
        ShopCtrl.instance.startShop(items, equips);
    }
}
