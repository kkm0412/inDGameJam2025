using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public TMP_Text stageTimer;
    public TMP_Text playerHpText;
    public TMP_Text enemyHpText;
    public TMP_Text playerComboText;
    public TMP_Text reverse;

    private void OnEnable()
    {
        GameManager.OnHpChanged += UpdatePlayerHp;
        Stage.OnEnemyHpChanged += UpdateEnemyHp;
    }

    private void OnDisable()
    {
        GameManager.OnHpChanged -= UpdatePlayerHp;
        Stage.OnEnemyHpChanged -= UpdateEnemyHp;

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
            stageTimer.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }

        if (GameManager.Instance.Combo > 0)
        {
            playerComboText.gameObject.SetActive(true);
            playerComboText.text = GameManager.Instance.Combo.ToString() + "\nCombo";
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

    void UpdateEnemyHp(int value)
    {
        enemyHpText.text = value.ToString();
    }
}
