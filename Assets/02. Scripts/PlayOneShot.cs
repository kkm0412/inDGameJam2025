using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShot : MonoBehaviour
{
    public void PlayAudio()
    {
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>().PlayOneShot(this.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>().clip);
    }

    public void PlaySFX(int num)
    {
        SoundManager.Instance.PlaySound(num);
    }
}
