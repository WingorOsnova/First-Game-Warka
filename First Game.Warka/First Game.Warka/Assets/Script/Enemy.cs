using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] int health;
    [SerializeField] float stopDistance,distanceToRunOut, speed;
    protected Player player;
    SpriteRenderer spR;
    [SerializeField] GameObject hitEffect;
    bool isDeath = false;

    bool canAttack = false;

    Vector3 addRandPosToGO;
    [SerializeField] ParticleSystem footParticale;
    [SerializeField] int minCoinsAdd, maxCoinsAdd;
    [SerializeField] AudioClip heartClip, deathClip;
    [SerializeField]protected AudioClip attacClip;
    public virtual void Start()
    {
       rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = Player.instance;
        spR = GetComponent<SpriteRenderer>();
        StartCoroutine(nameof(SetRandomPos));
        EnemySortingLayerManager.Instance.Add(spR);
    }

    private void OnDestroy()
    {
        EnemySortingLayerManager.Instance.Dell(spR);
    }
    public virtual void Update()
    {
        if(isDeath || !player) return;
        Scale(player.transform.position);
    }

    public void Damage(int damage)
    {
        if (isDeath ) return;
        health -= damage;

        Instantiate(hitEffect, transform.position, Quaternion.identity);
        SoundManager.instance.PlayerSound(heartClip);

        if (health <= 0)Death(); 

    }
     private void FixedUpdate()
    {
        if (isDeath) return;
        if (player && Vector2.Distance(transform.position, player.transform.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGO, speed * Time.fixedDeltaTime);
            anim.SetBool("run", true);

            footParticale.Pause();
            footParticale.Play();

            var emission = footParticale.emission;

            emission.rateOverTime = 10;
            canAttack = false ;

        }
        else if(player && Vector2.Distance(transform.position, player.transform.position) < distanceToRunOut)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGO, -speed * Time.fixedDeltaTime);
            anim.SetBool("run", true);

            footParticale.Pause();
            footParticale.Play();

            var emission = footParticale.emission;

            emission.rateOverTime = 10;
            canAttack = false;

        }
        else
        {
            anim.SetBool("run", false);


            var emission = footParticale.emission;

            emission.rateOverTime = 0;
            canAttack = true;

        }


    }

    IEnumerator SetRandomPos()
    { 
        addRandPosToGO = new Vector3(Random.Range(-stopDistance + 0.1f, stopDistance - 0.1f), Random.Range(-stopDistance + 0.1f, stopDistance - 0.1f));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(nameof(SetRandomPos));   
    }

    
    void Scale(Vector3 pos)
    {
        if (pos.x >= transform.position.x) transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else transform.rotation = Quaternion.Euler(0f, 180f, 0f);

    }

   protected void Death()
    {
        isDeath = true;
        Player.instance.AddMoney(Random.Range(minCoinsAdd, maxCoinsAdd));
        if (PlayerPrefs.GetInt("Position3") == 1) Player.instance.AddHeath(1);
        anim.SetTrigger("death");
        SoundManager.instance.PlayerSound(deathClip);

    }

    public IEnumerator DestroyObj()
    { 
        
        while (spR.color.a > 0)
        {
            float p = spR.color.a;
            spR.color = new Color(255f,255f,255f,p - 0.1f );
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }

    public virtual bool CheckIfCanAttack()
    { 
        return canAttack && !isDeath;
    }
}
