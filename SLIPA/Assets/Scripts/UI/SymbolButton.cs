using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
///   <para>This class is used for the clickable buttons on the
///   Input Assistance panel.</para>
/// </summary>
public class SymbolButton : MonoBehaviour, IPointerClickHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    public SymbolGroup symbolGroup;
    // The text of the button, which is both displayed and used for its value.
    public string Symbol;

    public void OnPointerClick(PointerEventData eventData)
    {
        symbolGroup.OnSymbolClicked(this);
    }

    /// <summary>
    ///   <para>Displays the appropriate tooltip when the button is hovered over.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(SymbolGroup.symbolDescriptions[Symbol]);
    }

    /// <summary>
    ///   <para>Hides the tooltip after the button is no loner hovered over.</para>
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        Symbol = GetComponent<TMP_Text>().text;
        symbolGroup.Subscribe(this);
    }
}
