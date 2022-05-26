public class LoseWindow : GUIScreens
{
    public void OnRestartButton()
    {
        UIController.OpenScene("Game");
    }
    
    public void OnBackButton()
    {
        UIController.OpenScene("Menu");
    }
}
