using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotController : PlayerController
{
    private const float ATTACK_DELAY = 15f;
    
    private void Start()
    {
        base.Start();
        
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2, ATTACK_DELAY));

            if (_planets.Count == 0)
            {
                break;
            }
            
            var planet = _planets[Random.Range(0, _planets.Count)];
            var shipsCount = Random.Range(1, planet.ShipsCount + 1);
            
            GameController.Instance.AttackRandomPlanet(planet, _playerType, shipsCount);
        }
    }
}
