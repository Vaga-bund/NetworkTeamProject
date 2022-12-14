using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviourPun
{
    PhotonView pv;
    public SlotMachineMgr slotMachineMgr;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pv.RPC("Open_BoardSprint_MiniGame", RpcTarget.AllViaServer);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            pv.RPC("Open_DodgeBomb_MiniGame", RpcTarget.AllViaServer);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pv.RPC("OpenSlotMachine", RpcTarget.All);

        }

    }

    [PunRPC]
    public void Open_BoardSprint_MiniGame()
    {
        PhotonNetwork.LoadLevel("MinigameSprint");
    }
    
    [PunRPC]
    public void Open_DodgeBomb_MiniGame()
    {
        PhotonNetwork.LoadLevel("MinigameDodge");
    }

    [PunRPC]
    public void OpenSlotMachine()
    {
        slotMachineMgr.slotActive(true);
    }
}
