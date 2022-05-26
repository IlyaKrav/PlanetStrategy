public class WinWindow : GUIScreens
{
    private void Start()
    {
        SoundController.Instance.OnWin();
    }

    public void OnRestartButton()
    {
        UIController.OpenScene("Game");
    }
    
    public void OnBackButton()
    {
        UIController.OpenScene("Levels");
    }
}
