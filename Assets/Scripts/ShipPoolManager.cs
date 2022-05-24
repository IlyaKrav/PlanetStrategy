using System.Collections.Generic;
using UnityEngine;

public class ShipPoolManager : MonoBehaviour
{
    [SerializeField] private Transform _poolParent;
    [SerializeField] private Ship _shipPrefab;

    private List<Ship> _ships = new List<Ship>();

    private static ShipPoolManager _instance;

    public static ShipPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ShipPoolManager>();
            }

            return _instance;
        }
    }
    
    public Ship GetShip()
    {
        var shipCount = _ships.Count;

        Ship ship;
        
        if (shipCount > 0)
        {
            ship = _ships[0];
            _ships.Remove(ship);
        }
        else
        {
            ship = Instantiate(_shipPrefab, _poolParent);
            _ships.Add(ship);
        }
        
        ship.gameObject.SetActive(true);
        return ship;
    }

    public void ReturnShip(GameObject ship)
    {
        ship.SetActive(false);
        ship.transform.SetParent(_poolParent);
    }
}
