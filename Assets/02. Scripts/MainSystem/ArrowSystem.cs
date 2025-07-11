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
    public GameObject playerHand;
    public GameObject arrowPrefab;
    public Transform arrowParent;

    public GameObject customer;
    public GameObject waitingcustomer;
    public GameObject waitingCustomer2;
    public GameObject createBread;

    public bool isReverse;

    public float spacing = 100f;
    public float limitTime = 5f;
    public Slider arrowTimer;
    private float currentTime = 0f;
    private List<ArrowKey> sequence = new();
    private int currentKey = 0;
    private bool isActive = false;
    private Queue<int> customerQueue = new Queue<int>();
    private List<int> customerIndexList = new List<int>();

    private Animator animator;
    private SpriteRenderer sr;

    private void Awake()
    {
        animator = playerHand.GetComponent<Animator>();
        sr = playerHand.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        arrowTimer.gameObject.SetActive(false);
        arrowTimer.maxValue = limitTime;

        for (int i = 0; i < 3; i++)
        {
            customerIndexList.Add(ChangeCustomerSprite());
        }
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

        animator.enabled = true; // 대기 애니메이션 활성화

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
        ClearCustomer();
        GetCustomer();
        currentKey = 0;
        isActive = true;
    }

    // TODO : 추후 (0,7)로 수정할 것
    public int ChangeCustomerSprite()
    {
        return Random.Range(0, 6);
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

    private void GetCustomer()
    {
        customer.transform.GetChild(customerIndexList[0]).gameObject.SetActive(true);
        waitingcustomer.transform.GetChild(customerIndexList[1]).gameObject.SetActive(true);
        waitingCustomer2.transform.GetChild(customerIndexList[2]).gameObject.SetActive(true);
    }

    private Sprite GetMixHandSprite(string index)
    {
        string path = $"MixHand/{index}";
        return Resources.Load<Sprite>(path);
    }

    /// <summary>
    /// 입력받은 키가 생성된 화살표와 같은 키인지 확인한다,
    /// </summary>
    /// <returns>생성된 화살표와 같은 키를 입력받으면 true, 다른 키를 입력받으면 false</returns>
    public void CheckInput(ArrowKey key)
    {
        if (!isActive) return;

        ArrowKey expected = sequence[currentKey];
        ArrowKey correctInput = isReverse ? GetOpposite(expected) : expected;

        if (key == correctInput)
        {
            // 성공 처리
            animator.enabled = false; // 애니메이션 비활성화
            sr.sprite = GetMixHandSprite(key.ToString());
            arrowParent.Find(currentKey.ToString()).gameObject.SetActive(false);
            currentKey++;
            SoundManager.Instance.PlaySound(0); // 사운드 재생
            if (currentKey >= sequence.Count)
                SuccessInput();
        }
        else
        {
            // 실패 처리
            animator.enabled = true; // 애니메이션 활성화
            SoundManager.Instance.PlaySound(1); // 사운드 재생
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
        animator.enabled = true; // 애니메이션 활성화
        int increHp = GameManager.Instance.Combo + 1;
        GameManager.Instance.IncreCombo();
        isActive = false;
        ClearArrow();
        Debug.Log("성공");
        GameManager.Instance.TakeDamage(-increHp);
        createBread.SetActive(true);
        createBread.GetComponent<Animator>().Play("breadEffect Animation");
        SoundManager.Instance.PlaySound(2); // 사운드 재생
        StartCoroutine(DelayedStartArrowInput());
    }

    private IEnumerator DelayedStartArrowInput()
    {
        customer.transform.GetChild(customerIndexList[0]).gameObject.GetComponent<Animator>().enabled = false;
        customer.transform.GetChild(customerIndexList[0]).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Customers/Customer{customerIndexList[0]}_3");
        yield return new WaitForSeconds(2f); // 한 프레임 기다림 → Destroy 반영됨
        customer.transform.GetChild(customerIndexList[0]).gameObject.GetComponent<Animator>().enabled = true;
        customerIndexList.RemoveAt(0);
        customerIndexList.Add(ChangeCustomerSprite());
        createBread.SetActive(false);
        StartArrowInput();
    }

    private void FailInput()
    {
        animator.enabled = true; // 애니메이션 활성화
        GameManager.Instance.ResetCombo();
        isActive = false;
        ClearArrow();
        arrowTimer.gameObject.SetActive(false);
        Debug.Log("실패");
        SoundManager.Instance.PlaySound(3); // 사운드 재생
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
        customer.transform.GetChild(customerIndexList[0]).gameObject.GetComponent<Animator>().enabled = false;
        customer.transform.GetChild(customerIndexList[0]).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Customers/Customer{customerIndexList[0]}_4");
        yield return new WaitForSeconds(2f); // 한 프레임 기다림 → Destroy 반영됨
        customer.transform.GetChild(customerIndexList[0]).gameObject.GetComponent<Animator>().enabled = true;
        customerIndexList.RemoveAt(0);
        customerIndexList.Add(ChangeCustomerSprite());
        StartArrowInput();
    }

    /// <summary>
    /// 생성된 화살표를 전부 제거
    /// </summary>
    public void ClearArrow()
    {
        foreach (Transform child in arrowParent)
            Destroy(child.gameObject);

        sequence.Clear();
    }
    
    /// <summary>
    /// 모든 고객들의 오브젝트를 비활성화합니다.
    /// </summary>
    private void ClearCustomer()
    {
        for (int i = 0; i < customer.transform.childCount; i++)
        {
            customer.transform.GetChild(i).gameObject.SetActive(false);
            waitingcustomer.transform.GetChild(i).gameObject.SetActive(false);
            waitingCustomer2.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
