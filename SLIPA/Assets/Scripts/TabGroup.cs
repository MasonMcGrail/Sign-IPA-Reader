using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// adapted from: https://www.youtube.com/watch?v=211t6r12XPQ

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<GameObject> pages;

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    // Changes the color of the tab when hovered over
    public void OnTabEnter(TabButton button)
    {
        button.background.color = button.tabHover;
    }

    // Returns the color of the tab to its default when no longer hovered over
    public void OnTabExit(TabButton button)
    {
        button.background.color = button.tabDefault;
    }

    public void OnTabSelected(TabButton button)
    {
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == index);
        }
    }
}
