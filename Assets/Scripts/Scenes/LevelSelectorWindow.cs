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
        HintsController.Instance.ChangeStateTo(HintsController.HintsState.SelectLevel);
        SaveManager.Instance.Init(_levelsPrefabs);
        Init();
    }

    private void Init()
    {
        var levelsPrefabsCount = _levelsPrefabs.Count;
        var saveDataLevels = SaveManager.Instance.SaveDataLevels;

        for (int i = 0; i < levelsPrefabsCount; i++)
        {
            var levelPrefab = _levelsPrefabs[i];
            var levelData = saveDataLevels[i];

            var levelSelector = Instantiate(_levelSelectorPrefab, _levelSelectorParent);
            levelSelector.Init(levelPrefab, i);
            levelSelector.SetEnable(levelData.isOpen);
            levelSelector.SetPass(levelData.isPassed);
            
            if (levelData.isOpen)
            {
                _navigation.AddItemToEnd(levelSelector.Navigation);
            }
        }

        _navigation.Init();
    }

    public void OnBack()
    {
        UIController.OpenScene("StartScene");
    }
}