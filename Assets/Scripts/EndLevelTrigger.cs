using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Player"))
        {
            UIController.instance.EndLevel();
            Destroy(gameObject.GetComponent<CharacterControler>(), 0.4f);
        }
    }
}
