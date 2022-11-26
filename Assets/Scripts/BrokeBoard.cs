using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokeBoard : MonoBehaviour
{
    private Renderer boardColor;
    // Start is called before the first frame update
    void Start()
    {
        boardColor = gameObject.GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            boardColor.material.color = Color.yellow;
            Destroy(this.gameObject, 5.0f);
        }
    }
}