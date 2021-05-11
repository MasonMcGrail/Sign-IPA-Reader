using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para>A tooltip system that displays descriptions provided by the
///   <see cref="SymbolGroup"/>.</para>
/// </summary>
/// <remarks>
///   <para>
///     Adapted from https://www.youtube.com/watch?v=HXFoUGw7eKk.
///   </para>
/// </remarks>
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
        current.tooltip.Text = content;
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
