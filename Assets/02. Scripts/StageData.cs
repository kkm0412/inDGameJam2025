using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageData
{
    private StageDatabase stageData;
    public string stageName;
    public int stageLevel;
    public int enemyStartHp;
    public int enemyHp;
    public float enemyHealInterval;
    public int enemyHealAmount;
    public int enemyBombDamage;
    public int playerHpBonusOnClear;

    public StageData(StageDatabase stageData)
    {
        this.stageData = stageData;
        this.stageName = this.stageData.stageName;
        this.stageLevel = this.stageData.stageLevel;
        this.enemyStartHp = this.stageData.enemyHp;
        this.enemyHp = this.stageData.enemyHp;
        this.enemyHealInterval = this.stageData.enemyHealInterval;
        this.enemyHealAmount = this.stageData.enemyHealAmount;
        this.enemyBombDamage = this.stageData.enemyBombDamage;
        this.playerHpBonusOnClear = this.stageData.playerHpBonusOnClear;

    }

   
}
