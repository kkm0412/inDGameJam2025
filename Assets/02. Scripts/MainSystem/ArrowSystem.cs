using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowSystem : MonoBehaviour
{
    public enum ArrowKey { Left, Right, Down, Up };

    public int spawnArrow;
    public GameObject arrowPrefab;
    public Transform arrowParent;
    public float spacing = 100f;
    private List<ArrowKey> sequence = new();
    private int currentKey = 0;
    private bool isActive = false;


    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartArrowInput(spawnArrow);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)
            || Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (CheckInput()) return;
            Debug.Log("틀림");
        }    
    }

    /// <summary>
    /// 화살표 생성 및 입력 시퀸스를 시작합니다
    /// </summary>
    /// <param name="count">생성할 화살표의 개수</param>
    public void StartArrowInput(int count)
    {
        ClearArrow();
        for (int i = 0; i < count; i++)
        {
            sequence.Add((ArrowKey)Random.Range(0, 4));
            GameObject arrow = Instantiate(arrowPrefab, arrowParent);
            arrow.name = i.ToString();
            Transform child = arrow.transform.Find("Arrow");
            child.GetComponent<Image>().sprite = CreateArrow(sequence[i]);
        }


        // UI 출력

        currentKey = 0;
        isActive = true;

        ArrangeChildrenCentered();
    }

    /// <summary>
    /// 생성된 화살표의 key에 맞게 Resources 폴더에서 화살표 sprite를 가져옴
    /// </summary>
    /// <param name="key">리소스를 불러올 화살표 enum</param>
    /// <returns>입력받은 키에 맞는 Sprite</returns>
    private Sprite CreateArrow(ArrowKey key)
    {
        string path = $"Arrows/{key}Arrow";
        return Resources.Load<Sprite>(path);
    }    

    /// <summary>
    /// 입력받은 키가 생성된 화살표와 같은 키인지 확인한다,
    /// </summary>
    /// <returns>생성된 화살표와 같은 키를 입력받으면 true, 다른 키를 입력받으면 false</returns>
    private bool CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && sequence[currentKey] == ArrowKey.Left
            || Input.GetKeyDown(KeyCode.RightArrow) && sequence[currentKey] == ArrowKey.Right
            || Input.GetKeyDown(KeyCode.DownArrow) && sequence[currentKey] == ArrowKey.Down
            || Input.GetKeyDown(KeyCode.UpArrow) && sequence[currentKey] == ArrowKey.Up)
        {
            arrowParent.Find(currentKey.ToString()).gameObject.SetActive(false);
            currentKey++;
            if (currentKey >= sequence.Count)
            {
                SuccessInput();
            }
            return true;
        }
        else
        {
            currentKey = 0;
            for (int i = 0; i < spawnArrow; i++)
            {
                Transform child = arrowParent.GetChild(i);
                child.gameObject.SetActive(true);
            }
            return false;
        }
    }


    /// <summary>
    /// 생성된 화살표를 중앙을 기준으로 간격이 spacing이 되도록 배치
    /// </summary>
    private void ArrangeChildrenCentered()
    {
        int count = arrowParent.childCount;
        float startX = -((count - 1) * spacing) / 2f;

        for (int i = 0; i < count; i++)
        {
            Transform child = arrowParent.GetChild(i);
            child.localPosition = new Vector3(startX + i * spacing, 0f, 0f);
        }
    }

    /// <summary>
    /// 현재 진행 중인 화살표 생성 및 입력 시퀸스의 모든 화살표를 순서대로 입력하여, 성공 처리
    /// </summary>
    private void SuccessInput()
    {
        isActive = false;
        ClearArrow();
        Debug.Log("성공");
    }

    /// <summary>
    /// 생성된 화살표를 전부 제거
    /// </summary>
    public void ClearArrow()
    {
        foreach(Transform child in arrowParent)
            Destroy(child.gameObject);

        sequence.Clear();
    }
}
