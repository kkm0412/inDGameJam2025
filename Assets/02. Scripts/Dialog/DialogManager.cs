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
    public TMP_Text dialogText;
    public Image cutSceneImage;
    public Dialog[] dialogList;
    public Dialog currentDialog;
    public List<DialogData> currentText;
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
        cutSceneImage.sprite = dialog.cutSceneImage;
        DisplayText();
    }

    public void AppearDialogUI()
    {
        dialogUI.SetActive(true);
        InitText(dialogList[GameManager.Instance.nowStage - 1]);
    }

    public void DisAppearDialogUI()
    {
        dialogUI.SetActive(false);
    }

    public void DisplayText()
    {
        dialogText.text = currentText[textIndex].dialogText;
    }

    public void NextText()
    {
        if (textIndex >= currentText.Count - 1)
        {
            EndDialog();
            return;
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
        textIndex = 0;
        currentText.Clear();
        dialogText.text = "";
        GameManager.Instance.StageStart();
        DisAppearDialogUI();
    }
}
