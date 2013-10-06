using UnityEngine;
using System.Collections;

public class GenerateTerrain : MonoBehaviour 
{
	public Material m_material;
	GameObject m_mesh;
	
	VoxelChunk[,,] m_voxelChunk;
	VoxelChunk[,,] m_voxelChunktemp;
	
	public int m_surfaceSeed = 4, m_caveSeed = 6;
	public int m_chunksX = 6, m_chunksY = 2, m_chunksZ = 6;
	public int m_voxelWidth = 32, m_voxelHeight = 32, m_voxelLength = 32;
	public int m_chunksAbove0 = 1;
	public float m_surfaceLevel = 0.0f;
	public bool bOnce = false;
	public int currentX, currentZ;
	public Transform player;
	public Vector3 lastPosN, lastPosNE, lastPosNW;
	void Start () 
	{
		//Make 2 perlin noise objects, one is used for the surface and the other for the caves
		PerlinNoise m_surfacePerlin = new PerlinNoise(m_surfaceSeed);
		player = GameObject.FindWithTag ("Player").transform;
	//	PerlinNoise  m_cavePerlin = new PerlinNoise(m_caveSeed);
	
		//Set some varibles for the marching cubes plugin
		MarchingCubes.SetTarget(0.0f);
		MarchingCubes.SetWindingOrder(2, 1, 0);
		MarchingCubes.SetModeToCubes();
		
		//create a array to hold the voxel chunks
		m_voxelChunk  = new VoxelChunk[m_chunksX,m_chunksY,m_chunksZ];
		
		//The offset is used to centre the terrain on the x and z axis. For the Y axis
		//you can have a certain amount of chunks above the y=0 and the rest will be below
		Vector3 offset = new Vector3(m_chunksX*m_voxelWidth*-0.5f, -(m_chunksY-m_chunksAbove0)*m_voxelHeight, m_chunksZ*m_voxelLength*-0.5f);
		
		for(int x = 0; x < m_chunksX; x++)
		{
			for(int y = 0; y < m_chunksY; y++)
			{
				for(int z = 0; z < m_chunksZ; z++)
				{
					//The position of the voxel chunk
					Vector3 pos = new Vector3(x*m_voxelWidth, y*m_voxelHeight, z*m_voxelLength);
					//Create the voxel object
					m_voxelChunk[x,y,z] = new VoxelChunk(pos+offset, m_voxelWidth, m_voxelHeight, m_voxelLength, m_surfaceLevel);
					//Create the voxel data
					m_voxelChunk[x,y,z].CreateVoxels(m_surfacePerlin);//, m_cavePerlin);
					//Smooth the voxels, is optional but I think it looks nicer
				//	m_voxelChunk[x,y,z].SmoothVoxels();
					//Create the normals. This will create smoothed normal.
					//This is optional and if not called the unsmoothed mesh normals will be used
				//	m_voxelChunk[x,y,z].CalculateNormals();
					//Creates the mesh form voxel data using the marching cubes plugin and creates the mesh collider
					m_voxelChunk[x,y,z].CreateMesh(m_material);
					
				}
			}
		}
		currentX = m_chunksX * m_voxelWidth;
		currentZ = m_chunksZ * m_voxelLength;
		
	}
	void Update()
	{
		RaycastHit hit;
		/*
		 * -------
		 * | |x| |
		 * -------
		 * | | | |
		 * -------
		 * | | | |
		 * -------
		 */ 
		if(Physics.Raycast (player.position + player.forward * 32 * m_voxelWidth, player.position + player.forward * 32 * m_voxelWidth - new Vector3(0, 128, 0), out hit))
		{
			Debug.DrawLine(player.position + player.forward * 32 * m_voxelWidth,  player.position + player.forward * 32 * m_voxelWidth - new Vector3(0, 128, 0), Color.red);
			//Debug.Log ("N Trafilem w: " + hit.transform.GetComponent<ChunkData>().m_pos);
			lastPosN = hit.transform.GetComponent<ChunkData>().m_pos;
		}
		else
		{
			if(Physics.Raycast (player.position + player.forward * 16 * m_voxelWidth, player.position + player.forward * 16 * m_voxelWidth - new Vector3(0, 128, 0), out hit))
			{
				Debug.DrawLine(player.position + player.forward * 16 * m_voxelWidth,  player.position + player.forward * 16 * m_voxelWidth - new Vector3(0, 128, 0), Color.green);
				//Debug.Log ("N Trafilem w: " + hit.transform.GetComponent<ChunkData>().m_pos);
				lastPosN = hit.transform.GetComponent<ChunkData>().m_pos;
				PerlinNoise m_surfacePerlin = new PerlinNoise(m_surfaceSeed);
				Vector3 offset = new Vector3(m_chunksX*m_voxelWidth*0.5f, -(m_chunksY-m_chunksAbove0)*m_voxelHeight, m_chunksZ*m_voxelLength*0.5f);
			//	Vector3 pos = lastPosN + new Vector3(108, 0, 0);
				m_voxelChunktemp = new VoxelChunk[1, 2, 1];
				for(int x = 0; x<1; x++)
				{
					for (int y = 0; y<2; y++)
					{
						for(int z = 0; z< 1; z++)
						{
							Vector3 pos = new Vector3((x+m_chunksX)*m_voxelWidth/32, y*m_voxelHeight, player.localPosition.z);
							Debug.Log ("POS: " + (pos + offset) + " / "  + (lastPosN + pos) + " / " + lastPosN);
							m_voxelChunktemp[x, y, z] = new VoxelChunk(pos, m_voxelWidth, m_voxelHeight, m_voxelLength, m_surfaceLevel);
							m_voxelChunktemp[x, y, z].CreateVoxels (m_surfacePerlin);
							m_voxelChunktemp[x, y, z].CreateMesh (m_material);
						}
					}
				}
				Debug.DrawLine(player.position + player.forward * 32 * m_voxelWidth,  -Vector3.up * 128, Color.red);
				m_chunksX++;
			}

		}
		/*if(player.localPosition.x > currentX)
		{
			wRaycastHit hit;
			Physics.Raycast (player.localPosition, -Vector3.up, out hit);
			Debug.Log ("HitX: " + hit.transform.localPosition.x + " / HitZ: " + hit.transform.localPosition.z);
			Vector3 offset = new Vector3(m_chunksX*m_voxelWidth*-0.5f, -(m_chunksY-m_chunksAbove0)*m_voxelHeight, m_chunksZ*m_voxelLength*-0.5f);
			PerlinNoise m_surfacePerlin = new PerlinNoise(m_surfaceSeed);
			m_voxelChunktemp = new VoxelChunk[1, m_chunksY, m_chunksZ];
			for(int x = 0; x<1; x++)
			{
				for (int y = 0; y<m_chunksY; y++)
				{
					for(int z = 0; z< m_chunksZ; z++)
					{
						Vector3 pos = new Vector3((x+m_chunksX)*m_voxelWidth, y*m_voxelHeight, z*m_voxelLength);
						m_voxelChunktemp[x, y, z] = new VoxelChunk(pos+offset, m_voxelWidth, m_voxelHeight, m_voxelLength, m_surfaceLevel);
						m_voxelChunktemp[x, y, z].CreateVoxels (m_surfacePerlin);
						m_voxelChunktemp[x, y, z].CreateMesh (m_material);
					}
				}
			}
			m_chunksX+= 1;
			currentX += 8*m_voxelWidth;
		}
		if(player.localPosition.z > currentZ)
		{
			Vector3 offset = new Vector3(m_chunksX*m_voxelWidth*-0.5f, -(m_chunksY-m_chunksAbove0)*m_voxelHeight, m_chunksZ*m_voxelLength*-0.5f);
			PerlinNoise m_surfacePerlin = new PerlinNoise(m_surfaceSeed);
			m_voxelChunktemp = new VoxelChunk[m_chunksX, m_chunksY, 1];
			for(int x = 0; x<m_chunksX; x++)
			{
				for (int y = 0; y<m_chunksY; y++)
				{
					for(int z = 0; z<1; z++)
					{
						Vector3 pos = new Vector3(x*m_voxelWidth, y*m_voxelHeight, (z+m_chunksZ)*m_voxelLength);
						m_voxelChunktemp[x, y, z] = new VoxelChunk(pos+offset, m_voxelWidth, m_voxelHeight, m_voxelLength, m_surfaceLevel);
						m_voxelChunktemp[x, y, z].CreateVoxels (m_surfacePerlin);
						m_voxelChunktemp[x, y, z].CreateMesh (m_material);
					}
				}
			}
			m_chunksZ+= 1;
			currentZ += 8*m_voxelLength;
		}
		if(player.localPosition.x < -currentX)
		{
			Vector3 offset = new Vector3(m_chunksX*m_voxelWidth*0.5f, -(m_chunksY-m_chunksAbove0)*m_voxelHeight, m_chunksZ*m_voxelLength*0.5f);
			PerlinNoise m_surfacePerlin = new PerlinNoise(m_surfaceSeed);
			m_voxelChunktemp = new VoxelChunk[1, m_chunksY, m_chunksZ];
			for(int x = 0; x<1; x++)
			{
				for (int y = 0; y<m_chunksY; y++)
				{
					for(int z = 0; z< m_chunksZ; z++)
					{
						Vector3 pos = new Vector3(-(x+m_chunksX)*m_voxelWidth, y*m_voxelHeight, -z*m_voxelLength);
						m_voxelChunktemp[x, y, z] = new VoxelChunk(pos+offset, m_voxelWidth, m_voxelHeight, m_voxelLength, m_surfaceLevel);
						m_voxelChunktemp[x, y, z].CreateVoxels (m_surfacePerlin);
						m_voxelChunktemp[x, y, z].CreateMesh (m_material);
					}
				}
			}
			m_chunksX+= 1;
			currentX += 8*m_voxelWidth;
		}
		if(player.transform.localPosition.z < -currentZ)
		{
			Vector3 offset = new Vector3(m_chunksX*m_voxelWidth*0.5f, -(m_chunksY-m_chunksAbove0)*m_voxelHeight, m_chunksZ*m_voxelLength*0.5f);
			PerlinNoise m_surfacePerlin = new PerlinNoise(m_surfaceSeed);
			m_voxelChunktemp = new VoxelChunk[m_chunksX, m_chunksY,1];
			for(int x = 0; x<m_chunksX; x++)
			{
				for (int y = 0; y<m_chunksY; y++)
				{
					for(int z = 0; z<1; z++)
					{
						Vector3 pos = new Vector3(-x*m_voxelWidth, y*m_voxelHeight, -(z+m_chunksZ)*m_voxelLength);
						m_voxelChunktemp[x, y, z] = new VoxelChunk(pos+offset, m_voxelWidth, m_voxelHeight, m_voxelLength, m_surfaceLevel);
						m_voxelChunktemp[x, y, z].CreateVoxels (m_surfacePerlin);
						m_voxelChunktemp[x, y, z].CreateMesh (m_material);
					}
				}
			}
			m_chunksZ+= 1;
			currentZ += 8*m_voxelLength;
		}*/
	}
	void Temp()
	{
		RaycastHit hit;
		/*
		 * -------
		 * | |x| |
		 * -------
		 * | | | |
		 * -------
		 * | | | |
		 * -------
		 */ 
		if(Physics.Raycast (player.position + player.forward * 32 * m_voxelWidth, player.position + player.forward * 32 * m_voxelWidth - new Vector3(0, 128, 0), out hit))
		{
			Debug.DrawLine(player.position + player.forward * 32 * m_voxelWidth,  player.position + player.forward * 32 * m_voxelWidth - new Vector3(0, 128, 0), Color.red);
			//Debug.Log ("N Trafilem w: " + hit.transform.GetComponent<ChunkData>().m_pos);
			lastPosN = hit.transform.GetComponent<ChunkData>().m_pos;
		}
		else
		{
			if(Physics.Raycast (player.position + player.forward * 16 * m_voxelWidth, player.position + player.forward * 16 * m_voxelWidth - new Vector3(0, 128, 0), out hit))
			{
				Debug.DrawLine(player.position + player.forward * 16 * m_voxelWidth,  player.position + player.forward * 16 * m_voxelWidth - new Vector3(0, 128, 0), Color.green);
				//Debug.Log ("N Trafilem w: " + hit.transform.GetComponent<ChunkData>().m_pos);
				lastPosN = hit.transform.GetComponent<ChunkData>().m_pos;
				PerlinNoise m_surfacePerlin = new PerlinNoise(m_surfaceSeed);
				Vector3 offset = new Vector3(m_chunksX*m_voxelWidth*0.5f, -(m_chunksY-m_chunksAbove0)*m_voxelHeight, m_chunksZ*m_voxelLength*0.5f);
			//	Vector3 pos = lastPosN + new Vector3(108, 0, 0);
				m_voxelChunktemp = new VoxelChunk[1, 2, 1];
				for(int x = 0; x<1; x++)
				{
					for (int y = 0; y<2; y++)
					{
						for(int z = 0; z< 1; z++)
						{
							Vector3 pos = new Vector3((x+lastPosN.x)*m_voxelWidth/32, y*m_voxelHeight, z*m_voxelLength);
							Debug.Log ("POS: " + (pos + offset) + " / "  + (lastPosN + pos) + " / " + lastPosN);
							m_voxelChunktemp[x, y, z] = new VoxelChunk(lastPosN + pos, m_voxelWidth, m_voxelHeight, m_voxelLength, m_surfaceLevel);
							m_voxelChunktemp[x, y, z].CreateVoxels (m_surfacePerlin);
							m_voxelChunktemp[x, y, z].CreateMesh (m_material);
						}
					}
				}
				Debug.DrawLine(player.position + player.forward * 32 * m_voxelWidth,  -Vector3.up * 128, Color.red);
				m_chunksX++;
			}

		}
		/*
		 * -------
		 * | | |x|
		 * -------
		 * | | | |
		 * -------
		 * | | | |
		 * -------
		 */ 
		if(Physics.Raycast (player.position + player.forward * 32 * m_voxelWidth + player.right * 128, player.position + player.forward * 16 * m_voxelWidth + player.right * 128 - new Vector3(0, 128, 0), out hit))
		{
			lastPosNE = hit.transform.GetComponent<ChunkData>().m_pos;	
		}
		else
		{
			if(Physics.Raycast (player.position + player.forward * 16 * m_voxelWidth + player.right * 128, player.position + player.forward * 16 * m_voxelWidth + player.right * 128 - new Vector3(0, 128, 0), out hit))
			{
				Debug.DrawLine(player.position + player.forward * 16 * m_voxelWidth + player.right * 128, player.position + player.forward * 16 * m_voxelWidth + player.right * 128 - new Vector3(0, 128, 0), Color.green);
				//Debug.Log ("N Trafilem w: " + hit.transform.GetComponent<ChunkData>().m_pos);
				lastPosNE = hit.transform.GetComponent<ChunkData>().m_pos;
				PerlinNoise m_surfacePerlin = new PerlinNoise(m_surfaceSeed);
				Vector3 offset = new Vector3(m_chunksX*m_voxelWidth*0.5f, -(m_chunksY-m_chunksAbove0)*m_voxelHeight, m_chunksZ*m_voxelLength*0.5f);
			//	Vector3 pos = lastPosN + new Vector3(108, 0, 0);
				m_voxelChunktemp = new VoxelChunk[1, 2, 1];
				for(int x = 0; x<1; x++)
				{
					for (int y = 0; y<2; y++)
					{
						for(int z = 0; z< 1; z++)
						{
							Vector3 pos = new Vector3((x+lastPosNE.x)*m_voxelWidth/32, y*m_voxelHeight, (z+lastPosNE.z)*m_voxelLength);
							//Debug.Log ("POS: " + (pos + offset) + " / "  + (lastPosNE + pos) + " / " + lastPosN);
							m_voxelChunktemp[x, y, z] = new VoxelChunk(lastPosNE + pos, m_voxelWidth, m_voxelHeight, m_voxelLength, m_surfaceLevel);
							m_voxelChunktemp[x, y, z].CreateVoxels (m_surfacePerlin);
							m_voxelChunktemp[x, y, z].CreateMesh (m_material);
						}
					}
				}
				//Debug.DrawLine(player.position + player.forward * 32 * m_voxelWidth,  -Vector3.up * 128, Color.red);
				m_chunksX++;
			}
		}
		
		/*
		 * -------
		 * |x| | | 
		 * -------
		 * | | | |
		 * -------
		 * | | | |
		 * -------
		 */ 
		if(Physics.Raycast (player.position + player.forward * 32 * m_voxelWidth - player.right * 128, -Vector3.up, out hit))
		{
			//Debug.Log ("NW Trafilem w: " + hit.transform.name);	
		}
		else
		{
			//Debug.Log ("Pudlo NW");	
		}	
	}
}
