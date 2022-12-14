using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    [Header("Countdown UI")]
    [SerializeField] private Text countdownDisplay;
    private int countdownTime;

    // Start is called before the first frame update
    void Start()
    {
        countdownTime = 3;

        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while(countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1.0f);

            countdownTime--;
        }

        countdownDisplay.text = "Start!";

        yield return new WaitForSeconds(1.0f);

        countdownDisplay.gameObject.SetActive(false);
        MiniGameController.Instance.StartMiniGame();

        yield return null;
    }
}