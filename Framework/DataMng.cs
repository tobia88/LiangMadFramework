using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class DataMng : MonoBehaviour {
    public static DataMng Instance { get; private set; }

    public static void Init() {
        Instance = FindObjectOfType<DataMng>();
    }

    public void Save(object p_data, string p_path) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Create(p_path);
        bf.Serialize(fs, p_data);
        fs.Close();

        Debug.Log("File Saved, Path: " + p_path);
    }

    public T Load<T>(string p_path) {
        if (!File.Exists(p_path))
            return default(T);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(p_path, FileMode.Open);
        T retval = (T) bf.Deserialize(fs);
        fs.Close();

        Debug.Log("File Loaded, Path: " + p_path);

        return retval;
    }
}
