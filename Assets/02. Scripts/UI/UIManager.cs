using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


public class UIManager : MonoBehaviour
{

    public TMP_Text stageTimer;
    public TMP_Text playerHpText;
    public TMP_Text enemyHpText;
    public TMP_Text playerComboText;
    public TMP_Text playerHealthText;
    public TMP_Text enemyHealthText;
    public TMP_Text reverse;

    private void OnEnable()
    {
        GameManager.OnHpChanged += UpdatePlayerHp;
    }

    private void OnDisable()
    {
        GameManager.OnHpChanged -= UpdatePlayerHp;
    }

    private void Start()
    {
        stageTimer.text = "02:00";
        //stageTimer.text = GameManager.Instance.stageTimeLimit.ToString();
    }

    private void Update()
    {
        
        if (GameManager.Instance.stageStart)
        {
            int minutes = (int)(GameManager.Instance.leftStageTime / 60);
            int seconds = (int)(GameManager.Instance.leftStageTime % 60);
            enemyHpText.text = Stage.Instance.GetStageData().enemyHp.ToString();
            stageTimer.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }

        if (GameManager.Instance.Combo > 0)
        {
            playerComboText.gameObject.SetActive(true);
            playerComboText.text = GameManager.Instance.Combo.ToString() + " Combo";
        }
        else
        {
            playerComboText.gameObject.SetActive(false);
        }
    }

    void UpdatePlayerHp(int value)
    {
        playerHpText.text = value.ToString();
    }

    public IEnumerator UpdatePlayerHealth(int value)
    {
        value = -value;
        playerHealthText.gameObject.SetActive(true);
        if (value < 0)
        {
            playerHealthText.text = value.ToString();
            playerHealthText.color = Color.red; // 플레이어가 데미지를 입으면 빨간색으로 표시
            SoundManager.Instance.PlaySound(8); // 데미지 사운드 재생
        }
        else
        {
            playerHealthText.text = "+" + value.ToString();
            playerHealthText.color = Color.green; // 플레이어가 회복하면 초록색으로 표시
        }

        yield return new WaitForSeconds(1f); // 1초 후에 HealthTextActive 메소드 호출

        HealthTextActive(true);
    }
    public IEnumerator UpdateEnemyHealth(int value, bool isDamage)
    {
        playerHealthText.gameObject.SetActive(true);
        if (isDamage)
        {
            enemyHealthText.text = "- " + value.ToString();
            enemyHealthText.color = Color.red; // 적이 데미지를 입으면 빨간색으로 표시
        }
        else
        {
            enemyHealthText.text = value.ToString();
            enemyHealthText.color = Color.green; // 적이 회복하면 초록색으로 표시
        }

        yield return new WaitForSeconds(1f); // 1초 후에 HealthTextActive 메소드 호출

        HealthTextActive(false);
    }

    void HealthTextActive(bool isPlayerHealth)
    {
        if (isPlayerHealth)
        {
            playerHealthText.gameObject.SetActive(false);
        }
        else
        {
            enemyHealthText.gameObject.SetActive(false);
        }
    }

    void UpdateEnemyHp(int value)
    {
        enemyHpText.text = value.ToString();
    }
}
