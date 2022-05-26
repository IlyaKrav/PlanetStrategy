using UnityEngine.SceneManagement;

public static class UIController
{
    public static void OpenScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public static void CloseScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);

    }
}
