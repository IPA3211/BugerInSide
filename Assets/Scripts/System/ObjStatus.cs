using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ObjStatus : MonoBehaviour
{
    public int _status = 0;

    public List<UnityEvent> _statusEvents;

    void Start()
    {
        if(GameEventStatus.instance._status.ContainsKey(SceneManager.GetActiveScene().buildIndex)){
            if(GameEventStatus.instance._status[SceneManager.GetActiveScene().buildIndex].ContainsKey(this.name)){
                _status = GameEventStatus.instance._status[SceneManager.GetActiveScene().buildIndex][this.name];
            }
        }
        checkStatusChange();
    }

    public void setStatusValue(int newValue){
        _status = newValue;
        if(GameEventStatus.instance._status.ContainsKey(SceneManager.GetActiveScene().buildIndex)){
            if(GameEventStatus.instance._status[SceneManager.GetActiveScene().buildIndex].ContainsKey(this.name)){
                GameEventStatus.instance._status[SceneManager.GetActiveScene().buildIndex][this.name] = _status;
            }
            else{
                GameEventStatus.instance._status[SceneManager.GetActiveScene().buildIndex].Add(this.name, _status);
            }
        }
        else{
            Dictionary<string, int> temp = new Dictionary<string, int>();
            temp.Add(this.name, _status);
            GameEventStatus.instance._status.Add(SceneManager.GetActiveScene().buildIndex, temp);
        }
        checkStatusChange();
    }
    
    public void checkStatusChange(){
        _statusEvents[_status].Invoke();
    }
}
