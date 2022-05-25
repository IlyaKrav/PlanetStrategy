using UnityEngine;
using UnityEngine.Events;

public class NavigationItem : MonoBehaviour
{
    public UnityEvent onClick;

    [SerializeField] private SpriteRenderer _selectCover;

    public bool IsSelected => _selectCover.gameObject.activeSelf;

    public void SetColorToSelector(Color color)
    {
        _selectCover.color = color;
    }

    public void SelectItem(bool isOn)
    {
        _selectCover.gameObject.SetActive(isOn);
    }
}