using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class GameOverManager : MonoBehaviour
{
    [SerializeField] RectTransform retryButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Starting());
    }
    IEnumerator Starting()
    {
    
        
        retryButton.DOScale(new Vector3(.25f, 1f, .25f), 0.5f);
        yield return new WaitForSeconds(.2f);
        retryButton.DOScale(new Vector3(.125f, .5f, .125f), 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
