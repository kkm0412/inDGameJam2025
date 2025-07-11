using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    private ArrowSystem arrowSystem;

    private bool stageStart = false;
    private void Awake()
    {
        arrowSystem = GetComponent<ArrowSystem>();
    }

    private void Start()
    {
        arrowSystem.customerImage.gameObject.SetActive(false);
        StartCoroutine(GameStart());
    }

    private void Update()
    {
        
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);
        arrowSystem.customerImage.gameObject.SetActive(true);
        arrowSystem.StartArrowInput(arrowSystem.spawnArrow);
    }
}
