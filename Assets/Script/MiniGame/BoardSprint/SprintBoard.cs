using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SprintBoard : MonoBehaviourPun
{
    public GameObject player;
    private void Start()
    {
        //var minigameController = FindObjectOfType<MiniGameController>();
        //minigameController.MinigameEvent.AddListener(RespawnPlayer);
        MiniGameController.Instance.MinigameEvent.AddListener(RespawnPlayer);
    }

    void RespawnPlayer()
    {
        Instantiate(player,
                                    Return_RandomPosition(),
                                    Quaternion.identity);
        //GameObject tempPlayer = PhotonNetwork.Instantiate("PlayerSample",
        //                            new Vector3(12.24f, 2.49f, 0),
        //                            Quaternion.identity,
        //                            0);

    }

    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = new Vector3(0.0f, 0.0f, 0.0f);

        float range_X = 4.5f;
        float range_Y = 4.0f;
        float range_Z = 3.0f;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}