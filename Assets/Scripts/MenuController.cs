using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private List<ToggleController> _menuButtons;
    
    private int _selectedButtonIndex;
    private int _menuButtonsCount;

    private void Start()
    {
        _selectedButtonIndex = 0;
        _menuButtons[_selectedButtonIndex].Select();
        _menuButtonsCount = _menuButtons.Count;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _selectedButtonIndex--;
            CheckToggleIndexToOutOfRange();
            _menuButtons[_selectedButtonIndex].Select();
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _selectedButtonIndex++;
            CheckToggleIndexToOutOfRange();
            _menuButtons[_selectedButtonIndex].Select();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _menuButtons[_selectedButtonIndex].onClick?.Invoke();
            
        }
    }

    private void CheckToggleIndexToOutOfRange()
    {
        if (_selectedButtonIndex > _menuButtonsCount - 1)
        {
            _selectedButtonIndex = 0;
        }
            
        if (_selectedButtonIndex < 0)
        {
            _selectedButtonIndex = _menuButtonsCount - 1;
        }
    }

    public void OpenGameScene()
    {
        SceneManager.LoadSceneAsync("Game");
    }
}
