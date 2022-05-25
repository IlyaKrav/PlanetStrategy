using System.Collections;
using UnityEngine;

public class ShipsController : MonoBehaviour
{
    private const int SHIP_HEIGHT = 5;

    [SerializeField] private Transform _shipParent;

    [SerializeField] private Test _test;
    
    public void SendShips(Planet targetPlanet, int shipsCount, GameController.PlayerType attacker, Color shipColor)
    {

        var shipHeightCount = (int)(shipsCount / SHIP_HEIGHT);
        var lastShipHeight = shipsCount % SHIP_HEIGHT;
        
        for (int i = 0; i < shipHeightCount; i++)
        {
            var ship = ShipPoolManager.Instance.GetShip();
            ship.SetColor(shipColor);
            ship.transform.SetParent(_shipParent);
            ship.transform.localPosition = new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
            StartCoroutine(MoveShipsAnimation(targetPlanet, ship, SHIP_HEIGHT, attacker));
        }

        if (lastShipHeight != 0)
        {
            var ship = ShipPoolManager.Instance.GetShip();
            ship.SetColor(shipColor);
            ship.transform.SetParent(_shipParent);
            // ship.transform.localPosition = new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
            ship.transform.localPosition = Vector2.zero;
            StartCoroutine(MoveShipsAnimation(targetPlanet, ship, lastShipHeight, attacker));
        }
    }

    private IEnumerator MoveShipsAnimation(Planet targetPlanet, Ship ship, int shipHeight, GameController.PlayerType attacker)
    {
        var time = 0f;
        var period = 5f;//todo В константу!
        var startPos = (Vector2)ship.transform.position;
        var targetPosition = targetPlanet.transform.position;
        // var endPos = new Vector2(targetPosition.x + Random.Range(0f, 0.5f), targetPosition.y); 
        var endPos = new Vector2(targetPosition.x, targetPosition.y);

        var k = (startPos.y - endPos.y) / (startPos.x - endPos.x);

        var b = endPos.y- k * endPos.x;

        _test.CalculatePoints(k, b);

        while (time < period)
        {
            time += Time.deltaTime;
            var lTime = time / period;
            var lPos = Vector2.Lerp(startPos, endPos, lTime);
            ship.transform.position = lPos;

            yield return null;
        }
        
        ShipPoolManager.Instance.ReturnShip(ship);
        targetPlanet.Attacked(shipHeight, attacker);
    }
}