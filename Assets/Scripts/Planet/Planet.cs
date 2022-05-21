using System.Collections;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private const float SPAWN_SHIPS_DELAY = 2f;
    
    [SerializeField] private GameController.PlanetType _planetType;
    [SerializeField] private int _shipsCount;
    [SerializeField] private NavigationItem _navigation;

    [SerializeField] private GameObject _playerSelectCover;
    [SerializeField] private GameObject _enemySelectCover;
    
    private bool _selectPlanet;

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
                            
                break;
            case GameController.PlanetType.FirstEnemy:
                _navigation.SelectCover = _enemySelectCover;

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
                            
                break;
            case GameController.PlanetType.FirstEnemy:
                _navigation.SelectCover = _enemySelectCover;

                break;
        }
    }

    public void Attacked(int enemyShips, GameController.PlanetType attackerType)
    {
        _shipsCount -= enemyShips;

        if (_shipsCount <= 0)
        {
            GameController.Instance.CapturePlanet(this);
            CapturePlanet(attackerType);
            _planetType = attackerType;
        }
    }

    public void SendShips(int shipsCount, Planet targetPlanet)
    {
        targetPlanet.Attacked(shipsCount, GameController.PlanetType.Player);
    }
    
    public void SelectPlanet()
    {
        switch (_planetType)
        {
            case GameController.PlanetType.Player:
                GameController.Instance.SelectedPlanet = this;
                Debug.Log("Planet " + gameObject.name + " selected");
                            
                break;
            case GameController.PlanetType.FirstEnemy:
                            
                if (GameController.Instance.SelectedPlanet != null)
                {
                    GameController.Instance.SelectedPlanet.SendShips(1, this);
                                
                    Debug.Log("Enemy ships = " + _shipsCount);
                }
                            
                break;
        }
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
        
            yield return new WaitForSecondsRealtime(SPAWN_SHIPS_DELAY);
        }
    }
}