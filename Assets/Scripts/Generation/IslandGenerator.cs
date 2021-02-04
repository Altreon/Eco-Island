using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
Classe : IslandGenerator
Système de génération des différents prefabs de l'île
*/

public class IslandGenerator : MonoBehaviour
{
	public static IslandGenerator Instance;

	[Min(0)]
	[SerializeField] private int mapChunkSize = 513;

	[SerializeField] private Terrain terrain = null;

	[SerializeField] private List<BiomeSettings> biomeSettings = new List<BiomeSettings>();

	[Header("Creature settings")]
	[SerializeField] private GameObject[] CreatureModels = null;
	[SerializeField] private Nest nestModel = null;
	/*[SerializeField] private int nbCreaturesGroup = 5;
	[Range(0,15)]
	[SerializeField] private int nbCreatureInEachGroup = 3;*/

	[Header("Other settings")]
	[SerializeField] private float terrainSizeX;
	[SerializeField] private float terrainSizeY;
	[SerializeField] private int generationSeed = 0;

	[SerializeField] private MapGenerator mapGen = null;

	private SpawnConfig spawnConfig;
	private int nbSpecies = 6;
	//private int nbCreaturesPerSpecie = 5;
	//private int nbCreatures = 30;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this);
		}
	}
	

	// Start is called before the first frame update
	void Start()
	{
		spawnConfig = CreatureFactory.Instance.configSpawn;
		nbSpecies = spawnConfig.Species.Length;
		//nbCreaturesPerSpecie = spawnConfig.NbCreaturesPerSpecie;
		//nbCreatures = nbCreaturesPerSpecie *nbSpecies;

		Generate(generationSeed, terrainSizeX, terrainSizeY);
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void Generate(int seed, float sizeX, float sizeY)
	{
		Random.InitState(seed);
		terrainSizeX = sizeX;
		terrainSizeY = sizeY;

		float[,] heightMap = mapGen.GenerateMap();
		int heightMultiplier = (int) terrain.terrainData.heightmapScale.y;

		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);
		float topLeftX = (width - 1) / -2f;
		float topLeftZ = (height - 1) / 2f;

		float sandThreshold = 0.2f * (mapGen.meshHeightCurve.Evaluate(mapGen.regions[1].minAltitude) * heightMultiplier);
		sandThreshold += 0.8f * (mapGen.meshHeightCurve.Evaluate(mapGen.regions[1].maxAltitude) * heightMultiplier);

		ArrayList entitiesCoordinates = new ArrayList();

		//Ensure that each element generated won't be on top of another
		while (entitiesCoordinates.Count < nbSpecies) {
			int[] coordinates = {Random.Range(0, Mathf.RoundToInt(terrainSizeX * 0.9f)), Random.Range(0, Mathf.RoundToInt(terrainSizeY * 0.9f))};
			bool containsCoordinates = false;
			for (int i = 0; i < entitiesCoordinates.Count; i++) {
				int[] arrayCoordinates = (int[])entitiesCoordinates[i];
				int x = arrayCoordinates[0];
				int y = arrayCoordinates[1];

				if (x == coordinates[0] && y == coordinates[1]) {
					containsCoordinates = true;
					break;
				}
			}
			if (!containsCoordinates) {
				float mapHeight = mapGen.meshHeightCurve.Evaluate(heightMap[coordinates[0], coordinates[1]]) * heightMultiplier;
				if (mapHeight > sandThreshold)
					entitiesCoordinates.Add(coordinates);
			}
		}

		//Biome generation

		for (int i = 0; i < biomeSettings.Count; i++) {
			int biomeSeed = seed + i;

			float[,] biomeNoiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, biomeSeed, biomeSettings[i].biomeNoiseScale, mapGen.octaves, mapGen.persistance, mapGen.lacunarity, mapGen.offset);
			float[,] entityNoiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, biomeSeed, biomeSettings[i].entityNoiseScale, mapGen.octaves, mapGen.persistance, mapGen.lacunarity, mapGen.offset);
						
			GameObject entityHolder = new GameObject();
			entityHolder.name = biomeSettings[i].name + " Group";

			//Pour flitrer la navmesh et gagner en perf, me dire si ça pose des souçis
			//entityHolder.transform.parent = this.transform;
			entityHolder.transform.parent = terrain.transform;

			float[] entityProbabilities = {};

			if (biomeSettings[i].useProbability) {
				float totalProbability = 0.0f;
				entityProbabilities = new float[biomeSettings[i].entitySettings.Length];

				for (int j = 0; j < biomeSettings[i].entitySettings.Length; j++) {
					totalProbability += biomeSettings[i].entitySettings[j].probability;
				}

				for (int j = 0; j < biomeSettings[i].entitySettings.Length; j++) {
					float entityProbability = biomeSettings[i].entitySettings[j].probability;
					entityProbabilities[j] = entityProbability/totalProbability;
				}
			}

			biomeSettings[i].biomeNoiseMap = biomeNoiseMap;
			biomeSettings[i].entityNoiseMap = entityNoiseMap;
			biomeSettings[i].entityHolder = entityHolder;
			biomeSettings[i].entityProbabilities = entityProbabilities;
		}

		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {

				float strongestWeight = 0f;
				int strongestBiomeIndex = 0;

				for (int i = 0; i < biomeSettings.Count; i++) {
					float weight = biomeSettings[i].biomeNoiseMap[x, y];

					if (mapGen.useFalloff) {
						weight = Mathf.Clamp01(weight - mapGen.falloffMap[x, y]);
						//TODO j'ai ajouter un weightFactor, tu peux supprimer si ça te plait pas
						weight *= biomeSettings[i].weightFactor;
					}

					if (weight > strongestWeight) {
						strongestWeight = weight;
						strongestBiomeIndex = i;
					}
				}

				float biomeCurrentHeight = strongestWeight;

				if (biomeCurrentHeight > biomeSettings[strongestBiomeIndex].biomeThreshold) {
					float entityCurrentHeight = biomeSettings[strongestBiomeIndex].entityNoiseMap[x, y];
					float mapHeight = mapGen.meshHeightCurve.Evaluate(heightMap[x, y]) * heightMultiplier / ((mapGen.islandElevationOffset / 6.0f) + 1.0f) + mapGen.islandElevationOffset;
					if (entityCurrentHeight > biomeSettings[strongestBiomeIndex].entityThreshold && mapHeight > sandThreshold) {
						Vector3 position = new Vector3(topLeftX + x, mapHeight, topLeftZ - y);
						Quaternion spawnRotation;
						
						int selectedEntity = 1;
						if (biomeSettings[strongestBiomeIndex].useProbability) {
							float randomSelection = Random.Range(0.0f, 1.0f);
							float cumulativeProbability = 0.0f;
							for (int j = 0; j < biomeSettings[strongestBiomeIndex].entityProbabilities.Length; j++) {
								cumulativeProbability += biomeSettings[strongestBiomeIndex].entityProbabilities[j];
								if (randomSelection <= cumulativeProbability) {
									selectedEntity = j;
									break;
								}
							}
						}
						else {
							selectedEntity = Random.Range(0, biomeSettings[strongestBiomeIndex].entitySettings.Length);
						}

						if (biomeSettings[strongestBiomeIndex].entitySettings[selectedEntity].rotateWithTerrainSlope) {
							RaycastHit hit;
							Physics.Raycast(position, Vector3.down, out hit, 200, LayerMask.GetMask("Terrain"));
							spawnRotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);
						}
						else {
							int randomAngle = Random.Range(0, 360);
							spawnRotation = Quaternion.Euler(0, randomAngle, 0);
						}

						GameObject entityInstance = Instantiate(biomeSettings[strongestBiomeIndex].entitySettings[selectedEntity].model, position, spawnRotation);
						entityInstance.transform.parent = biomeSettings[strongestBiomeIndex].entityHolder.transform;
						if (biomeSettings[strongestBiomeIndex].entitySettings[selectedEntity].useRescale) {
							float randomScale = biomeSettings[strongestBiomeIndex].entitySettings[selectedEntity].scaleCurve.Evaluate(Random.value);
							entityInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
						}
					}
				}
			}
		}

		NavMeshSurface nm = terrain.GetComponent<NavMeshSurface>();
		nm.BuildNavMesh();
		

		//Creature generation
		List<Creature>[] spawnedCreatures = CreatureFactory.Instance.SetupSpawn();

		int creatureIndex = 0;

		/*List<Transform> creatureParents = new List<Transform>();
		List<Vector3> creaturePositions = new List<Vector3>();
		List<Quaternion> creatureRotations = new List<Quaternion>();*/
		for (int i = 0; i < nbSpecies; i++)
		{
			float mapHeight;
			int positionCenterX;
			int positionCenterZ;
			int positionX;
			int positionZ;
			int[] coordinates = (int[])entitiesCoordinates[i];
			int nbCreaturesInstanciated = 0;

			positionCenterX = coordinates[0];
			positionCenterZ = coordinates[1];
			int positionIndex = 0;
			int creatureType = Random.Range(0, CreatureModels.Length);
			
			bool nestSpawned = false;

			while (nbCreaturesInstanciated < spawnConfig.Species[i].NbCreaturesInit) {
				Creature creature = spawnedCreatures[i][nbCreaturesInstanciated];
				bool containsCoordinates = false;
				
				if (positionIndex % 3 == 0) {
					positionX = positionCenterX + 2 * (positionIndex/3);
					positionZ = positionCenterZ;
				}
				else if (positionIndex % 3 == 1) {
					positionX = positionCenterX + 2 * (positionIndex/3);
					positionZ = positionCenterZ + 2;
				}
				else {
					positionX = positionCenterX + 2 * (positionIndex/3);
					positionZ = positionCenterZ - 2;
				}

				for (int j = 0; j < entitiesCoordinates.Count; j++) {
					int[] entityCoordinates = (int[])entitiesCoordinates[j];
					int entityX = entityCoordinates[0];
					int entityZ = entityCoordinates[1];

					if (positionX == entityX && positionZ == entityZ) {
						if (positionX != positionCenterX || positionZ != positionCenterZ) {
							containsCoordinates = true;
							break;
						}
					}
				}

				if (containsCoordinates) {
					positionIndex++;
					continue;
				}
				
				mapHeight = mapGen.meshHeightCurve.Evaluate(heightMap[positionX, positionZ]) * heightMultiplier / ((mapGen.islandElevationOffset / 6.0f) + 1.0f) + mapGen.islandElevationOffset;

				Vector3 position = new Vector3(topLeftX + positionX, mapHeight + 0.1f, topLeftZ - positionZ);
				//creaturePositions[creatureIndex] = position;
				creature.transform.position = position;


				int randomAngle = Random.Range(0, 360);
				//creatureRotations[creatureIndex] = Quaternion.Euler(0, randomAngle, 0);
				creature.transform.rotation = Quaternion.Euler(0, randomAngle, 0);

				if (!nestSpawned) {
					Vector3 nestPosition = new Vector3(position.x, mapHeight + 0.1f, position.z);

					RaycastHit hit;
					Physics.Raycast(nestPosition, Vector3.down, out hit, 200, LayerMask.GetMask("Terrain"));
            		nestPosition = hit.point;
            		Quaternion spawnRotation = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(90, 0, 0);

					Nest nestInstance = Instantiate(nestModel, nestPosition, spawnRotation);
					nestInstance.transform.parent = this.transform;
					nestInstance.SpecieID = i;
					nestSpawned = true;
				}

				//GameObject creature = CreatureModels[creatureType];

				//GameObject creatureInstance = Instantiate(creature, position, Quaternion.Euler(0, randomAngle, 0));
				//creatureInstance.transform.parent = this.transform;
				//creatureParents[creatureIndex] = this.transform;
				creature.transform.parent = this.transform;
				nbCreaturesInstanciated++;
				creatureIndex++;

				positionIndex++;
			}
		}

		/*creatureIndex = 0;

		for (int nbEspece = 0; nbEspece < spawnedCreatures.Length; nbEspece++) {
			for (int i = 0; i < spawnedCreatures[nbEspece].Length; i++) {
				spawnedCreatures[nbEspece][i].transform.parent = creatureParents[creatureIndex];
				spawnedCreatures[nbEspece][i].transform.position = creaturePositions[creatureIndex];
				spawnedCreatures[nbEspece][i].transform.rotation = creatureRotations[creatureIndex];
				creatureIndex++;
			}
		}*/

		CreatureFactory.Instance.StartGame();
	}
	
}

[System.Serializable]
public class BiomeSettings {
	public string name;
	public EntitySettings[] entitySettings;
	public float biomeNoiseScale;
	[Range(0,1)]
	public float biomeThreshold;
	public float entityNoiseScale;
	[Range(0,1)]
	public float entityThreshold;
	public bool useProbability;
	public float weightFactor = 1;
	[HideInInspector]
	public float[,] biomeNoiseMap;
	[HideInInspector]
	public float[,] entityNoiseMap;
	[HideInInspector]
	public GameObject entityHolder;
	[HideInInspector]
	public float[] entityProbabilities;
}

[System.Serializable]
public class EntitySettings {
	public GameObject model;
	public bool useRescale = false;
	public bool rotateWithTerrainSlope = false;
	public AnimationCurve scaleCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
	public float probability = 0;
}