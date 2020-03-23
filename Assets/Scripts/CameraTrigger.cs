using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour {

    public Vector2 maxXandY, minXandY;

    private CameraFollow cameraFollow;

	// Use this for initialization
	void Start () {

        cameraFollow = FindObjectOfType<CameraFollow>();

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            cameraFollow.maxXAndY = maxXandY;
            cameraFollow.minXAndY = minXandY;
        }
    }

}
