using UnityEngine;

public static class DataHolder
{
    private static GameObject _currentLevel;

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