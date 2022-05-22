using UnityEngine;
using UnityEngine.Events;

public class ActionManager : MonoBehaviour
{
    public UnityAction<Planet, GameController.PlanetType> CapturePlanet;

    public static ActionManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
