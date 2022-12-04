using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour {

	static Rigidbody rb;
	public static Vector3 diceVelocity;
	public bool isThrowDice = false;
    public int PrintDiceCount = 0;

    // Use this for initialization
    void Start () {
		rb = GetComponent<Rigidbody> ();
        StartCoroutine(Dice());
    }

    // Update is called once per frame
    void Update()
    {
        diceVelocity = rb.velocity;
    }

    IEnumerator Dice()
    {
        GameManager.Instance.diceNumber = 0;
        yield return new WaitForSeconds(0.2f);
            float dirX = Random.Range(0, 500);
            float dirY = Random.Range(0, 500);
            float dirZ = Random.Range(0, 500);
            transform.rotation = Quaternion.identity;
            rb.AddTorque(dirX, dirY, dirZ);
    }

    public void ThrowDice()
    {
        isThrowDice = true;
        PrintDiceCount = 1;
        //Debug.Log("닿임");
        //gameObject.SetActive(false);
        GameManager.Instance.diceNumPanel.SetActive(true);
        if (PrintDiceCount == 1)
        {
            int DiceRandomNum = Random.Range(1, 6);

            switch (DiceRandomNum)
            {
                case 1:
                    gameObject.name = "DiceNum_6";
                    break;
                case 2:
                    gameObject.name = "DiceNum_5";
                    break;
                case 3:
                    gameObject.name = "DiceNum_4";
                    break;
                case 4:
                    gameObject.name = "DiceNum_3";
                    break;
                case 5:
                    gameObject.name = "DiceNum_2";
                    break;
                case 6:
                    gameObject.name = "DiceNum_1";
                    break;
            }

            switch (gameObject.name)
            {
                case "DiceNum_1":
                    GameManager.Instance.diceNumber = 6;
                    break;
                case "DiceNum_2":
                    GameManager.Instance.diceNumber = 5;
                    break;
                case "DiceNum_3":
                    GameManager.Instance.diceNumber = 4;
                    break;
                case "DiceNum_4":
                    GameManager.Instance.diceNumber = 3;
                    break;
                case "DiceNum_5":
                    GameManager.Instance.diceNumber = 2;
                    break;
                case "DiceNum_6":
                    GameManager.Instance.diceNumber = 1;
                    break;
            }
        }
        GameManager.Instance.DiceNum();
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.CompareTag("Player"))
    //    {
    //    }
    //}
}
