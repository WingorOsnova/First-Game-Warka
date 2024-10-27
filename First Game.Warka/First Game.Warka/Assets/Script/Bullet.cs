using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float deathTime;
    [SerializeField] int damage;
    [System.Serializable]

    public enum Type
    {
        Player,
        Enemy,
    }
    [SerializeField] Type type ;
       

    void Start()
    {
        Invoke(nameof(Death), deathTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Death();
        }
        if (collision.gameObject.tag == "Enemy" && type == Type.Player)
        {
            collision.gameObject.GetComponent<Enemy>().Damage(damage);     
            Death();
        }
        if (collision.gameObject.tag == "Player" && type == Type.Enemy)
        {
            collision.gameObject.GetComponent<Player>().Damage(damage);
            Death();
        }


    }
    void Death()
    {
        Destroy(gameObject);
    }
}
