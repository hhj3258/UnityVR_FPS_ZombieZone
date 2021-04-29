using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Anim
{
    public AnimationClip idle;
    public AnimationClip fire;
    public AnimationClip reload;
}
public class PlayerCtrl : MonoBehaviour
{
    public float hp = 100;

    private Animation animation;
    public Anim anim;
    public GameObject bullet;
    public ParticleSystem muzzleFlash;
    private int maxBullet = 12;
    public int remainBullet;
    public Transform firePos;
    private float delayTime = 0;
    private float delayTimeOffset = 0.5f;
    private float reloadTimeOffset = 2;

    
    private AudioSource audioSource;

    public AudioClip fireSound;
    public AudioClip reloadSound;

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animation>();
        animation.clip = anim.idle;

        audioSource = GetComponent<AudioSource>();

        remainBullet = maxBullet;
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
            return;

        if (delayTime > 0)
            delayTime -= Time.deltaTime;
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (remainBullet > 0)
                    Fire();
                else
                    Reload();

                GameManager.instance.UpdateMagazine(remainBullet);
            }
        }
    }

    void Fire()
    {
        remainBullet--;

        animation.CrossFade(anim.fire.name, 0.3f);

        delayTime = delayTimeOffset;

        Instantiate(bullet, firePos.position, firePos.rotation);

        muzzleFlash.Play();

        audioSource.clip = fireSound;
        audioSource.Play();
    }

    void Reload()
    {
        remainBullet = maxBullet;

        animation.CrossFade(anim.reload.name, 0.3f);

        delayTime = reloadTimeOffset;

        audioSource.clip = reloadSound;
        audioSource.Play();
    }

    public void Attacked(float damage)
    {
        if (hp <= 0)
            return;

        hp -= damage;

        if (hp <= 0)
            hp = 0;

        GameManager.instance.UpdateHP(hp);
    }
}
