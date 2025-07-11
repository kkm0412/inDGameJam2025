using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
