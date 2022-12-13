using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviourPun
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitGameRPC();
        }
    }

    [PunRPC]
    public void InitGameRPC()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {

            }
        }
        PhotonNetwork.LoadLevel("MiniGameSprint");
    }
}
