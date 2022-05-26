public class WinWindow : GUIScreens
{
    private void Start()
    {
        HintsController.Instance.ChangeStateTo(HintsController.HintsState.None);
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
