using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] int health;
    [SerializeField] float stopDistance,distanceToRunOut, speed;
    Player player;
    SpriteRenderer spR;
    [SerializeField] GameObject hitEffect;
    bool isDeath = false;

    bool canAttack = false;

    Vector3 addRandPosToGO;
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
        if(isDeath) return;
        Scale(player.transform.position);
    }

    public void Damage(int damage)
    {
        if (isDeath) return;
        health -= damage;

        Instantiate(hitEffect, transform.position, Quaternion.identity);

        if (health <= 0)Death(); 

    }
     private void FixedUpdate()
    {
        if (isDeath) return;
        if (Vector2.Distance(transform.position, player.transform.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGO, speed * Time.fixedDeltaTime);
            anim.SetBool("run", true);
            canAttack = false ;

        }
        else if (Vector2.Distance(transform.position, player.transform.position) < distanceToRunOut)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position + addRandPosToGO, -speed * Time.fixedDeltaTime);
            anim.SetBool("run", true);
            canAttack = false;

        }
        else
        {
            anim.SetBool("run", false);
            canAttack = true;

        }


    }

    IEnumerator SetRandomPos()
    { 
        addRandPosToGO = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(nameof(SetRandomPos));   
    }

    
    void Scale(Vector3 pos)
    {
        if (pos.x >= transform.position.x) transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else transform.rotation = Quaternion.Euler(0f, 180f, 0f);

    }

    void Death()
    {
        isDeath = true;
        anim.SetTrigger("death");
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
        return canAttack;
    }
}
