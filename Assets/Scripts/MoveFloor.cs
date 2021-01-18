using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    public Vector3 startPosition;
    private bool isMoving = false;
    public void Move(AudioClip clip)
    {
        if(!isMoving)
        {
            StartCoroutine(MoveCoroutine(transform.position, startPosition + new Vector3(0, 5, 0), 5f));
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.clip = clip;
            audio.volume = 0.2f;
            audio.pitch = 1f;
            audio.Play();
        }
    }

    private IEnumerator MoveCoroutine(Vector3 from, Vector3 to, float duration)
    {
        isMoving = true;
        float step;
        float elapsed = 0;
        while (Vector3.Distance(transform.position,to)>0.1f)
        {
            elapsed += Time.deltaTime;
            step = elapsed / duration;
            yield return new WaitForEndOfFrame();
            transform.position = Vector3.Lerp(from, to, step);
        }
        transform.position = to;
        isMoving = false;
    }
}
