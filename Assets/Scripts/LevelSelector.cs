using UnityEngine;
using UnityEngine.UI;

public class LevelSelector: MonoBehaviour
{
    [SerializeField] private Text _levelText;
    [SerializeField] private NavigationItem _navigation;
    [SerializeField] private GameObject _disableCover;
    [SerializeField] private GameObject _passCover;

    private GameObject _levelPrefab;
    private int _levelIndex;
    
    public NavigationItem Navigation => _navigation;
    
    public void Init(GameObject levelPrefab, int index)
    {
        _levelText.text = (index + 1).ToString();
        _levelIndex = index;
        _levelPrefab = levelPrefab;
    }

    public void SetEnable(bool active)
    {
        _disableCover.SetActive(!active);
    }
    
    public void SetPass(bool pass)
    {
        if (pass)
        {
            _disableCover.SetActive(false);
        }
        
        _passCover.SetActive(pass);
    }
    
    public void OnPlayClick()
    {
        SoundController.Instance.OnTap();

        DataHolder.CurrentLevel = _levelPrefab;
        DataHolder.CurrentLevelIndex = _levelIndex;

        UIController.OpenSceneNotAsync("Loading");
    }
}
