using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SymbolGroup : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField inputField;
    // Currently does nothing, but may be used later for additional functionality.
    List<SymbolButton> symbolButtons;

    public static Dictionary<string, string> symbolDescriptions;
    // Allows for text to be copied to the clipboard.
    private static TextEditor textEditor = new TextEditor();

    [SerializeField] private EventSystem eventSystem;

    void Start()
    {
        symbolDescriptions = new Dictionary<string, string>
        {
            // head symbols
            { "m", "center of hairline" },
            { "p", "between eyebrows" },
            { "b", "tip of nose" },
            { "f", "center of lips" },
            { "v", "tip of chin" },
            // torso symbols
            { "n", "front the neck" },
            { "t", "clavicle" },
            { "d", "center of the sternum" },
            { "s", "bottom of the sternum" },
            { "z", "navel" },
            // arm symbols
            { "ɲ", "shoulder" },
            { "c", "bicep" },
            { "ɟ", "inside of elbow" },
            { "ʃ", "center of forearm" },
            { "ʒ", "wrist (palmar side)" },
            // finger symbols
            { "ŋ", "tip of finger" },
            { "k", "fingernail" },
            { "g", "middle finger segment" },
            { "x", "base finger segment" },
            { "ɣ", "knuckle" },
            // foot/leg symbols
            { "ɴ", "hip" },
            { "q", "mid-thigh" },
            { "ɢ", "outside of knee" },
            { "χ", "shin" },
            { "ʁ", "tip of foot" },
            // minor place symbols
            { "θ", "center of ear (outward facing)" },
            { "ð", "earlobe (outward facing)" },
            { "ɾ", "center of palm (palmar side)" },
            { "ʔ", "front of sacrum" },
            { "h", "front of pubis" },
            // movement/direction symbols
            { "i", "forward and left" },
            { "e", "forward" },
            { "ɛ", "forward and right" },
            { "ɨ", "left" },
            { "ə", "-" },
            { "a", "right" },
            { "u", "backward and left" },
            { "o", "backward" },
            { "ɔ", "backward and right" },

            { "í", "forward, left, up" },
            { "é", "forward and up" },
            { "ɛ́", "forward, right, up" },
            { "ɨ́", "left and up" },
            { "ə́", "up" },
            { "á", "right and up" },
            { "ú", "backward, left, up" },
            { "ó", "backward and up" },
            { "ɔ́", "backward, right, up" },

            { "ì", "forward, left, down" },
            { "è", "forward and down" },
            { "ɛ̀", "forward, right, down" },
            { "ɨ̀", "left and down" },
            { "ə̀", "down" },
            { "à", "right and down" },
            { "ù", "backward, left, down" },
            { "ò", "backward and down" },
            { "ɔ̀", "backward, right, down" },

            { "ī", "forward and left" },
            { "ē", "forward" },
            { "ɛ̄", "forward and right" },
            { "ɨ̄", "left" },
            { "ə̄", "-" },
            { "ā", "right" },
            { "ū", "backward and left" },
            { "ō", "backward" },
            { "ɔ̄", "backward and right" },
            //rotations
            { "y", "pitching forwards" },
            { "ɯ", "pitching backwards" },
            { "ø", "yawing left" },
            { "ɤ", "yawing right" },
            { "œ", "rolling left" },
            { "ʌ", "rolling right" }
        };

        // The following section sets the redundant vowel symbol + tone bar combinations
        // to be equal to their accented counterparts.
        string[] plainVowelSymbols = { "i", "e", "ɛ", "ɨ", "ə", "a", "u", "o", "ɔ" };
        string[][] accentedVowelSymbols = {
            new string[]{ "í", "é", "ɛ́", "ɨ́", "ə́", "á", "ú", "ó", "ɔ́" },
            new string[]{ "ī", "ē", "ɛ̄", "ɨ̄", "ə̄", "ā", "ū", "ō", "ɔ̄" },
            new string[]{ "ì", "è", "ɛ̀", "ɨ̀", "ə̀", "à", "ù", "ò", "ɔ̀" }
        };
        string[] toneBars = { "˦", "˧", "˨" };
        for (int i = 0; i < plainVowelSymbols.Length; i++)
        {
            for (int j = 0; j < toneBars.Length; j++)
            {
                symbolDescriptions[plainVowelSymbols[i] + toneBars[j]] =
                    symbolDescriptions[accentedVowelSymbols[j][i]];
            }
        }
    }

    /// <summary>
    ///   Called by the <see cref="SymbolButton"/> objects when the re created,
    ///   adding them to the group/
    /// </summary>
    /// <param name="button"></param>
    /// <remarks><see cref="symbolButtons"/> currently does nothing.</remarks>
    public void Subscribe(SymbolButton button)
    {
        if (symbolButtons == null)
        {
            symbolButtons = new List<SymbolButton>();
        }
        symbolButtons.Add(button);
    }

    // will need for hover functiionality
    public void OnSymbolEnter(SymbolButton button)
    {
        ////ResetTabs();
        ////button.backgound.sprite = tabHover;
        //if (symbolButtons == null || button != symbolButtons)
        //{
        //    //button.backgound.sprite = tabHover;
        //}
    }

    /// <summary>
    ///   <para>Copies the text of a <see cref="SymbolButton"/> to the clipboard,
    ///   then gives focus to the input field and moved its carat to the end of
    ///   the current input.</para>
    /// </summary>
    /// <param name="button">The <see cref="SymbolButton"/> clicked.</param>
    public void OnSymbolClicked(SymbolButton button)
    {
        textEditor.text = button.Symbol;
        textEditor.SelectAll();
        textEditor.Copy();
        eventSystem.SetSelectedGameObject(inputField.gameObject);
        inputField.caretPosition = inputField.text.Length;
        inputField.ForceLabelUpdate();
    }
}
