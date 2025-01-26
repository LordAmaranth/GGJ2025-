using Project.GGJ2025;
using TMPro;
using UnityEngine;

public class HelpText : MonoBehaviour
{
    public string defaultText1;
    public string defaultText2;
    public TextMeshProUGUI text;

    void Update()
    {
        text.SetText($"{DataStore.Instance.helpIndex + 1}{defaultText1}\n{defaultText2}");
    }
}
