using UnityEngine;
using System.Collections.Generic;

public class FlarePool : MonoBehaviour
{
    public static FlarePool Instance; // 싱글톤 인스턴스

    public GameObject flarePrefab; // 플레어 프리팹 (인스펙터에서 할당)
    public int poolSize = 20; // 초기 풀 크기

    private Queue<GameObject> pool; // 풀 저장소

    void Awake()
    {
        Instance = this; // 싱글톤 초기화
        InitializePool();
    }

    // 풀 초기화
    private void InitializePool()
    {
        pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject flare = Instantiate(flarePrefab);
            flare.SetActive(false);
            flare.transform.SetParent(transform); // 부모 오브젝트에 정리
            pool.Enqueue(flare);
        }
    }

    // 풀에서 플레어 가져오기
    public GameObject GetFlare()
    {
        if (pool.Count > 0)
        {
            GameObject flare = pool.Dequeue();
            flare.SetActive(true);
            return flare;
        }
        else
        {
            // 풀이 비었을 경우 새로 생성
            GameObject newFlare = Instantiate(flarePrefab);
            newFlare.SetActive(true);
            return newFlare;
        }
    }

    // 플레어 반환
    public void ReturnFlare(GameObject flare)
    {
        flare.SetActive(false);
        pool.Enqueue(flare);
    }
}