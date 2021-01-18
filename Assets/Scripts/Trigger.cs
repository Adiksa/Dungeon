using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public GameObject Trap;
    public AudioClip clip;
    public GameObject dust;
    private void Start()
    {
        Trap.AddComponent<MoveFloor>().startPosition = Trap.transform.position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameObject particle = Instantiate(dust);
            particle.transform.position = transform.position + new Vector3(0f, 0.5f, 0f);
            particle.GetComponentInChildren<ParticleSystem>().Play();
            Trap.GetComponent<MoveFloor>().Move(clip);
            Destroy(gameObject.GetComponent<Trigger>());
        }
    }
}