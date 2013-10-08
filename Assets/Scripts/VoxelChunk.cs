using UnityEngine;
using System.Collections;
public class VoxelChunk
{
	float[,,] m_voxels;
	Vector3[,,] m_normals;
	Vector3 m_pos;
	Vector3 rowNumber;
	GameObject m_mesh;
	float m_surfaceLevel;
	
	int[,] m_sampler = new int[,] 
	{
		{1,-1,0}, {1,-1,1}, {0,-1,1}, {-1,-1,1}, {-1,-1,0}, {-1,-1,-1}, {0,-1,-1}, {1,-1,-1}, {0,-1,0},
		{1,0,0}, {1,0,1}, {0,0,1}, {-1,0,1}, {-1,0,0}, {-1,0,-1}, {0,0,-1}, {1,0,-1}, {0,0,0},
		{1,1,0}, {1,1,1}, {0,1,1}, {-1,1,1}, {-1,1,0}, {-1,1,-1}, {0,1,-1}, {1,1,-1}, {0,1,0}
	};
	
	public VoxelChunk(Vector3 pos, int width, int height, int length, float surfaceLevel)
	{
		m_surfaceLevel = surfaceLevel;
		//As we need some extra data to smooth the voxels and create the normals we need a extra 5 voxels
		//+1 one to create a seamless mesh. +2 to create smoothed normals and +2 to smooth the voxels
		//This is a little unoptimsed as it means some data is being generated that has alread been generated in other voxel chunks
		//but it is simpler as we dont need to access the data in the other voxels. You could try and copy the data
		//needed in for the other voxel chunks as a optimisation step
		m_voxels = new float[width+1, height+1, length+1];
		//As the extra data is basically a border of voxels around the chunk we need to offset the position
		//It does not need to be done but it means that te voxel position (once translated) matches its world position
		m_pos = pos - new Vector3(1,1,1);
	}
	
	//All of these sample functions create the noise needed for a certain effect, ie caves, moutains etc.
	//There is no reason I have choosen the technique and values for anyone of the noise functions
	//Its just what I have decided looks nice. There is no wrong or right way to do it.
	//The noise functions are one of the slowest to call so I tried to keep it simple while still having nice looking terrain
	
	//The last three values in the noise functions are octaves, frequency and amplitude.
	//More ocatves will create more detail but is slower.
	//Higher/lower frquency will 'strech/shrink' out the noise.
	//Amplitude defines roughly the range of the noise, ie amp = 1.0 means roughly -1.0 to +1.0 * range of noise
	//The range of noise is 0.5 for 1D, 0.75 for 2D and 1.5 for 3D
	
	float SampleMountains(float x, float z, PerlinNoise perlin)
	{
		//This creates the noise used for the mountains. It used something called 
		//domain warping. Domain warping is basically offseting the position used for the noise by
		//another noise value. It tends to create a warped effect that looks nice.
		float w = perlin.FractalNoise2D(x, z, 3, 120.0f, 32.0f);
		//Clamp noise to 0 so mountains only occur where there is a positive value
		//The last value (32.0f) is the amp that defines (roughly) the maximum mountaion height
		//Change this to create high/lower mountains
		return Mathf.Min(0.0f, perlin.FractalNoise2D(x+w, z+w, 6, 120.0f, 32.0f) );
	}
	
	float SampleGround(float x, float z, PerlinNoise perlin)
	{
		//This creates the noise used for the ground.
		//The last value (8.0f) is the amp that defines (roughly) the maximum 
		//and minimum vaule the ground varies from the surface level
		return perlin.FractalNoise2D(x, z, 4, 80.0f, 8.0f);
	}
	
	float SampleCaves(float x, float y, float z, PerlinNoise perlin)
	{
		//The creates the noise used for the caves. It uses domain warping like the moiuntains
		//to creat long twisting caves.
		float w = perlin.FractalNoise3D(x, y, z, 1, 40.0f, 32.0f);
		//The last vaule is the cave amp and defines the maximum cave diameter. A larger value will create
		//larger caves (A higher frequency will also create larger caves). It is unitless, 1 != 1m
		return Mathf.Abs(perlin.FractalNoise3D(x+w, y*2.0f+w, z+w, 2, 40.0f, 2.0f));
	}
	
	public void CreateVoxels(PerlinNoise surfacePerlin)//, PerlinNoise cavePerlin)
	{
		//float startTime = Time.realtimeSinceStartup;
		
		//Creates the data the mesh is created form. Fills m_voxels with values between -1 and 1 where
		//-1 is a soild voxel and 1 is a empty voxel.
		
		int w = m_voxels.GetLength(0);
		int h = m_voxels.GetLength(1);
		int l = m_voxels.GetLength(2);
		
		for(int x = 0; x < w; x++)
		{
			for(int z = 0; z < l; z++)
			{
				//world pos is the voxels position plus the voxel chunks position
				float worldX = x+m_pos.x;
				float worldZ = z+m_pos.z;
				
				float groundHt = SampleGround(worldX, worldZ, surfacePerlin);
				
				float mountainHt = SampleMountains(worldX, worldZ, surfacePerlin);
				
				float ht = mountainHt + groundHt;
		
				for(int y = 0; y < h; y++)
				{
					float worldY = y+m_pos.y-m_surfaceLevel;
				
					//If we take the heigth value and add the world
					//the voxels will change from positiove to negative where the surface cuts through the voxel chunk
					m_voxels[x,y,z] = Mathf.Clamp(ht + worldY , -1.0f, 1.0f);
					
					//float caveHt = SampleCaves(worldX, worldY, worldZ, cavePerlin);
					
					//This fades the voxel value so the caves never appear more than 16 units from
					//the surface level.
					//float fade = 1.0f - Mathf.Clamp01(Mathf.Max(0.0f, worldY)/16.0f);
					
					//m_voxels[x,y,z] += caveHt * fade;
					
					m_voxels[x,y,z] = Mathf.Clamp(m_voxels[x,y,z], -1.0f, 1.0f);
				}
			}
		}
		
		//Debug.Log("Create voxels time = " + (Time.realtimeSinceStartup-startTime).ToString() );
		
	}
	
	
	Vector3 TriLinearInterpNormal(Vector3 pos)
	{	
		int x = (int)pos.x;
		int y = (int)pos.y;
		int z = (int)pos.z;
		
		float fx = pos.x-x;
		float fy = pos.y-y;
		float fz = pos.z-z;
		
		Vector3 x0 = m_normals[x,y,z] * (1.0f-fx) + m_normals[x+1,y,z] * fx;
		Vector3 x1 = m_normals[x,y,z+1] * (1.0f-fx) + m_normals[x+1,y,z+1] * fx;
		
		Vector3 x2 = m_normals[x,y+1,z] * (1.0f-fx) + m_normals[x+1,y+1,z] * fx;
		Vector3 x3 = m_normals[x,y+1,z+1] * (1.0f-fx) + m_normals[x+1,y+1,z+1] * fx;
		
		Vector3 z0 = x0 * (1.0f-fz) + x1 * fz;
		Vector3 z1 = x2 * (1.0f-fz) + x3 * fz;
		
		return z0 * (1.0f-fy) + z1 * fy;
	}
	
	public void CreateMesh(Material mat)
	{
		//float startTime = Time.realtimeSinceStartup;
		
		Mesh mesh = MarchingCubes.CreateMesh(m_voxels,0,0);
		if(mesh == null) return;
		
		int size = mesh.vertices.Length;
		
		if(m_normals != null)
		{
			Vector3[] normals = new Vector3[size];
			Vector3[] verts = mesh.vertices;
			
			//Each verts in the mesh generated is its position in the voxel array
			//and you can use this to find what the normal at this position.
			//The verts are not at whole numbers though so you need to use trilinear interpolation
			//to find the normal for that position
			
			for(int i = 0; i < size; i++)
				normals[i] = TriLinearInterpNormal(verts[i]);
			
			mesh.normals = normals;
		}
		else
		{
			mesh.RecalculateNormals();
		}
		
	//	Color[] control = new Color[size];
	//	Vector3[] meshNormals = mesh.normals;
			
		/*for(int i = 0; i < size; i++)
		{
			//This creates a control map used to texture the mesh based on the slope
			//of the vert. Its very basic and if you modify how this works yoou will
			//you will probably need to modify the shader as well.
			float dpUp = Vector3.Dot(meshNormals[i], Vector3.up);
			
			//Red channel is the sand on flat areas
			float R = (Mathf.Max(0.0f, dpUp) < 0.8f) ? 0.0f : 1.0f;
			//Green channel is the gravel on the sloped areas
			float G = Mathf.Pow(Mathf.Abs(dpUp), 2.0f);
			
			//Whats left end up being the rock face on the vertical areas

			control[i] = new Color(R,G,0,0);
		}
		
		//May as well store in colors 
		mesh.colors = control;
		*/
		m_mesh = new GameObject("Voxel Mesh " + m_pos.x.ToString() + " " + m_pos.y.ToString() + " " + m_pos.z.ToString());
		m_mesh.AddComponent<ChunkData>();
		m_mesh.GetComponent<ChunkData>().m_pos = m_pos;
		m_mesh.transform.parent = GameObject.Find ("Terrain").transform;
		m_mesh.AddComponent<MeshFilter>();
		m_mesh.AddComponent<MeshRenderer>();
		m_mesh.renderer.material = mat;
		m_mesh.GetComponent<MeshFilter>().mesh = mesh;
		m_mesh.transform.localPosition = m_pos * 32;
		m_mesh.transform.localScale = new Vector3(32,32,32);
		MeshCollider collider = m_mesh.AddComponent<MeshCollider>();
		collider.sharedMesh = mesh;
		
		//Debug.Log("Create mesh time = " + (Time.realtimeSinceStartup-startTime).ToString() );
	}
}
