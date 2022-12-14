using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPun
{

    [Header("DiceNunm")]
    public GameObject diceNumPanel;
    public Text diceNunmtext;
    public int diceNumber;

    [Header("Board")]
    public GameObject board;
    public Transform[] board_pos;
    public List<Transform> board_area = new List<Transform>();

    private PhotonView PV;



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

        PV = GetComponent<PhotonView>();
    }



    // Use this for initialization
    void Start()
    {
        diceNumPanel.SetActive(false);
        FillNode();
    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(CreatePlayer());
    }

    IEnumerator CreatePlayer()
    {
        while (!NetworkManager.instance.isGameStart)
        {
            yield return new WaitForSeconds(0.5f);
        }

        GameObject tempPlayer = PhotonNetwork.Instantiate("PlayerParnet",
                                    new Vector3(0, 1.85f, 0),
                                    Quaternion.identity,
                                    0);


        yield return null;
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

    [PunRPC]
    public void SpawnPlayer()
    {
        StartCoroutine(CreatePlayer());
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




}