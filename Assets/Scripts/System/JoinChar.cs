using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinChar : MonoBehaviour
{
    public List<CharacterData> characters;
    public void joinChar(){
        foreach (var item in characters)
        {
            GameSystem.instance.joinChar(item);
        }
    }
}
