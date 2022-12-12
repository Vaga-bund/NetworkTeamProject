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
    public List<GameObject> ActiveitemPanel = new List<GameObject>();
    public List<Text> itemCountText = new List<Text>();
    public List<Button> itemButton = new List<Button>();
    public int[] itemCount;
    public int[] addItemCount;
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
        InventoryInit();
    }

    public void InventoryInit()
    {
        for (int i = 0; i < itemCount.Length; i++)
        {
            itemCount[i] = 0;
        }
        for (int i = 0; i < itemCount.Length; i++)
        {
            addItemCount[i] = 0;
        }

        for (int i = 0; i < itemCountText.Count; i++)
        {
            itemCountText[i].text = itemCount[i].ToString();
        }

        for (int i = 0; i < itemButton.Count; i++)
        {
            itemButton[i].enabled = false;
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            itemUpdate();
        }
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

    public void ClickButton()
    {
        Debug.Log("ÀÀ¾Ö");
    }


    public void itemUpdate()
    {
        int NumRandom_Item = Random.Range(1, 6);
        Debug.Log(NumRandom_Item);

        switch (NumRandom_Item)
        {
            case 1:
                addItemCount[0]++;
                itemCount[0] = addItemCount[0];
                if (itemCount[0] >= 0)
                {
                    ActiveitemPanel[0].SetActive(false);
                    itemButton[0].enabled = true;
                }
                itemCountText[0].text = itemCount[0].ToString();
                break;
            case 2:
                addItemCount[1]++;
                itemCount[1] = addItemCount[1];
                if (itemCount[1] >= 0)
                {
                    ActiveitemPanel[1].SetActive(false);
                    itemButton[1].enabled = true;
                }
                itemCountText[1].text = itemCount[1].ToString();
                break;
            case 3:
                addItemCount[2]++;
                itemCount[2] = addItemCount[2];
                if (itemCount[2] >= 0)
                {
                    ActiveitemPanel[2].SetActive(false);
                    itemButton[2].enabled = true;
                }
                itemCountText[2].text = itemCount[2].ToString();
                break;
            case 4:
                addItemCount[3]++;
                itemCount[3] = addItemCount[3];
                if (itemCount[3] >= 0)
                {
                    ActiveitemPanel[3].SetActive(false);
                    itemButton[3].enabled = true;
                }
                itemCountText[3].text = itemCount[3].ToString();
                break;
            case 5:
                addItemCount[4]++;
                itemCount[4] = addItemCount[4];
                if (itemCount[4] >= 0)
                {
                    ActiveitemPanel[4].SetActive(false);
                    itemButton[4].enabled = true;
                }
                itemCountText[4].text = itemCount[4].ToString();
                break;
        }

    }

}