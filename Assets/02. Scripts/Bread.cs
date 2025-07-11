using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : MonoBehaviour
{
    // 빵의 종류를 랜덤으로 설정
    public void RandomBread()
    {
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Bread/Bread_{Random.Range(1, 5)}");
    }
}
