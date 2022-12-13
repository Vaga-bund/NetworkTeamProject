using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
/*
public class MiniGameController
{
    [Header("MinigameEvents")]
    public UnityEvent MinigameEvent = new UnityEvent();

    private static MiniGameController instance = null;

    private MiniGameController()
    {
        instance = this;
    }
    public static MiniGameController Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new MiniGameController();
            }
            return instance;
        }
    }

    public void StartMiniGame()
    {
        MinigameEvent.Invoke();
    }
}
*/
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
                //���� �ν��Ͻ��� ���ٸ� �ϳ� �����ؼ� �־��ش�.
                instance = new MiniGameController();
            }
            return instance;
        }
    }

    public void StartMiniGame()
    {
        MinigameEvent.Invoke();
    }
}