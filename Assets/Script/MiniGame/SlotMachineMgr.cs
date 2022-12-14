using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineMgr : MonoBehaviour
{
    public GameObject[] SlotSkillObject;
    public Sprite[] SkillSprite;

    PhotonView pv;

    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> SlotSprite = new List<Image>();
    }
    public DisplayItemSlot[] DisplayItemSlots;

    private List<int> StartList = new List<int>();
    private List<int> ResultIndexList = new List<int>();
    int ItemCnt = 2;
    int[] answer = { 2, 1 };

    int randomIndex;

    bool isSlot = false;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ItemCnt * DisplayItemSlots.Length; i++)
        {
            StartList.Add(i);
        }

        for (int i = 0; i < DisplayItemSlots.Length; i++)
        {

            for (int j = 0; j < ItemCnt; j++)
            {
                randomIndex = Random.Range(0, StartList.Count);
                if (i == 0 && j == 1 || i == 1 && j == 0 || i == 2 && j == 2)
                {
                    ResultIndexList.Add(StartList[randomIndex]);
                }
                DisplayItemSlots[i].SlotSprite[j].sprite = SkillSprite[StartList[randomIndex]];

                if (j == 0)
                {
                    DisplayItemSlots[i].SlotSprite[ItemCnt].sprite = SkillSprite[StartList[randomIndex]];
                }
                StartList.RemoveAt(randomIndex);
            }

        }

        gameObject.SetActive(false);
    }


    public void slotActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        isSlot = true;

        if(isActive)
        {
            StartSlotMachine();
        }
    }
    public void StartSlotMachine()
    {
        for (int i = 0; i < DisplayItemSlots.Length; i++)
        {
            StartCoroutine(StartSlot(i));
        }
    }
    IEnumerator StartSlot(int SlotIndex)
    {
        for (int i = 0; i < (ItemCnt * (6 + SlotIndex * 2) + answer[SlotIndex]) * 3; i++)
        {
            SlotSkillObject[SlotIndex].transform.localPosition -= new Vector3(0, 50f, 0);
            if (SlotSkillObject[SlotIndex].transform.localPosition.y < 50f)
            {
                SlotSkillObject[SlotIndex].transform.localPosition += new Vector3(0, 200f, 0);
            }
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1.0f);
    }
}