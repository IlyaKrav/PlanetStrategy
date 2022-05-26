using UnityEngine;
using UnityEngine.UI;

public class HintsController : MonoBehaviour
{
    private const string SELECT_PLANET_TEXT = "Select your planet";
    private const string SELECT_ENEMY_TEXT = "Select enemy planet";
    private const string NONE_TEXT = "";
    private const string SELECT_LEVEL_TEXT = "Select level";
    
    [SerializeField] private Text _text;

    private HintsState _state;

    public enum HintsState
    {
        None,
        SelectPlanet,
        SelectEnemy,
        SelectLevel
    }

    public static HintsController Instance = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
        
        Instance.ChangeStateTo(HintsState.None);
    }

    public void ChangeStateTo(HintsState state)
    {
        _state = state;
        ChangeText();
    }

    private void ChangeText()
    {
        switch (_state)
        {
            case HintsState.None:
                _text.text = NONE_TEXT;
                break;
            
            case HintsState.SelectEnemy:
                _text.text = SELECT_ENEMY_TEXT;
                break;
            
            case HintsState.SelectLevel:
                _text.text = SELECT_LEVEL_TEXT;
                break;
            
            case HintsState.SelectPlanet:
                _text.text = SELECT_PLANET_TEXT;
                break;
        }
    }
}
