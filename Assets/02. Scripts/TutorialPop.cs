using UnityEngine;

public class TutorialPop : MonoBehaviour
{
    public GameObject popupPanel; // Inspector에서 연결

    public GameObject[] pages;
    private int currentPage = 0;

    private void Awake()
    {
        currentPage = 0;
    }

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
        pages[currentPage].SetActive(true);
    }
    
    public void OnClickNext()
    {
        if (currentPage < pages.Length - 1)
        {
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
        }
        else
        {
            pages[currentPage].SetActive(false);

            ClosePopup();
        }
    }


    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        currentPage = 0;
    }
}


