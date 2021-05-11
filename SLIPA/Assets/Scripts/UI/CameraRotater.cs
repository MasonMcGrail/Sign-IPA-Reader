using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///   <para>Allows the user to rotate the camera around the avatar
///   by clicking and dragging.</para>
/// </summary>
public class CameraRotater : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float speed;
    public GameObject target;
    private Vector3 defaultOrientation;
    private bool inFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        defaultOrientation = target.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (inFrame && Input.GetMouseButton(0))
        {
            target.transform.eulerAngles += speed *
                new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
        }
    }

    /// <summary>
    ///   <para>Changes the camera's transform to match its default.</para>
    /// </summary>
    public void ResetCamera()
    {
        target.transform.eulerAngles = defaultOrientation;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inFrame = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inFrame = false;
    }
}
