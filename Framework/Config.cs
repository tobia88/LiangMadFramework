using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config {
    public const string SceneDataPath = "Datas/Scenes";
    public const string ProjectRootPath = "Assets/_Project";

    public static string SceneSaveDataPath { get { return ProjectRootPath + "/Resources/" + SceneDataPath; } }
}
