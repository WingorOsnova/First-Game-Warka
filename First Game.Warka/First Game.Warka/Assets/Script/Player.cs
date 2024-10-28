using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int health;
    int maxHealth;
    Rigidbody2D rb;
    Vector2 moveVelicity;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootpos;
    [SerializeField] float timeBtwShoot = 2;
    float shootTimer;
    Animator animator;
    SpriteRenderer spR;

    [SerializeField] TextMeshProUGUI text;
    public static Player instance;
    [SerializeField] GameObject hitEffect;

    [SerializeField] Sprite[] spritesMuzzleFlash;
    [SerializeField] SpriteRenderer muzzleFlashSpr;
    [SerializeField] float dashForce, timeBtwDash, dashTime;
    float dashTimer;
    bool isDashing = false;

    [SerializeField] Slider healthBar;
    [SerializeField] Slider dashthBar;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        spR = GetComponent<SpriteRenderer>();
        
        shootTimer = timeBtwShoot;
        dashTimer = timeBtwDash;
        maxHealth = health;
        UpdateHealthUI();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && shootTimer >= timeBtwShoot)
        {
            Shoot();
            shootTimer = 0;

        }

        dashTimer += Time.deltaTime;
        dashthBar.value =  dashTimer / timeBtwDash;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashTimer >= timeBtwDash)
            { 
                dashTimer = 0;
                ActivateDash();
            }
        }

        if (timeBtwShoot - shootTimer < 0) return;
        text.text = ((int)
            ((timeBtwShoot - shootTimer)
            * 100)
            /100f).ToString();

    }
    #region Base Function move  
    private void FixedUpdate()
    {
        Move();

        if (isDashing) Dash();
    }
    void Dash()
    {
        rb.AddForce(moveVelicity * Time.fixedDeltaTime * dashForce * 100);
    }
    void ActivateDash()
    { 
        isDashing = true;

        Invoke(nameof(DeActivateDash), dashTime);
    }
    void DeActivateDash()
    {
        isDashing = false;
        
    }
    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"),
                   Input.GetAxisRaw("Vertical"));
        Debug.Log(moveInput);

        if(moveInput != Vector2.zero)
        {
            animator.SetBool("Run", true);
                
        }
        else
        {
            animator.SetBool("Run", false);
        }

        ScalePlayer(moveInput.x);

        moveVelicity = moveInput.normalized * speed;
        rb.MovePosition(rb.position + moveVelicity * Time.fixedDeltaTime);
    }

    void ScalePlayer(float x)
    {
        if (x == 1)
            spR.flipX = false;

        else if (x == -1) spR.flipX = true;
    }
    
    #endregion 
    void Shoot()
    {
       Instantiate(bullet, shootpos.position,shootpos.rotation);
        StartCoroutine(nameof(SetMuzzleFlash));
    }

    IEnumerator SetMuzzleFlash()
    {
        muzzleFlashSpr.enabled = true;
        muzzleFlashSpr.sprite = spritesMuzzleFlash[Random.Range(0, spritesMuzzleFlash.Length)];

        yield return new WaitForSeconds(0.1f);

        muzzleFlashSpr.enabled = false;
    }

    public void Damage(int damage)
    {
        health -= damage;
        
        Instantiate(hitEffect, transform.position, Quaternion.identity);

        camerafollow.instance.CamShake();

        UpdateHealthUI();

        if (health <= 0) Destroy(gameObject);
        

    }

    void UpdateHealthUI()
    {
        healthBar.value = (float)health / maxHealth;
    }    
}


     