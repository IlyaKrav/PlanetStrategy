using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipsController : MonoBehaviour
{
    private const int SHIP_HEIGHT = 5;

    [SerializeField] private Transform _shipParent;

    [SerializeField] private Test _test;

    [SerializeField] private List<Planet> _planets;

    public (Vector2, Vector2) CalculatePoints(float kk, float pp, Planet planet)
    {
        var a = planet.transform.position.x;
        var b = planet.transform.position.y;
        var r = planet.transform.localScale.x;

        var k = kk;
        var p = pp;

        var A = (1 + k * k);
        var B = -2 * a + 2 * k * p - 2 * k * b;
        var C = a * a + p * p - 2 * p * b + b * b - r * r;

        var D = B * B - 4 * A * C;
        var Dsqrt = Mathf.Sqrt(D);

        var x1 = (-B + Dsqrt) / (2 * A);
        var x2 = (-B - Dsqrt) / (2 * A);

        var y1 = k * x1 + p;
        var y2 = k * x2 + p;

        var vec1 = new Vector2(x1, y1);
        var vec2 = new Vector2(x2, y2);

        return (vec1, vec2);
        
        Debug.LogError(planet.transform.name);
        Debug.LogError("1 = " + x1 + " " + y1);
        Debug.LogError("2 = " + x2 + " " + y2);
    }

    public void SendShips(Planet targetPlanet, int shipsCount, GameController.PlayerType attacker, Color shipColor)
    {
        var shipHeightCount = (int) (shipsCount / SHIP_HEIGHT);
        var lastShipHeight = shipsCount % SHIP_HEIGHT;
        
        for (int i = 0; i < shipHeightCount; i++)
        {
            var ship = ShipPoolManager.Instance.GetShip();
            ship.SetColor(shipColor);
            ship.transform.SetParent(_shipParent);
            ship.transform.localPosition = new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));

            var startPos = (Vector2) ship.transform.position;
            var targetPosition = targetPlanet.transform.position;
            // var endPos = new Vector2(targetPosition.x + Random.Range(0f, 0.5f), targetPosition.y); 
            var endPos = new Vector2(targetPosition.x, targetPosition.y);

            if (startPos.x - endPos.x == 0)
            {
                endPos.x += 0.01f;
            }
            
            var k = (startPos.y - endPos.y) / (startPos.x - endPos.x);
            var b = endPos.y - k * endPos.x;
            
            var way = new List<Vector2>();
            way.Add(startPos);
            
            foreach (var planet in _planets)
            {
                var a = CalculatePoints(k, b, planet);

                if (!float.IsNaN(a.Item1.x))
                {
                    continue;
                }
                
                Vector2 vec1;
                Vector2 vec2;
                
                vec1 = a.Item1;

                if (a.Item1 != a.Item2)
                {
                    vec2 = a.Item2;
                    
                    way.Add(vec1);
 
                    var k1 = (vec1.y + planet.transform.localScale.y - vec2.y + planet.transform.localScale.y) /
                             (vec1.x + planet.transform.localScale.x - vec2.x + planet.transform.localScale.x);
                    
                    way.Add(new Vector2(vec1.x + planet.transform.localScale.x, vec1.y + planet.transform.localScale.y));
                    way.Add(new Vector2(vec2.x + planet.transform.localScale.x, vec2.y + planet.transform.localScale.y));
                    way.Add(vec2);
                }
            }
            
            way.Add(endPos);

            StartCoroutine(MoveShipsAnimation(targetPlanet, ship, SHIP_HEIGHT, attacker, way));
        }

        if (lastShipHeight != 0)
        {
            var ship = ShipPoolManager.Instance.GetShip();
            ship.SetColor(shipColor);
            ship.transform.SetParent(_shipParent);
            // ship.transform.localPosition = new Vector2(Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
            ship.transform.localPosition = Vector2.zero;

            var startPos = (Vector2) ship.transform.position;
            var targetPosition = targetPlanet.transform.position;
            // var endPos = new Vector2(targetPosition.x + Random.Range(0f, 0.5f), targetPosition.y); 
            var endPos = new Vector2(targetPosition.x, targetPosition.y);
            
            if (startPos.x - endPos.x == 0)
            {
                endPos.x += 0.01f;
            }
            
            var k = (startPos.y - endPos.y) / (startPos.x - endPos.x);
            var b = endPos.y - k * endPos.x;
            
            var way = new List<Vector2>();
            way.Add(startPos);
            
            _planets = _planets.OrderBy(point => Vector3.Distance(startPos, point.transform.position)).ToList();

            foreach (var planet in _planets)
            {
                var a = CalculatePoints(k, b, planet);

                if (float.IsNaN(a.Item1.x))
                {
                    continue;
                }
                
                Vector2 vec1;
                Vector2 vec2;
                
                if (a.Item1 != a.Item2)
                {
                    if (startPos.x > endPos.x)
                    {
                        vec1 = a.Item2;
                        vec2 = a.Item1;
                    }
                    else
                    {
                        vec1 = a.Item1;
                        vec2 = a.Item2;
                    }
                    
                    way.Add(vec2);

                    var hhh = vec2.y - planet.transform.position.y;

                    var yy = 1;

                    if (hhh > 0)
                    {
                        yy = 1;
                    }
                    else
                    {
                        yy *= -1;
                    }
                    
                    var shift = Mathf.Min(yy * (vec1.y - vec2.y) / 2, 1f);

                    var perX = planet.transform.position.x + shift;
                    var perY = -(1 / k) * (perX - planet.transform.position.x) + planet.transform.position.y;
                    
                    way.Add(new Vector2(perX, perY));

                    way.Add(vec1);
                }
            }
            
            way.Add(endPos);
            
            StartCoroutine(MoveShipsAnimation(targetPlanet, ship, lastShipHeight, attacker, way));
        }
    }

    private IEnumerator MoveShipsAnimation(Planet targetPlanet, Ship ship, int shipHeight,
        GameController.PlayerType attacker, List<Vector2> way)
    {
        for (int i = 1; i < way.Count; i++)
        {
            var startPos = way[i - 1];
            var endPos = way[i];
            
            var time = 0f;
            var period = 5f; //todo В константу!
            
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

    private IEnumerator DifficultMoveShipsAnimation(Planet targetPlanet, Ship ship, int shipHeight,
        GameController.PlayerType attacker)
    {
        var time = 0f;
        var period = 5f; //todo В константу!
        var startPos = (Vector2) ship.transform.position;
        var targetPosition = targetPlanet.transform.position;
        // var endPos = new Vector2(targetPosition.x + Random.Range(0f, 0.5f), targetPosition.y); 
        var endPos = new Vector2(targetPosition.x, targetPosition.y);

        var k = (startPos.y - endPos.y) / (startPos.x - endPos.x);
        var b = endPos.y - k * endPos.x;

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