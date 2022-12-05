using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionBomb;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject instantBomb = Instantiate(explosionBomb, this.gameObject.transform.position, Quaternion.identity);
        instantBomb.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        Destroy(instantBomb.gameObject, 1.0f);

        Destroy(this.gameObject);
    }
}