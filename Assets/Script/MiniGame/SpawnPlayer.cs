using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SpawnPlayer : MonoBehaviourPunCallbacks
{
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {

        if(pv.IsMine)
        {
            //RespawnPlayer();
            //pv.RPC("RespawnPlayer", RpcTarget.All);
            MiniGameController.Instance.MinigameEvent.AddListener(RespawnPlayer);
        }
        else
        {
            //pv.RPC("RespawnPlayer", RpcTarget.All);
            MiniGameController.Instance.MinigameEvent.AddListener(RespawnPlayer);
        }
    }

    [PunRPC]
    void RespawnPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate("PlayerSample", Return_PlayerRandomPos(), Quaternion.identity, 0);
        //StartCoroutine(CreatePlayer());
    }

    IEnumerator CreatePlayer()
    {
        GameObject tempPlayer = PhotonNetwork.Instantiate("PlayerSample",
                                    Return_PlayerRandomPos(),
                                    Quaternion.identity,
                                    0);

        yield return null;
    }

    Vector3 Return_PlayerRandomPos()
    {
        Vector3 originPosition = new Vector3(0.0f, 0.0f, 0.0f);

        float range_X = 4.5f;
        float range_Y = 1.0f;
        float range_Z = 3.0f;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}