using UnityEngine;
using UnityEngine.Events;

public class NavigationItem : MonoBehaviour
{
    public UnityEvent onClick;

    [SerializeField] private GameObject _selectCover;

    public GameObject SelectCover
    {
        set => _selectCover = value;
    }
   
    public void SelectItem(bool isOn)
    {
        _selectCover.SetActive(isOn);
    }
}
