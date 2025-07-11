using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class ArrowSystem : MonoBehaviour
{
    public enum ArrowKey { Left, Right, Down, Up, Space };

    public int spawnArrow;
    public GameObject arrowPrefab;
    public Transform arrowParent;

    public Image customerImage;

    public bool isReverse;

    public float spacing = 100f;
    public float limitTime = 5f;
    public Slider arrowTimer;
    private float currentTime = 0f;
    private List<ArrowKey> sequence = new();
    private int currentKey = 0;
    private bool isActive = false;

    private PlayerInput inputActions;

    private void Awake()
    {
        inputActions = new PlayerInput();
    }

    private void OnEnable()
    {
        inputActions.GamePlay.Enable();

        inputActions.GamePlay.InputUp.performed += ctx => CheckInput(ArrowKey.Up);
        inputActions.GamePlay.InputDown.performed += ctx => CheckInput(ArrowKey.Down);
        inputActions.GamePlay.InputLeft.performed += ctx => CheckInput(ArrowKey.Left);
        inputActions.GamePlay.InputRight.performed += ctx => CheckInput(ArrowKey.Right);
        inputActions.GamePlay.InputSpace.performed += ctx => CheckInput(ArrowKey.Space);
    }

    private void OnDisable()
    {
        inputActions.GamePlay.Disable();
    }

    private void Start()
    {
        arrowTimer.gameObject.SetActive(false);
        arrowTimer.maxValue = limitTime;
    }

    private void Update()
    {
        if (isActive)
        {
            currentTime -= Time.deltaTime;
            arrowTimer.value = currentTime;
            if (currentTime <= 0)
            {
                FailInput();
            }    
        }
    }

    private ArrowKey GetOpposite(ArrowKey key)
    {
        return key switch
        {
            ArrowKey.Left => ArrowKey.Right,
            ArrowKey.Right => ArrowKey.Left,
            ArrowKey.Up => ArrowKey.Down,
            ArrowKey.Down => ArrowKey.Up,
            _ => key // Space는 반대 없음
        };
    }

    /// <summary>
    /// 화살표 생성 및 입력 시퀸스를 시작합니다
    /// </summary>
    /// <param name="count">생성할 화살표의 개수</param>
    public void StartArrowInput()
    {
        int nowStage = GameManager.Instance.nowStage;
        int count = 0;
        ClearArrow();
        if (nowStage < 3)
        {
            isReverse = false;
        }
        else
        {
            isReverse = Random.value < 0.3f;
        }
        Debug.Log(isReverse);

        if (nowStage == 1)
        {
            count = 6;
        }
        else if (nowStage == 2)
        {
            count = 7;
        }
        else if (nowStage == 3)
        {
            if (isReverse)
            {
                count = 5;
            }
            else
            {
                count = 8;
            }
        }
        arrowTimer.gameObject.SetActive(true);
        currentTime = limitTime;
        // 정방향 + 스페이스
        if (!isReverse && nowStage >= 2)
        {
            for (int i = 0; i < count; i++)
            {
                sequence.Add((ArrowKey)Random.Range(0, 5));
                GameObject arrow = Instantiate(arrowPrefab, arrowParent);
                arrow.name = i.ToString();
                Transform child = arrow.transform.Find("Arrow");
                CreateArrow(sequence[i], child);
            }
        }
        // 정방향
        else if (!isReverse && nowStage == 1)
        {
            for (int i = 0; i < count; i++)
            {
                Debug.Log("정방향");
                sequence.Add((ArrowKey)Random.Range(0, 4));
                GameObject arrow = Instantiate(arrowPrefab, arrowParent);
                arrow.name = i.ToString();
                Transform child = arrow.transform.Find("Arrow");
                CreateArrow(sequence[i], child);
            }
        }
        // 역방향
        else if (isReverse && nowStage >= 3)
        {
            for (int i = 0; i < count; i++)
            {
                sequence.Add((ArrowKey)Random.Range(0, 4));
                GameObject arrow = Instantiate(arrowPrefab, arrowParent);
                arrow.name = i.ToString();
                Transform child = arrow.transform.Find("Arrow");
                CreateArrow(sequence[i], child);
            }
        }

        ArrangeChildrenCentered();
        customerImage.sprite = GetCustomerSprite(ChangeCustomerSprite());
        currentKey = 0;
        isActive = true;

        
    }
    


    // TODO : 추후 (0,7)로 수정할 것
    public int ChangeCustomerSprite()
    {
        return Random.Range(0, 4);
    }

    /// <summary>
    /// 생성된 화살표의 key에 맞게 Resources 폴더에서 화살표 sprite를 가져옴
    /// </summary>
    /// <param name="key">리소스를 불러올 화살표 enum</param>
    /// <returns>입력받은 키에 맞는 Sprite</returns>
    private void CreateArrow(ArrowKey key, Transform child)
    {
        string path = $"Arrows/{key}Arrow";
        Sprite arrowSprite = Resources.Load<Sprite>(path);

        child.GetComponent<Image>().sprite = arrowSprite;
        
        if (key == ArrowKey.Left)
        {
            child.GetComponent<Image>().color = Color.red;
        }
        else if (key == ArrowKey.Right)
        {
            child.GetComponent<Image>().color = Color.green;
        }
        else if (key == ArrowKey.Up)
        {
            child.GetComponent<Image>().color = Color.blue;
        }
        else if (key == ArrowKey.Down)
        {
            child.GetComponent<Image>().color = Color.yellow;
        }
    }

    private Sprite GetCustomerSprite(int index)
    {
        string path = $"Customers/Customer{index}";
        return Resources.Load<Sprite>(path);
    }

    /// <summary>
    /// 입력받은 키가 생성된 화살표와 같은 키인지 확인한다,
    /// </summary>
    /// <returns>생성된 화살표와 같은 키를 입력받으면 true, 다른 키를 입력받으면 false</returns>
    private void CheckInput(ArrowKey key)
    {
        if (!isActive) return;

        ArrowKey expected = sequence[currentKey];
        ArrowKey correctInput = isReverse ? GetOpposite(expected) : expected;

        if (key == correctInput)
        {
            // 성공 처리
            arrowParent.Find(currentKey.ToString()).gameObject.SetActive(false);
            currentKey++;

            if (currentKey >= sequence.Count)
                SuccessInput();
        }
        else
        {
            // 실패 처리
            Debug.Log("틀림");
            currentKey = 0;
            for (int i = 0; i < sequence.Count; i++)
                arrowParent.GetChild(i).gameObject.SetActive(true);
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
        int increHp = GameManager.Instance.Combo + 1;
        GameManager.Instance.IncreCombo();
        isActive = false;
        ClearArrow();
        Debug.Log("성공");
        GameManager.Instance.TakeDamage(-increHp);
        StartCoroutine(DelayedStartArrowInput());
    }

    private IEnumerator DelayedStartArrowInput()
    {
        yield return null; // 한 프레임 기다림 → Destroy 반영됨
        StartArrowInput();
    }

    private void FailInput()
    {
        GameManager.Instance.ResetCombo();
        isActive = false;
        ClearArrow();
        arrowTimer.gameObject.SetActive(false);
        Debug.Log("실패");
        StartCoroutine(DelayedFailInput());
    }

    public void StopInput()
    {
        isActive = false;
        ClearArrow();
        arrowTimer.gameObject.SetActive(false);
    }

    private IEnumerator DelayedFailInput()
    {
        customerImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f); // 한 프레임 기다림 → Destroy 반영됨
        customerImage.gameObject.SetActive(true);
        StartArrowInput();
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
