using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public EnemyAttackController enemyAttackController;
    public static Action<int> OnHpChanged;
    [SerializeField]
    private ArrowSystem arrowSystem;

    // 플레이어의 체력
    [SerializeField]
    private int playerHp = 100;
    public int PlayerHp => playerHp;

    // 플레이어 체력 회복 범위 (예: 1~3)
    [SerializeField]
    private int playerHeal = 1;
    public int PlayerHeal => playerHeal;

    [SerializeField]
    // 플레이어 폭탄의 데미지 범위 (예: 5~10)
    private int[] playerBombDamage = { 30, 60, 90 };
    public int[] PlayerBombDamage => playerBombDamage;

    [SerializeField]
    // 플레이어 폭탄의 데미지 범위 (예: 5~10)
    private int playerParryDamage = 30;
    public int PlayerParryDamage => playerParryDamage;

    // 폭탄 사용 쿨타임 (초)
    [SerializeField]
    private float playerBombCooldown = 10f;
    public float PlayerBombCooldown => playerBombCooldown;

    [SerializeField]
    private int combo = 0;
    public int Combo => combo;

    // 각 스테이지 제한 시간 (초)
    public float stageTimeLimit = 120f;
    public float leftStageTime { get; private set; }

    // 게임 시작 시의 스테이지 레벨 (0부터 시작 아님, 1부터 시작)
    public int nowStage = 1;

    public bool stageStart = false;

    public GameObject endingUI;
    public GameObject clearUI;
    public GameObject overUI;
    public GameObject pauseUI;
    public bool isPaused = false;

    public GameObject fadeOutBlack;

    public Coroutine gameCoro;
    public Coroutine enemyCoro;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        combo = 0;
        leftStageTime = stageTimeLimit;
        isPaused = false;
        pauseUI.SetActive(false);
        clearUI.SetActive(false);
        overUI.SetActive(false);
    }

    private void Start()
    {
        OnHpChanged?.Invoke(playerHp);
        arrowSystem.customer.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (stageStart)
        {
            leftStageTime -= Time.deltaTime;
            if (leftStageTime <= 0f)
            {
                stageStart = false;
                StageEnd();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseUI.activeSelf)
            {
                PauseOff();
            }
            else
            {
                PauseOn();
            }
        }
    }

    public void StageStart()
    {
        gameCoro = StartCoroutine(GameStart());
        enemyCoro = StartCoroutine(enemyAttackController.EnemyAttack());
       
    }

    IEnumerator GameStart()
    {
        arrowSystem.throwBackGround.SetActive(false);
        arrowSystem.customer.gameObject.SetActive(false);
        arrowSystem.waitingcustomer.gameObject.SetActive(false);
        arrowSystem.waitingCustomer2.gameObject.SetActive(false);
        arrowSystem.arrowTimer.gameObject.SetActive(false);
        arrowSystem.arrowBackground.SetActive(false);
        Stage.Instance.InitStageData(Stage.Instance.stageBase[nowStage - 1]);
        GetComponent<UIManager>().enemyHpText.text = Stage.Instance.GetStageData().enemyHp.ToString();
        yield return new WaitForSeconds(3f);
        arrowSystem.arrowParent.gameObject.SetActive(true);
        Stage.Instance.StartAutoHeal();
        arrowSystem.enemySprite.sprite = arrowSystem.GetEnemySprite();
        stageStart = true;
        arrowSystem.customer.gameObject.SetActive(true);
        arrowSystem.StartArrowInput();
    }

    public void IncreCombo()
    {
        combo += 1;
        this.gameObject.GetComponent<UIManager>().playerComboText.GetComponent<Animator>().SetTrigger("isCombo");
    }

    public void ResetCombo()
    {
        combo = 0;
    }

    public void TakeDamage(int amount)
    {
        playerHp -= amount;
        OnHpChanged?.Invoke(playerHp);
        if (playerHp <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("게임 오버");
        StopCoroutine(enemyCoro);
        SoundManager.Instance.PlayBackgroundMusic(5);
        enemyAttackController.StopAttack();
        Stage.Instance.StopAllCoroutines();
        StopCoroutine(arrowSystem.arrowCoro);
        overUI.SetActive(true);
        arrowSystem.StopInput();
    }

    public void EnemyOver()
    {
        stageStart = false;
        Debug.Log("적 사망");
    }

    public void StageEnd()
    {
        if (playerHp > Stage.Instance.GetStageData().enemyHp)
        {
            clearUI.SetActive(true);
            SoundManager.Instance.PlaySound(9);
        }
        else
        {
            GameOver();
        }
    }

    public void NextStage()
    {
        if (nowStage == 3)
        {
            GameEnding();
        }
        else
        {
            StartCoroutine(NextStageFadeOut());
        }
    }

    public void GameEnding()
    {
        SoundManager.Instance.PlayBackgroundMusic(4);
        endingUI.SetActive(true);

    }

    public IEnumerator NextStageFadeOut()
    {
        SoundManager.Instance.backgroundAudioSource.Stop();
        fadeOutBlack.SetActive(true);
        fadeOutBlack.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1.5f);
        fadeOutBlack.GetComponent<Animator>().enabled = false;
        
        fadeOutBlack.SetActive(false);
        clearUI.SetActive(false);
        TakeDamage(-Stage.Instance.GetStageData().playerHpBonusOnClear);
        leftStageTime = stageTimeLimit; // 스테이지 시간 초기화
        GetComponent<UIManager>().stageTimer.text = "02:00";
        nowStage += 1;
        SoundManager.Instance.PlayBackgroundMusic(nowStage);
        DialogManager.Instance.AppearDialogUI();
        Stage.Instance.InitStageData(Stage.Instance.stageBase[nowStage - 1]);
    }

    public void PauseOn()
    {
        pauseUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f; // 게임 일시정지
    }

    public void PauseOff()
    {
        pauseUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }



    public void ReStartGame()
    {
        SceneManager.LoadScene("MainStage");

    }

    public void GoTitle()
    {
        SceneManager.LoadScene("TitleScene");

    }


}
