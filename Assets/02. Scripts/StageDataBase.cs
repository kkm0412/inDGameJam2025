using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDatabase", menuName = "Game/Stage Database")]
public class StageDatabase : ScriptableObject
{
    // 스테이지 이름 (표시용)
    public string stageName = "스테이지 1";

    // 스테이지 숫자 (1부터 시작)
    public int stageLevel = 1;

    // 적의 체력
    public int enemyHP = 100;

    // 적 체력 회복 간격 (초)
    public float enemyHealInterval = 2f;

    // 회복 시 회복량
    public int enemyHealAmount = 1;

    // 적 폭탄의 데미지 범위 (예: 5~10)
    public int enemyBombDamageRange = 5;

    // 이 스테이지를 클리어했을 때 플레이어에게 추가되는 체력
    public int playerHPBonusOnClear = 100;

}