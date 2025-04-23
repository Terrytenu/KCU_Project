using UnityEngine;
using System.Collections;
//using static AutomaticGunScriptLPFP;

public class Gun : MonoBehaviour
{
    public float damage = 10f; // 공격데미지
    public float range = 30f; // Raycast 발사거리
    public GameObject Maincamera; // 발사 기준 카메라
    public ParticleSystem Muzzelflash; // 총구 불꽃효과
    public GameObject Flareeffect;
    public float Muzzelforce = 30f; // 적용할 힘
    //public AudioSource shootAudioSource;
    //public soundClips SoundClips;

    public int maxAmmo = 10;             // 최대 총알 수
    public float reloadTime = 2f;        // 재장전 소요 시간
    private int currentAmmo;             // 현재 남은 총알 수
    private bool isReloading = false;    // 재장전 여부

    private AudioSource shootAudioSource; // 총 발사 소리를 재생할 AudioSource
    public AudioClip shootSound;         // 총 발사 시 재생할 AudioClip

    private void Awake()
    {
        // 초기 총알 수 할당
        currentAmmo = maxAmmo;
        Debug.Log("게임 시작: 현재 총알 = " + currentAmmo);

        shootAudioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        // 재장전 중이면 발사하지 않음
        if (isReloading)
            return;

        // 발사 입력 체크
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire1 버튼 누름. currentAmmo = " + currentAmmo);

            if (currentAmmo > 0)
            {
                shoot();

            }
            else
            {
                // 총알 없으면 재장전 시작
                Debug.Log("총알 부족. 재장전 시작합니다.");
                StartCoroutine(reload());
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(reload());
        }

    }

    //void Start()
    //{
    //shootAudioSource.clip = SoundClips.shootSound;
    //}

    void shoot()
    {
        //shootAudioSource.Play();

        currentAmmo--;
        Debug.Log("발사 후 남은 총알: " + currentAmmo);

        // 총 발사 소리 재생
        if (shootAudioSource != null && shootSound != null)
        {
            shootAudioSource.PlayOneShot(shootSound);
        }

        if (Muzzelflash != null && !Muzzelflash.gameObject.activeSelf)
        {
            Muzzelflash.gameObject.SetActive(true);
        }

        Muzzelflash.Play();

        RaycastHit hit;

        // Maincamera에서 forward 방향으로 Raycast 발사
        if (Physics.Raycast(Maincamera.transform.position, Maincamera.transform.forward, out hit, range))
        {
            Debug.Log("충돌한 오브젝트 : " + hit.transform.name);

            // 충돌한 대상에 Target 컴포넌트가 있으면 데미지 적용
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            // Rigidbody가 있다면 물리적 힘 적용
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal * Muzzelforce);
            }

            // 충돌 지점에 피격 효과 생성 및 2초 후 제거
            GameObject MuzzelGO = Instantiate(Flareeffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(MuzzelGO, 2f);

        }
    }

    IEnumerator reload()
    {
        isReloading = true;
        Debug.Log("재장전 시작...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("재장전 완료! 총알: " + currentAmmo);
    }

}