using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

using System.Linq;

public class SymbolButton : MonoBehaviour, IPointerClickHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    public SymbolGroup symbolGroup;
    private TMP_Text text;
    public string Symbol { get => text.text; }

    public void OnPointerClick(PointerEventData eventData)
    {
        symbolGroup.OnSymbolClicked(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(SymbolGroup.symbolDescriptions[Symbol]);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        symbolGroup.Subscribe(this);
    }
}
