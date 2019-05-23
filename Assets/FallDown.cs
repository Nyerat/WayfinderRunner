using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDown : MonoBehaviour
{



    private void OnCollisionEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Destroy(collision);
        }
    }
}
