using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
public class MiniGameController
{
    private static MiniGameController instance;

    public UnityEvent MinigameEvent = new UnityEvent();

    public static MiniGameController Instance
    {
        get
        {
            if (null == instance)
            {
                //게임 인스턴스가 없다면 하나 생성해서 넣어준다.
                instance = new MiniGameController();
            }
            return instance;
        }
    }

    public void StartMiniGame()
    {
        //PhotonView.RPC("StartAllEvent", RpcTarget.All);
        MinigameEvent.Invoke();
    }

    [PunRPC]
    void StartAllEvent()
    {
        MinigameEvent.Invoke();
    }
}