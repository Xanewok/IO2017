using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/* Might by platform dependent : */
public static class SaveLoad
{

    private static string path = Application.persistentDataPath + "/INFO.dat";

    public static void Save(Data data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        bf.Serialize(file, data);
        file.Close();
    }

    public static Data Load()
    {
        Data data;
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            data = (Data)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            data = new Data();
        }
        return data;
    }
}
