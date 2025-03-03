using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    public GameObject missilePrefab;  // 발사할 미사일 프리팹
    public Transform firePoint;       // 미사일이 발사될 위치와 방향

    private void Update()
    {
        // 예를 들어 Fire2 (마우스 오른쪽 버튼) 입력 시 미사일 발사
        if (Input.GetButtonDown("Fire2"))
        {
            LaunchMissile();
        }
    }

    void LaunchMissile()
    {
        // firePoint의 위치와 회전을 기준으로 미사일 인스턴스 생성
        Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
    }
}

