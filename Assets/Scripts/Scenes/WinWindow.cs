using UnityEngine;
using UnityEngine.SceneManagement;

public class WinWindow : MonoBehaviour
{
    public void OnRestartButton()
    {
        SceneManager.LoadSceneAsync("Game");
    }
    
    public void OnBackButton()
    {
        SceneManager.LoadSceneAsync("Menu");
    }
}
