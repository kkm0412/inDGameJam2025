using System;
using System.Collections;
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

    [Tooltip("여기에 대사 텍스트 넣기")]
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject textBar;


    TMP_Text tmp_txt;
    Image textBarImage;
    int textIndex = 0;
    [SerializeField] private int maxTextIndex = 5;
    int chapterIndex = 0;
    [SerializeField] private int maxChapterIndex = 4;
    private String[] charText;

    // Start is called before the first frame update
    void Start()
    {
        SetText();  //대사집 텍스트 설정
        tmp_txt = textObject.GetComponent<TMP_Text>();
        textBarImage = textBar.GetComponent<Image>();

        tmp_txt.enabled = false;
        textBarImage.enabled = false;
        textCanvas.SetActive(false);
    }

    /// <summary>
    /// 스테이지 별로 사용할 대사 입력 부분 0~4: 1스테이지, 5~9: 2스테이지, 10~14 3스테이지, 나머지는 엔딩
    /// </summary>
    void SetText()
    {
        charText = new string[] {
        //1스테이지
        "1",
        "2",
        "3그런데... 저 앞집 놈, 뭐야? 지금 내 가게에 빵을 던졌다고?",
        "4좋아, 끝났다. 지금까지는 ‘상도덕’이란 걸 좀 지켜줬지.",
        "5이제? 그딴 건 없다. 캐삭빵이다, 이 자식아.",
        //2스테이지
        "6앞집은 정리됐다. 근데 말이야... 주변 가게들, 눈빛이 이상해.",
        "7가만히 있던 놈들이 한 입씩 물고 들어오는구먼?",
        "8내 빵 맛본 놈들이 다들 질투가 났나 보지?",
        "9그래, 좋아. 한 번 똑바로 붙자. 장사는 전쟁이야.",
        "10내 반죽엔 발효 대신 분노가 들어간다. 똑똑히 봐둬.",
        //3스테이지
        "11다 쓸었다, 다. 이젠 이 동네 빵집들 이름만 들어도 기절하지.",
        "12그런데... 저기, 저 간판. ‘라스트오븐’? 듣기만 해도 느끼하군.",
        "13", "14", "15",
        //엔딩
        "16", "17", "18", "19", "20"

        };
    }

    /// <summary>
    /// 챕터별 텍스트 및 이미지 출력
    /// </summary>
    /// <param name="chapter">출력할 챕터 입력(0-3)</param>
    public void PrintText(int chapter)
    {
        textCanvas.SetActive(true);
        StartCoroutine(ChangeTMPText(chapter));

    }

    //Text 오브젝트의 TMP_Text 수정
    IEnumerator ChangeTMPText(int chapter)
    {
        tmp_txt.enabled = true;
        textBarImage.enabled = true;
        chapterIndex = 0;
        // if (chapter == 1)    //챕터별로 텍스트 개수 다르게 할거면 설정
        // {
        //     maxTextIndex = ?;
        // }
        while (chapterIndex < maxTextIndex)
        {
            if (Input.anyKeyDown)
            {
                Debug.Log(tmp_txt.text + chapter);
                tmp_txt.text = charText[chapter * 5 + chapterIndex];
                chapterIndex++;
                yield return null;
            }
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        tmp_txt.enabled = false;
        textBarImage.enabled = false;
        textCanvas.SetActive(false);
    }


}
