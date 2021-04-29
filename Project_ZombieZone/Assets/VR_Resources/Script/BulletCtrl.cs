using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // 총알 데미지
    public float damage = 40;

    // 총알 스피트
    public float speed = 100;

    // 총알이 무언가에 맞았을 때 나오는 스파크 이펙트
    public GameObject sparkEffect;



    // 상태 초기화
    void Start()
    {
        // 힘을 받아 앞으로 나아가게 함
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);

        // 총알 제거
        Destroy(this.gameObject, 5f);
    }



    void Update()
    {
        
    }


    // 충돌 했을 때
    private void OnCollisionEnter(Collision collision)
    {
        // 총알 스파크 생성
        GameObject spark = Instantiate(sparkEffect, this.transform.position, Quaternion.identity);
        // 스파크 일정 시간 뒤 제거
        Destroy(spark, spark.GetComponent<ParticleSystem>().duration + 0.2f);

        if(collision.collider.tag == "ENEMY")
        {
            EnemyCtrl enemy = collision.collider.GetComponent<EnemyCtrl>();
            if (enemy != null)
            {
                // 적 hp감소
                enemy.Attacked(damage);

                // 총알 제거
                Destroy(this.gameObject);
            }
        }

    }
}
