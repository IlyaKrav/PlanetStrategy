using UnityEngine;

public class ShipPoolManager : MonoBehaviour
{
    [SerializeField] private Transform _poolParent;
    [SerializeField] private GameObject _shipPrefab;

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
    
    public GameObject GetShip()
    {
        var shipCount = _poolParent.childCount;

        GameObject ship;
        
        if (shipCount > 0)
        {
            ship = _poolParent.GetChild(0).gameObject;
        }
        else
        {
            ship = Instantiate(_shipPrefab, _poolParent);
        }
        
        ship.SetActive(true);
        return ship;
    }

    public void ReturnShip(GameObject ship)
    {
        ship.SetActive(false);
        ship.transform.SetParent(_poolParent);
    }
}
