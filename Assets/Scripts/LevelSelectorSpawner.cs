using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _levelsPrefabs;
    [SerializeField] private LevelSelectorWindow _levelSelectorPrefab;
    [SerializeField] private Transform _levelSelectorParent;
    
    void Start()
    {
        Init();
    }

    private void Init()
    {
        var levelsPrefabsCount = _levelsPrefabs.Count;

        for (int i = 0; i < levelsPrefabsCount; i++)
        {
            var levelPrefab = _levelsPrefabs[i];

            var levelSelector = Instantiate(_levelSelectorPrefab, _levelSelectorParent);
            levelSelector.Init(levelPrefab, i);
        }
    }
}
