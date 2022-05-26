using UnityEngine;

public class GameWindow : MonoBehaviour
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
