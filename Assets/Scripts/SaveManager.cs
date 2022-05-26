using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SAVE_FILE_NAME = "/saveData.json";

    private List<LevelData> _saveDataLevels = new List<LevelData>();
    private List<LevelData> _currentData = new List<LevelData>();

    public List<LevelData> SaveDataLevels => _saveDataLevels;

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
                isOpen = false,
                isPassed = false
            });
        }

        _currentData[0].isOpen = true;

        SetSaveData();

        if (_saveDataLevels == null || _currentData.Count != _saveDataLevels.Count)
        {
            UpdateData();
        }
    }

    public void UpdateData()
    {
        for (var i = 0; i < _currentData.Count; i++)
        {
            var level = _currentData[i];

            if (_saveDataLevels.Count <= i)
            {
                Debug.LogError("13");
                _saveDataLevels.Add(level);
            }
        }
        
        var saveData = new SaveData()
        {
            levels = _saveDataLevels
        };

        var saveDataText = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.dataPath + SAVE_FILE_NAME, saveDataText);

        _saveDataLevels = new List<LevelData>();
    }

    public void UpdateLevelData(int index)
    {
        _saveDataLevels[index].isPassed = true;

        if (index + 1 < _saveDataLevels.Count)
        {
            _saveDataLevels[index + 1].isOpen = true;
        }

        var saveData = new SaveData()
        {
            levels = _saveDataLevels
        };

        var saveDataText = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.dataPath + SAVE_FILE_NAME, saveDataText);
    }

    public void SetSaveData()
    {
        if (File.Exists(Application.dataPath + SAVE_FILE_NAME))
        {
            var json = File.ReadAllText(Application.dataPath + SAVE_FILE_NAME);
            _saveDataLevels = JsonUtility.FromJson<SaveData>(json).levels;
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
    public bool isPassed;
}