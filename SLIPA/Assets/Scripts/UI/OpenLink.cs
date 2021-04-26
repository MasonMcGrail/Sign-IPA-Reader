using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para>Opens a link to the Github repository for this project.</para>
/// </summary>
public class OpenLink : MonoBehaviour
{
    public void Open()
    {
        Application.OpenURL("https://github.com/MasonMcGrail/Sign-IPA-Reader/");
    }
}
