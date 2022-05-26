using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    private const float SPAWN_SHIPS_DELAY = 2f;

    [SerializeField] private LevelController.PlayerType _playerType;
    [SerializeField] private int _shipsCount;
    [SerializeField] private NavigationItem _navigation;
    [SerializeField] private ShipsController shipsController;

    [SerializeField] private SpriteRenderer _planetRenderer;

    [SerializeField] private Text _countShipText;

    [SerializeField] private bool _spawnShips = true;
    
    private bool _selectPlanet;

    public int ShipsCount => _shipsCount;

    public bool IsSpawnShips => _spawnShips;
    
    public NavigationItem Navigation => _navigation;

    public LevelController.PlayerType PlayerType
    {
        get => _playerType;
        set => _playerType = value;
    }

    private void Start()
    {
        if (_spawnShips)
        {
            StartSpawnShips();
        }
        
        SetPlanetShipsCount();
    }

    public void Init(LevelController.PlayerType playerType, Color planetColor)
    {
        _playerType = playerType;
        SetPlanetColor(planetColor);
    }

    public void SetPlanetColor(Color planetColor)
    {
        _planetRenderer.color = planetColor;
    }

    public void Attacked(int enemyShips, LevelController.PlayerType attackerType)
    {
        if (attackerType == _playerType)
        {
            _shipsCount += enemyShips;
            SetPlanetShipsCount();
            return;
        }

        if (enemyShips >= _shipsCount)
        {
            enemyShips -= _shipsCount;
            _shipsCount = enemyShips;

            ActionManager.Instance.CapturePlanet?.Invoke(this, attackerType, _playerType);
            _playerType = attackerType;
        }
        else
        {
            _shipsCount -= enemyShips;
        }

        SetPlanetShipsCount();
    }

    public void SendShips(int shipsCount, Planet targetPlanet, LevelController.PlayerType attaker)
    {
        var overPlanets = LevelController.Instance.AllPlanets;
        overPlanets.Remove(targetPlanet);
        overPlanets.Remove(this);
        
        shipsController.SendShips(targetPlanet, shipsCount,  attaker, _planetRenderer.color, overPlanets);
        
        _shipsCount -= shipsCount;
        SetPlanetShipsCount();
    }

    public void SelectPlanet()
    {
        switch (_playerType)
        {
            case LevelController.PlayerType.Player:
                LevelController.Instance.SelectedPlanet = this;
                
                break;
            default:

                if (LevelController.Instance.SelectedPlanet != null)
                {
                    var attackerPlanet = LevelController.Instance.SelectedPlanet;
                    var shipsInPercent = (float) attackerPlanet.ShipsCount / 10; //todo В константу!!
                    var shipsCount = (int)Mathf.Ceil(shipsInPercent * LevelController.Instance.SliderShipValue);

                    attackerPlanet.SendShips(shipsCount, this, attackerPlanet.PlayerType);
                }

                break;
        }
    }

    private void SetPlanetShipsCount()
    {
        _countShipText.text = _shipsCount.ToString();
    }

    public void StartSpawnShips()
    {
        _spawnShips = true;
        StartCoroutine(SpawnShips());
    }

    private IEnumerator SpawnShips()
    {
        while (true)
        {
            _shipsCount++;
            SetPlanetShipsCount();

            yield return new WaitForSecondsRealtime(SPAWN_SHIPS_DELAY);
        }
    }
}