using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Dialog", menuName = "Game/DialogData")]
public class Dialog : ScriptableObject
{   
    public List<DialogData> dialogList = new ();

}

[System.Serializable]
public class DialogData
{
    public Sprite cutSceneImage;
}
