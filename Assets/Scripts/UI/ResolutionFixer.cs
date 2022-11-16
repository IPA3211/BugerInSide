using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Windows;

[System.Serializable]
public class Resolutions{
    public int w;
    public int h;

    public override string ToString()
    {
        return w + " X " + h;
    }
}

public class ResolutionFixer : MonoBehaviour
{
    public static ResolutionFixer instance;
    public List<Resolutions> resolutions;
    public bool mode;
    int index = -1;
    public int indexLimit = 0;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        var m  = PlayerPrefs.GetInt("ScreenMode", -1);
        index = PlayerPrefs.GetInt("DisplayIndex", -1);
        if(index < 0){
            setIndex(findFitResolution());
        }
        else{
            findFitResolution();
        }

        if(m < 0){
            mode = Screen.fullScreen;
        }
        else{
            mode = (m == 1);
        }

        SetResolution();
        SceneManager.sceneLoaded += ChangedActiveScene;
    }

    public void setIndex(int i){
        index = i;
        PlayerPrefs.SetInt("DisplayIndex", index);
    }

    public void setMode(bool m){
        mode = m;
        PlayerPrefs.SetInt("ScreenMode", m ? 1 : 0);
    }

    public int getIndex(){
        return index;
    }

    public int findFitResolution(){
        for(int i = resolutions.Count - 1; i >= 0; i--){
            if(resolutions[i].w <= Display.main.systemWidth && resolutions[i].h <= Display.main.systemHeight) {
                indexLimit = i;
                Debug.Log(resolutions[i].w + " " + resolutions[i].h);
                return i;
            }
        }
        return 0;
    }

    private void ChangedActiveScene(Scene scene, LoadSceneMode mode)
    {
        SetResolution();
    }

    public void SetResolution()
    {
        int setWidth = resolutions[index].w;
        int setHeight = resolutions[index].h;

        int deviceWidth = Display.main.systemHeight; // 기기 너비 저장
        int deviceHeight = Display.main.systemWidth; // 기기 높이 저장
        PlayerPrefs.SetInt("Screenmanager Resolution Width", setWidth);
        PlayerPrefs.SetInt("Screenmanager Resolution Height", setHeight);
        PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", (int)(mode ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed));
        
        //Screen.SetResolution(setWidth, setWidth, mode ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed); // SetResolution 함수 제대로 사용하기

        
        //Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), mode ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed); // SetResolution 함수 제대로 사용하기
        
        Screen.SetResolution(setWidth, setHeight, mode ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
        
        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }

        Debug.Log("device resolution : " + deviceWidth + ", " + deviceHeight);
        Debug.Log("setting resolution : " + setWidth + ", " + setHeight);
        Debug.Log("actual set resolution : " + Screen.width + ", " + Screen.height);
    }
}
