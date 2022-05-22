using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Planet> _playerPlanets;
    [SerializeField] private List<Planet> _enemyPlanets;
    [SerializeField] private List<Planet> _neutralPlanets;

    [SerializeField] private NavigationItems _playerNavigation;
    [SerializeField] private NavigationItems _enemyNavigation;

    [SerializeField] private SliderShipsController _sliderShipsController;
    [SerializeField] private Slider _winSlider;
    
    private static GameController _instance;
    
    private Planet _selectedPlanet;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }

            return _instance;
        }
    }

    public float SliderShipValue => _sliderShipsController.SliderShipValue;
    
    public Planet SelectedPlanet
    {
        get => _selectedPlanet;
        set
        {
            var enablePlayerNavigation = value == null;
            _enemyNavigation.enabled = !enablePlayerNavigation;
            _playerNavigation.enabled = enablePlayerNavigation;
            _selectedPlanet = value;
        }
    }

    private void Start()
    {
        ActionManager.Instance.CapturePlanet += OnCapturePlanet;
    }

    private void Update()
    {
        UpdateWinSlider();
    }

    private void OnCapturePlanet(Planet capturedPlanet, PlanetType attackerType)
    {
        _playerNavigation.AddItemToEnd(capturedPlanet.Navigation);
        _enemyNavigation.RemoveItem(capturedPlanet.Navigation);
        _enemyNavigation.SelectNextItem();

        SetPlanetToAttacker(capturedPlanet, attackerType);
    }

    public void SetPlanetToAttacker(Planet capturedPlanet, PlanetType attacker)
    {
        switch (attacker)
        {
            case PlanetType.Player:
                _enemyPlanets.Remove(capturedPlanet);
                _playerPlanets.Add(capturedPlanet);
                break;
            
            case PlanetType.FirstEnemy:
                _playerPlanets.Remove(capturedPlanet);
                _enemyPlanets.Add(capturedPlanet);
                break;
        }
    }
    
    public void UnselectPlanets()
    {
        _enemyNavigation.Disable(true);
        _playerNavigation.Enable(false);
        _selectedPlanet = null;
    }

    private void UpdateWinSlider()
    {
        var totalPayerCount = 0;
        var totalEnemyCount = 0;
        
        foreach (var planet in _playerPlanets)
        {
            totalPayerCount += planet.ShipsCount;
        }
        
        foreach (var planet in _enemyPlanets)
        {
            totalEnemyCount += planet.ShipsCount;
        }

        _winSlider.value = (totalPayerCount * 100) / (totalPayerCount + totalEnemyCount);
    }

    private void OnDestroy()
    {
        ActionManager.Instance.CapturePlanet -= OnCapturePlanet;
    }

    public enum PlanetType
    {
        Empty,
        Player,
        FirstEnemy
    }
}
