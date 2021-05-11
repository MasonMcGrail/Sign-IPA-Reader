using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para>The tooltip referenced by the <see cref="TooltipSystem"/>.</para>
/// </summary>
[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI contentField;
    // When the tooltip is hidden, this is disabled.
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    public string Text
    {
        get => contentField.text;
        set
        {
            contentField.text = value;
            layoutElement.enabled = value.Length > characterWrapLimit;
        }
    }
}
