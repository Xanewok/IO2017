using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

namespace YAGTSS.Serialization
{
    public static class SaveGameSerializer
    {
        public const string SaveDataFileName = "savedata.dat";
        public static readonly string DefaultSavePath = Application.persistentDataPath + "/" + SaveDataFileName;

        public static void Save(SaveData data)
        {
            Save(data, DefaultSavePath);
        }

        public static void Save(SaveData data, string path)
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

        public static SaveData Load()
        {
            return Load(DefaultSavePath);
        }

        public static SaveData Load(string path)
        {
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    return bf.Deserialize(fs) as SaveData;
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    return new SaveData();
                }
                finally
                {
                    fs.Close();
                }
            }

            return new SaveData();
        }
    }
}
