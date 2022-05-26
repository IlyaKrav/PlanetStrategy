public class GameWindow : GUIScreens
{
    void Awake()
    {
        var level = DataHolder.CurrentLevel;

        if (level != null)
        {
            Instantiate(level);
        }
        
        HintsController.Instance.ChangeStateTo(HintsController.HintsState.SelectPlanet);
    }
}
