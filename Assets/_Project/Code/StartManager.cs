using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;


public class StartManager : MonoBehaviour
{
    [SerializeField] RectTransform button;
    [SerializeField] Image IMG;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Starting());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Starting()
    {
        IMG.DOFade(0, 1);
        yield return new WaitForSeconds(.1f);
        button.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
        yield return new WaitForSeconds(.2f);
        button.DOScale(new Vector3(.25f, .25f, .25f), 0.5f);
    }

    public void ChangeLanguage()
    {
        GameSentence.enLanguage = !GameSentence.enLanguage;
        GameSentence.onChangeLanguage();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
