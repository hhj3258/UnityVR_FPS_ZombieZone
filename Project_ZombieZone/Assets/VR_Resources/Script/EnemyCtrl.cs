using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 적 스크립트
// 생성되고 몇초 후 플레이어를 향해 이동
// 플레이어의 공격(총알)에 피격시 1초간 멈춤
// 플레이어에 닿으면 플레이어의 hp감소하고 적은 사라짐
public class EnemyCtrl : MonoBehaviour
{
    // 애니메이션 제어 컴포넌트
    private Animator animator;

    // 애니메이션 딜레이 시간
    // 딜레이가 0이면 이동
    // 딜레이가 0보다 크면 정지 (피격, 죽음 상태)
    // 초기 딜레이는 2 (2초 후 이동 시작)
    public float delayTime = 2;

    // 적의 hp
    public float hp = 100;

    // 적의 이동 속도
    private float speed = 2;

    // 적의 공격력
    private float damage = 10;

    // 플레이어의 위치
    private Transform player;



    // 상태 초기화
    void Start()
    {
        // 플레이어를 탐색
        player = FindObjectOfType<PlayerCtrl>().transform;

        animator = GetComponent<Animator>();

        // 움직이는 속도 랜덤 설정
        speed = Random.Range(1f, 3f);

        // 공격력 랜덤 설정
        damage = Random.Range(10, 20);
    }



    // 매 프레임 실행
    void Update()
    {
        // 딜레이가 0보다 클 때
        if (delayTime > 0 )
        {
            // 딜레이 감소
            delayTime -= Time.deltaTime;

            // 딜레이가 0보다 작아지면 움직이는 모션 실행
            if(delayTime <= 0)
                animator.SetBool("IsMove", true);
        }
        // 딜레이가 0이하 일 떄
        else
        {
            // 플레이어를 향해 이동
            this.transform.LookAt(player);
            this.transform.position += this.transform.forward * Time.deltaTime * speed;
        }
    }



    // 피격
    public void Attacked(float damage = 100)
    {

        // hp감소
        hp -= damage;

        // hp가 0이하면 죽음
        if (hp <= 0)
        {
            animator.SetTrigger("IsDie");
            delayTime = 5;
            Destroy(this.gameObject, 5);

            // 충돌 제거
            this.GetComponent<Collider>().enabled = false;

            GameManager.instance.UpdateKill();
        }
        // 그 외의 경우에는 피격 상태
        // 1초 동안 멈춤
        else
        {
            delayTime = 1;
            animator.SetBool("IsMove", false);
        }

    }



    // 적의 공격
    // 적이 플레이어에 다가와 닿으면 플레이어의 hp감소
    private void OnTriggerEnter(Collider other)
    {
        // 닿은 물체가 플레이어인 경우
        PlayerCtrl player = other.GetComponent<PlayerCtrl>();

        if (player != null)
        {
            // 플레이어 공격
            player.Attacked(damage);
            
            // 자신은 제거
            Destroy(this.gameObject);
        }
    }

}
