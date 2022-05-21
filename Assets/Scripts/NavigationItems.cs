using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavigationItems : MonoBehaviour
{
    public UnityEvent onBack;

    [SerializeField] private List<NavigationItem> _items;
    [SerializeField] private bool _selectFirstItem;
    
    private int _selectedItemIndex;
    private int _itemsCount;

    private void Start()
    {
        if (_selectFirstItem)
        {
            _selectedItemIndex = 0;
            _items[_selectedItemIndex].SelectItem(true);
        }

        _itemsCount = _items.Count;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _selectedItemIndex--;
            CheckToggleIndexToOutOfRange();
            SelectItem(_items[_selectedItemIndex]);
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _selectedItemIndex++;
            CheckToggleIndexToOutOfRange();
            SelectItem(_items[_selectedItemIndex]);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _items[_selectedItemIndex].onClick?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onBack?.Invoke();
        }
    }

    private void SelectItem(NavigationItem selectItem)
    {
        foreach (var item in _items)
        {
            item.SelectItem(item == selectItem);
        }
    }

    public void UnselectItems()
    {
        foreach (var item in _items)
        {
            item.SelectItem(false);
        }
    }
    
    private void CheckToggleIndexToOutOfRange()
    {
        if (_selectedItemIndex > _itemsCount - 1)
        {
            _selectedItemIndex = 0;
        }
            
        if (_selectedItemIndex < 0)
        {
            _selectedItemIndex = _itemsCount - 1;
        }
    }

    public void AddItemToEnd(NavigationItem item)
    {
        _items.Add(item);
        _itemsCount = _items.Count;
    }

    public void RemoveItem(NavigationItem item)
    {
        _items.Remove(item);
        _itemsCount = _items.Count;
    }
}
