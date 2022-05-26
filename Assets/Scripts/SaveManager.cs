using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SAVE_FILE_NAME = "/saveData.json";

    private List<LevelData> _saveData = new List<LevelData>();
    private List<LevelData> _currentData = new List<LevelData>();

    public static SaveManager Instance = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
    }

    public void Init(List<GameObject> levels)
    {
        _currentData = new List<LevelData>();
        
        for (var i = 0; i < levels.Count; i++)
        {
            _currentData.Add(new LevelData()
            {
                index = i,
                isOpen = false
            });
        }

        _currentData[0].isOpen = true;

        SetSaveData();

        if (_saveData == null || _saveData.Count == 0)
        {
            _saveData = _currentData;
            UpdateData();
        }
    }

    public void UpdateData()
    {
        var saveData = new SaveData
        {
            levels = _currentData
        };
        var saveDataText = JsonUtility.ToJson(saveData);

        Debug.LogError(saveData);
        
        File.WriteAllText(Application.dataPath + SAVE_FILE_NAME, saveDataText);
    }

    public void SetSaveData()
    {
        if (File.Exists(Application.dataPath + SAVE_FILE_NAME))
        {
            var json = File.ReadAllText(Application.dataPath + SAVE_FILE_NAME);
            _saveData = JsonUtility.FromJson<SaveData>(json).levels;
        }
    }
}

[Serializable]
public class SaveData
{
    public List<LevelData> levels;
}

[Serializable]
public class LevelData
{
    public int index;
    public bool isOpen;
}