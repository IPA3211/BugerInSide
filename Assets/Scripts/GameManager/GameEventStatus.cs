using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

[System.Serializable]
public class vValue {
    public List<string> k;
    public List<int> v;
    public vValue(List<string> a, List<int> b){
        k = a;
        v = b;
    }
}

[System.Serializable]
public class GameEventStatusSave{
    [SerializeField]
    public List<int> kList;
    [SerializeField]
    public List<vValue> vList;
    public GameEventStatusSave(GameEventStatus raw){
        var kList = new List<int>(raw._status.Keys);
        var vListWithDictionary = new List<Dictionary<string, int>>(raw._status.Values);
        var vList = new List<vValue>();
        foreach(var a in vListWithDictionary){
            var kvList = new List<string>(a.Keys);
            var vvList = new List<int>(a.Values);

            vList.Add(new vValue(kvList, vvList));
        }
    }

    public void load(GameEventStatus raw){
        Dictionary<int, Dictionary<string, int>> _status = new Dictionary<int, Dictionary<string, int>>();
        List<Dictionary<string, int>> _saa = new List<Dictionary<string, int>>();
        foreach(var a in vList){
            Dictionary<string, int> temp = 
			a.k.Zip(a.v, (k, v) => new { k, v }).ToDictionary(a => a.k, a => a.v);
            _saa.Add(temp);
        }

        _status = kList.Zip(_saa, (k, v) => new { k, v }).ToDictionary(a => a.k, a => a.v);
        raw._status = _status;
    }
}
public class GameEventStatus : MonoBehaviour
{  
    public static GameEventStatus instance;
    SceneManager a;
    public Dictionary<int, Dictionary<string, int>> _status;

    void Awake(){
        if(instance == null){
            instance = this;
            instance._status = new Dictionary<int, Dictionary<string, int>>();
        }
    }
}
