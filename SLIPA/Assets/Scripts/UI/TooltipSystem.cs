using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// adapted from: https://www.youtube.com/watch?v=HXFoUGw7eKk

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public Tooltip tooltip;

    private void Awake()
    {
        current = this;
    }

    public static void Show(string content)
    {
        //yield return new WaitForSeconds(1);
        current.tooltip.Text = content;
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
