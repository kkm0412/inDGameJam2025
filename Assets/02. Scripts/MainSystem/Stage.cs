using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public static Stage Instance { get; private set; }

    public StageDatabase[] stageBase;
    private StageData stageData;

    bool stageStart = false;

    public static Action<int> OnEnemyHpChanged;


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
    }
    
    public void InitStageData(StageDatabase data)
    {
        stageData = new StageData(data);
    }

    public StageData GetStageData()
    {
        return stageData;
    }

    private void Start()
    {
        OnEnemyHpChanged?.Invoke(GetStageData().enemyHp);

    }

    private void Update()
    {
        //if (GameManager.Instance.stageStart && !stageStart)
        //{
        //    StartAutoHeal();
        //    stageStart = true;
        //}
    }

    public void StartAutoHeal()
    {
        StartCoroutine(EnemyAutoHeal(stageData.enemyHealAmount, stageData.enemyHealInterval));
    }

    public void TakeDamage(int amount)
    {
        stageData.enemyHp -= amount;
        Debug.Log(stageData.enemyHp);

        if (stageData.enemyHp <= 0 )
        {
            GameManager.Instance.EnemyOver();
        }
    }

    IEnumerator EnemyAutoHeal(int amount, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            TakeDamage(-amount);
        }
    }

}
