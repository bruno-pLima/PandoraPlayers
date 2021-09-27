using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameController : MonoBehaviour
{
    public static GameController Singleton;
    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
            Singleton.ActualScene = gameObject.scene;
            DontDestroyOnLoad(this);
        }
        else
        {
            Singleton.ActualScene = gameObject.scene;
            Destroy(gameObject);
        }
    }

    [Header("Players")]
    public GameObject Player1;
    public bool CriarPlayer1;
    public GameObject Player2;
    public bool CriarPlayer2;
    [Space(5)]
    public float MaxDistancePlayers = 20;

    [Header("HUD Options")]
    public GameObject HUD1;
    public GameObject HUD2;

    [Header("HUD")]
    public GameObject HUD;

    [HideInInspector] public GameObject Scene_HUD;

    [Header("Fade Effects")]
    public GameObject FadeObject;

    [Header("Sound Config")]
    public AudioMixer MixerGeral;
    public float BGM_Volume;
    public float SFX_Volume;

    Scene ActualScene;

    [HideInInspector]
    public GameObject ScenePlayer1;
    [HideInInspector]
    public GameObject ScenePlayer2;

    [HideInInspector]
    public PlayerController P1;
    [HideInInspector]
    public PlayerController P2;
    [HideInInspector]
    public int scoreP1 = 0, scoreP2 = 0;

    //public List<Arena> Arenas = new List<Arena>();

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadActualScene();
    }

    private void LoadActualScene()
    {
        if (ActualScene.name.ToUpper().Contains("Menu".ToUpper()))
        {
            StartCoroutine(FadeOut(FadeObject));
        }
        else if (ActualScene.name.ToUpper().Contains("Teste".ToUpper()) /*|| ActualScene.name.ToUpper().Contains("Boss".ToUpper()*/)
        {
            //StartCoroutine(FadeOut(FadeObject));
            if (!FindObjectOfType<HUD>())
            {
                Scene_HUD = Instantiate(HUD);
            }

            StartPlayers();
        }
        else if (ActualScene.name.ToUpper().Contains("Ilhas".ToUpper()) || ActualScene.name.ToUpper().Contains("Boss".ToUpper()))
        {
            MaxDistancePlayers = 20;

            StartCoroutine(FadeOut(FadeObject));

            if (!FindObjectOfType<HUD>())
            {
                Scene_HUD = Instantiate(HUD);
            }

            Cursor.visible = false;

            StartPlayers();
        }
        else if (ActualScene.name.ToUpper().Contains("Galeria".ToUpper()))
        {
            return;
        }
        else
        {
            print("Cena Não Encontrada GAME CONTROLLER IS DEAD");
        }
    }

    void A()
    {
        ScenePlayer1.SetActive(true);
        ScenePlayer2.SetActive(true);
    }

    void StartPlayers()
    {
        ScenePlayer1 = Instantiate(Player1);
        ScenePlayer2 = Instantiate(Player2);

        P1 = ScenePlayer1.GetComponent<PlayerController>();
        P2 = ScenePlayer2.GetComponent<PlayerController>();

        ScenePlayer1.name = "Player1";
        ScenePlayer2.name = "Player2";

        P2.isPlayer2 = true;

        if (ActualScene.name.ToUpper().Contains("Boss".ToUpper()))
        {
            MaxDistancePlayers = 12;
            ScenePlayer1.transform.position = new Vector3(0, 1.3f, -37);
            ScenePlayer2.transform.position = new Vector3(3.5f, 1.3f, -37);
            ScenePlayer1.SetActive(false);
            ScenePlayer2.SetActive(false);
            Invoke("A", 9.2f);
        }
        else if (CheckPointController.Singleton != null)
        {
            CheckPointController.Singleton.SetPlayers(ScenePlayer1, ScenePlayer2);
        }
        else
        {
            ScenePlayer1.transform.position = new Vector3(-5, 0, 0);
            ScenePlayer2.transform.position = new Vector3(+5, 0, 0);
        }

        if (CriarPlayer1 && !CriarPlayer2)
        {
            P2.Test();
            MaxDistancePlayers = 999;
        }
        else if (!CriarPlayer1 && CriarPlayer2)
        {
            P1.Test();
            MaxDistancePlayers = 999;
        }
    }

    [Range(.2f, .8f)]
    public float FadeSpeed = .4f;

    IEnumerator FadeOut(GameObject canvas)
    {
        GameObject CloneCanvas = Instantiate(canvas);

        for (float t = 1f; t >= 0; t -= Time.fixedDeltaTime * (FadeSpeed + 0.05f))
        {
            CloneCanvas.GetComponent<Canvas>().GetComponent<CanvasGroup>().alpha = t;
            yield return null;
        }

        Destroy(CloneCanvas);
        yield return null;
    }

    IEnumerator FadeIn(GameObject canvas)
    {
        GameObject CloneCanvas = Instantiate(canvas);

        for (float t = 0f; t <= 1; t = t + Time.fixedDeltaTime * (FadeSpeed + 0.05f))
        {
            CloneCanvas.GetComponent<Canvas>().GetComponent<CanvasGroup>().alpha = t;
            yield return null;
        }

        //Destroy(CloneCanvas);
        yield return null;
    }

    #region Controle de Cena

    IEnumerator ILoadScene(string Scene)
    {
        Time.timeScale = 1;
        yield return StartCoroutine(FadeIn(FadeObject));
        SceneManager.LoadScene(Scene);
        yield return null;
    }

    IEnumerator ILoadScene(int Scene)
    {
        Time.timeScale = 1;
        yield return StartCoroutine(FadeIn(FadeObject));
        SceneManager.LoadScene(Scene);
        yield return null;
    }

    public void LoadScene(string Scene)
    {
        StartCoroutine(ILoadScene(Scene));
    }

    public void LoadScene(int Scene)
    {
        StartCoroutine(ILoadScene(Scene));
    }

    #endregion   

    public void ChangePlayers()
    {
        GameObject temp = Player1;
        Player1 = Player2;
        Player2 = temp;

        HUD = HUD.name.Contains("1") ? HUD2 : HUD1;
    }

    public void CheckPlayerIsAlive()
    {
        if (!P1.ToVivo && !P2.ToVivo)
        {
            LoadScene(ActualScene.name);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            LoadScene("4_Boss");
        }
    }


    public void UpdateScore()
    {
        GameController.Singleton.Scene_HUD.GetComponent<HUD>().UpdateScore(scoreP1, scoreP2);
    }

}
