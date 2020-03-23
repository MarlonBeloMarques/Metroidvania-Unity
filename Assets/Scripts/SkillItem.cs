using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillItem : MonoBehaviour {

    public PlayerSkill skill;
    public string message;

	// Use this for initialization
	void Start () {
		
        if(FindObjectOfType<Player>().GetSkill(skill))
        {
            Destroy(gameObject);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player != null)
        {
            player.SetPlayerSkill(skill);
            FindObjectOfType<UIManager>().SetMessage(message);
            Destroy(gameObject);
        }
    }

}
