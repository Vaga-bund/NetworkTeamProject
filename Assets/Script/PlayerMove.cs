using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPunCallbacks
{

    private Rigidbody rb;
    [Header("Dice")]
    public DiceScript dice;

    [Header("PalyerMove")]
    public GameObject move;
    public int curPos;
    public bool isMoving;

    private Transform tr;
    private PhotonView PV;

    private Vector3 currPos;
    private Quaternion currRot;

    private void Awake()
    {
        PV = transform.Find("Player").GetComponent<PhotonView>();
        tr = transform.Find("Player").GetComponent<Transform>();
        rb = transform.Find("Player").GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        dice = GameObject.Find("Dice_Red").GetComponent<DiceScript>();

        if (PV.IsMine)
        {
            Camera.main.GetComponent<FollowCam>().target = tr.gameObject.transform;
            //Debug.LogError("카메라 적용");
        }
        //else
        //{
        //    Camera.main.GetComponent<FollowCam>().target = tr.gameObject.transform;
        //    //Debug.LogError("카메라 적용");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.inventoryOpen == false)
        //{
        //    GameManager.Instance.inventoryOpen = true;
        //    GameManager.Instance.InventoryPanel.gameObject.SetActive(true);
        //    dice.gameObject.SetActive(false);
        //}
        //else if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.inventoryOpen == true)
        //{
        //    GameManager.Instance.inventoryOpen = false;
        //    GameManager.Instance.InventoryPanel.gameObject.SetActive(false);
        //    dice.gameObject.SetActive(true);
        //}
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && dice.isThrowDice == false)
        {
            if (PV.IsMine)
            {
                rb.AddForce(Vector3.up * 120);
                dice.ThrowDice();
                StartCoroutine(Move());
            }
            else
            {
                rb.AddForce(Vector3.up * 120);
                dice.ThrowDice();
                StartCoroutine(Move());
            }
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();

        }
    }


    IEnumerator Move()
    {
        yield return new WaitForSeconds(1f);
        if (isMoving)
        {
            yield break;
        }
        rb.useGravity = false;
        isMoving = true;
        GameManager.Instance.diceNumPanel.SetActive(false);
        while (GameManager.Instance.diceNumber > 0)
        {
            curPos++;
            curPos %= GameManager.Instance.board_area.Count;

            Vector3 nextPos = GameManager.Instance.board_area[curPos].position;
            while (MoveToNextBoard(nextPos))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
            GameManager.Instance.diceNumber--;
        }

        isMoving = false;
        rb.useGravity = true;
        dice.isThrowDice = false;
    }

    public bool MoveToNextBoard(Vector3 goal)
    {
        return goal != (move.transform.position = Vector3.MoveTowards(move.transform.position, goal, 5f * Time.deltaTime));
    }
}