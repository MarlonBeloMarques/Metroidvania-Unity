using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float speed;
    public int health;
    public GameObject itemDrop;
    public ConsumableItem item;
    public int damage;
    public int souls;
    public Vector2 damageForce;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anima;
    private Vector3 playerDistance;
    private bool facingRight = false;
    private bool isDead = false;
    private SpriteRenderer sprite;
	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anima = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

       if(!isDead)
        {
            playerDistance = player.transform.position - transform.position;
            if (Mathf.Abs(playerDistance.x) < 12 && Mathf.Abs(playerDistance.y) < 3)
            {
                rb.velocity = new Vector2(speed * (playerDistance.x / Mathf.Abs(playerDistance.x)), rb.velocity.y);
            }

            anima.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

            if (rb.velocity.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (rb.velocity.x < 0 && facingRight)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void TakeDamage( int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            anima.SetTrigger("Dead");
            FindObjectOfType<Player>().souls += souls;
            FindObjectOfType<UIManager>().UpdateUI();
            if(item != null)
            {
                GameObject tempItem = Instantiate(itemDrop, transform.position, transform.rotation);
                tempItem.GetComponent<ItemDrop>().item = item;
            }
        }
        else
        {
            StartCoroutine(DamageCoroutine());
        }
    }

    IEnumerator DamageCoroutine()
    {
        for (float i = 0; i < 0.2; i += 0.2f)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            player.TakeDamage(damage);
            Vector2 newDamageForce = new Vector2(damageForce.x * (playerDistance.x / Mathf.Abs(playerDistance.x)), damageForce.y);
            player.GetComponent<Rigidbody2D>().AddForce(newDamageForce, ForceMode2D.Impulse);
        }
    }
}
