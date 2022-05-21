using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    public UnityEvent onClick;
    
    public void Select()
    {
        _toggle.isOn = true;
    }
}
