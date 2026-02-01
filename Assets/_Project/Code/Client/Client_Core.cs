using System.Collections;
using System.Data.Common;
using System.Linq;
using DigitalRuby.Tween;
using TMPro;
using UnityEngine;

public class Client_Core : MonoBehaviour
{
    private int idList => Game_Manager.Instance.Clients.FindIndex(x => x == this);
    public bool InExactlyPos => transform.position == nextPos;

    
    public int End;

    [SerializeField] private float speed;
    [SerializeField] private Collider col;

    [SerializeField] private TextMeshPro txt;

    [SerializeField] private MeshFilter maskFilter;

    private Vector3 nextPos;

    public ClientData data;

    string sentence;

    private bool hi;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        data = new ClientData();

        if(data.correctSentence)
        {
            sentence = Game_Manager.Instance.goodSentenceHimno[Random.Range(0, Game_Manager.Instance.goodSentenceHimno.Length)].GetString();
        }
        else
        {
            sentence = Game_Manager.Instance.badSentenceHimno[Random.Range(0, Game_Manager.Instance.badSentenceHimno.Length)].GetString();
        }
        if(data.staffLevel == 0)
        {
            maskFilter.mesh = Game_Manager.Instance.avariableMasks[data.mask];
        }
        else
        {
            maskFilter.mesh = Game_Manager.Instance.avariableMasks[data.staffLevel - 1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos;
        if(End == 0)
        {
            pos = Game_Manager.Instance.Spawn.GetClientPos(idList);

        }
        else
        {
            pos = new Vector3(End * 16, 0, 3);
            if(transform.position == pos) Destroy(gameObject);
        }
        SmoothLookAt(transform, pos, 10);
        nextPos = pos;
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
        
        bool ready = idList == 0 && InExactlyPos;
        col.enabled = ready;

        if(!hi)
        {
            hi = true;
            GetComponent<Animator>().SetTrigger("hi");
        }
    }

    public void Interact()
    {
        CleanText();
        StartCoroutine(PrintText(sentence));
    }

    IEnumerator PrintText(string s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            txt.text += s[i];
            yield return new WaitForSeconds(0.05f);
        }

        txt.text = s;
    }

    void CleanText()
    {
        txt.text = string.Empty;
        StopAllCoroutines();
    }

    void SmoothLookAt(Transform objeto, Vector3 objetivo, float suavidad)
    {
        Vector3 direccion = objetivo - objeto.position;

        if (direccion == Vector3.zero) return;

        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
        objeto.rotation = Quaternion.Slerp(
            objeto.rotation,
            rotacionObjetivo,
            suavidad * Time.deltaTime
        );
    }
}

public class ClientData
{
    public int mask;
    public int staffLevel = 0;
    public bool correctSentence;
    public bool colapse;

    public ClientData()
    {
        if(Random.Range(0, 3) != 0)
        {
            mask = Game_Manager.Instance.roundData.bannedMask[(int)Random.Range(0, Game_Manager.Instance.roundData.bannedMask.Length)];
        }
        else
        {
            mask = (int)Random.Range(0, Game_Manager.Instance.avariableMasks.Length);
        }

        correctSentence = Random.Range(0, 2) == 0;
        colapse = Random.Range(0, Game_Manager.Instance.roundData.probabilityToMutateClient) == 0;
    
        if(Random.Range(0, 4) == 0)
            staffLevel = Random.Range(1, 4);
    }
}