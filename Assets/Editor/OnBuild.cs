using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Text.RegularExpressions;
using System.IO;

public class OnPostBuild : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPostprocessBuild(BuildReport report)
    {
		var lines = Regex.Split (report.summary.outputPath, "/");
        string path = "";
        for(int i = 0; i < lines.Length - 1; i++){
            path += lines[i] + "/";
        }

        path += "Dialogues/Scripts/";
        
        DirectoryInfo di = new DirectoryInfo(path);

        if(di.Exists == false){
            di.Create();
        }

        di = new System.IO.DirectoryInfo(Application.dataPath + "/Resources/Dialogues/Scripts/");
        foreach (System.IO.FileInfo File in di.GetFiles())
        {
            if (File.Extension.ToLower().CompareTo(".csv") == 0)
            {
                File.CopyTo(path + File.Name, true);
            }
        }
    }

class OnPreBuild : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        PlayerSettings.bundleVersion = System.DateTime.Today.ToString("MM.dd");
    }
}
}
