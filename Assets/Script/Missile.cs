
using Meta.WitAi;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 20f;         // 미사일의 속도
    public float lifeTime = 5f;       // 미사일이 날아가는 최대 시간
    public float damage = 50f;        // 미사일 데미지
    public GameObject explosionEffect; // 충돌 시 생성할 폭발 효과 프리팹 (옵션)

    public AudioClip flightSound;     // 미사일이 날아갈 때 나는 소리
    public AudioClip explosionSound;  // 미사일 폭발 소리

    private Rigidbody rb;

    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && flightSound != null)
        {
            audioSource.clip = flightSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        // 미사일을 발사체의 forward 방향으로 일정 속도로 날리기
        // rb.velocity = transform.forward * speed;
        rb.velocity = transform.right * speed;

        // lifeTime 후에 미사일 파괴
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌 시, 충돌한 대상에 Target 스크립트가 있다면 데미지 적용
        Target target = collision.gameObject.GetComponent<Target>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }

        // 폭발 효과 생성 
        if (explosionEffect != null)
        {
            // 충돌한 첫 번째 접촉 지점 사용
            Vector3 contactPoint = collision.contacts[0].point;
            GameObject explosion = Instantiate(explosionEffect, contactPoint, Quaternion.identity);
            Destroy(explosion, 2f);
        }

        // 폭발 사운드
        if (explosionSound != null)
        {
            Debug.Log("폭발 소리 재생 시도");
            AudioSource.PlayClipAtPoint(explosionSound, collision.contacts[0].point);
        }

        // 미사일 파괴
        Destroy(gameObject);
    }
}


