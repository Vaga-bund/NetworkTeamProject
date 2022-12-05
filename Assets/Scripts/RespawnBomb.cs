using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBomb : MonoBehaviour
{
    [Header("RespawnPoints")]
    [SerializeField] private GameObject rangeObject;
    [SerializeField] private BoxCollider rangeCollider;
    [Header("Bombs")]
    [SerializeField] private GameObject bombs;

    private void Start()
    {
        StartCoroutine(RandomRespawn_Coroutine());
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            GameObject instantBomb = Instantiate(bombs, Return_RandomPosition(), Quaternion.identity);
        }
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;

        float range_X = rangeCollider.bounds.size.x;
        float range_Y = 10.0f;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}