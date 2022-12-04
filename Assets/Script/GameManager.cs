using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("DiceNunm")]
    public GameObject diceNumPanel;
    public Text diceNunmtext;
    public int diceNumber;

    [Header("Board")]
    public GameObject board;
    public Transform[] board_pos;
    public List<Transform> board_area = new List<Transform>();

    [Header("Inventory")]
    public GameObject InventoryPanel;
    public bool inventoryOpen = false;

    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    // Use this for initialization
    void Start()
    {
        diceNumPanel.SetActive(false);
        InventoryPanel.SetActive(false);
        FillNode();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DiceNum()
    {
        diceNunmtext.text = diceNumber.ToString();
    }

    //Draw Route Board
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        FillNode();

        for (int i = 0; i < board_area.Count; i++)
        {
            Vector3 currentPos = board_area[i].position;
            if (i > 0)
            {
                Vector3 prevPos = board_area[i - 1].position;
                Gizmos.DrawLine(prevPos, currentPos);
            }
        }

    }

    public void SpawnPlayer()
    {

    }

    public void FillNode()
    {
        board_area.Clear();

        board_pos = board.transform.GetComponentsInChildren<Transform>();

        
        foreach (Transform child in board.transform)
        {
            if (child != board.transform)
            {
                int board_areaCount = 0;
                if (board_areaCount <= board_area.Count)
                {
                    board_area.Add(child.GetChild(board_areaCount));
                    board_areaCount++;
                }
                //Debug.Log(child.GetChild(0));
            }
            if (child == board.transform.GetChild(0))
            {
                SpawnPlayer();
            }

        }
    }

}