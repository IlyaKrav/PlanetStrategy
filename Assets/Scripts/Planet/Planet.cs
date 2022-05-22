using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    private const float SPAWN_SHIPS_DELAY = 2f;
    
    [SerializeField] private GameController.PlanetType _planetType;
    [SerializeField] private int _shipsCount;
    [SerializeField] private NavigationItem _navigation;

    [SerializeField] private GameObject _playerSelectCover;
    [SerializeField] private GameObject _enemySelectCover;
    [SerializeField] private GameObject _playerMark;
    [SerializeField] private GameObject _enemyMark;

    [SerializeField] private Text _countShipText;
    
    private bool _selectPlanet;

    public int ShipsCount => _shipsCount;
    
    public NavigationItem Navigation => _navigation;
    
    public GameController.PlanetType PlanetType
    {
        set => _planetType = value;
    }

    private void Start()
    {
        StartSpawnShips();

        switch (_planetType)
        {
            case GameController.PlanetType.Player:
                _navigation.SelectCover = _playerSelectCover;
                _playerMark.SetActive(true);
                _enemyMark.SetActive(false);
                            
                break;
            case GameController.PlanetType.FirstEnemy:
                _navigation.SelectCover = _enemySelectCover;
                _playerMark.SetActive(false);
                _enemyMark.SetActive(true);

                break;
            default:
                _navigation.SelectCover = _enemySelectCover;
                _playerMark.SetActive(false);
                _enemyMark.SetActive(false);
                break;
        }
    }

    public void CapturePlanet(GameController.PlanetType captureType)
    {
        _navigation.SelectItem(false);
        
        switch (captureType)
        {
            case GameController.PlanetType.Player:
                _navigation.SelectCover = _playerSelectCover;
                _playerMark.SetActive(true);
                _enemyMark.SetActive(false);
                            
                break;
            case GameController.PlanetType.FirstEnemy:
                _navigation.SelectCover = _enemySelectCover;
                _playerMark.SetActive(false);
                _enemyMark.SetActive(true);

                break;
        }
    }

    public void Attacked(int enemyShips, GameController.PlanetType attackerType)
    {
        if (enemyShips >= _shipsCount)
        {
            enemyShips -= _shipsCount;
            _shipsCount = enemyShips;
            
            GameController.Instance.CapturePlanet(this);
            CapturePlanet(attackerType);
            _planetType = attackerType;
        }
        else
        {
            _shipsCount -= enemyShips;
        }
        SetPlanetShipsCount();
    }

    public void SendShips(int shipsCount, Planet targetPlanet)
    {
        _shipsCount -= shipsCount;
        SetPlanetShipsCount();
        targetPlanet.Attacked(shipsCount, GameController.PlanetType.Player);
    }
    
    public void SelectPlanet()
    {
        switch (_planetType)
        {
            case GameController.PlanetType.Player:
                GameController.Instance.SelectedPlanet = this;
                            
                break;
            default:
                            
                if (GameController.Instance.SelectedPlanet != null)
                {
                    var attackingPlanet = GameController.Instance.SelectedPlanet;
                    var a = (float)attackingPlanet.ShipsCount / 10;//todo В константу!!
                    var shipsCount =  (int)(a * GameController.Instance.SliderShipValue);
                    attackingPlanet.SendShips(shipsCount, this);
                }
                            
                break;
        }
    }

    private void SetPlanetShipsCount()
    {
        _countShipText.text = _shipsCount.ToString();
    }

    private void StartSpawnShips()
    {
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