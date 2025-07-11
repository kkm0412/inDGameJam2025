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
        
    }

    private void Update()
    {
        
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(3f);
        arrowSystem.StartArrowInput(arrowSystem.spawnArrow);
    }
}
