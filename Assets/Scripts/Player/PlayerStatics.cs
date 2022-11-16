using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatics
{
    public static bool isDialogueOn = false;
    public static bool isDialogueBtnOn = false;
    public static bool isMenuOn = false;
    public static bool isCutSceneOn = false;
    public static bool isSceneChange = false;
    public static void resetStatics(){
        isDialogueOn = false;
        isDialogueBtnOn = false;
        isMenuOn = false;
        isCutSceneOn = false;
        isSceneChange = false;
    }
    public static bool isCanMove(){
        return isDialogueOn || isMenuOn || isCutSceneOn || isSceneChange;
    }
}
