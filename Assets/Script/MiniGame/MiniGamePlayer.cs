using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.Asteroids;

public class MiniGamePlayer : MonoBehaviourPun, IPunObservable
{
    private float x_Axis = 0f;
    private float z_Axis = 0f;
    private Transform tr;
    private Rigidbody rb;
    public float speed = 1f;
    public float rotSpeed = 100f;
    private Animator animator;
    private PhotonView pv;
    private Vector3 currPos;
    private Quaternion currRot;

    //public TextMesh playerName;
    string name = "";

    private bool isDie = false;
    private int hp = 1;
    private float respawnTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        //animator = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();

        pv.ObservedComponents[0] = this;

        if (pv.IsMine)
        {
            //Camera.main.GetComponent<FollowCam>().targetTr = tr.Find("Cube").gameObject.transform;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine && isDie == false)
        {
            x_Axis = Input.GetAxis("Horizontal");
            z_Axis = Input.GetAxis("Vertical");

            //Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
            Vector3 moveDir = new Vector3(x_Axis, -0.5f, z_Axis);

            moveDir *= speed;
            rb.velocity = moveDir;
            //tr.Translate(moveDir.normalized * Time.deltaTime * speed);
            //tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));

            if (moveDir.magnitude > 0)
            {
                //animator.SetFloat("Speed", 1.0f);
            }
            else
            {
                //animator.SetFloat("Speed", 0.0f);
            }
        }
        else if (!pv.IsMine)
        {
            if (tr.position != currPos)
            {
                //animator.SetFloat("Speed", 1.0f);
                tr.position = Vector3.Lerp(tr.position, currPos, Time.deltaTime * 10.0f);
            }
            else
            {
                //animator.SetFloat("Speed", 0.0f);
            }

            if (tr.rotation != currRot)
            {
                //tr.rotation = Quaternion.Lerp(tr.rotation, currRot, Time.deltaTime * 10.0f);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
            stream.SendNext(name);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            SetPlayerName((string)stream.ReceiveNext());
        }
    }

    public void SetPlayerName(string name)
    {
        this.name = name;
        //GetComponent<PlayerCtrl>().playerName.text = this.name;
    }

    [PunRPC]
    public void OnDamage(int damage)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            hp -= damage;

            pv.RPC("ApplyUpdateHealth", RpcTarget.Others, hp);
            pv.RPC("OnDamage", RpcTarget.Others, damage);
        }

        if (hp <= 0)
        {
            //animator.SetTrigger("Die");
            //StartCoroutine(RespawnPlayer(respawnTime));
            pv.RPC("OpenMainGame", RpcTarget.AllViaServer);
        }
        else
        {
            //animator.SetTrigger("Hit");
        }
    }

    [PunRPC]
    private void OpenMainGame()
    {
        PhotonNetwork.LoadLevel("MainGameScene");
    }

    [PunRPC]
    public void ApplyUpdateHealth(int newhp)
    {
        hp = newhp;
    }

    private void OnTriggerEnter(Collider coll)
    {
        Debug.Log("HIT!:" + coll.gameObject.tag);
    }

    IEnumerator RespawnPlayer(float waitTime)
    {
        Debug.Log("Died!");
        isDie = true;
        StartCoroutine(PlayerVisible(false, 0.5f));
        yield return new WaitForSeconds(waitTime);

        tr.position = new Vector3(Random.Range(-20.0f, 20.0f), 0.0f, Random.Range(-20.0f, 20.0f));

        hp = 1;
        isDie = false;
        //animator.SetTrigger("Reset");
        StartCoroutine(PlayerVisible(true, 1.0f));
    }

    IEnumerator PlayerVisible(bool visibled, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        GetComponentInChildren<MeshRenderer>().enabled = visibled;
    }
}
