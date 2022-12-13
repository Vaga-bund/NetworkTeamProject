using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    //int Count = 0;
    [SerializeField]
    [Header("ETC")]
    Text joinInfoText;
    string joinLobbyState = "";
    public PhotonView PV;

    #region �г���, ���Ӱ��� string / bool
    string playerName = "";

    bool isGameStart = false;
    bool isLoggin = false;
    bool isReady = false;

    int playerNum = 1;
    int myNum = 0;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    #endregion

    [Header("MainMenuPanel")]
    public GameObject mainMenuPanel;
    public Text welComeText;
    public Button lobbyJoinBtn;
    public Button settingBtn;
    public Button quitBtn;

    [Header("UserNameCreatePanel")]
    public GameObject userNameCreatePanel;
    public Text nickNameCheck;
    string jnickNameCheckState = "";
    public Button joinBtn;
    public InputField nickNameInputField;

    [Header("SettingPanel")]
    public GameObject settingPanel;
    public InputField nickNameCurInput;
    public Slider soundSlider;
    public Toggle soundToggle;
    public Button closeBtn;
    public Button changeNickNameBtn;

    [Header("MainLobbyPanel")]
    public GameObject mainLobbyPanel;
    public Text lobbyInfoText;
    public InputField roomInputField;
    public Button[] CellBtn;
    public Button roomFindBtn;
    public Button roomCreateBtn;
    public Button lobbyCloseBtn;
    public Button PreviousBtn;
    public Button NextBtn;


    [Header("RoomCreatePanel")]
    public GameObject roomCreatePanel;
    public Button roomCreatePanelCloseBtn;
    public Button createBtn;
    public InputField roomNameInput;
    public Toggle roomPw;
    public Text playerNumText;

    //[Header("RoomPwPanel")]
    [Header("RoomPanel")]
    public GameObject roomPanel;
    public InputField chatInput;
    public Text[] chatText;
    public Text roomInfoText;
    public Button[] gameStart_ReadyBtn;
    public Toggle[] playerCount;


    private void Awake()
    {
        userNameCreatePanel.SetActive(true);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        jnickNameCheckState = "";
        if (nickNameCheck)
        {
            nickNameCheck.text = jnickNameCheckState;
        }

        joinLobbyState = "";
        if (joinInfoText)
        {
            joinInfoText.text = PhotonNetwork.NetworkClientState.ToString();
        }
    }
    public static NetworkManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(NetworkManager)) as NetworkManager;

                if (instance == null)
                    Debug.Log("no singleton obj");
            }

            return instance;
        }
    }


    void Start()
    {

    }

    void Update()
    {
        CheckLobbyPlayerCount();
    }

    public override void OnConnectedToMaster() => isReady = true;

    public void Connect()
    {
        if (PhotonNetwork.IsConnected && isReady)
        {
            joinLobbyState = "���� ���� ����";
            if (joinInfoText)
            {
                joinInfoText.text = joinLobbyState;
            }

        }
        else
        {
            joinLobbyState = "�������� : ������ ������ ������� ����\n���� ��õ���...";
            if (joinInfoText)
            {
                joinInfoText.text = joinLobbyState;
            }
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public bool master()
    {
        return PhotonNetwork.LocalPlayer.IsMasterClient;
    }

    #region �г��Ӽ���
    public void SetPlayerName()
    {
        if (isGameStart == false && isLoggin == false)
        {
            if (nickNameInputField.text == null || nickNameInputField.text == ""
                || nickNameInputField.text == " " || nickNameInputField.text == "  "
                || nickNameInputField.text == "  " || nickNameInputField.text == "   ")
            {
                jnickNameCheckState = "��ĭ �г����� ����Ͻ� �� �����ϴ�.";
                nickNameCheck.text = jnickNameCheckState;
                nickNameCheck.color = Color.red;
                nickNameInputField.text = string.Empty;

            }
            if (nickNameInputField.text.Length < 4 || nickNameInputField.text.Length > 8)
            {
                jnickNameCheckState = "4���� �̻� 8���� ���Ϸ�\n�г����� �ۼ����ּ���.";
                nickNameCheck.text = jnickNameCheckState;
                nickNameCheck.color = Color.red;
                nickNameInputField.text = string.Empty;

            }
            else
            {
                jnickNameCheckState = "�г����� �����Ǿ����ϴ�.\n��ø� ��ٷ��ּ���";
                nickNameCheck.text = jnickNameCheckState;
                nickNameCheck.color = Color.blue;
                playerName = nickNameInputField.text;
                PhotonNetwork.LocalPlayer.NickName = playerName;
                nickNameInputField.text = string.Empty;
                //Debug.Log("connect �õ�!" + isGameStart + ", " + isLoggin);
                PhotonNetwork.ConnectUsingSettings();
                OnConnectedToMaster();
                OpenMainMenu();
            }
        }
    }
    #endregion

    #region ���θ޴�����
    public void OpenMainMenu()
    {
        Connect();
        userNameCreatePanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        welComeText.text = playerName + " ���� �����ϼ̽��ϴ�.\nȯ���մϴ�.";
    }
    #endregion

    #region �κ񿭱�
    public void OpenLobby()
    {
        mainMenuPanel.SetActive(false);
        mainLobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        myList.Clear();
        CheckLobbyPlayerCount();
        joinLobbyState = "�κ� ���� ����";
        Debug.Log("?");
        if (joinInfoText)
            joinInfoText.text = joinLobbyState;
    }

    public void LeaveLobby()
    {
        //PhotonNetwork.LeaveLobby();
        GameToMainMenu();
    }
    #endregion

    #region �κ� ������ �� ȣ��
    public void CheckLobbyPlayerCount()
    {
        lobbyInfoText.text = "�κ�: " + (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + " / " + "����: " + PhotonNetwork.CountOfPlayers;
    }
    #endregion

    #region ��������
    public void OpenSetting()
    {
        settingPanel.SetActive(true);
        nickNameCurInput.placeholder.GetComponent<Text>().color = new Color(0.38f, 0.38f, 0.38f, 0.5f);
        nickNameCurInput.placeholder.GetComponent<Text>().text = "������ �г����� �ۼ����ּ���.";
    }
    #endregion

    #region ��������
    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }
    #endregion

    #region ����ȭ�����ΰ���
    public void GameToMainMenu()
    {
        mainLobbyPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        joinLobbyState = "���� ���� ����";
        if (joinInfoText)
            joinInfoText.text = joinLobbyState;
    }
    #endregion

    #region �г��Ӻ���
    public void ChangeName()
    {
        if (nickNameCurInput.text == null || nickNameCurInput.text == ""
                || nickNameCurInput.text == " " || nickNameCurInput.text == "  "
                || nickNameCurInput.text == "  " || nickNameCurInput.text == "   ")
        {
            nickNameCurInput.text = string.Empty;
            nickNameCurInput.placeholder.GetComponent<Text>().color = new Color(1.0f, 0.0f, 0.0f, 0.5f); ;
            nickNameCurInput.placeholder.GetComponent<Text>().text = "��ĭ �г����� ����Ͻ� �� �����ϴ�.";
        }
        if (nickNameCurInput.text.Length < 4 || nickNameCurInput.text.Length > 8)
        {
            nickNameCurInput.text = string.Empty;
            nickNameCurInput.placeholder.GetComponent<Text>().color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
            nickNameCurInput.placeholder.GetComponent<Text>().text = "4���� �̻� 8���� ���Ϸ� �г����� �ۼ����ּ���.";

        }
        else
        {
            playerName = nickNameCurInput.text;
            PhotonNetwork.LocalPlayer.NickName = playerName;
            nickNameCurInput.text = string.Empty;
            nickNameCurInput.placeholder.GetComponent<Text>().color = new Color(0.38f, 0.38f, 0.38f, 0.5f);
            nickNameCurInput.placeholder.GetComponent<Text>().text = "����Ǿ����ϴ�.";
            welComeText.text = playerName + " ���� �����ϼ̽��ϴ�.\nȯ���մϴ�.";
        }
    }
    #endregion

    #region �游���, ����, ������
    public void openCreateroomPanel()
    {
        roomCreatePanel.SetActive(true);
        playerNumText.text = playerNum.ToString();
        roomNameInput.text = "";

    }
    public void CloseCreateroomPanel()
    {
        roomCreatePanel.SetActive(false);
    }

    public void playerNumUp()
    {
        if (playerNum < 8)
        {
            playerNum++;
            playerNumText.text = playerNum.ToString();
        }
    }
    public void playerNumDown()
    {
        if (playerNum > 1)
        {
            playerNum--;
            playerNumText.text = playerNum.ToString();
        }
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomNameInput.text == "" ?
            "Room" + Random.Range(0, 100) : roomNameInput.text, new RoomOptions { MaxPlayers = (byte)playerNum });
        MyListRenewal();
    }
    public override void OnJoinedRoom()
    {
        mainLobbyPanel.SetActive(false);
        roomCreatePanel.SetActive(false);
        roomPanel.SetActive(true);
        joinLobbyState = "�� ���� ����";
        if (joinInfoText)
            joinInfoText.text = joinLobbyState;

        if (master())
        {
            int max = PhotonNetwork.CurrentRoom.MaxPlayers - 1;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable
            {
                { "0", PhotonNetwork.LocalPlayer.ActorNumber }, { "1", 0 }, { "2", 2<= max ? 0 : -1 },
                { "3", 2<= max ? 0 : -1 }, { "4", 2<= max ? 0 : -1 }, { "5", 2<= max ? 0 : -1 },
                { "6", 2<= max ? 0 : -1 }, { "7", 2<= max ? 0 : -1 }
            });
        }
        RoomRenewal();
        chatInput.text = "";
        for (int i = 0; i < chatText.Length; i++) chatText[i].text = "";
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { roomNameInput.text = ""; CreateRoom(); }

    public override void OnJoinRandomFailed(short returnCode, string message) { roomNameInput.text = ""; CreateRoom(); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "���� �����ϼ̽��ϴ�</color>");
    }

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        roomPanel.SetActive(false);
        mainLobbyPanel.SetActive(true);
        MyListRenewal();
        RoomRenewal();
        OnJoinedLobby();
        joinLobbyState = "�κ� ���� ����";
        if (joinInfoText)
            joinInfoText.text = joinLobbyState;
    }

    void RoomRenewal()
    {
        for (int i = 0; i < playerCount.Length; i++)
        {
            playerCount[i].transform.GetChild(1).GetComponent<Text>().text = "";
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerCount[i].transform.GetChild(1).GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
        }

        if (master())
        {
            gameStart_ReadyBtn[1].gameObject.SetActive(false);
            gameStart_ReadyBtn[0].interactable = true;
        }
        else
        {
            gameStart_ReadyBtn[0].gameObject.SetActive(false);
            gameStart_ReadyBtn[1].interactable = true;
        }

        roomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "�� / " + "�ִ�" + PhotonNetwork.CurrentRoom.MaxPlayers + "��";
    }

    // ���� ����, �κ񳪰���
    public void Disconnect() => PhotonNetwork.Disconnect();

    //���� �Ѱ��ֱ�
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[0]);
    }


    public void InitGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount != playerNum) return;

        PV.RPC("InitGameRPC", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void InitGameRPC()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                myNum = i;
            }
        }
        SceneManager.LoadScene("MainGameScene");
    }
    #endregion


    #region �渮��Ʈ ����
    // ����ư -2 , ����ư -1 , �� ����
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        multiple = 0;
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // �ִ�������
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // ����, ������ư
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // �������� �´� ����Ʈ ����
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion

    #region ä��
    public void Send()
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + chatInput.text);
        chatInput.text = "";
    }

    [PunRPC] // RPC�� �÷��̾ �����ִ� �� ��� �ο����� �����Ѵ�
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < chatText.Length; i++)
            if (chatText[i].text == "")
            {
                isInput = true;
                chatText[i].text = msg;
                break;
            }
        if (!isInput) // ������ ��ĭ�� ���� �ø�
        {
            for (int i = 1; i < chatText.Length; i++) chatText[i - 1].text = chatText[i].text;
            chatText[chatText.Length - 1].text = msg;
        }
    }
    #endregion


    public void Click()
    {
        PV.RPC("MasterReceiveRPC", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void MasterReceiveRPC()
    {
        PV.RPC("OtherReceiveRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void OtherReceiveRPC()
    {

    }

}