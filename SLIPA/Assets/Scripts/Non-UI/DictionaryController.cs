using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
///   <para>This class controls the dictionary in the Dictionary panel.</para>
/// </summary>
public class DictionaryController : MonoBehaviour
{
    /// <summary><para>The container for the dictinary entries.</para></summary>
    [SerializeField] private VerticalLayoutGroup vlg;
    /// <summary><para>A button prefab that was made for this class.</para></summary>
    [SerializeField] private GameObject dictPrefab;
    [SerializeField] private TMP_InputField inputField;
    /// <summary>
    ///   <para>The dictionary that contains keys and their corresponding
    ///   transcription text.</para>
    /// </summary>
    private Dictionary<string, DictionaryEntryButton> signDict;

    // Start is called before the first frame update
    void Start()
    {
        // A preset dictionary with some keys and values.
        Dictionary<string, string> testDict = new Dictionary<string, string>()
        {
            { "ASL \"S\"", "AN" },
            { "ASL \"L\"", "IZ" },
            { "ASL \"C\"", "S" },
        };
        signDict = InitDict(testDict);
        DisplayContents();
    }

    /// <summary>
	///   <para>Initializes <see cref="signDict"/>.</para>
	/// </summary>
    private Dictionary<string, DictionaryEntryButton> InitDict(Dictionary<string, string> dict)
    {
        Dictionary<string, DictionaryEntryButton> newDict = new Dictionary<string, DictionaryEntryButton>();
        foreach (KeyValuePair<string, string> entry in dict)
        {
            if (!newDict.ContainsKey(entry.Key))
            {
                GameObject gameObject = Instantiate(dictPrefab);
                DictionaryEntryButton entryButton = new DictionaryEntryButton(gameObject)
                {
                    EntryText = entry.Key,
                    SignIPAText = "\"" + entry.Value + "\""
                };
                // Makes it so that when the button is clicked, the value of the
                // input field changes to match.
                entryButton.AddListener(delegate{ CopyToInputField(entry.Value); });
                newDict[entry.Key] = entryButton;
            }
        }
        return newDict;
    }

    /// <summary>
    ///   <para>Empties tge <see cref="VerticalLayoutGroup"/>, then populates it
    ///   with all of the entries of the dictionary in alphabetical order.</para>
    /// </summary>
    private void DisplayContents()
    {
        vlg.transform.DetachChildren();
        foreach (var entry in signDict.OrderBy(e => e.Key))
        {
            signDict[entry.Key].MakeChildOf(vlg);
        }
    }

    /// <summary>
    ///   <para>Copies the text of <see cref="DictionaryEntryButton"/> into
    ///   the <see cref="inputField"/>.</para>
    /// </summary>
    /// <param name="text"></param>
    private void CopyToInputField(string text)
    {
        inputField.text = text;
    }

    /// <summary>
    ///   <para>A wrapper for a button prefab that was designed for this class.</para>
    /// </summary>
    private class DictionaryEntryButton
    {
        private GameObject buttonPrefab;
        public Button button;
        private TMP_Text entryTextField, signIPATextField;
        public string EntryText
        {
            get => entryTextField.text;
            set => entryTextField.text = value;
        }
        public string SignIPAText
        {
            get => signIPATextField.text;
            set => signIPATextField.text = value;
        }

        public DictionaryEntryButton(GameObject gameObject)
        {
            buttonPrefab = gameObject;
            button = buttonPrefab.GetComponent<Button>();
            GameObject entryObject = buttonPrefab.transform.GetChild(0).gameObject;
            entryTextField = entryObject.GetComponent<TMP_Text>();
            GameObject signIPAObject = buttonPrefab.transform.GetChild(1).gameObject;
            signIPATextField = signIPAObject.GetComponent<TMP_Text>();
        }

        /// <summary>
        ///   <para>Simplifies the process of setting the parent of the buttons.</para>
        /// </summary>
        /// <param name="parent"></param>
        public void MakeChildOf(LayoutGroup parent)
        {
            buttonPrefab.transform.SetParent(parent.transform, false);
        }

        /// <summary>
        ///   <para>Simplifies the process of adding listeners to the buttons.</para>
        /// </summary>
        /// <param name="call"></param>
        public void AddListener(UnityEngine.Events.UnityAction call)
        {
            button.onClick.AddListener(call);
        }
    }
}
