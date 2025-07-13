using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    [SerializeField] private GameObject textCanvas;

    [Tooltip("여기에 대사 출력하는 오브젝트 넣기")]
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject textBar;
    [SerializeField] public GameObject playerStand;


    PlayerTalkUIControl talkImageCtrl;
    StoryText storyText;

    TMP_Text tmp_txt;
    Image textBarImage;
    public List<string> storyLines;
    public List<bool> faceLines;

    private int currentLineIndex = 0;
    private bool isPlaying = false;


    private void Start()
    {
        storyText = GetComponent<StoryText>();

        talkImageCtrl = playerStand.GetComponent<PlayerTalkUIControl>();
        tmp_txt = textObject.GetComponent<TMP_Text>();
        textBarImage = textBar.GetComponent<Image>();

        textCanvas.SetActive(false);
        //PrintText(1); //테스트 용도
        //PrintText(2); //테스트 용도
    }

    private void Update()
    {
        if (!isPlaying) return;

        // 키를 누르고 뗐을 때 다음 줄 진행
        if (Input.anyKeyDown)
        {
            AdvanceLine();
        }
    }

    /// <summary>
    /// 스테이지별 텍스트 및 이미지 출력 int로 스테이지 수정
    /// </summary>
    /// <param name="stageNum">출력할 스테이지 입력(1-4)</param>
    public void PrintText(int stageNum)
    {
        storyLines = storyText.GetStoryTextList(stageNum);
        faceLines = storyText.GetFaceList(stageNum);

        currentLineIndex = 0;
        isPlaying = true;
        textCanvas.SetActive(true);
        ShowCurrentLine();
    }

    //줄 출력
    private void ShowCurrentLine()
    {
        if (currentLineIndex < storyLines.Count)
        {
            string line = storyLines[currentLineIndex];
            tmp_txt.text = line;
            Debug.Log(line);
            talkImageCtrl.IsMouthMove(faceLines[currentLineIndex]);
        }
    }

    //다음 줄 진행 메서드
    private void AdvanceLine()
    {
        currentLineIndex++;
        if (currentLineIndex >= storyLines.Count)
        {
            isPlaying = false;
            textCanvas.SetActive(false);
        }
        else
        {
            ShowCurrentLine();
        }
    }
}