using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    private const float SPAWN_SHIPS_DELAY = 2f;

    [SerializeField] private GameController.PlayerType _playerType;
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

    public GameController.PlayerType PlayerType
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
    }

    public void Init(GameController.PlayerType playerType, Color planetColor)
    {
        _playerType = playerType;
        SetPlanetColor(planetColor);
    }

    public void SetPlanetColor(Color planetColor)
    {
        _planetRenderer.color = planetColor;
    }
    
    public void CapturePlanet(GameController.PlayerType captureType)
    {
        // _navigation.SelectItem(false);

        // switch (captureType)
        // {
        //     case GameController.PlayerType.Player:
        //         _navigation.SelectCover = _playerSelectCover;
        //         _playerMark.SetActive(true);
        //         _enemyMark.SetActive(false);
        //
        //         break;
        //     case GameController.PlayerType.FirstEnemy:
        //         _navigation.SelectCover = _enemySelectCover;
        //         _playerMark.SetActive(false);
        //         _enemyMark.SetActive(true);
        //
        //         break;
        // }
    }

    public void Attacked(int enemyShips, GameController.PlayerType attackerType)
    {
        if (attackerType == _playerType)
        {
            _shipsCount += enemyShips;
            return;
        }

        if (enemyShips >= _shipsCount)
        {
            enemyShips -= _shipsCount;
            _shipsCount = enemyShips;

            ActionManager.Instance.CapturePlanet?.Invoke(this, attackerType, _playerType);
            CapturePlanet(attackerType);
            _playerType = attackerType;
        }
        else
        {
            _shipsCount -= enemyShips;
        }

        SetPlanetShipsCount();
    }

    public void SendShips(int shipsCount, Planet targetPlanet, GameController.PlayerType attaker)
    {
        shipsController.SendShips(targetPlanet, shipsCount, attaker, _planetRenderer.color);
        
        _shipsCount -= shipsCount;
        SetPlanetShipsCount();
    }

    public void SelectPlanet()
    {
        switch (_playerType)
        {
            case GameController.PlayerType.Player:
                GameController.Instance.SelectedPlanet = this;

                break;
            default:

                if (GameController.Instance.SelectedPlanet != null)
                {
                    var attackerPlanet = GameController.Instance.SelectedPlanet;
                    var shipsInPercent = (float) attackerPlanet.ShipsCount / 10; //todo В константу!!
                    var shipsCount = (int)Mathf.Ceil(shipsInPercent * GameController.Instance.SliderShipValue);
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