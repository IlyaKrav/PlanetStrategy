using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
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
