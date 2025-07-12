using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Title : MonoBehaviour
{
    public GameObject TitlePlayer;
    public GameObject blackScreenImage;
    void Start()
    {
        blackScreenImage.GetComponent<Animator>().enabled = false;
        blackScreenImage.SetActive(false);
    }

    void Update()
    {

    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }

    IEnumerator StartGameCoroutine()
    {
        TitlePlayer.GetComponent<Animator>().SetTrigger("GameStart");
        blackScreenImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        blackScreenImage.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
        
    }
}
