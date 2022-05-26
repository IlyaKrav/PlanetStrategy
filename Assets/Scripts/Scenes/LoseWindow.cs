using System;

public class LoseWindow : GUIScreens
{
    private void Start()
    {
        SoundController.Instance.OnLose();
    }

    public void OnRestartButton()
    {
        UIController.OpenScene("Game");
    }
    
    public void OnBackButton()
    {
        UIController.OpenScene("Menu");
    }
}
