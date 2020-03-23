using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    private Animator anima;
    private int damage;

	// Use this for initialization
	void Start () {
        anima = GetComponent<Animator>();
	}

    public void PlayAnimation(AnimationClip clip)
    {
        anima.Play(clip.name);
    }

    public void SetWeapon(int damageValue)
    {
        damage = damageValue;
    }

    public int GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage + FindObjectOfType<Player>().strength);
        }
    }
}
