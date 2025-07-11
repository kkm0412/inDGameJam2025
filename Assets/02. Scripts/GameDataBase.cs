using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "Game/Game Database")]
public class GameDatabase : ScriptableObject
{
    // 플레이어의 최대 체력
    public int playerHP = 100;

    // 플레이어 체력 회복 범위 (예: 1~3)
    public Vector2Int playerHealRange = new Vector2Int(1, 3);

    // 플레이어 폭탄의 데미지 범위 (예: 5~10)
    public Vector2Int playerBombDamageRange = new Vector2Int(5, 10);

    // 폭탄 사용 쿨타임 (초)
    public float playerBombCooldown = 10f;

    // 각 스테이지 제한 시간 (초)
    public float stageTimeLimit = 120f;

    // 게임 시작 시의 스테이지 레벨 (0부터 시작 아님, 1부터 시작)
    public int startStageLevel = 1;

    // 스테이지별 데이터 리스트
    public List<StageInfo> stages;
}

// 개별 스테이지 데이터 구조
[System.Serializable]
public class StageInfo
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
    public Vector2Int enemyBombDamageRange = new Vector2Int(5, 10);

    // 이 스테이지를 클리어했을 때 플레이어에게 추가되는 체력
    public int playerHPBonusOnClear = 100;
}

public class StageInfo1
{
    public string stageName = "스테이지 2";

    
    public int stageLevel = 2;

    // 적의 체력
    public int enemyHP = 200;

    // 적 체력 회복 간격 (초)
    public float enemyHealInterval = 2f;

    // 회복 시 회복량
    public int enemyHealAmount = 2;

    // 적 폭탄의 데미지 범위 (예: 5~10)
    public Vector2Int enemyBombDamageRange = new Vector2Int(10, 20);

    // 이 스테이지를 클리어했을 때 플레이어에게 추가되는 체력
    public int playerHPBonusOnClear = 200;
}

public class StageInfo2
{
    public string stageName = "스테이지 3";


    public int stageLevel = 3;

    // 적의 체력
    public int enemyHP = 400;

    // 적 체력 회복 간격 (초)
    public float enemyHealInterval = 2f;

    // 회복 시 회복량
    public int enemyHealAmount = 4;

    // 적 폭탄의 데미지 범위 (예: 5~10)
    public Vector2Int enemyBombDamageRange = new Vector2Int(20, 40);

    // 이 스테이지를 클리어했을 때 플레이어에게 추가되는 체력
    public int playerHPBonusOnClear = 400;
}