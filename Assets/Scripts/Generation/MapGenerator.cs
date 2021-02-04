using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.AI;

/**
Classe : MapGenerator
Système de génération du Terrain de l'île
*/

public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, BiomeNoiseMap, Mesh, FalloffMap, Terrain};
	public DrawMode drawMode;

	[SerializeField] private Terrain terrain = null;

	[Min(0)]
	[SerializeField] private int mapChunkSize = 513;
	[Range(0,6)]
	public int levelOfDetail;
	public float noiseScale;

	public int octaves;
	[Range(0, 1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public float biomeNoiseScale;
	[Range(0, 1)]
	public float biomeThreshold;
	public bool reverseBiomeThreshold;

	public int biomeOctaves;
	[Range(0, 1)]
	public float biomePersistance;
	public float biomeLacunarity;
	
	public int biomeSeed;
	public Vector2 biomeOffset;

	public float entityNoiseScale;
	[Range(0, 1)]
	public float entityThreshold;

	public bool useFalloff;
	public float a = 3;
	public float b = 2.2f;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;
	[HideInInspector]
	public float islandElevationOffset = 0.0f;

	public bool autoUpdate;

	public List<TerrainType> regions = new List<TerrainType>();

	public float[,] falloffMap;

	void Awake() {
		falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, a, b);
	}

	public float[,] GenerateMap() {
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				if (useFalloff) {
					noiseMap[x, y] = Mathf.Clamp01(noiseMap [x, y] - falloffMap [x, y]);
					if (noiseMap[x, y] < 0.09f && falloffMap[x, y] < 0.2f) noiseMap[x, y] = 0.09f;
				}
				float currentHeight = noiseMap [x, y];
				for (int i = 0; i < regions.Count; i++) {
					if (currentHeight <= regions [i].maxAltitude) {
						colourMap [y * mapChunkSize + x] = regions [i].colour;
						break;
					}
				}
			}
		}

		float[,] terrainHeightMap = new float[mapChunkSize, mapChunkSize];
		islandElevationOffset = terrain.terrainData.heightmapScale.y - 6.0f;
		float offsetRatio = (islandElevationOffset / 6.0f) + 1.0f;

		for (int x = 0; x < mapChunkSize; x++) {
			for (int y = 0; y < mapChunkSize; y++) {
				float sampleHeight = meshHeightCurve.Evaluate(noiseMap [y, mapChunkSize - x - 1]);
				terrainHeightMap[x, y] = ((sampleHeight) / offsetRatio) + (((falloffMap[x, y] < 0.8f && sampleHeight >= 0.01f) || (falloffMap[x, y] < 0.2f)) ? (1.0f - 1.0f/offsetRatio) : 0.0f);
			}
		}

		float[,] biomeNoiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, biomeSeed, biomeNoiseScale, biomeOctaves, biomePersistance, biomeLacunarity, biomeOffset);
		float[,] entityNoiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, biomeSeed, entityNoiseScale, biomeOctaves, biomePersistance, biomeLacunarity, biomeOffset);

		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				if (useFalloff) {
					biomeNoiseMap[x, y] = Mathf.Clamp01(biomeNoiseMap[x, y] - falloffMap[x, y]);
				}

				float biomeCurrentHeight = biomeNoiseMap[x, y];

				if (!reverseBiomeThreshold) {
					if (biomeCurrentHeight > biomeThreshold) {
						float entityCurrentHeight = entityNoiseMap[x, y];
						if (entityCurrentHeight > entityThreshold) {
							biomeNoiseMap[x, y] = 1;
						}
						else {
							biomeNoiseMap[x, y] = 0.3f;
						}
					}
					else {
						biomeNoiseMap[x, y] = 0;
					}
				}

				else {
					if (biomeCurrentHeight < biomeThreshold) {
						float entityCurrentHeight = entityNoiseMap[x, y];
						if (entityCurrentHeight > entityThreshold) {
							biomeNoiseMap[x, y] = 1;
						}
						else {
							biomeNoiseMap[x, y] = 0.3f;
						}
					}
					else {
						biomeNoiseMap[x, y] = 0;
					}
				}
				
			}
		}

		MapDisplay display = FindObjectOfType<MapDisplay>();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap(noiseMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
		} else if (drawMode == DrawMode.BiomeNoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap(biomeNoiseMap));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
		} else if (drawMode == DrawMode.FalloffMap) {
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(FalloffGenerator.GenerateFalloffMap(mapChunkSize, a, b)));
		} else if (drawMode == DrawMode.Terrain) {
			display.DrawTerrain(terrain, terrainHeightMap);
			terrain.GetComponent<TerrainTexture>().SetTexture(terrain, meshHeightCurve, regions);
			//NavMeshSurface nm = terrain.GetComponent<NavMeshSurface>();
			//nm.BuildNavMesh();
		}

		return noiseMap;
	}

	void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}

		falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, a, b);
	}
}

[System.Serializable]
public class TerrainType {
	public string name;
	public int index;
	public bool defaultTexture = false;
	[Range(0.0f,1.0f)]
	public float minSteepness;
	[Range(0.0f,1.0f)]
	public float maxSteepness;
	[Range(0.0f,1.0f)]
	public float minAltitude;
	[Range(0.0f,1.0f)]
	public float maxAltitude;
	public Color colour;
}