using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {

    public ConsumableItem item;
    private SpriteRenderer sprite;

	// Use this for initialization
	void Start () {

        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = item.image;

        for (int i = 0; i < Inventory.inventory.items.Count; i++)
        {
            if (Inventory.inventory.items[i] == item)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Inventory.inventory.AddItem(item);
            FindObjectOfType<UIManager>().UpdateUI();
            FindObjectOfType<UIManager>().SetMessage(item.message);
            Destroy(gameObject);
        }
    }
}
