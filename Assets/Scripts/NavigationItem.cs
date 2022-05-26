using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NavigationItem : MonoBehaviour
{
    public UnityEvent onClick;

    [SerializeField] private SpriteRenderer _selectCover;
    [SerializeField] private Image _selectCoverImage;

    public bool IsSelected => _selectCover.gameObject.activeSelf;

    public void SetColorToSelector(Color color)
    {
        if (_selectCover != null)
        {
            _selectCover.color = color;
        }

        if (_selectCoverImage != null)
        {
            _selectCoverImage.color = color;
        }
    }

    public void SelectItem(bool isOn)
    {
        if (_selectCoverImage != null)
        {
            _selectCoverImage.gameObject.SetActive(isOn);
        }
        
        if (_selectCover != null)
        {
            _selectCover.gameObject.SetActive(isOn);
        }
    }
}