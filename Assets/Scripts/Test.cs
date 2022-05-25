using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private List<Planet> _planets;

    public void CalculatePoints(float kk, float pp)
    {
        foreach (var planet in _planets)
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
        
            var x1 = (-B + Dsqrt)/(2 * A);
            var x2 = (-B - Dsqrt)/(2 * A);

            var y1 = k * x1 + p;
            var y2 = k * x2 + p;
        
            Debug.LogError(planet.transform.name);
            Debug.LogError("1 = " + x1 + " " + y1);
            Debug.LogError("2 = " + x2 + " " + y2);
        }
        

    }
}