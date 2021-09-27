using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameController_OLD : MonoBehaviour
{
    public static GameController_OLD Singleton = null;



    //----- Geral ---------------------
    public bool FloatingIsland;
    public bool GamePaused;
    public GameObject PausePanel;

    [Space(20)]
    //----- Cena ---------------------
    [HideInInspector]
    public Scene scene;
    private bool reloadScene;

    [Space(20)]
    // ----- Jogador -----------------
    public bool P1IsAlive;
    public bool P2IsAlive;
    public GameObject player1;
    public GameObject player2;
    [Space(20)]
    public GameObject InstantiatedPlayer1;
    public GameObject InstantiatedPlayer2;
    [HideInInspector]
    public GameObject playerParent;

    [Space(20)]
    [HideInInspector]
    public Vector3 Player1StartPosition;
    [HideInInspector]
    public Vector3 Player2StartPosition;

    [HideInInspector]
    public Vector3 Player1SpawnPosition;
    [HideInInspector]
    public Vector3 Player2SpawnPosition;


    //----- Sound Config ---------------------
    public AudioMixer MixerGeral;
    [HideInInspector]
    public float BGM_Volume;
    [HideInInspector]
    public float SFX_Volume;


    // ----- Check Points -----------------
    [Space(20)]
    [HideInInspector]
    public GameObject cenarioController;

    [HideInInspector]
    public int LastCheckPoint;
    [HideInInspector]
    public Vector3 CheckPointPosition;
    private int Checkpoint;


    // ----- Arena -----------------
    public bool ArenaCompleted;


    // ----- Black Canvas -----------------
    public float FadeSpeed;
    public Canvas BlackCanvas;
    [HideInInspector]
    public Canvas InstantiatedBlackCanvas;





    private void Awake()
    {

        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (Singleton != this)
        {
            Destroy(gameObject);
        }

        BGM_Volume = -30;
        SFX_Volume = -20;
        ArenaCompleted = false;
    }


    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadScene();
    }


    /// <summary>
    /// Instancia os jogadores e seta algumas propriedades de jogo; Esse script é rodado assim que a cena carrega.
    /// </summary>
    private void LoadScene()
    {

        if (CheckSceneIndex(1))
        {    //Verifica se a scena atual tem o index 1 (Menu do jogo).

            if (!InstantiatedBlackCanvas)
            {  //Cria BlackCanvas caso o GameController n seja inicializado no menu, e sim na cena 2
                InstantiatedBlackCanvas = Instantiate(BlackCanvas);
                InstantiatedBlackCanvas.transform.SetParent(this.transform);
            }

            Invoke("CanvasFadeIn", 0.09f);

        }
        else if (CheckSceneIndex(2))
        {        //Verifica se a scena atual tem o index 2 (Primeira fase do jogo).

            SetSoundVolume();

            if (!InstantiatedBlackCanvas)
            {  //Cria BlackCanvas caso o GameController n seja inicializado no menu, e sim na cena 2
                InstantiatedBlackCanvas = Instantiate(BlackCanvas);
                InstantiatedBlackCanvas.transform.SetParent(this.transform);
            }


            Cursor.visible = false;
            Cursor.SetCursor(null, HotSpot, CursorMode.Auto);
            reloadScene = false;
            GamePaused = false;


            //    StartCoroutine(InstantiatePlayers());
            //    Invoke("SetParent", 0.09f);
            Invoke("CanvasFadeIn", 0.09f);
            //    Invoke("GetcheckPoint", 0.09f);

        }
        else if (CheckSceneIndex(3))
        {
            if (!InstantiatedBlackCanvas)
            {
                InstantiatedBlackCanvas = Instantiate(BlackCanvas);
                InstantiatedBlackCanvas.transform.SetParent(this.transform);
            }

            Invoke("CanvasFadeIn", 0.09f);
        }
    }

    Vector2 HotSpot = new Vector2(Screen.width, Screen.height);

    private void Update()
    {
        GameControll();
    }






    // ============== Scene =========================================

    /// <summary>
    /// Retorna verdadeiro caso a cena atual tenha o mesmo index passado via parâmetro.
    /// </summary>
    /// <returns></returns>
    private bool CheckSceneIndex(int sceneIndex)
    {

        if (SceneManager.GetActiveScene().buildIndex == sceneIndex)
        {
            return true;
        }
        else
        {
            return false;
        }
    }






    // ============== Check Point =========================================

    /// <summary>
    /// 
    /// </summary>
    private void SetSoundVolume()
    {

    }


    // ============== Check Point =========================================


    public void SetCheckPoint()
    {
        Checkpoint = LastCheckPoint;
    }


    private void GetcheckPoint()
    {
        LastCheckPoint = Checkpoint;
    }








    // ============== Players =========================================

    /// <summary>
    /// Instancia os 2 jogadores ao mesmo tempo. Usado pra inicio de fase, ou quando ambos morrerem.
    /// </summary>
    /// <param name="P1Position"></param>
    /// <param name="P2Position"></param>
    private IEnumerator InstantiatePlayers()
    {

        InstantiatedPlayer1 = Instantiate(player1, Vector3.zero, player1.transform.rotation);
        InstantiatedPlayer1.name = "Player 1";
        InstantiatedPlayer1.GetComponent<PlayerController>().BackToLife(P1IsAlive);


        InstantiatedPlayer2 = Instantiate(player2, Vector3.zero, player2.transform.rotation);
        InstantiatedPlayer2.name = "Player 2";
        InstantiatedPlayer2.GetComponent<PlayerController>().BackToLife(P2IsAlive);

        InstantiatedPlayer1.GetComponent<PlayerController>().SetParter(InstantiatedPlayer2.GetComponent<PlayerController>());
        InstantiatedPlayer2.GetComponent<PlayerController>().SetParter(InstantiatedPlayer1.GetComponent<PlayerController>());

        yield return new WaitForSeconds(0.1f);

        if (LastCheckPoint == 0)
        {
            Player1SpawnPosition = Player1StartPosition;
            Player2SpawnPosition = Player2StartPosition;
        }
        else
        {
            Player1SpawnPosition = new Vector3(CheckPointPosition.x - 1, CheckPointPosition.y, CheckPointPosition.z);
            Player2SpawnPosition = new Vector3(CheckPointPosition.x + 1, CheckPointPosition.y, CheckPointPosition.z);
        }


        InstantiatedPlayer1.transform.position = Player1SpawnPosition;
        InstantiatedPlayer2.transform.position = Player2SpawnPosition;

    }


    /// <summary>
    /// Instancia o player 1.
    /// </summary>
    /// <param name="position"></param>
    private void InstantiatePlayer1(Vector3 position)
    {
        InstantiatedPlayer1 = Instantiate(player1, position, player1.transform.rotation);
        InstantiatedPlayer1.name = "Player 1";
        InstantiatedPlayer1.GetComponent<PlayerController>().BackToLife();
    }


    /// <summary>
    /// Instancia o player2.
    /// </summary>
    /// <param name="position"></param>
    private void InstantiatePlayer2(Vector3 position)
    {
        InstantiatedPlayer2 = Instantiate(player2, position, player2.transform.rotation);
        InstantiatedPlayer2.name = "Player 2";
        InstantiatedPlayer2.GetComponent<PlayerController>().BackToLife();
    }


    private void SetParent()
    {
        InstantiatedPlayer1.transform.parent = playerParent.transform;
        InstantiatedPlayer2.transform.parent = playerParent.transform;
    }


    /// <summary>
    /// Retorna true caso os 2 jogadores estejam mortos;
    /// </summary>
    private bool BothPlayersDead()
    {

        if (!InstantiatedPlayer1.GetComponent<PlayerController>().ToVivo && !InstantiatedPlayer2.GetComponent<PlayerController>().ToVivo)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void Reviveplayer(GameObject player)
    {
        player.GetComponent<PlayerController>().BackToLife();
        player.GetComponent<PlayerController>().SetLifeFull();
    }


    // =============================


    /// <summary>
    /// Controla o jogo em termos de vitória ou derrota dos jogadores.
    /// </summary>
    private void GameControll()
    {
        if (CheckSceneIndex(2))
        {                                        //Verifica se a scena atual tem o index 1.           
            if (BothPlayersDead() && !reloadScene) { RestartGame(); }    // Reinicia o jogo quando ambos os jogadores morrem.
        }
    }


    private void RestartGame()
    {
        reloadScene = true;
        CanvasFadeOut();
        Invoke("ReloadScene", 2.5f);
    }


    private void ReloadScene()
    {
        scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(2);
    }


    public void CanvasFadeOut()
    {
        StartCoroutine(BlackCanvasFadeOut(InstantiatedBlackCanvas));
    }


    public void CanvasFadeIn()
    {
        StartCoroutine(BlackCanvasFadeIn(InstantiatedBlackCanvas));
    }


    IEnumerator BlackCanvasFadeOut(Canvas canvas)
    {

        for (float t = 0f; t <= 1; t = t + Time.deltaTime * (FadeSpeed + 0.05f))
        {

            if (canvas)
            {
                canvas.GetComponent<CanvasGroup>().alpha = t;
                yield return null;
            }
        }
    }


    IEnumerator BlackCanvasFadeIn(Canvas canvas)
    {

        yield return new WaitForSeconds(1);

        for (float t = 1f; t >= 0; t -= Time.deltaTime * (FadeSpeed + 0.05f))
        {
            canvas.GetComponent<CanvasGroup>().alpha = t;
            yield return null;
        }
    }
}