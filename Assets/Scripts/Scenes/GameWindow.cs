using UnityEngine;

public class GameWindow : GUIScreens
{
    void Awake()
    {
        var level = DataHolder.CurrentLevel;

        if (level != null)
        {
            Instantiate(level);
        }
    }
}
