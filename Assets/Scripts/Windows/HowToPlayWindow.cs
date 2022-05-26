using UnityEngine;
using UnityEngine.UI;

public class HowToPlayWindow : GUIScreens
{
    [SerializeField] private Scrollbar _scrollbar;
    
    private void Start()
    {
        HintsController.Instance.ChangeStateTo(HintsController.HintsState.Back);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIController.OpenScene("StartScene");
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (_scrollbar.value > 0)
            {
                _scrollbar.value -= 0.01f;
            }
        }
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (_scrollbar.value < 1)
            {
                _scrollbar.value += 0.01f;
            }
        }
    }
}
