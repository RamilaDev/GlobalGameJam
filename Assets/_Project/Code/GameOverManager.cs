using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{
    [SerializeField] RectTransform retryButton;

    [SerializeField] private VideoPlayer video;
    [SerializeField] private MenuManager manager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Starting());

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        video.Prepare();
    }
    IEnumerator Starting()
    {
    
        
        retryButton.DOScale(new Vector3(.25f, 1f, .25f), 0.5f);
        yield return new WaitForSeconds(.2f);
        retryButton.DOScale(new Vector3(.125f, .5f, .125f), 0.5f);
    }

    private bool playing;
    public void SetVideo()
    {
        if (playing) return;

        playing = true;
        video.gameObject.SetActive(true);
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        video.Play();

        yield return new WaitForSeconds(0.5f); // we are a 40 minutes of the end of the jam FCKKK
        // Espera a que el video termine
        yield return new WaitUntil(() => !video.isPlaying);

        playing = false;
        manager.Retry();
    }
}
