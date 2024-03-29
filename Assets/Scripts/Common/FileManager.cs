﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManager<T> where T : struct
{
    //JSON 파일을 로드하는 로직(T타입 으로 인하여 어떠한 형태의 스트럭트 적용가능)
    public static T? Load(string fileName)
    {
        T? items = null;

        string filePath = $"{Application.persistentDataPath}\\{fileName}";

        if (File.Exists(filePath))
        {
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string jsonStr = streamReader.ReadToEnd();
                items = JsonUtility.FromJson<T>(jsonStr);
            }
        }
        return items;
    }
    //JSON 파일을 저장하는 로직 
    public static void Save(T items, string fileName)
    {
        string filePath = $"{Application.persistentDataPath}\\{fileName}";

        using (StreamWriter streamWriter = new StreamWriter(filePath))
        {
            string jsonStr = JsonUtility.ToJson(items);
            streamWriter.Write(jsonStr);
        }
    }
}