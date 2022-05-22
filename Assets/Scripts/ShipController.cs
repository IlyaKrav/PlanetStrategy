using System.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private const int SHIP_HEIGHT = 5;

    [SerializeField] private Transform _shipParent;

    public void SendShips(Planet targetPlanet, int shipsCount)
    {
        var shipHeightCount = (int)(shipsCount / SHIP_HEIGHT);
        var lastShipHeight = shipsCount % SHIP_HEIGHT;
        
        for (int i = 0; i < shipHeightCount; i++)
        {
            var ship = ShipPoolManager.Instance.GetShip();
            ship.transform.SetParent(_shipParent);
            ship.transform.localPosition = new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
            StartCoroutine(MoveShipsAnimation(targetPlanet, ship, SHIP_HEIGHT));
        }

        if (lastShipHeight != 0)
        {
            var ship = ShipPoolManager.Instance.GetShip();
            ship.transform.SetParent(_shipParent);
            ship.transform.localPosition = new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
            StartCoroutine(MoveShipsAnimation(targetPlanet, ship, lastShipHeight));
        }
    }

    private IEnumerator MoveShipsAnimation(Planet targetPlanet, GameObject ship, int shipHeight)
    {
        var time = 0f;
        var period = 5f;//todo В константу!
        var startPos = (Vector2)ship.transform.position;
        var targetPosition = targetPlanet.transform.position;
        var endPosition = new Vector2(targetPosition.x + Random.Range(0f, 0.5f),
            targetPosition.y + Random.Range(0f, 0.5f)); 

        while (time < period)
        {
            time += Time.deltaTime;
            var lTime = time / period;
            var lPos = Vector2.Lerp(startPos, endPosition, lTime);
            ship.transform.position = lPos;

            yield return null;
        }
        
        ShipPoolManager.Instance.ReturnShip(ship);
        targetPlanet.Attacked(shipHeight, GameController.PlanetType.Player);
    }
}