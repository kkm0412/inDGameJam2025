using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ArrowSystem : MonoBehaviour
{
    public enum ArrowKey { Left, Right, Down, Up };

    public int spawnArrow;
    public GameObject arrowPrefab;
    public Transform arrowParent;

    public Image customerImage;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartArrowInput(spawnArrow);
        } 
    }

    /// <summary>
    /// 화살표 생성 및 입력 시퀸스를 시작합니다
    /// </summary>
    /// <param name="count">생성할 화살표의 개수</param>
    public void StartArrowInput(int count)
    {
        ClearArrow();
        arrowTimer.gameObject.SetActive(true);
        currentTime = limitTime;
        for (int i = 0; i < count; i++)
        {
            sequence.Add((ArrowKey)Random.Range(0, 4));
            GameObject arrow = Instantiate(arrowPrefab, arrowParent);
            arrow.name = i.ToString();
            Transform child = arrow.transform.Find("Arrow");
            child.GetComponent<Image>().sprite = CreateArrow(sequence[i]);
        }
        ArrangeChildrenCentered();
        customerImage.sprite = GetCustomerSprite(ChangeCustomerSprite());
        currentKey = 0;
        isActive = true;

        
    }

    public int ChangeCustomerSprite()
    {
        return Random.Range(0, 4);
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

        if (key == sequence[currentKey])
        {
            arrowParent.Find(currentKey.ToString()).gameObject.SetActive(false);
            currentKey++;
            if (currentKey >= sequence.Count)
            {
                SuccessInput();
            }
        }
        else
        {
            currentKey = 0;
            for (int i = 0; i < spawnArrow; i++)
            {
                Transform child = arrowParent.GetChild(i);
                child.gameObject.SetActive(true);
            }
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
        StartCoroutine(DelayedStartArrowInput());
    }

    private IEnumerator DelayedStartArrowInput()
    {
        yield return null; // 한 프레임 기다림 → Destroy 반영됨
        StartArrowInput(spawnArrow);
    }

    private void FailInput()
    {
        isActive = false;
        ClearArrow();
        arrowTimer.gameObject.SetActive(false);
        Debug.Log("실패");
        StartCoroutine(DelayedFailInput());
    }

    private IEnumerator DelayedFailInput()
    {
        customerImage.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f); // 한 프레임 기다림 → Destroy 반영됨
        customerImage.gameObject.SetActive(true);
        StartArrowInput(spawnArrow);
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
