using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class DataMng : MonoBehaviour {
    private static DataMng _Instance;

    public static DataMng Instance {
        get {
            if (GameCtrl.Instance == null) {
                GameCtrl.Init();
            }

            return _Instance;
        }
    }

    public static string DefaultPath {
        get { return Application.persistentDataPath; }
    }


    public static void Init() {
        var mng = FindObjectOfType<DataMng>();

        if (mng != null)
            Destroy(mng.gameObject);

        var go = new GameObject("DataMng");

        _Instance = go.AddComponent<DataMng>();
        _Instance.DataPath = DefaultPath;

        DontDestroyOnLoad(go);
    }

    public string DataPath { get; private set; }

    public void SetDataPath(string p_path) {
        DataPath = p_path;
    }

    public void Save(object p_data, string p_fileName) {
        string path = DataPath + "/" + p_fileName;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(path);
        bf.Serialize(fs, p_data);
        fs.Close();

        Debug.Log("File Saved, Path: " + path);
    }

    public T Load<T>(string p_fileName) {
        string path = DataPath + "/" + p_fileName;

        if (!File.Exists(path))
            return default(T);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(path, FileMode.Open);
        T retval = (T) bf.Deserialize(fs);
        fs.Close();

        Debug.Log("File Loaded, Path: " + path);

        return retval;
    }
}
