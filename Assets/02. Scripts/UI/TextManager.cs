using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.EditorTools;
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

    // Start is called before the first frame update
    void Start()
    {
        storyText = GetComponent<StoryText>();

        talkImageCtrl = playerStand.GetComponent<PlayerTalkUIControl>();
        tmp_txt = textObject.GetComponent<TMP_Text>();
        textBarImage = textBar.GetComponent<Image>();

        textCanvas.SetActive(false);
        //PrintText(1); //테스트 용도
    }

    /// <summary>
    /// 스테이지별 텍스트 및 이미지 출력
    /// </summary>
    /// <param name="stageNum">출력할 스테이지 입력(1-4)</param>
    public void PrintText(int stageNum)
    {
        storyLines = storyText.GetStoryTextList(stageNum);
        faceLines = storyText.GetFaceList(stageNum);
        textCanvas.SetActive(true);
        StartCoroutine(ChangeTMPText(stageNum));
    }

    //Text 오브젝트의 TMP_Text 수정. 건들기 ㄴㄴ
    IEnumerator ChangeTMPText(int chapter)
    {
        int faceIndex = 0;
        foreach (string line in storyLines)
        {
            Debug.Log(line);
            talkImageCtrl.IsMouseMove(faceLines[faceIndex]);  //컷신 이미지 조정할거면.
            tmp_txt.text = line;
            faceIndex += 1;
            yield return new WaitUntil(() => Input.anyKeyDown);
            yield return new WaitWhile(() => Input.anyKey);
        }
        textCanvas.SetActive(false);
    }
}
