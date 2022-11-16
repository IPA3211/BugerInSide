using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneInfo{
    public string sceneName;
    public Vector2 position;
}

public class ChangeScene : MonoBehaviour
{
    public SceneInfo sceneToChange;

    public void loadScene(){
        loadScene(sceneToChange);
    }

    public void loadSceneWithoutEffect(){
        loadSceneWithoutEffect(sceneToChange);
    }

    public static void loadScene(SceneInfo si){
        PlayerStatics.isSceneChange = true;
        ChangeSceneEffect.instance.normalChangeScene(() => {
            PlayerStatics.isSceneChange = false;
            PlayerStatics.resetStatics();
            GameStatics.setSceneChangePos(si.position);
            SceneManager.LoadScene(si.sceneName);
        });
    }

    public static void loadSceneWithoutEffect(SceneInfo si){
        GameStatics.setSceneChangePos(si.position);
        SceneManager.LoadScene(si.sceneName);
    }
}
