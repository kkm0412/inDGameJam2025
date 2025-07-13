using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject TitlePlayer;
    public GameObject blackScreenImage;
    void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic(0);
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
        SoundManager.Instance.backgroundAudioSource.Stop();
        yield return new WaitForSeconds(0.5f);
        blackScreenImage.GetComponent<Animator>().enabled = true;
        SoundManager.Instance.PlaySound(7);
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlayBackgroundMusic(1);
        SceneManager.LoadScene("MainStage");
    }
}
