using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// UI표시 : 플레이시간, 플레이어의 남은 HP
// 특정 시간마다 적 자동 생성
// 플레이어가 죽었을 때 게임 재시작
public class GameManager : MonoBehaviour
{

    // 외부 스크립트에서 쓰기 위한 객체
    public static GameManager instance;

    // 적 오브젝트 프리펩
    public GameObject enemy;

    // 적의 생성(스폰) 위치
    public Transform[] spwanPoint;

    // 스폰 딜레이
    private float spwanDelayTime = 0;

    // 게임 시작 여부
    // 게임 시작시 true로 변경
    // 게임 오버시 false로 변경
    public bool isStart = false;

    // 게임 진행시간 (플레이 타임)
    public float playTime = 0;

    // 플레이 타임을 보여주는 UI
    public Text playTimeText;

    // 플레이어의 남은 HP를 보여주는 UI
    public Text hpText;

    // 플레이어의 남은 탄창 보여주는 UI
    public Text magazineText;

    // 플레이어의 킬수 보여주는 UI
    public Text killText;
    private int killCount = 0;

    // 게임 오버시 보여주는 UI창
    public GameObject restart;



    // 상태 초기화
    void Start()
    {
        if (instance == null)
            instance = this;

        isStart = true;
        spwanDelayTime = 3;
    }



    // 매 프레임 실행
    void Update()
    {
        // 플레이 타임 증가
        playTime += Time.deltaTime;
        playTimeText.text = "Time : " + playTime.ToString("F1");


        // 스폰 딜레이에 따라 적 생성
        if (spwanDelayTime > 0)
        {
            spwanDelayTime -= Time.deltaTime;

            if(spwanDelayTime <= 0)
            {
                // 적 생성
                EnemySpwan();

                // 스폰 딜레이는 2~4초 값으로 재 설정
                spwanDelayTime = Random.Range(2f, 4f);
            }
        }


        // 게임 오버시 마우스 클릭하면 게임 재시작
        if(!isStart && Input.GetMouseButtonDown(0))
        {
            Restart();
        }

    }



    // 적 생성
    void EnemySpwan()
    {
        int idx = Random.Range(0, spwanPoint.Length);
        Instantiate(enemy, spwanPoint[idx]);
    }



    // 플레이어의 HP를 UI에 표시
    public void UpdateHP(float hp)
    {
        hpText.text = "HP : " + ((int)hp);

        if (hp <= 0)
            GameOver();
    }



    // 플레이어의 HP를 UI에 표시
    public void UpdateMagazine(int count)
    {
        magazineText.text = count.ToString();
    }



    // 플레이어의 킬 수를 UI에 표시
    public void UpdateKill()
    {
        killCount++;
        killText.text = "Kill : " + killCount.ToString();
    }
    


    // 게임 오버
    // 플레이어의 HP가 0이하일 때 실행
    void GameOver()
    {
        Time.timeScale = 0;
        isStart = false;
        restart.SetActive(true);
    }



    // 게임 재시작
    // 씬을 재 로드하면 재시작이 안되어서 종료로 변경
    void Restart()
    {
        //SceneManager.LoadScene(0);
        Application.Quit();
    }
}
