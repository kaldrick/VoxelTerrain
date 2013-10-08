using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour {
	public Vector3 centerChunkPos;
	public List<ChunkData> chunks;
	public GenerateTerrain terrain;
	public enum Dir { North, NorthEast, NorthWest, West, East, South, SouthEast, SouthWest };
	public Dir currentDir;
	VoxelChunk m_voxelChunktemp; 
	//public System.Predicate<ChunkData> chunkPredicate = new System.Predicate<ChunkData>(checkChunk);
	// Use this for initialization
	void Start () {
		terrain = GameObject.Find ("TerrainGenerator").GetComponent<GenerateTerrain>();
	}
	void checkChunk(ChunkData chunk)
	{
		//N
		if(!GameObject.Find ("Voxel Mesh " + (chunk.m_pos.x+8).ToString() + " " + (chunk.m_pos.y).ToString() + " " + (chunk.m_pos.z).ToString()))
		{
			PerlinNoise m_surfacePerlin = new PerlinNoise(terrain.m_surfaceSeed);
			Vector3 pos = new Vector3(chunk.m_pos.x+9, chunk.m_pos.y+1, chunk.m_pos.z+1);
			m_voxelChunktemp = new VoxelChunk(pos, terrain.m_voxelWidth, terrain.m_voxelHeight, terrain.m_voxelLength, terrain.m_surfaceLevel);
			m_voxelChunktemp.CreateVoxels (m_surfacePerlin);
			m_voxelChunktemp.CreateMesh (terrain.m_material);
			//chunk.bOnce = true;
		}
		//NW
		if(!GameObject.Find ("Voxel Mesh " + (chunk.m_pos.x+8).ToString() + " " + (chunk.m_pos.y).ToString() + " " + (chunk.m_pos.z + 8).ToString()))
		{
			PerlinNoise m_surfacePerlin = new PerlinNoise(terrain.m_surfaceSeed);
			Vector3 pos = new Vector3(chunk.m_pos.x+9, chunk.m_pos.y+1, chunk.m_pos.z+9);
			m_voxelChunktemp = new VoxelChunk(pos, terrain.m_voxelWidth, terrain.m_voxelHeight, terrain.m_voxelLength, terrain.m_surfaceLevel);
			m_voxelChunktemp.CreateVoxels (m_surfacePerlin);
			m_voxelChunktemp.CreateMesh (terrain.m_material);
			//chunk.bOnce = true;
		}
		//NE
		if(!GameObject.Find ("Voxel Mesh " + (chunk.m_pos.x+8).ToString() + " " + (chunk.m_pos.y).ToString() + " " + (chunk.m_pos.z - 8).ToString()))
		{
			PerlinNoise m_surfacePerlin = new PerlinNoise(terrain.m_surfaceSeed);
			Vector3 pos = new Vector3(chunk.m_pos.x+9, chunk.m_pos.y+1, chunk.m_pos.z-7);
			m_voxelChunktemp = new VoxelChunk(pos, terrain.m_voxelWidth, terrain.m_voxelHeight, terrain.m_voxelLength, terrain.m_surfaceLevel);
			m_voxelChunktemp.CreateVoxels (m_surfacePerlin);
			m_voxelChunktemp.CreateMesh (terrain.m_material);
			//chunk.bOnce = true;
		}
		//S
		if(!GameObject.Find ("Voxel Mesh " + (chunk.m_pos.x-8).ToString() + " " + (chunk.m_pos.y).ToString() + " " + (chunk.m_pos.z).ToString()))
		{
			PerlinNoise m_surfacePerlin = new PerlinNoise(terrain.m_surfaceSeed);
			Vector3 pos = new Vector3(chunk.m_pos.x-7, chunk.m_pos.y+1, chunk.m_pos.z+1);
			m_voxelChunktemp = new VoxelChunk(pos, terrain.m_voxelWidth, terrain.m_voxelHeight, terrain.m_voxelLength, terrain.m_surfaceLevel);
			m_voxelChunktemp.CreateVoxels (m_surfacePerlin);
			m_voxelChunktemp.CreateMesh (terrain.m_material);
			//chunk.bOnce = true;
		}
		//SW
		if(!GameObject.Find ("Voxel Mesh " + (chunk.m_pos.x-8).ToString() + " " + (chunk.m_pos.y).ToString() + " " + (chunk.m_pos.z + 8).ToString()))
		{
			PerlinNoise m_surfacePerlin = new PerlinNoise(terrain.m_surfaceSeed);
			Vector3 pos = new Vector3(chunk.m_pos.x-7, chunk.m_pos.y+1, chunk.m_pos.z+9);
			m_voxelChunktemp = new VoxelChunk(pos, terrain.m_voxelWidth, terrain.m_voxelHeight, terrain.m_voxelLength, terrain.m_surfaceLevel);
			m_voxelChunktemp.CreateVoxels (m_surfacePerlin);
			m_voxelChunktemp.CreateMesh (terrain.m_material);
			//chunk.bOnce = true;
		}
		//SE
		if(!GameObject.Find ("Voxel Mesh " + (chunk.m_pos.x-8).ToString() + " " + (chunk.m_pos.y).ToString() + " " + (chunk.m_pos.z - 8).ToString()))
		{
			PerlinNoise m_surfacePerlin = new PerlinNoise(terrain.m_surfaceSeed);
			Vector3 pos = new Vector3(chunk.m_pos.x-7, chunk.m_pos.y+1, chunk.m_pos.z-7);
			m_voxelChunktemp = new VoxelChunk(pos, terrain.m_voxelWidth, terrain.m_voxelHeight, terrain.m_voxelLength, terrain.m_surfaceLevel);
			m_voxelChunktemp.CreateVoxels (m_surfacePerlin);
			m_voxelChunktemp.CreateMesh (terrain.m_material);
			//chunk.bOnce = true;
		}
		//W
		if(!GameObject.Find ("Voxel Mesh " + (chunk.m_pos.x).ToString() + " " + (chunk.m_pos.y).ToString() + " " + (chunk.m_pos.z + 8).ToString()))
		{
			PerlinNoise m_surfacePerlin = new PerlinNoise(terrain.m_surfaceSeed);
			Vector3 pos = new Vector3(chunk.m_pos.x+1, chunk.m_pos.y+1, chunk.m_pos.z+9);
			m_voxelChunktemp = new VoxelChunk(pos, terrain.m_voxelWidth, terrain.m_voxelHeight, terrain.m_voxelLength, terrain.m_surfaceLevel);
			m_voxelChunktemp.CreateVoxels (m_surfacePerlin);
			m_voxelChunktemp.CreateMesh (terrain.m_material);
			//chunk.bOnce = true;
		}
		//E
		if(!GameObject.Find ("Voxel Mesh " + (chunk.m_pos.x).ToString() + " " + (chunk.m_pos.y).ToString() + " " + (chunk.m_pos.z - 8).ToString()))
		{
			PerlinNoise m_surfacePerlin = new PerlinNoise(terrain.m_surfaceSeed);
			Vector3 pos = new Vector3(chunk.m_pos.x+1, chunk.m_pos.y+1, chunk.m_pos.z-7);
			m_voxelChunktemp = new VoxelChunk(pos, terrain.m_voxelWidth, terrain.m_voxelHeight, terrain.m_voxelLength, terrain.m_surfaceLevel);
			m_voxelChunktemp.CreateVoxels (m_surfacePerlin);
			m_voxelChunktemp.CreateMesh (terrain.m_material);
			//chunk.bOnce = true;
		}
		//Debug.Log ("Difference: " + (chunk.m_pos - centerChunkPos) + " Chunk name: " + chunk.name + " Current Dir: " + currentDir);
		/*if((((chunk.m_pos.z - centerChunkPos.z == -8 || chunk.m_pos.z - centerChunkPos.z == 8)  && chunk.m_pos.x - centerChunkPos.x == 0) || (chunk.m_pos.x - centerChunkPos.x == 0 && chunk.m_pos.z - centerChunkPos.z == 0)) && currentDir == Dir.North)
		{
			if(!GameObject.Find ("Voxel Mesh " + (chunk.m_pos.x+8).ToString() + " " + (chunk.m_pos.y).ToString() + " " + (chunk.m_pos.z).ToString()))
			{
				PerlinNoise m_surfacePerlin = new PerlinNoise(terrain.m_surfaceSeed);
				Vector3 pos = new Vector3(chunk.m_pos.x+9, chunk.m_pos.y+1, chunk.m_pos.z+1);
				m_voxelChunktemp = new VoxelChunk(pos, terrain.m_voxelWidth, terrain.m_voxelHeight, terrain.m_voxelLength, terrain.m_surfaceLevel);
				m_voxelChunktemp.CreateVoxels (m_surfacePerlin);
				m_voxelChunktemp.CreateMesh (terrain.m_material);
				Debug.Log ("Voxel Mesh "  + (chunk.m_pos.x+8).ToString() + " " + (chunk.m_pos.y).ToString() + " " + (chunk.m_pos.z).ToString());
				//chunk.bOnce = true;
			}
		}*/
		/*if(((chunk.m_pos.x - centerChunkPos.x == -8 || chunk.m_pos.x - centerChunkPos.x == 8) && (chunk.m_pos.z - centerChunkPos.z == 0)) || (chunk.m_pos - centerChunkPos) == new Vector3(0,0,0))
		{
			Debug.Log ("CurrentDir (wersja 1)" + currentDir);
			PerlinNoise m_surfacePerlin = new PerlinNoise(terrain.m_surfaceSeed);
		//	m_voxelChunktemp = new VoxelChunk[1, 2, 1];
		//	for(int x = 0; x<1; x++)
		//	{
		//		for (int y = 0; y<2; y++)
		//		{
		//			for(int z = 0; z< 1; z++)
		//			{

						Vector3 pos = new Vector3(chunk.m_pos.x+9, chunk.m_pos.y, chunk.m_pos.z + 9);
						m_voxelChunktemp = new VoxelChunk(pos, terrain.m_voxelWidth, terrain.m_voxelHeight, terrain.m_voxelLength, terrain.m_surfaceLevel);
						m_voxelChunktemp.CreateVoxels (m_surfacePerlin);
						m_voxelChunktemp.CreateMesh (terrain.m_material);
						
		//			}
		//		}
		//	}
			i++;
			Debug.Log ("Stworzylem : " + i + " obiektow " + pos);
		}*/
		
	/*	if(((chunk.m_pos.z - centerChunkPos.z == -8 || chunk.m_pos.z - centerChunkPos.z == 8) && (chunk.m_pos.x - centerChunkPos.x == 0)) || (chunk.m_pos - centerChunkPos) == new Vector3(0,0,0))
		{
			Debug.Log ("CurrentDir (wersja 2)" + currentDir);
			PerlinNoise m_surfacePerlin = new PerlinNoise(terrain.m_surfaceSeed);
			m_voxelChunktemp = new VoxelChunk[1, 2, 1];
			for(int x = 0; x<1; x++)
			{
				for (int y = 0; y<2; y++)
				{
					for(int z = 0; z< 1; z++)
					{
						Vector3 pos = new Vector3(chunk.m_pos.x+9, chunk.m_pos.y, chunk.m_pos.z + 1);
						m_voxelChunktemp[x, y, z] = new VoxelChunk(pos, terrain.m_voxelWidth, terrain.m_voxelHeight, terrain.m_voxelLength, terrain.m_surfaceLevel);
						m_voxelChunktemp[x, y, z].CreateVoxels (m_surfacePerlin);
						m_voxelChunktemp[x, y, z].CreateMesh (terrain.m_material);
						
					}
				}
			}
		}*/
		//return chunk;	
	}
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Physics.Raycast(transform.position, -Vector3.up, out hit);
		if(centerChunkPos != hit.transform.GetComponent<ChunkData>().m_pos)
		{
			centerChunkPos = hit.transform.GetComponent<ChunkData>().m_pos;
			Debug.Log ("ZMIENIAM" + centerChunkPos);
		}
		if(chunks.Count < 18)
		{
			chunks.ForEach(checkChunk);	
		}
	}
}
