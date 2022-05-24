using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<PlayerController> _playersList;

    [SerializeField] private NavigationItems _playerNavigation;
    [SerializeField] private NavigationItems _enemyNavigation;

    [SerializeField] private SliderShipsController _sliderShipsController;
    [SerializeField] private Slider _winSlider;

    private Dictionary<PlayerType, PlayerController> _playersDictionary = new Dictionary<PlayerType, PlayerController>();

    private Planet _selectedPlanet;
    
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
    
    private static GameController _instance;

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

    private void Start()
    {
        ActionManager.Instance.CapturePlanet += OnCapturePlanet;

        foreach (var player in _playersList)
        {
            _playersDictionary.Add(player.PlayerType, player);
        }
    }

    private void Update()
    {
        UpdateWinSlider();
    }

    private void OnCapturePlanet(Planet capturedPlanet, PlayerType attackerType, PlayerType attacking)
    {
        if (attackerType == PlayerType.Player)
        {
            _playerNavigation.AddItemToEnd(capturedPlanet.Navigation);
            _enemyNavigation.RemoveItem(capturedPlanet.Navigation);
            _enemyNavigation.SelectNextItem();
        }

        if (!capturedPlanet.IsSpawnShips)
        {
            capturedPlanet.StartSpawnShips();
        }
        
        SetPlanetToAttacker(capturedPlanet, attackerType, attacking);
    }

    public void SetPlanetToAttacker(Planet capturedPlanet, PlayerType attacker, PlayerType attacking)
    {
        var player = _playersDictionary[attacker];
        var enemy = _playersDictionary[attacking];
        
        player.Planets.Add(capturedPlanet);
        enemy.Planets.Remove(capturedPlanet);

        capturedPlanet.SetPlanetColor(player.PlanetsColor);

        if (enemy.Planets.Count == 0)
        {
            _playersList.Remove(enemy);
            _playersDictionary.Remove(attacking);
        }
    }

    public void AttackRandomPlanet(Planet attackerPlanet, PlayerType attackerType, int shipsCount)
    {
        if (_playersList.Count == 1)
        {
            foreach (var player in _playersList)
            {
                if (player is BotController bot)
                {
                    bot.StopAllCoroutines();
                }
            }
        }
        
        var temporaryPlayers = new List<PlayerController>();

        foreach (var player in _playersList)
        {
            if (attackerType == player.PlayerType || player.Planets.Count == 0)
            {
                continue; 
            }
            
            temporaryPlayers.Add(player);
        }

        var enemy = temporaryPlayers[Random.Range(0, temporaryPlayers.Count)];
        var attackingPlanet = enemy.Planets[Random.Range(0, enemy.Planets.Count)];
        
        attackerPlanet.SendShips(shipsCount, attackingPlanet, attackerType);
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
        var totalEnemiesCount = 0;

        foreach (var player in _playersList)
        {
            if (player.PlayerType == PlayerType.Player)
            {
                foreach (var planet in player.Planets)
                {
                    totalPayerCount += planet.ShipsCount;
                }
            }
            else
            {
                foreach (var planet in player.Planets)
                {
                    totalEnemiesCount += planet.ShipsCount;
                }
            }
        }

        _winSlider.value = (totalPayerCount * 100) / (totalPayerCount + totalEnemiesCount);
    }

    private void OnDestroy()
    {
        ActionManager.Instance.CapturePlanet -= OnCapturePlanet;
    }

    public enum PlayerType
    {
        Neutral,
        Player,
        FirstEnemy
    }
}
