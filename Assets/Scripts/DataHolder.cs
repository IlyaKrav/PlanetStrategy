using UnityEngine;

public static class DataHolder
{
    private static GameObject _currentLevel;
    private static int _currentLevelIndex;

    public static int CurrentLevelIndex
    {
        get
        {
            return _currentLevelIndex;
        }
        set
        {
            _currentLevelIndex = value;
        }
    }
    
    public static GameObject CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
        set
        {
            _currentLevel = value;
        }
    }
}