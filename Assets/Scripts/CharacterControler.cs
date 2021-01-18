using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

public class CharacterControler : MonoBehaviour
{
    private float timeToMove = 0.4f;
    private bool isMoving;
    private Vector3 origPos;
    private Vector3 targetPos;
    private Animator characterAnimator;
    public UnityEvent OnIsMovingChanged;
    // Update is called once per frame
    private void Awake()
    {
        characterAnimator = transform.GetChild(0).GetComponent<Animator>();
        OnIsMovingChanged = new UnityEvent();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !isMoving)
        {
            StartCoroutine(MovePlayer(new Vector3(0, 0, 3)));
            characterAnimator.SetInteger("Direction", 0);
        }
        if (Input.GetKey(KeyCode.A) && !isMoving)
        {
            StartCoroutine(MovePlayer(new Vector3(-3, 0, 0)));
            characterAnimator.SetInteger("Direction", 2);
        }
        if (Input.GetKey(KeyCode.S) && !isMoving)
        {
            StartCoroutine(MovePlayer(new Vector3(0, 0, -3)));
            characterAnimator.SetInteger("Direction", 1);
        }
        if (Input.GetKey(KeyCode.D) && !isMoving)
        {
            StartCoroutine(MovePlayer(new Vector3(3, 0, 0)));
            characterAnimator.SetInteger("Direction", 3);
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        OnIsMovingChanged.Invoke();
        characterAnimator.SetBool("isMoving", true);
        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction;
        while(elapsedTime < timeToMove)
        {   
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if(Vector3.Distance(transform.position, targetPos) < 0.5f)
            transform.position = targetPos;
        isMoving = false;
        characterAnimator.SetBool("isMoving", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Wall"))
        {
            characterAnimator.SetTrigger("hitWall");
            targetPos = origPos;
            origPos = transform.position;
        }
        if(collision.collider.CompareTag("DoNotMove"))
        {
            targetPos = origPos;
            origPos = transform.position;
        }
    }
}
