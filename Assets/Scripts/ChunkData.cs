using UnityEngine;
using System.Collections;

public class ChunkData : MonoBehaviour {
	public Vector3 m_pos;
	public GameObject player;
	public bool bOnce = false;
	public int i = 0;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("First Person Controller");
	}
	
	// Update is called once per frame
	void Update () {
		if(((player.GetComponent<PlayerData>().centerChunkPos.x - m_pos.x < 9) && (player.GetComponent<PlayerData>().centerChunkPos.x - m_pos.x > -9)) && ((player.GetComponent<PlayerData>().centerChunkPos.z - m_pos.z < 9) && (player.GetComponent<PlayerData>().centerChunkPos.z - m_pos.z > -9)))
		{
			//Debug.Log ("Name: " + gameObject.name  + " vect difference " + (player.GetComponent<PlayerData>().lastPosN - m_pos));
			if(!player.GetComponent<PlayerData>().chunks.Contains(this))
			{
				player.GetComponent<PlayerData>().chunks.Add (this);
			}
		}
		else
		{
			if(player.GetComponent<PlayerData>().chunks.Contains(this))
			{
				player.GetComponent<PlayerData>().chunks.Remove (this);
			}
		}
	}
}
