using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTalkUIControl : MonoBehaviour
{
    [Tooltip("0번에 기본, 1번에 말하는거")]
    [SerializeField] private GameObject[] playerImage = new GameObject[2];
    void Start()
    {
        playerImage[1].SetActive(false);
    }
    /// <summary>
    /// 대화 이미지 조정. false 일시 입 다뭄, true일시 입 열음.
    /// </summary>
    /// <param name="isTalk"></param>
    public void IsMouseMove(bool isTalk)
    {

        if (isTalk)
        {
            playerImage[1].SetActive(true);
            playerImage[0].SetActive(false);
        }
        else
        {
            playerImage[0].SetActive(true);
            playerImage[1].SetActive(false);
        }
    }
}
