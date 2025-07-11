using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private ArrowSystem arrowSystem;

    private void Awake()
    {
        
        arrowSystem = GetComponent<ArrowSystem>();
    }

    private void Start()
    {
        arrowSystem.customer.gameObject.SetActive(false);
        StartCoroutine(GameStart());
    }

    private void Update()
    {
        
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.stageStart = true;
        arrowSystem.customer.gameObject.SetActive(true);
        arrowSystem.StartArrowInput();
    }
}
