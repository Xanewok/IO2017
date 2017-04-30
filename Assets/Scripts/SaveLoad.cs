using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    private static string path = Application.persistentDataPath + "/INFO.dat";

    public static void Save(Data data)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            bf.Serialize(fs, data);
        }
        finally
        {
            fs.Close();
        }
    }

    public static Data Load()
    {
        Data data;
        if (File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                data = (Data) bf.Deserialize(fs);
            }
            finally
            {
                fs.Close();
            }
        }
        else
        {
            data = new Data();
        }
        return data;
    }
}
