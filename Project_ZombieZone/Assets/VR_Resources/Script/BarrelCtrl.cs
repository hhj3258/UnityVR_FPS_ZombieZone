using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    // 폭발 이펙트
    public GameObject explosionEffect;

    // 드럼통 텍스쳐
    public Texture[] textures;

    // 찌그러진 드럼통 모양
    public Mesh[] meshes;

    // 드럼통 모양을 바꿀 컴포넌트
    private MeshFilter meshFilter;

    // 드럼통에 힘을 가할 컴포넌트
    private Rigidbody rigid;


    // 이 드럼통이 맞은 횟수
    public int hitCount = 0;


    // 폭발 사운드
    private AudioSource audioSource;



    // 상태 초기화
    void Start()
    {
        // 랜덤으로 드럼통 텍스쳐를 적용
        // 빨, 노, 파 중 하나로 적용
        int idx = Random.Range(0, textures.Length);
        GetComponent<MeshRenderer>().material.mainTexture = textures[idx];

        // 컴포넌트 설정
        meshFilter = GetComponentInChildren<MeshFilter>();
        rigid = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }



    void Update()
    {
        
    }


    // 충돌 했을 때
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌체가 Bullet인 경우
        if(collision.collider.tag == "BULLET")
            // 맞은 횟수 증가
            hitCount++;

        // 맞은 횟수가 3이상이면 폭발
        if (hitCount >= 3)
            Explosion();
    }


    // 폭발
    void Explosion()
    {

        // 사운드 재생
        audioSource.Play();

        // 맞은 횟수 초기화
        hitCount = 0;

        // 폭발 이펙트 생성
        GameObject explo = Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
        // 폭발 이펙트는 5초뒤 제거
        Destroy(explo, 5);

        // 현재 물체의 질량 감소
        rigid.mass = 5;
        // 현재 물체에 힘을 가함(위쪽으로 1000만큼)
        rigid.AddForce(Vector3.up * 1000);

        // 주변에 힘을 가함
        IndirectDamage();

        // 랜덤으로 드럼통 찌그러진 모양을 적용
        int idx = Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[idx];

        // 5초뒤 이 드럼통 제거
        Destroy(this.gameObject, 5);
    }



    // 주변에 힘을 가함
    void IndirectDamage()
    {
        // 자신을 중심으로 반경 7이내의 물체를 탐색
        Collider[] colls = Physics.OverlapSphere(this.transform.position, 5f);

        // 위에서 탐색된 물체에 대해서
        foreach(Collider coll in colls)
        {
            // 강체(Rigidbody)인지를 체크
            Rigidbody rigid = coll.GetComponent<Rigidbody>();

            // 이 물체가 강체 인 경우
            if(rigid != null)
            {
                // 이물체에 힘을 가함
                rigid.AddExplosionForce(1200, this.transform.position, 5, 1000);
            }

            // 이 물체가 적인 경우
            EnemyCtrl enemy = coll.GetComponent<EnemyCtrl>();
            if (enemy != null)
            {
                // 적에게 100만큼 데미지
                enemy.Attacked(100);
            }
        }
        

    }
}
