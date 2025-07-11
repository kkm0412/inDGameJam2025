using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public TMP_Text stageTimer;
    public TMP_Text playerHpText;
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
        stageTimer.text = GameManager.Instance.stageTimeLimit.ToString();
    }

    private void Update()
    {
        if (GameManager.Instance.stageStart)
        {
            stageTimer.text = GameManager.Instance.leftStageTime.ToString("F0");
        }

    }

    void UpdatePlayerHp(int value)
    {
        playerHpText.text = value.ToString();

    }
}
