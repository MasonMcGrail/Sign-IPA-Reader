using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para>This class controls sends the input that the user types to the
///   <see cref="AvatarAnimator"/> to be evaluated.</para>
/// </summary>
public class SignIO : MonoBehaviour
{
    private static TMPro.TMP_InputField inputField;
    /// <summary>
    ///   <para>This error message displays when the user enters malformed input.</para>
    /// </summary>
    public TMPro.TextMeshProUGUI errorMessage;
    public string Text
    {
        get => inputField.text;
        set => inputField.text = value;
    }
    public AvatarAnimator fingerMover;

    // Start is called before the first frame update
    void Start()
    {
        inputField = GetComponent<TMPro.TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) { UpdateAvatar(); }
    }

    /// <summary>
    ///   <para>Attempts to update the avatar based on the user's input.
    ///   If the user enters malformed input, an error message appears.</para>
    /// </summary>
    public void UpdateAvatar()
    {
        errorMessage.gameObject.SetActive(!fingerMover.ReadInput(Text));
    }
}
