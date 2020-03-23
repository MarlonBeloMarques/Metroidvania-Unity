using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorDrop : MonoBehaviour {

    public Armor armor;

    private SpriteRenderer sprite;

	// Use this for initialization
	void Start () {

        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = armor.image;

        for (int i = 0; i < Inventory.inventory.armors.Count; i++)
        {
            if(Inventory.inventory.armors[i] == armor)
            {
                Destroy(gameObject);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player != null)
        {
            player.AddArmor(armor);
            Inventory.inventory.AddArmor(armor);
            FindObjectOfType<UIManager>().SetMessage(armor.message);
            Destroy(gameObject);
        }
    }
}
