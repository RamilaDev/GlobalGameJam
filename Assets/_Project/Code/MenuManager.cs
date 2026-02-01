using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

// What tha hell is this
// This code was created with
// Chat GPT OMG


public class MenuManager : MonoBehaviour
{
    [SerializeField] public Animator animator; // Arrastra tu objeto con el Animator aquí
    [SerializeField] public Animator animator2; // Arrastra tu objeto con el Animator aquí
    [SerializeField] public Animator animator3; // Arrastra tu objeto con el Animator aquí
    public string triggerName = "OnClick()"; // Nombre del parámetro Trigger
    public string triggerName2 = "Door"; // Nombre del parámetro Trigger
    public string triggerName3 = "spin"; // Nombre del parámetro Trigger
    [SerializeField] RectTransform start; 
    [SerializeField] Image gameOver;
    [SerializeField] Image fadeStart;

    [Space]

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip initSound;
    [SerializeField] private AudioClip endSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOver.DOFade(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator StartScreen()
    {
        Debug.Log("Empieza");
        animator.SetTrigger(triggerName);
        animator2.SetTrigger(triggerName2);
        Debug.Log("el botón funciona");
        start.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 0.4f);
        yield return new WaitForSeconds(.2f);
        start.DOScale(new Vector3(.0f, .0f, .0f), 0.2f);
        yield return new WaitForSeconds(1f);
        fadeStart.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("scene_game");
        
        Debug.Log("Termina");
    }

    public void PressPlay()
    {
        StartCoroutine(StartScreen());
    }
 
    IEnumerator TryAgain()
    {
        Debug.Log("Empieza");
        start.DOScale(new Vector3(.25f, 1f, .25f), 0.4f);
        yield return new WaitForSeconds(.2f);
        animator3.SetTrigger(triggerName3);
        start.DOScale(new Vector3(0, 0, 0), 0.2f);
        yield return new WaitForSeconds(.2f);
        gameOver.DOFade(1, 1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("scene_start");
        Debug.Log("Termina");
    }


    IEnumerator LoopSound()
    {
        source.loop = false;
        source.clip = initSound;
        source.Play();
        while (source.isPlaying) yield return null;

        source.loop = true;
        source.clip = endSound;
        source.Play();
    }


    public void Retry()
    {

        StartCoroutine(TryAgain());
    }
}
