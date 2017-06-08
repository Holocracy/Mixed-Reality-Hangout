using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : Photon.MonoBehaviour {
    public GameObject floorGrid;

	// Use this for initialization
	void Start () {

        Debug.Log(PhotonNetwork.player.isLocal);
        floorGrid = GameObject.Find("FloorGrid");
		
	}
	
	// Update is called once per frame
	void Update () {

     

        if (photonView.isMine)
        {
            //Only update the X and Z positons (no height movements, just along the ground plane). Fix y height at floor plane level (that's where the Avatar's feet go)
            this.transform.position = new Vector3(Camera.main.transform.position.x,floorGrid.transform.position.y, Camera.main.transform.position.z);
            
            //allow rotation only along one axis, to prevent awkward pivoting of the avatar when the user looks up and down
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x, floorGrid.transform.position.y, this.transform.position.z);
        }


    }
}
