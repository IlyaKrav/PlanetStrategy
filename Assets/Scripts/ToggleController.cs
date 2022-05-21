using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    public event UnityAction OnClick;
    
    public void Select()
    {
        _toggle.isOn = true;
    }

    public void Click()
    {
        OnClick?.Invoke();
    }
}
