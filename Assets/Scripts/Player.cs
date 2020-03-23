using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerSkill
{
    dash, doubleJump
}

public class Player : MonoBehaviour {

    private Rigidbody2D rig2d;
    private SpriteRenderer sprite;

    public int souls;
    public Transform groundCheck;
    public float maxSpeed;
    public float dashForce;
    private float speed;
    private bool facinRight = true;
    private bool onGround;
    private bool jump = false;
    private bool doubleJump;
    public float jumpForce;
    public Weapon weaponEquipped;
    private Animator anima;
    private Attack attack;
    public float fireRate;
    private float nextAttack;
    public ConsumableItem item;
    public int maxHealth;
    public int maxMana;
    private int health;
    private int mana;
    public int strength;
    public int defense;
    public Armor armor;
    private bool canDamage = true;
    private bool isDead = false;
    private bool dash = false;
    public bool doubleJumpSkill = false;
    public bool dashSkill = false;
    public int manaCost;
    public Rigidbody2D projectil;
    private GameManager gm;

	// Use this for initialization
	void Start () {
        rig2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        gm = GameManager.gm;
        anima = GetComponent<Animator>();
        attack = GetComponentInChildren<Attack>();
        SetPlayer();
        health = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {

        if(!isDead)
        {
            onGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
            if (onGround)
            {
                doubleJump = false;
            }

            if (Input.GetButtonDown("Jump") && (onGround || (!doubleJump && doubleJumpSkill)))
            {
                jump = true;
                if (!doubleJump && !onGround)
                {
                    doubleJump = true;
                }
            }

            if (Input.GetButtonDown("Fire1") && Time.time > nextAttack && weaponEquipped != null)
            {
                dash = false;
                anima.SetTrigger("Attack");
                attack.PlayAnimation(weaponEquipped.animation);
                nextAttack = Time.time + fireRate;
            }

            if (Input.GetButtonDown("Fire3"))
            {
                UseItem(item);
                Inventory.inventory.RemoveItem(item);
                FindObjectOfType<UIManager>().UpdateUI();
            }

            if(Input.GetKeyDown(KeyCode.Q) && onGround && !dash && dashSkill)
            {
                rig2d.velocity = Vector2.zero;
                anima.SetTrigger("Dash");
            }

            if(Input.GetButtonDown("Fire2") && mana >= manaCost)
            {
                Rigidbody2D tempProjectile = Instantiate(projectil, transform.position, transform.rotation);
                tempProjectile.GetComponent<Attack>().SetWeapon(50);
                if(facinRight)
                {
                    tempProjectile.AddForce(new Vector2(5, 10),ForceMode2D.Impulse);
                }
                else
                {
                    tempProjectile.AddForce(new Vector2(-5, 10), ForceMode2D.Impulse);
                }

                mana -= manaCost;
                FindObjectOfType<UIManager>().UpdateUI();
            }
        }
       

    }

    private void FixedUpdate()
    {
        if(!isDead)
        {
            float h = Input.GetAxisRaw("Horizontal");

            if (canDamage && !dash)
            {
                rig2d.velocity = new Vector2(h * speed, rig2d.velocity.y);
            }

            anima.SetFloat("Speed", Mathf.Abs(h));

            if (h > 0 && !facinRight)
            {
                Flip();
            }
            else if (h < 0 && facinRight)
            {
                Flip();
            }

            if (jump)
            {
                rig2d.velocity = Vector2.zero;
                rig2d.AddForce(Vector2.up * jumpForce);
                jump = false;
            }

            if(dash)
            {
                int hforce = facinRight ? 1 : -1;
                rig2d.velocity = Vector2.left * dashForce * hforce;
            }
        }
       
    }

    void Flip()
    {
        facinRight = !facinRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

    }

    public void AddWeapon(Weapon weapon)
    {
        weaponEquipped = weapon;
        attack.SetWeapon(weaponEquipped.damage);
    }

    public void AddArmor(Armor item)
    {
        armor = item;
        defense = armor.defense;
    }

    public void UseItem(ConsumableItem item)
    {
        health += item.healthGain;
    
        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        mana += item.manaGain;

        if(mana >=maxMana)
        {
            mana = maxMana;
        }

    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMana()
    {
        return mana;
    }

    public void TakeDamage(int damage)
    {
        if(canDamage)
        {
            canDamage = false;
            health -= (damage - defense);
            FindObjectOfType<UIManager>().UpdateUI();
            if (health <= 0)
            {
                anima.SetTrigger("Dead");
                Invoke("ReloadScene", 3f);
                isDead = true;
            }
            else
            {
                StartCoroutine(DamageCoroutine());
            }
        }
    }

    IEnumerator DamageCoroutine()
    {
        for (float i = 0; i < 0.6f; i+=0.2f)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        canDamage = true;
    }

    void ReloadScene()
    {
        Souls.instance.gameObject.SetActive(true);
        Souls.instance.souls = souls;
        Souls.instance.transform.position = transform.position;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DashTrue()
    {
        dash = true;
    }

    public void DashFalse()
    {
        dash = false;
    }

    public void SetPlayerSkill(PlayerSkill skill)
    {
        if(skill == PlayerSkill.dash)
        {
            dashSkill = true;
        }
        else if(skill == PlayerSkill.doubleJump)
        {
            doubleJumpSkill = true;
        }
    }

    public void SetPlayer()
    {
        Vector3 playerPos = new Vector3(gm.playerPosX, gm.playerPosY, 0);
        transform.position = playerPos;
        maxHealth = gm.health;
        maxMana = gm.mana;
        speed = maxSpeed;
        health = maxHealth;
        mana = maxMana;
        strength = gm.streength;
        souls = gm.souls;
        doubleJumpSkill = gm.canDoubleJump;
        dashSkill = gm.canBackDash;

        if(gm.currentArmorId > 0)
        {
            AddArmor(Inventory.inventory.itemDataBase.GetArmor(gm.currentArmorId));
        }
        if(gm.currentWeaponId > 0)
        {
            AddWeapon(Inventory.inventory.itemDataBase.GetWeapon(gm.currentWeaponId));
        }
    }

    public bool GetSkill(PlayerSkill skill)
    {
        if (skill == PlayerSkill.dash)
        {
            return dashSkill;
        }
        else if (skill == PlayerSkill.doubleJump)
        {
            return doubleJumpSkill;
        }

        else return false;
    }
}
