using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DigitalRuby.Tween;
using System.Linq;
using UnityEngine.SceneManagement;

public class Game_Manager : Elven.Singleton<Game_Manager>
{
    //
    // Variables
    //

    [SerializeField] private GameObject client_prefab;
    [SerializeField] private float max_time_spawn_client;
    [SerializeField] private Client_Spawn s;
    [SerializeField] private Interact_Trigger[] buttons;
    [SerializeField] private TextMeshPro pointsText;
    [SerializeField] private AudioSource sound;

    public Mesh[] avariableMasks;
    public Material[] avariableMasksMaterials;

    public Mesh[] avariableGoldMasks;
    public Material[] avariableGoldMasksMaterials;

    public Client_Spawn Spawn => s;

    private List<Client_Core> clients = new();
    public List<Client_Core> Clients => clients;

    private float actual_time_spawn_client;

    public GameSentence[] goodSentenceHimno;
    public GameSentence[] badSentenceHimno;
    public GameSentence[] goodSentenceMandament;
    public GameSentence[] badSentenceMandament;

    public RoundData roundData;
    public int cuote = 30;

    public bool ClientOnReady => clients[0].InExactlyPos;

    [SerializeField] private GameObject arrow_Ring;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;

    [SerializeField] private MeshFilter forbiddenMask_0;
    [SerializeField] private MeshFilter forbiddenMask_1;
    [SerializeField] private MeshFilter forbiddenMask_2;
    [SerializeField] private GameObject forbiddenHimno;

    private int attendedClientInThisRound;
    private int level;


    private bool tutorialVisible;
    [SerializeField] private GameObject tutorialObj;
    [SerializeField] private GameSentence pointSentence;
    [SerializeField] private TextMeshPro pointText;

    // ======================================================

    void Start()
    {
        roundData = new RoundData(0);
        StartCoroutine(CuoteRutine());
    }

    void Update()
    {
        UpdateSpawnTimer();
        SpawnNewClient();
        ClientLogic();
        UpdateUI();
        UpdateForbiddenMasks();
        CheckRoundProgress();

        if(cuote < 0)
        {
            SceneManager.LoadScene("scene_GameOver");
        }
    }

    // ===================== UPDATE PARTS =====================

    void UpdateSpawnTimer()
    {
        if (actual_time_spawn_client < max_time_spawn_client)
            actual_time_spawn_client += Time.deltaTime;
    }

    void UpdateUI()
    {
        pointsText.text = cuote.ToString();

        if (clients.Count > 0)
            arrow_Ring.gameObject.SetActive(clients[0].End != 0);

        if(level != 0 && !tutorialVisible)
        {
            tutorialVisible = true;
            tutorialObj.SetActive(false);
        }

        pointsText.text = string.Format(pointSentence.GetString(), level);
    }

    void UpdateForbiddenMasks()
    {
        if (roundData.bannedMask.Length > 0)
            forbiddenMask_0.mesh = avariableMasks[roundData.bannedMask[0]];
        else
            forbiddenMask_0.mesh = null;

        if (roundData.bannedMask.Length > 1)
            forbiddenMask_1.mesh = avariableMasks[roundData.bannedMask[1]];
        else
            forbiddenMask_1.mesh = null;

        if (roundData.bannedMask.Length > 2)
            forbiddenMask_2.mesh = avariableMasks[roundData.bannedMask[2]];
        else
            forbiddenMask_2.mesh = null;

        forbiddenHimno.SetActive(roundData.goodSentenceHimno);
    }

    void CheckRoundProgress()
    {
        if (attendedClientInThisRound >= roundData.clientsToNextLevel)
        {
            roundData = new RoundData(level);
            attendedClientInThisRound = 0;
        }
    }

    // ===================== CLIENT =====================

    void ClientLogic()
    {
        if (!ClientOnReady) return;
        // reservado para lógica futura
    }

    public void PositiveButton()
    {
        if (!ClientOnReady) return;
        ResolveClientDecision(true);
    }

    public void NegativeButton()
    {
        if (!ClientOnReady) return;
        ResolveClientDecision(false);
    }

    void ResolveClientDecision(bool positive)
    {
        Client_Core client = clients[0];
        client.End = positive ? 1 : -1;

        bool correct = EvaluateClient(client, positive);
        ApplyClientResult(client, correct);
    }

    bool EvaluateClient(Client_Core client, bool positive)
    {
        bool maskBanned = roundData.bannedMask.Contains(client.data.mask);

        bool sentenceWrong =
            roundData.goodSentenceHimno &&
            !client.data.correctSentence;

        if (!positive)
        {
            // FALSE → rechazo correcto si algo está mal
            return maskBanned || sentenceWrong;
        }

        // TRUE → aceptar correcto solo si todo está bien
        return !maskBanned && !sentenceWrong;
    }


    void ApplyClientResult(Client_Core client, bool correct)
    {
        int value = client.data.staffLevel * 2 * roundData.multiplyPoints;

        if (correct)
        {
            AddMoney(value);
            attendedClientInThisRound++;
        }
        else
        {
            AddMoney(-value);
        }
    }

    // ===================== SPAWN =====================

    void SpawnNewClient()
    {
        if (clients.Count >= s.Max_Clients) return;
        if (actual_time_spawn_client < max_time_spawn_client) return;

        actual_time_spawn_client = 0;

        GameObject client = Instantiate(
            client_prefab,
            s.GetClientPos(s.Max_Clients + 1),
            Quaternion.identity
        );

        clients.Add(client.GetComponent<Client_Core>());
    }

    // ===================== MONEY =====================

    void AddMoney(int p)
    {
        cuote += p;

        if (p < 0)
        {
            sound.PlayOneShot(loseClip);
            CouteTextRedffect();
        }
        else
        {
            sound.PlayOneShot(winClip);
            CouteTextGreenEffect();
        }
    }

    // ===================== TIME =====================

    IEnumerator CuoteRutine()
    {
        bool f = false;
        while (true)
        {
            if(!f) // La mayor cagada de la historia, a ver si asi no tarda en refrescar
            {
                yield return null;
                f = true;
            }
            else
            {
                yield return new WaitForSeconds(roundData.cuoteSpeed); 
            }
            cuote -= roundData.cuoteSpeed;
        }
    }

    // ===================== DATA =====================

    [System.Serializable]
    public class RoundData
    {
        public int[] bannedMask;
        public int cuoteSpeed;
        public int probabilityToMutateClient;
        public bool goodSentenceHimno;
        public int clientsToNextLevel;
        public int multiplyPoints;

        public RoundData(int level)
        {
            if (level <= 0) level = 1;

            bannedMask = new int[level < 3 ? level : 3];

            for (int i = 0; i < bannedMask.Length; i++)
            {
                bannedMask[i] = (int)Random.Range(
                    0,
                    Game_Manager.Instance.avariableMasks.Length
                );
            }

            if (level >= 4)
                goodSentenceHimno = true;

            probabilityToMutateClient = 20 / level;
            if (probabilityToMutateClient < 5)
                probabilityToMutateClient = 5;

            cuoteSpeed = level;
            clientsToNextLevel = 3 * level;
            multiplyPoints = (int)(2 * (level + 1));
        }
    }

    // ===================== FX =====================

    void CouteTextGreenEffect()
    {
        gameObject.Tween(
            "text_point_effect",
            Color.green,
            Color.white,
            0.5f,
            TweenScaleFunctions.CubicEaseIn,
            d => pointsText.color = d.CurrentValue
        );
    }

    void CouteTextRedffect()
    {
        gameObject.Tween(
            "text_point_effect",
            Color.red,
            Color.white,
            0.5f,
            TweenScaleFunctions.CubicEaseIn,
            d => pointsText.color = d.CurrentValue
        );
    }
}
