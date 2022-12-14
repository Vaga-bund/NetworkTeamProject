using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject explosionBomb;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject instantBomb = Instantiate(explosionBomb, this.gameObject.transform.position, Quaternion.identity);

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<MiniGamePlayer>().OnDamage(1);
        }
        
        Destroy(instantBomb.gameObject, 1.0f);

        Destroy(this.gameObject);
    }
}