using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DictionaryController : MonoBehaviour
{
    public VerticalLayoutGroup vlg;
    public GameObject dictPrefab;
    public TMP_InputField inputField;
    private Dictionary<string, DictionaryEntryButton> nameSignDict;

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, string> testDict = new Dictionary<string, string>()
        {
            { "ASL \"S\"", "AN" },
            { "ASL \"L\"", "IZ" },
            { "ASL \"C\"", "S" },
            { "ASL CAT", "Xɲla,ɨ" }, //first type K then
            { "ASL AND", "Tda,ɨ" }, //first time SC then
            { "ASL WHITE", "Tso,e" } //first time EQ then
        };
        nameSignDict = InitDict(testDict);
        DisplayContents();
    }

    private Dictionary<string, DictionaryEntryButton> InitDict(Dictionary<string, string> dict)
    {
        Dictionary<string, DictionaryEntryButton> newDict = new Dictionary<string, DictionaryEntryButton>();
        foreach (KeyValuePair<string, string> entry in dict)
        {
            if (!newDict.ContainsKey(entry.Key)) // currently not doing anything
            {
                GameObject gameObject = Instantiate(dictPrefab);
                DictionaryEntryButton entryButton = new DictionaryEntryButton(gameObject);
                entryButton.EntryText = entry.Key;
                entryButton.SignIPAText = "\"" + entry.Value + "\"";
                entryButton.AddListener(delegate{ CopyToInputField(entry.Value); });
                newDict[entry.Key] = entryButton;
            }
        }
        return newDict;
    }

    public void DisplayContents()
    {
        vlg.transform.DetachChildren();
        foreach (var entry in nameSignDict.OrderBy(e => e.Key))
        {
            nameSignDict[entry.Key].MakeChildOf(vlg);
        }
    }

    public void CopyToInputField(string text)
    {
        inputField.text = text;
    }

    // wrapper for button prefab, which is not a subclass due to complications
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

        public void MakeChildOf(LayoutGroup parent)
        {
            buttonPrefab.transform.SetParent(parent.transform, false);
        }

        public void AddListener(UnityEngine.Events.UnityAction call)
        {
            button.onClick.AddListener(call);
        }
    }
}
