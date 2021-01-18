using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloorTriger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            collision.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;
            Destroy(collision.collider.gameObject.GetComponent<CharacterControler>(),0.3f);
        }
    }
}
