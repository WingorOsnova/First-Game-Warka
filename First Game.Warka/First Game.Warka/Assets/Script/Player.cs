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

    [SerializeField] Transform shootPos;
    [SerializeField] Transform[] shootSuperPos;

    [SerializeField] float timeBtwShoot = 2;
    float shootTimer;

    [SerializeField] float timeBtwSuperShoot = 2;
    float shootSuperTimer;

    Animator animator;
    SpriteRenderer spR;

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI Supertext;

    public static Player instance;
    [SerializeField] GameObject hitEffect;

    [SerializeField] Sprite[] spritesMuzzleFlash;
    [SerializeField] SpriteRenderer muzzleFlashSpr;
    [SerializeField] float dashForce, timeBtwDash, dashTime;
    float dashTimer;
    bool isDashing = false;

    [SerializeField] Slider healthBar;
    [SerializeField] Slider dashthBar;
    bool canBEDamage = true;
    [SerializeField] ParticleSystem footParticale;
    [SerializeField] GameObject deathPanel;


    private void Awake()
    {
        instance = this;

        Shop.instance.buySeconPosition += UpdateTimeBtwShoot;
        Shop.instance.buySeconPositionDash += UpdateTimeBtwDash;

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
        shootSuperTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && shootSuperTimer >= timeBtwSuperShoot && PlayerPrefs.GetInt("Position1") == 1)
        {
            SuperShoot();
            shootSuperTimer = 0;

        }

        dashTimer += Time.deltaTime;
        dashthBar.value = dashTimer / timeBtwDash;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashTimer >= timeBtwDash)
            {
                dashTimer = 0;
                ActivateDash();
            }
        }

        if (timeBtwSuperShoot - shootSuperTimer < 0) return;
        text.text = ((int)
            ((timeBtwSuperShoot - shootSuperTimer)
            * 100)
            / 100f).ToString();



    }
    #region Base Function move  
    private void FixedUpdate()
    {
        Move();

        if (isDashing) Dash();
    }

    void UpdateTimeBtwShoot()
    {
        timeBtwShoot -= 0.3f;
        timeBtwSuperShoot -= 2.0f;
    }
    void UpdateTimeBtwDash()
    {
        timeBtwDash -= 2.0f;
    }
    void Dash()
    {
        rb.AddForce(moveVelicity * Time.fixedDeltaTime * dashForce * 100);
    }
    void ActivateDash()
    {
        isDashing = true;
        canBEDamage = false;

        Invoke(nameof(DeActivateDash), dashTime);
    }
    void DeActivateDash()
    {
        isDashing = false;
        canBEDamage = true;

    }
    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"),
                   Input.GetAxisRaw("Vertical"));
        Debug.Log(moveInput);

        if (moveInput != Vector2.zero)
        {
            animator.SetBool("Run", true);

            footParticale.Pause();
            footParticale.Play();

            var emission = footParticale.emission;

            emission.rateOverTime = 10;
        }
        else
        {
            animator.SetBool("Run", false);

            var emission = footParticale.emission;

            emission.rateOverTime = 0;
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
        Instantiate(bullet, shootPos.position, shootPos.rotation);
        StartCoroutine(nameof(SetMuzzleFlash));
    }
    void SuperShoot()
    {
        for (int i = 0; i < shootSuperPos.Length; i++)
        {
            Instantiate(bullet, shootSuperPos[i].position, shootSuperPos[i].rotation);
        }
        
        camerafollow.instance.CamShake();

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
        if (!canBEDamage) return;
        health -= damage;

        Instantiate(hitEffect, transform.position, Quaternion.identity);

        camerafollow.instance.CamShake();

        UpdateHealthUI();

        if (health <= 0)
        {
            deathPanel.SetActive(true);
            Destroy(gameObject);

        }
    }
    public void AddHeath(int value)
    { 
        health += value;
        if (health > maxHealth) health = maxHealth;
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthBar.value = (float)health / maxHealth;
    }
    [HideInInspector] public int currentMoney;
    [SerializeField] TextMeshProUGUI coinsText;
    public void AddMoney(int value)
    { 
        currentMoney += value;
        coinsText.text = "У вас: " + currentMoney.ToString() + " Монеток";
    }
}


     