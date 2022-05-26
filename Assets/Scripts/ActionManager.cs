using System;
using UnityEngine;
using UnityEngine.Events;

public class ActionManager : MonoBehaviour
{
    public UnityAction<Planet, LevelController.PlayerType, LevelController.PlayerType> CapturePlanet;

    public static ActionManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        CapturePlanet = null;
    }
}
