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

    /// <summary>
    ///   <para>Changes the coolor of a tab when hovered over.</para>
    /// </summary>
    /// <param name="button">The button whose color is changed.</param>
    public void OnTabEnter(TabButton button)
    {
        button.background.color = button.tabHover;
    }

    /// <summary>
    ///   <para>Returns the color of the tab to its default when no longer hovered over.</para>
    /// </summary>
    /// <param name="button">The button whose color is changed.</param>
    public void OnTabExit(TabButton button)
    {
        button.background.color = button.tabDefault;
    }

    /// <summary>
    ///   <para>When a button is selected, every page is made inactive other
    ///   than the one that that button corresponds to.</para>
    /// </summary>
    /// <param name="button"></param>
    public void OnTabSelected(TabButton button)
    {
        // Each button is a child of its border, so the sibling index of its
        // parent must be referenced instead of its own.
        int index = button.transform.parent.GetSiblingIndex();
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == index);
        }
    }
}
