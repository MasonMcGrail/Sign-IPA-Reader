using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI contentField;
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

    //public RectTransform rectTransform;

    //private void Awake()
    //{
    //    rectTransform = GetComponent<RectTransform>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Vector2 position = Input.mousePosition;

    //    float pivotX = position.x / Screen.width;
    //    float pivotY = position.y / Screen.height;

    //    rectTransform.pivot = new Vector2(pivotX, pivotY);

    //    transform.position = position;
    //}
}
