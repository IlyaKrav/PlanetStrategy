using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private NavigationItems _playerNavigation;
    [SerializeField] private NavigationItems _enemyNavigation;

    [SerializeField] private SliderShipsController _sliderShipsController;
    
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

    public void CapturePlanet(Planet capturedPlanet)
    {
        _playerNavigation.AddItemToEnd(capturedPlanet.Navigation);
        _enemyNavigation.RemoveItem(capturedPlanet.Navigation);
        _enemyNavigation.SelectNextItem();
    }

    public void UnselectPlanets()
    {
        _enemyNavigation.enabled = false;
        _playerNavigation.enabled = true;
        _selectedPlanet = null;

        _enemyNavigation.UnselectItems();
    }

    public enum PlanetType
    {
        Empty,
        Player,
        FirstEnemy
    }
}
