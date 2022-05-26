using UnityEngine;
using UnityEngine.UI;

public class LevelSelector: MonoBehaviour
{
    [SerializeField] private Text _levelText;
    [SerializeField] private NavigationItem _navigation;
    
    private GameObject _levelPrefab;

    public NavigationItem Navigation => _navigation;
    
    public void Init(GameObject levelPrefab, int index)
    {
        _levelText.text = (index + 1).ToString();
        _levelPrefab = levelPrefab;
    }

    public void OnPlayClick()
    {
        DataHolder.CurrentLevel = _levelPrefab;

        UIController.OpenSceneNotAsync("Loading");
    }
}
