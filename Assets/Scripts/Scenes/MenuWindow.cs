using UnityEngine;

public class MenuWindow : GUIScreens
{
    [SerializeField] private NavigationItems _navigation;

    private void Start()
    {
        _navigation.Init();
    }

    public void OpenLevelsScene()
    {
        UIController.OpenScene("Levels");
    }
}
