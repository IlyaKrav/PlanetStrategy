using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorWindow : GUIScreens
{
    [SerializeField] private List<GameObject> _levelsPrefabs;
    [SerializeField] private LevelSelector _levelSelectorPrefab;
    [SerializeField] private Transform _levelSelectorParent;
    [SerializeField] private NavigationItems _navigation;
    
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
            
            _navigation.AddItemToEnd(levelSelector.Navigation);
        }
        
        _navigation.Init();
    }

    public void OnBack()
    {
        UIController.OpenScene("Menu");
    }
}
