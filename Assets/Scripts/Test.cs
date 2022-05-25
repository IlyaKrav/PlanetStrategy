using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
     /*Circle properties*/
        public float x0, y0, r;
        /*Line properties*/
        public float k, p;

        [SerializeField] private List<Planet> _planets;

        public void StartSh(float k, float p)
        {
            this.k = k;
            this.p = p;

            foreach (var planet in _planets)
            {
                x0 = planet.transform.position.x;
                y0 = planet.transform.position.y;

                r = planet.transform.localScale.x / 2;
                
                Main();
            }
        }
        
        public void Main()
        {
            float a = square(k) + 1;
            float b = 2 * (k * (p - y0) - x0);
            float c = square(b - y0) + square(x0) - square(r);
 
            float x1, x2;
            if (trySolutionSquareEquation(a, b, c, out x1, out x2))
            {
                Debug.Log($"First point : [{x1};{y(x1)}]");
                Debug.Log($"Second point : [{0};{1}], x2, y(x2)");
            }
            else
            {
                Debug.Log("Equation not have solution");                
            }
        }
 
        /*Function of line*/
        private float y(float x)
        {
            return k * x + p;
        }
 
        /*Get square of value*/
        private float square(float value)
        {
            return Mathf.Pow(value, 2);
        }
 
        /*Solving equation*/
        private bool trySolutionSquareEquation(float a, float b, float c, out float x1, out float x2)
        {
            x1 = float.NaN;
            x2 = float.NaN;

            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                return false;
            }
 
            float k = -b / 2 * a;
            discriminant /= 2 * a;
            x1 = k - Mathf.Sqrt(discriminant);
            x2 = k + Mathf.Sqrt(discriminant);
 
            return true;
        }
}
