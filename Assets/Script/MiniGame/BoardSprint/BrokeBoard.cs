using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokeBoard : MonoBehaviour
{

    private MeshRenderer boardMesh;

    Material[] materials;
    // Start is called before the first frame update
    void Start()
    {
        boardMesh = gameObject.GetComponent<MeshRenderer>();
        materials = boardMesh.GetComponent<Renderer>().materials;


    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {

            boardMesh.material = materials[1];


            Destroy(this.gameObject, 5.0f);
        }
    }
}