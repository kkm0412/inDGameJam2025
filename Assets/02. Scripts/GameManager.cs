using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int playerBombDamage = 5;
    public int PlayerBombDamage => playerBombDamage;

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

    public GameObject clearUI;

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
        clearUI.SetActive(false);
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
        yield return new WaitForSeconds(3f);
        Stage.Instance.InitStageData(Stage.Instance.stageBase[nowStage - 1]);
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
        arrowSystem.StopInput();
    }

    public void EnemyOver()
    {
        stageStart = false;
        
        Debug.Log("적 사망");
    }

    public void StageEnd()
    {
        clearUI.SetActive(true);
        Debug.Log("스테이지 종료");
    }

    public void NextStage()
    {
        clearUI.SetActive(false);
        TakeDamage(-Stage.Instance.GetStageData().playerHpBonusOnClear);
        leftStageTime = stageTimeLimit; // 스테이지 시간 초기화
        nowStage += 1;
        DialogManager.Instance.AppearDialogUI();
        Stage.Instance.InitStageData(Stage.Instance.stageBase[nowStage - 1]);
    }
}
