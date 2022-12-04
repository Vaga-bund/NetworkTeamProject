using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonsterHQ : MonoBehaviourPun
{

    private int hp = 100;
    private bool isDie = false;
    public GameObject spawn;
    public GameObject target;
    public GameObject mage;

    Transform spawnPoint;
    Transform targetPoint;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = spawn.GetComponent<Transform>();
        targetPoint = target.GetComponent< Transform > ();

        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(CheckMonsterSpawn());
        }
    }

    IEnumerator CheckMonsterSpawn()
    {
        while(isDie == false)
        {
            yield return new WaitForSeconds(3.0f);
            if (gameObject.tag == "Blue")
            {
                GameObject mage = PhotonNetwork.Instantiate("MageBlue", spawnPoint.position, spawnPoint.rotation);
                if (PhotonNetwork.IsMasterClient)
                {
                    mage.GetComponent<MageBlue>().SetTarget(targetPoint);
                }
            }
            else if (gameObject.tag == "Red")
            {
                GameObject mage = PhotonNetwork.Instantiate("MageRed", spawnPoint.position, spawnPoint.rotation);
                if (PhotonNetwork.IsMasterClient)
                {
                    mage.GetComponent<MageRed>().SetTarget(targetPoint);
                }
            }
        }
    }

    [PunRPC]
    void OnDamage(int damage)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            hp -= damage;
            photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, hp);
            photonView.RPC("OnDamage", RpcTarget.Others, damage);

        }
        if(hp <= 0)
        {
            HQDie();
        }
    }
    [PunRPC]
    public void ApplyUpdateHealth(int newhp)
    {
        hp = newhp;
    }

    void HQDie()
    {
        StopAllCoroutines();
        isDie = true;

        Destroy(gameObject, 1.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BULLET")
        {
            Destroy(collision.gameObject);
            OnDamage((int)collision.gameObject.GetComponent<Bullet>().damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BLUESTAFF" || other.gameObject.tag == "REDSTAFF")
        {
            //Destroy(other.gameObject);
            OnDamage(10);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
