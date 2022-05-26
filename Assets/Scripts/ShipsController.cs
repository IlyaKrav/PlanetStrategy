using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipsController : MonoBehaviour
{
    private const int SHIP_HEIGHT = 5;

    [SerializeField] private Transform _shipParent;

    public void SendShips(Planet targetPlanet, int shipsCount, LevelController.PlayerType attacker, Color shipColor, List<Planet> overPlanets)
    {
        var shipHeightCount = shipsCount / SHIP_HEIGHT;
        var lastShipHeight = shipsCount % SHIP_HEIGHT;

        for (int i = 0; i < shipHeightCount; i++)
        {
            var ship = ShipPoolManager.Instance.GetShip();
            ship.SetColor(shipColor);
            ship.transform.SetParent(_shipParent);
            ship.transform.localPosition = new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));

            var startPos = (Vector2) ship.transform.position;
            var targetPosition = targetPlanet.transform.position;
            var endPos = new Vector2(targetPosition.x + Random.Range(0f, 0.5f), targetPosition.y); 

            var way = CalculateWay(startPos, endPos, overPlanets);

            StartCoroutine(MoveShipsAnimation(targetPlanet, ship, SHIP_HEIGHT, attacker, way));
        }

        if (lastShipHeight != 0)
        {
            var ship = ShipPoolManager.Instance.GetShip();
            ship.SetColor(shipColor);
            ship.transform.SetParent(_shipParent);
            ship.transform.localPosition = new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));

            var startPos = (Vector2) ship.transform.position;
            var targetPosition = targetPlanet.transform.position;
            var endPos = new Vector2(targetPosition.x + Random.Range(0f, 0.5f), targetPosition.y);

            var way = CalculateWay(startPos, endPos, overPlanets);

            StartCoroutine(MoveShipsAnimation(targetPlanet, ship, lastShipHeight, attacker, way));
        }
    }
    
    public (Vector2, Vector2) CalculatePoints(float wayLineRatio, float wayLineShift, Planet planet)
    {
        var planetPos = planet.transform.position;
        
        var planetPosX = planetPos.x;
        var planetPosY = planetPos.y;
        var overRange = planet.transform.localScale.x;

        var firstCoefOver = (1 + wayLineRatio * wayLineRatio);
        var secCoefOver = -2 * planetPosX + 2 * wayLineRatio * wayLineShift - 2 *  wayLineRatio * planetPosY;
        var thirdCoefOVer = planetPosX * planetPosX + wayLineShift * wayLineShift - 2 * wayLineShift * planetPosY + planetPosY * planetPosY - overRange * overRange;

        var disc = secCoefOver * secCoefOver - 4 * firstCoefOver * thirdCoefOVer;
        var discsqrt = Mathf.Sqrt(disc);

        var firstPointOverX = (-secCoefOver + discsqrt) / (2 * firstCoefOver);
        var secondPointOverX = (-secCoefOver - discsqrt) / (2 * firstCoefOver);

        var firstPointOverY =  wayLineRatio * firstPointOverX + wayLineShift;
        var secondPointOverY =  wayLineRatio * secondPointOverX + wayLineShift;

        var firstPointOver = new Vector2(firstPointOverX, firstPointOverY);
        var secondPointOver = new Vector2(secondPointOverX, secondPointOverY);

        return (firstPointOver, secondPointOver);
    }

    private List<Vector2> CalculateWay(Vector2 startPos, Vector2 endPos, List<Planet> overPlanets)
    {
        if (startPos.x - endPos.x == 0)
        {
            endPos.x += 0.01f;
        }

        var wayLineRatio = (startPos.y - endPos.y) / (startPos.x - endPos.x);
        var wayLineShift = endPos.y - wayLineRatio * endPos.x;

        var way = new List<Vector2>();
        way.Add(startPos);

        overPlanets = overPlanets.OrderBy(point => Vector3.Distance(startPos, point.transform.position)).ToList();

        foreach (var planet in overPlanets)
        {
            var overVectors = CalculatePoints(wayLineRatio, wayLineShift, planet);

            if (float.IsNaN(overVectors.Item1.x))
            {
                continue;
            }

            if (overVectors.Item1 != overVectors.Item2)
            {
                Vector2 startOverPos;
                Vector2 endOverPos;

                if (startPos.x > endPos.x)
                {
                    endOverPos = overVectors.Item2;
                    startOverPos = overVectors.Item1;
                }
                else
                {
                    endOverPos = overVectors.Item1;
                    startOverPos = overVectors.Item2;
                }

                way.Add(startOverPos);

                var overPlanetPosition = startOverPos.y - planet.transform.position.y;

                var mult = 1;

                if (overPlanetPosition > 0)
                {
                    mult = 1;
                }
                else
                {
                    mult *= -1;
                }

                var shift = Mathf.Min(mult * (endOverPos.y - startOverPos.y) / 2, 1f);

                var planetPos = planet.transform.position;
                
                var overPointX = planetPos.x + shift;
                var overPointY = -(1 / wayLineRatio) * (overPointX - planetPos.x) + planetPos.y;

                way.Add(new Vector2(overPointX, overPointY));

                way.Add(endOverPos);
            }
        }

        way.Add(endPos);

        return way;
    }

    private IEnumerator MoveShipsAnimation(Planet targetPlanet, Ship ship, int shipHeight,
        LevelController.PlayerType attacker, List<Vector2> way)
    {
        var totalPeriod = 5f; //todo В константу!
        var wayCount = way.Count;
        var period = totalPeriod / wayCount;

        for (int i = 1; i < wayCount; i++)
        {
            var startPos = way[i - 1];
            var endPos = way[i];

            var time = 0f;

            while (time < period)
            {
                time += Time.deltaTime;
                var lTime = time / period;
                var lPos = Vector2.Lerp(startPos, endPos, lTime);
                ship.transform.position = lPos;

                yield return null;
            }
        }

        ShipPoolManager.Instance.ReturnShip(ship);
        targetPlanet.Attacked(shipHeight, attacker);
    }
}