using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPun
{

    private Rigidbody rb;
    [Header("Dice")]
    public DiceScript dice;

    [Header("PalyerMove")]
    public GameObject move;
    public int curPos;
    public bool isMoving;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dice = GameObject.Find("Dice_Red").GetComponent<DiceScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.inventoryOpen == false)
        {
            GameManager.Instance.inventoryOpen = true;
            GameManager.Instance.InventoryPanel.gameObject.SetActive(true);
            dice.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.inventoryOpen == true)
        {
            GameManager.Instance.inventoryOpen = false;
            GameManager.Instance.InventoryPanel.gameObject.SetActive(false);
            dice.gameObject.SetActive(true);
        }
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0)) && dice.isThrowDice == false && GameManager.Instance.inventoryOpen == false)
        {
            rb.AddForce(Vector3.up * 120);
            dice.ThrowDice();
            StartCoroutine(Move());
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
            Debug.Log("¿Ãµø");
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