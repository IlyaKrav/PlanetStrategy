using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OpenGameScene()
    {
        SceneManager.LoadSceneAsync("Game");
    }
}
