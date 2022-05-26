using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] private List<PlayerController> _playersList;

    [SerializeField] private NavigationItems _userNavigation;
    [SerializeField] private NavigationItems _enemyNavigation;

    [SerializeField] private SliderShipsController _sliderShipsController;
    [SerializeField] private Slider _winSlider;

    private Dictionary<PlayerType, PlayerController>
        _playersDictionary = new Dictionary<PlayerType, PlayerController>();

    private Planet _selectedPlanet;

    public float SliderShipValue => _sliderShipsController.SliderShipValue;

    public List<Planet> AllPlanets
    {
        get
        {
            var allPlanets = new List<Planet>();

            foreach (var player in _playersList)
            {
                allPlanets.AddRange(player.Planets);
            }
            
            return allPlanets;
        }  
    }

    public Planet SelectedPlanet
    {
        get => _selectedPlanet;
        set
        {
            var enablePlayerNavigation = value == null;
            _enemyNavigation.enabled = !enablePlayerNavigation;
            _userNavigation.enabled = enablePlayerNavigation;

            if (!enablePlayerNavigation)
            {
                _enemyNavigation.SelectFirstItem();
            }

            _selectedPlanet = value;
        }
    }

    private static LevelController _instance;

    public static LevelController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LevelController>();
            }

            return _instance;
        }
    }

    private void Start()
    {
        ActionManager.Instance.CapturePlanet += OnCapturePlanet;

        foreach (var player in _playersList)
        {
            if (player.PlayerType == PlayerType.Player)
            {
                foreach (var planet in player.Planets)
                {
                    _userNavigation.AddItemToEnd(planet.Navigation);
                }
            }
            else
            {
                foreach (var planet in player.Planets)
                {
                    _enemyNavigation.AddItemToEnd(planet.Navigation);
                }
            }

            _playersDictionary.Add(player.PlayerType, player);
        }

        _userNavigation.Init();
    }

    private void Update()
    {
        CountTotalShips();
    }

    private void OnCapturePlanet(Planet capturedPlanet, PlayerType attackerType, PlayerType attacking)
    {
        var planetNavigation = capturedPlanet.Navigation;

        if (attacking == PlayerType.Player && attackerType == PlayerType.Player)
        {
            //nothing
        }
        else if (attackerType == PlayerType.Player)
        {
            _userNavigation.AddItemToEnd(planetNavigation);
            _enemyNavigation.RemoveItem(planetNavigation);

            if (planetNavigation.IsSelected)
            {
                if (attacking != PlayerType.Player)
                {
                    _enemyNavigation.UnselectItem(planetNavigation);
                }
            }
        }
        else if (attacking == PlayerType.Player)
        {
            if (planetNavigation.IsSelected)
            {
                _userNavigation.SelectNextItem();
            }

            _enemyNavigation.AddItemToEnd(planetNavigation);
            _userNavigation.RemoveItem(planetNavigation);
        }

        if (!capturedPlanet.IsSpawnShips)
        {
            capturedPlanet.StartSpawnShips();
        }

        SoundController.Instance.OnCapture();
        SetPlanetToAttacker(capturedPlanet, attackerType, attacking);
    }

    public void SetPlanetToAttacker(Planet capturedPlanet, PlayerType attacker, PlayerType attacking)
    {
        var player = _playersDictionary[attacker];
        var enemy = _playersDictionary[attacking];

        player.Planets.Add(capturedPlanet);
        enemy.Planets.Remove(capturedPlanet);

        capturedPlanet.SetPlanetColor(player.PlanetsColor);
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

        if (temporaryPlayers.Count == 0)
        {
            return;
        }

        var enemy = temporaryPlayers[Random.Range(0, temporaryPlayers.Count)];
        var attackingPlanet = enemy.Planets[Random.Range(0, enemy.Planets.Count)];

        attackerPlanet.SendShips(shipsCount, attackingPlanet, attackerType);
    }

    public void UnselectPlanets()
    {
        HintsController.Instance.ChangeStateTo(HintsController.HintsState.SelectPlanet);
        _enemyNavigation.Disable(true);
        _userNavigation.UnselectItems();
        _userNavigation.Enable();
        _selectedPlanet = null;
    }

    private void CountTotalShips()
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
            else if (player.PlayerType != PlayerType.Neutral)
            {
                foreach (var planet in player.Planets)
                {
                    totalEnemiesCount += planet.ShipsCount;
                }
            }
        }

        CheckWinner(totalPayerCount, totalEnemiesCount);
        UpdateWinSlider(totalPayerCount, totalEnemiesCount);
    }

    private void CheckWinner(float totalPayerCount, float totalEnemiesCount)
    {
        if (totalPayerCount == 0 && _playersDictionary[PlayerType.Player].Planets.Count == 0)
        {
            SceneManager.LoadSceneAsync("LoseWindow", LoadSceneMode.Additive);
            enabled = false;
            return;
        }
        
        if (totalEnemiesCount == 0)
        {
            SaveManager.Instance.UpdateLevelData(DataHolder.CurrentLevelIndex);
            SceneManager.LoadSceneAsync("WinWindow", LoadSceneMode.Additive);
            enabled = false;
            return;
        }
    }
    
    private void UpdateWinSlider(float totalPayerCount, float totalEnemiesCount)
    {
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
        EnemyOne,
        EnemyTwo,
        EnemyThree,
        EnemyFour,
        EnemyFive
    }
}