using UnityEngine;
using UnityEngine.Events;

public class NavigationItem : MonoBehaviour
{
    [SerializeField] private GameObject _selectCover;

    public GameObject SelectCover
    {
        set => _selectCover = value;
    }
    
    public UnityEvent onClick;
    
    public void SelectItem(bool isOn)
    {
        _selectCover.SetActive(isOn);
    }
}
