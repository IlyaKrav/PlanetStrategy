using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private GameController.PlanetType _planetType;

    private bool _selectPlanet;

    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
    //
    //         RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
    //
    //         if (hit.collider != null)
    //         {
    //             Debug.Log(hit.collider.gameObject.name);
    //         }
    //         
    //     }
    // }
    
    public void SelectPlanet(bool isOn)
    {
        if (_planetType == GameController.PlanetType.Player)
        {
            _selectPlanet = isOn;
        }

        Debug.LogError(transform.name + " " + _selectPlanet);
    }
}