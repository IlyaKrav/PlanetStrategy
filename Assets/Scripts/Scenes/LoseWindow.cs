public class LoseWindow : GUIScreens
{
    private void Start()
    {
        HintsController.Instance.ChangeStateTo(HintsController.HintsState.None);
        SoundController.Instance.OnLose();
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
