using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NavigationItems : MonoBehaviour
{
    public UnityEvent onBack;

    [SerializeField] private KeyCode _selectItemKey = KeyCode.Return;
    [SerializeField] private KeyCode _backKey = KeyCode.Escape;
    
    [SerializeField] private List<NavigationItem> _items;
    [SerializeField] private bool _selectFirstItemOnStart;

    [SerializeField] private Color _selectorColor;

    private bool _onEnableAvailable;
    private int _selectedItemIndex;
    private int _itemsCount;

    public void Enable()
    {
        enabled = true;
    }

    public void Disable(bool unselectItems)
    {
        enabled = false;

        if (unselectItems)
        {
            UnselectItems();
        }
    }

    public void Init()
    {
        foreach (var item in _items)
        {
            item.SetColorToSelector(_selectorColor);
        }
        
        if (_selectFirstItemOnStart)
        {
            _selectedItemIndex = 0;
            _items[_selectedItemIndex].SelectItem(true);
        }

        _itemsCount = _items.Count;
    }

    public void SelectFirstItem()
    {
        _items[0].SelectItem(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnKeySelectPreviousItem();
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnKeySelectNextItem();
        }

        if (Input.GetKeyDown(_selectItemKey))
        {
            if (_itemsCount == 0) return;

            _items[_selectedItemIndex].onClick?.Invoke();
        }
        
        if (Input.GetKeyDown(_backKey))
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

    public void UnselectItem(NavigationItem item)
    {
        item.SelectItem(false);
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
        item.SetColorToSelector(_selectorColor);
        _items.Add(item);
        _itemsCount = _items.Count;
    }

    public void RemoveItem(NavigationItem item)
    {
        _items.Remove(item);
        _itemsCount = _items.Count;
    }

    public void SelectNextItem()
    {
        if (_itemsCount <= 1)
        {
            UnselectItems();
            return;
        }
        
        _selectedItemIndex++;
        CheckToggleIndexToOutOfRange();
        SelectItem(_items[_selectedItemIndex]);
    }
    
    public void OnKeySelectNextItem()
    {
        if (_itemsCount <= 0)
        {
            return;
        }
        
        _selectedItemIndex++;
        CheckToggleIndexToOutOfRange();
        SelectItem(_items[_selectedItemIndex]);
    }
    
    public void OnKeySelectPreviousItem()
    {
        if (_itemsCount == 0) return;
        
        _selectedItemIndex--;
        CheckToggleIndexToOutOfRange();
        SelectItem(_items[_selectedItemIndex]);
    }
}
