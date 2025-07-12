using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryText : MonoBehaviour
{
    public GameObject talkText;

    public List<string> stage_01_Texts = new List<string>
    {
        "좋아, 오늘도 장사를 시작해볼까",
        "그전에... 빠트린건 없는지 확인해보자",
        "오븐 문제없고, 반죽도 문제없고... 음, 폭탄도 준비됐네",
        "엉? 폭탄이 왜 여기 있냐고?",
        "그야 상대방 가게를 '빵'하고 날려버리기 위함이지",
        "자 그럼. 오늘도 장사를 시작해볼까",
    };

    public List<string> stage_02_Texts = new List<string>
    {
        "전 가게를",
        "이제는 가게를 날려버릴 때가 됐어",
        "폭탄을 사용해서 상대방 가게를 날려버리자",
        "폭탄을 사용하려면, 폭탄 아이콘을 클릭하고, 원하는 위치에 놓으면 돼",
    };
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
