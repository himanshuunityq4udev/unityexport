using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiRefrenceProvider : MonoBehaviour
{
    [Header("Scripts Refrence")]
    [SerializeField] private  MenuController _menuController;

    [SerializeField] private List<Page> _pageList;

    [SerializeField] private Dictionary<string, Page> _pagesDictionary;

  
    public MenuController _MenuController => _menuController;
    public Dictionary<string, Page> _PagesDictionary => _pagesDictionary;
    private void Awake()
    {
        _pagesDictionary = new Dictionary<string, Page>();

        foreach (var page in _pageList)
        {
            if (!_PagesDictionary.ContainsKey(page.panelName.ToString()))
            {
                _PagesDictionary.Add(page.panelName.ToString(), page);
            }
            else
            {
                Debug.LogWarning($"Duplicate page name found: {page.panelName.ToString()} on GameObject {page.gameObject.name}");
            }
        }
    }

    public Page GetPageByName(string pageName)
    {
        if (_pagesDictionary.TryGetValue(pageName, out Page page))
        {
            return page;
        }
        return null; // Page not found
    }
}
