using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignIO : MonoBehaviour
{
    private static TMPro.TMP_InputField inputField;
    public TMPro.TextMeshProUGUI errorMessage;
    public string Text
    {
        get => inputField.text;
        set => inputField.text = value;
    }
    public FingerMover fingerMover;

    void Start()
    {
        inputField = GetComponent<TMPro.TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) { UpdateAvatar(); }
    }

    public void UpdateAvatar()
    {
        // the error message displays if the input text is invalid
        errorMessage.gameObject.SetActive(!fingerMover.ReadInput(Text));
    }
}
