using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteChar : MonoBehaviour
{
    public List<CharacterData> characters;
    public void deleteChar(){
        foreach (var item in characters)
        {
            GameSystem.instance.exitChar(item);
        }
    }
}
