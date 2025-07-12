using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject TitlePlayer;
    public GameObject blackScreenImage;
    public GameObject PauseCanvas;

    bool isPause = false;
    void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic(0);
        blackScreenImage.GetComponent<Animator>().enabled = false;
        blackScreenImage.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePause();
        }
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

    public void GamePause()
    {
        isPause = !isPause ? true : false;

        if (isPause)
        {
            Time.timeScale = 0f;
            PauseCanvas.SetActive(true);
            SoundManager.Instance.backgroundAudioSource.Pause();
        }
        else
        {
            Time.timeScale = 1f;
            PauseCanvas.SetActive(false);
            SoundManager.Instance.backgroundAudioSource.UnPause();
        }
    }

    public void BackToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name.ToString());
    }

    IEnumerator StartGameCoroutine()
    {
        TitlePlayer.GetComponent<Animator>().SetTrigger("GameStart");
        blackScreenImage.SetActive(true);
        SoundManager.Instance.backgroundAudioSource.Stop();
        yield return new WaitForSeconds(1f);
        blackScreenImage.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlaySound(7);
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlayBackgroundMusic(1);
        this.gameObject.SetActive(false);
        
    }
}
