using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectorWindow : MonoBehaviour
{
    [SerializeField] private Text _levelText;

    private GameObject _levelPrefab;

    public void Init(GameObject levelPrefab, int index)
    {
        _levelText.text = (index + 1).ToString();
        _levelPrefab = levelPrefab;
    }

    public void OnPlayClick()
    {
        DataHolder.CurrentLevel = _levelPrefab;

        SceneManager.LoadSceneAsync("Game");
    }
}
