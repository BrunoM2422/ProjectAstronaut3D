using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    [NaughtyAttributes.Button("Save")]
    private void Save()
    {
        SaveSetup setup = new SaveSetup();
        setup.lastLevel = 1;
        setup.playerName = "Bruno";

        string setupToJson = JsonUtility.ToJson(setup, true);
        Debug.Log(setupToJson);
        SaveFile(setupToJson);
    }

    private void SaveFile(string json)
    {
        string path = Application.dataPath + "/save.txt";


        Debug.Log(path);
        File.WriteAllText(path, json);

    }

}

[System.Serializable]
public class SaveSetup
{
    public int lastLevel;
    public string playerName;
}
