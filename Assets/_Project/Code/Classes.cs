using System;

[System.Serializable]
public class GameSentence
{
    public static bool enLanguage = true;

    public string es;
    public string en;

    public string GetString()
    {
        return enLanguage ? en : es;
    }

    public static Action onChangeLanguage;
}
