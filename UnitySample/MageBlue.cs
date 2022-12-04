using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MageBlue : MonoBehaviourPun
{
    public enum MonsterState { idle, trace, attack, die };
    public MonsterState monsterState = MonsterState.idle;
    private Transform monsterTr;
    private Transform playerTr;
    private Transform finalTargetTr;

    private NavMeshAgent nvAgent;
    private Animator animator;

    public float traceDist = 10.0f;
    public float attackDist = 2.0f;
    public bool isDie = false;

    private int hp = 100;
    private PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        monsterTr = GetComponent<Transform>(); 
        nvAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());

    }

    // Update is called once per frame
    void Update()
    {   

    }
    
    public void SetTarget(Transform tr)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            finalTargetTr = tr;
        }
    }

    IEnumerator CheckMonsterState()
    {
        while(isDie == false)
        {
            yield return new WaitForSeconds(0.2f);

            if(playerTr)
            {
                float dist = Vector3.Distance(playerTr.position, monsterTr.position);

                if (playerTr.gameObject.CompareTag("RED"))
                {
                    if (playerTr.gameObject.GetComponent<MageRed>() != null)
                    {
                        if (playerTr.gameObject.GetComponent<MageRed>().isDie)
                        {
                            monsterState = MonsterState.idle;
                            playerTr = null;
                        }
                    }
                }


                if (monsterState == MonsterState.die)
                {
                    ;
                }

                else if (dist <= attackDist)
                {
                    monsterState = MonsterState.attack;
                    //Debug.Log("CheckMonsterState() : attack!");
                }
                else if (dist <= traceDist)
                {
                    monsterState = MonsterState.trace;
                    //Debug.Log("CheckMonsterState() : Trace!");
                }
                else
                {

                    monsterState = MonsterState.idle;
                    // Debug.Log("CheckMonsterState() : idle!");
                    playerTr = null;
                }
            }
            else
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, traceDist);

                for(int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].tag == "Player")
                    {
                        playerTr = colliders[i].GetComponent<Transform>();
                        break;
                    }
                    else if (colliders[i].CompareTag("RED"))
                    {
                        playerTr = colliders[i].GetComponent<Transform>();
                        break;
                    }
                }
            }

        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.idle:
                    {
                        if (finalTargetTr.position != null)
                        {
                            nvAgent.destination = finalTargetTr.position;
                            nvAgent.isStopped = false;
                            animator.SetBool("IsTrace", true);
                            animator.SetBool("IsAttack", false);
                        }
                        break;
                    }
                case MonsterState.trace:
                    {
                        if (playerTr.position != null)
                        {
                            nvAgent.destination = playerTr.position;
                            nvAgent.isStopped = false;
                            animator.SetBool("IsTrace", true);
                            animator.SetBool("IsAttack", false);
                        }
                        break;
                    }
                case MonsterState.attack:
                    {
                        nvAgent.isStopped = true;
                        animator.SetBool("IsAttack", true);
                        break;
                    }

            }
            yield return null;
        }
    }

    [PunRPC]
    void OnDamage(int damage)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            hp -= damage;

            pv.RPC("ApplyUpdateHealth", RpcTarget.Others, hp);
            pv.RPC("OnDamage", RpcTarget.Others, damage);
        }

        if( hp <= 0 )
        {
            MonsterDie();
        }
        else
        {
            animator.SetTrigger("IsHit");
            
            
        }
    }
    [PunRPC]
    public void ApplyUpdateHealth(int newhp)
    {
        hp = newhp;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "BULLET")
        {
            Destroy(collision.gameObject);
            hp -= (int)collision.gameObject.GetComponent<Bullet>().damage;
            OnDamage(hp);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("REDSTAFF"))
        {
            //Destroy(other.gameObject);
            Debug.Log("레드공격");
            OnDamage(10);
        }
    }


    void MonsterDie()
    {
        StopAllCoroutines();
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.isStopped = true;
        animator.SetTrigger("IsDie");

        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach (Collider collider in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            collider.enabled = false;
        }

        Destroy(gameObject, 2.0f);
    }




}
