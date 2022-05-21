using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            var obj = hit.collider;
            
            if (obj)
            {
                var planet = hit.collider.GetComponent<Planet>();
            
                if (planet != null)
                {
                    Debug.Log("ThisIsPlanet");
                }
            }
        }
    }
}
