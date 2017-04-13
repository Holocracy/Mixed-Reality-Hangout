using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Debug.Log(PhotonNetwork.player.isLocal);
		
	}
	
	// Update is called once per frame
	void Update () {

     

        if (PhotonView.isMine)
        {
            this.transform.position = Camera.main.transform.position;
            this.transform.rotation = Camera.main.transform.rotation;
        }


    }
}
