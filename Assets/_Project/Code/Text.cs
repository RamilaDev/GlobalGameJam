using TMPro;
using UnityEngine;

public class Game_Text : MonoBehaviour
{
    [SerializeField] private GameSentence text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameSentence.onChangeLanguage += Upda;

        Upda();
    }

    void Upda()
    {
        if (GetComponent<TextMeshProUGUI>())
            GetComponent<TextMeshProUGUI>().text = text.GetString();


        if (GetComponent<TextMeshPro>())
            GetComponent<TextMeshPro>().text = text.GetString();
    }
}
