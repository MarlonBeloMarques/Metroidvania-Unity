using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public Key key;
    public Sprite doorOpen;
    private SpriteRenderer sprite;
    private BoxCollider2D colisor;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        colisor = GetComponent<BoxCollider2D>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(Inventory.inventory.CheckKey(key))
            {
                sprite.sprite = doorOpen;
                colisor.enabled = false;
            }
        }
        else
        {
            FindObjectOfType<UIManager>().SetMessage("Precisa da " + key.keyName);
        }
    }
}
