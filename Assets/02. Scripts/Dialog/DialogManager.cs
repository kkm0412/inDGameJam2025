using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;


public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }


    public GameObject dialogUI;
    public Image cutSceneImage;
    public Dialog[] dialogList;
    public Dialog currentDialog;
    public List<DialogData> currentText;

    public Image startButtonImage;
    public Button nextDialogButton;
    public Button endDialogButton;

    public bool isTalking = false;
    int textIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AppearDialogUI();
    }

    public void InitText(Dialog dialog)
    {
        currentDialog = dialog;
        for (int i = 0; i < dialog.dialogList.Count; i++)
        {
            currentText.Add(dialog.dialogList[i]);
        }
        DisplayText();
    }

    public void AppearDialogUI()
    {
        dialogUI.SetActive(true);
        InitText(dialogList[GameManager.Instance.nowStage - 1]);
        nextDialogButton.gameObject.SetActive(true  );
    }

    public void DisAppearDialogUI()
    {
        dialogUI.SetActive(false);
    }

    public void DisplayText()
    {
        cutSceneImage.sprite = currentText[textIndex].cutSceneImage;
    }

    public void NextText()
    {
        if (textIndex >= currentText.Count - 1)
        {
            endDialogButton.gameObject.SetActive(true);
            startButtonImage.gameObject.SetActive(true);
            nextDialogButton.gameObject.SetActive(false);
        }
        else
        {
            textIndex += 1;
            DisplayText();
        }
    }

    public void EndDialog()
    {
        isTalking = false;

        endDialogButton.gameObject.SetActive(false);
        startButtonImage.gameObject.SetActive(false);
        nextDialogButton.gameObject.SetActive(false);

        textIndex = 0;
        currentText.Clear();
        GameManager.Instance.StageStart();
        DisAppearDialogUI();
    }
}
