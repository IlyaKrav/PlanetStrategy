using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] protected List<Planet> _planets;
    [SerializeField] protected LevelController.PlayerType _playerType;
    [SerializeField] private Color _planetsColor;

    public Color PlanetsColor => _planetsColor;

    protected void Start()
    {
        foreach (var planet in _planets)
        {
            planet.Init(_playerType, _planetsColor);
        }
    }

    public LevelController.PlayerType PlayerType => _playerType;

    public List<Planet> Planets => _planets;
}
