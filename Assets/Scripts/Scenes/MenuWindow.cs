using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuWindow : MonoBehaviour
{
    [SerializeField] private NavigationItems _navigation;

    private void Start()
    {
        _navigation.Init();
    }

    public void OpenGameScene()
    {
        SceneManager.LoadSceneAsync("Game");
    }
}
