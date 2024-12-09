using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class SaveSystem : MonoBehaviour
{

	[Serializable]
	public class SaveAccesses
	{
		public VolumeSettings volumeSettings;
		public TutorialManager tutorialManager;
		public Camera mainCamera;
		public MoneyManager moneyManager;
		public Transform tilesParent;
		public TilemapManager tilemapManager;
		public Grid grid;
		public InventoryManager inventoryManager;
		public QuestManager questManager;
	}

	[SerializeField] private SaveAccesses accesses;

	private static VolumeSettings volumeSettings;
	private static TutorialManager tutorialManager;
	private static Camera mainCamera;
	private static MoneyManager moneyManager;
	private static Transform tilesParent;
	private static TilemapManager tilemapManager;
	private static Grid grid;
	private static InventoryManager inventoryManager;
	private static QuestManager questManager;

    private void Awake()
	{
		volumeSettings = accesses.volumeSettings;
		tutorialManager = accesses.tutorialManager;
		mainCamera = accesses.mainCamera;
		moneyManager = accesses.moneyManager;
		tilesParent = accesses.tilesParent;
		tilemapManager = accesses.tilemapManager;
		grid = accesses.grid;
		inventoryManager = accesses.inventoryManager;
		questManager = accesses.questManager;

		//Save("Assets/Scripts/ScriptSave/SaveData.json");
	}


	public static void Save()
	{
		Save(Application.persistentDataPath + "/SaveData.json");
	}

	public static void Load()
	{
		SaveData load = GenerateLoadData(Application.persistentDataPath + "/SaveData.json");

		#region // ConfigData
		//volumeSettings.SetMusicVolume(load.configData.musicVolume);
		//volumeSettings.SetSFXVolume(load.configData.sfxVolume);
		#endregion
		#region // TutorialData
		tutorialManager.Inicialize(load.tutorialData.isDone);
		#endregion
		#region // CameraData
		mainCamera.transform.position = load.cameraData.position;
		mainCamera.orthographicSize = load.cameraData.size;
		#endregion
		#region // MoneyData
		moneyManager.SetCurrentMoney(load.moneyData.money);
		#endregion
		#region // AmbientData
		Ambient.SetCurrentTemperature(load.ambientData.currentTemperature);
		Ambient.SetCurrentClimate(load.ambientData.currentClimate);
		AmbientManager.SetCurrentSeason(load.ambientData.currentSeason);
		AmbientManager.SetSeasonalFactor(load.ambientData.seasonalFactor);
		#endregion
		#region // IDS_Data
		IDS.SetEcologico(load.idsData.ecologico);
		IDS.SetEconomico(load.idsData.economico);
		IDS.SetSocial(load.idsData.social);
		IDS.CalcularIDS();
		#endregion		
		#region // InventoryData
		/*
		for (int i = 0; i < load.inventoryData.itens.Count; i++)
		{
			inventoryManager.playerInventory.items.Add(load.inventoryData.itens[i]);
		}
		*/
		inventoryManager.playerInventory.items = load.inventoryData.itens;
		#endregion
		#region // QuestData
		/*
        for (int i = 0; i < load.questData.availableQuests.Count; i++)
        {
            questManager.availableQuests.Add(load.questData.availableQuests[i]);
        }
        for (int i = 0; i < load.questData.activeQuests.Count; i++)
        {
            questManager.activeQuests.Add(load.questData.activeQuests[i]);
        }
		*/
		questManager.availableQuests = load.questData.availableQuests;
		questManager.activeQuests = load.questData.activeQuests;
		questManager.CompleteActiveQuests();
        #endregion

        #region // TileData

		foreach (CustomTileData tileData in load.tilesData.customTileDatas)
		{
			LoadCustomTile(tileData);
		}

		foreach (TreeTileData tileData in load.tilesData.treeTileDatas)
		{
			LoadTreeTile(tileData);
		}

        /*foreach (PlantTileData tileData in load.tilesData.plantTileDatas)
        {
            LoadPlantTile(tileData);
        }*/

        foreach (HouseTileData tileData in load.tilesData.houseTileDatas)
        {
            LoadHouseTile(tileData);
        }

        foreach (var key in tilemapManager.tilesDictionary.Keys)
		{
			Debug.Log("Key: " + key + " | " + "Valor: " + tilemapManager.tilesDictionary[key]);
		}

        #endregion
    }

    private static void Save(string path)
	{
		string json = JsonUtility.ToJson(GenerateSaveData(), true);

		File.WriteAllText(path, json);
	}

	private static SaveData GenerateLoadData(string path)
	{
		string json = File.ReadAllText(path);

		return JsonUtility.FromJson<SaveData>(json);
	}

	public static SaveData GenerateSaveData()
	{
		SaveData save = new SaveData();

		save.configData = new ConfigData(volumeSettings.musicSlider.value, volumeSettings.sfxSlider.value);
		save.tutorialData = new TutorialData(tutorialManager.tutirialCompleted);
		save.cameraData = new CameraData(mainCamera.transform.position, mainCamera.orthographicSize);
		save.moneyData = new MoneyData(moneyManager.GetCurrentMoney());
		save.ambientData = new AmbientData(Ambient.GetCurrentTemperature(), Ambient.GetCurrentClimate(), AmbientManager.GetCurrentSeason(), AmbientManager.GetSeasonalFactor());
		save.idsData = new IDS_Data(IDS.GetIDS(), IDS.GetEcologico(), IDS.GetEconomico(), IDS.GetSocial());
		save.inventoryData = new InventoryData(inventoryManager.playerInventory.items);
		save.questData = new QuestData(questManager.availableQuests, questManager.activeQuests);

		// FilesData
		CustomTileBase[] customTileList = GetTilesOfType<CustomTileBase>();
        List<CustomTileData> customTileDatas = new List<CustomTileData>();
        foreach (CustomTileBase tile in customTileList)
		{
			Vector3Int position = Vector3Int.zero;
			foreach (var _base in tilemapManager.tilesDictionary.Keys)
			{
				if (_base == tile)
				{
					position = grid.WorldToCell(tilemapManager.tilesDictionary[_base]);
                }
			}

			CustomTileData customData = new CustomTileData(tile.tilePosition, tile.customTilePrefab, tile.plowedTilePrefab, tile.isPlantable, 
				tile.nitrogen, tile.phosphorus, tile.potassium, tile.humidity, tile.rotationState);
			customTileDatas.Add(customData);
		}

		TreeTile[] treeTileList = GetTilesOfType<TreeTile>();
        List<TreeTileData> treeTileDatas = new List<TreeTileData>();               
        foreach (TreeTile tile in treeTileList)
		{
            Vector3Int position = Vector3Int.zero;
            foreach (var _base in tilemapManager.tilesDictionary.Keys)
            {
                if (_base == tile)
                {
                    position = grid.WorldToCell(tilemapManager.tilesDictionary[_base]);
                }
            }

            TreeTileData treeData = new TreeTileData(tile.tilePosition, tile.treePrefab, tile.groundPrefab, tile.wasCuted);
			treeTileDatas.Add(treeData);
		}

		PlantTile[] plantTileList = GetTilesOfType<PlantTile>();
        List<PlantTileData> plantTileDatas = new List<PlantTileData>();
        foreach (PlantTile tile in plantTileList)
		{
            /*Vector3Int position = Vector3Int.zero;
            foreach (var _base in tilemapManager.tilesDictionary.Keys)
            {
                if (_base == tile)
                {
                    position = grid.WorldToCell(tilemapManager.tilesDictionary[_base]);
                }
            }*/

            PlantTileData plantData = new PlantTileData(tile.soilTile.tilePosition, tile.growthPrefabs, tile.rotPrefab, tile.growthTimes,
				tile.GetIdealTemperature(), tile.GetIdealClimate(), tile.GetGreenTolerance(), tile.GetYellowTolerance(),
				tile.isPlanted, tile.isFullyGrown, tile.isRotten, tile.harvestedItem, tile.soilTile,
				tile.GetGrowthStage(), tile.GetTotalGrowthTime(), tile.GetCurrentGrowthTime(), tile.GetCurrentGrowthInstance(),
				tile.requiredNitrogen, tile.requiredPhosphorus, tile.requiredPotassium, tile.returnNitrogen, tile.returnPhosphorus, tile.returnPotassium);
			plantTileDatas.Add(plantData);
		}

		HouseTile[] houseTileList = GetTilesOfType<HouseTile>();
        List<HouseTileData> houseTileDatas = new List<HouseTileData>();
        foreach (HouseTile tile in houseTileList)
		{
            Vector3Int position = Vector3Int.zero;
            foreach (var _base in tilemapManager.tilesDictionary.Keys)
            {
                if (_base == tile)
                {
                    position = grid.WorldToCell(tilemapManager.tilesDictionary[_base]);
                }
            }

            HouseTileData houseData = new HouseTileData(tile.tilePosition, tile.groundPrefab);
			houseTileDatas.Add(houseData);
		}

		save.tilesData = new TilesData(customTileDatas, treeTileDatas, plantTileDatas, houseTileDatas);

		Debug.Log("Relatório de salvamento gerado!");
		return save;
	}

	private static T[] GetTilesOfType<T>() where T : TileBase
	{
        List<T> tilesOfType = new List<T>();

        // Iterar sobre todas as posições do Tilemap
        BoundsInt bounds = tilemapManager.tilemap.cellBounds;
        TileBase[] allTiles = tilemapManager.tilemap.GetTilesBlock(bounds);

        foreach (TileBase tile in allTiles)
        {
            if (tile is T typedTile) // Verifica se o tile é do tipo desejado
            {
                tilesOfType.Add(typedTile);
            }
        }

        return tilesOfType.ToArray();
    }

    #region // Generate Tiles

    private static void LoadCustomTile(CustomTileData tile)
	{
        CustomTileBase customTile = ScriptableObject.CreateInstance<CustomTileBase>();
		customTile.Initialize(tile.customTilePrefab, tile.plowedTilePrefab, tile.isPlantable,
				tile.nitrogen, tile.phosphorus, tile.potassium, tile.humidity, tile.rotationState);
        tilemapManager.tilemap.SetTile(tile.tilePosition, customTile);

		tilemapManager.tilemap.RefreshTile(tile.tilePosition);
	}

	private static void LoadTreeTile(TreeTileData tile)
	{
		TreeTile treeTile = ScriptableObject.CreateInstance<TreeTile>();
		treeTile.Initialize(tile.treePrefab, tile.groundPrefab, tile.wasCuted);
        tilemapManager.tilemap.SetTile(tile.tilePosition, treeTile);

        tilemapManager.tilemap.RefreshTile(tile.tilePosition);
    }

	private static void LoadPlantTile()
	{

	}

	private static void LoadHouseTile(HouseTileData tile)
	{
		HouseTile houseTile = ScriptableObject.CreateInstance<HouseTile>();
		houseTile.Initialize(tile.groundPrefab);
        tilemapManager.tilemap.SetTile(tile.tilePosition, houseTile);

        tilemapManager.tilemap.RefreshTile(tile.tilePosition);
    }

	#endregion

	public static bool isFirstTime()
	{
		return !File.Exists(Application.persistentDataPath + "/SaveData.json");
	}

	#region // Data Classes

	[Serializable]
	public class SaveData
	{
		public ConfigData configData;
		public TutorialData tutorialData;
		public CameraData cameraData;
		public MoneyData moneyData;
		public AmbientData ambientData;
		public IDS_Data idsData;
		public TilesData tilesData;
		public InventoryData inventoryData;
		public QuestData questData;
	}

	[Serializable]
	public class ConfigData
	{
		public float musicVolume;
		public float sfxVolume;

		// Construtor
		public ConfigData(float musicVolume, float sfxVolume)
		{
			this.musicVolume = musicVolume;
			this.sfxVolume = sfxVolume;
		}
	}

	[Serializable]
	public class TutorialData
	{
		public bool isDone;

		// Costrutor
		public TutorialData(bool isDone)
		{
			this.isDone = isDone;
		}
	}

	[Serializable]
	public class CameraData
	{
		public Vector3 position;
		public float size;

		// Construtor
		public CameraData(Vector3 position, float size)
		{
			this.position = position;
			this.size = size;
		}
	}

	[Serializable]
	public class MoneyData
	{
		public int money;

		// Construtor
		public MoneyData(int money)
		{
			this.money = money;
		}
	}

	[Serializable]
	public class AmbientData
	{
		public Ambient.Temperature currentTemperature;
		public Ambient.Climate currentClimate;
		public AmbientManager.Season currentSeason;
		public float seasonalFactor;

		// Construtor
		public AmbientData(Ambient.Temperature currentTemperature, Ambient.Climate currentClimate, AmbientManager.Season currentSeason, float seasonalFactor)
		{
			this.currentTemperature = currentTemperature;
			this.currentClimate = currentClimate;
			this.currentSeason = currentSeason;
			this.seasonalFactor = seasonalFactor;
		}
	}

	[Serializable]
	public class IDS_Data
	{
		public int ids;
		public int ecologico;
		public int economico;
		public int social;

		// Construtor
		public IDS_Data(int ids, int ecologico, int economico, int social)
		{
			this.ids = ids;
			this.ecologico = ecologico;
			this.economico = economico;
			this.social = social;
		}
	}

	[Serializable]
	public class TilesData
	{
		public List<CustomTileData> customTileDatas;
		public List<TreeTileData> treeTileDatas;
		public List<PlantTileData> plantTileDatas;
        public List<HouseTileData> houseTileDatas;

		// Construtor
		public TilesData(List<CustomTileData> customTileDatas, List<TreeTileData> treeTileDatas, List<PlantTileData> plantTileDatas, List<HouseTileData> houseTileDatas)
		{
			this.customTileDatas = customTileDatas;
			this.treeTileDatas = treeTileDatas;
			this.plantTileDatas = plantTileDatas;
			this.houseTileDatas = houseTileDatas;
		}
    }

	[Serializable]
	public class CustomTileData
	{
		public Vector3Int tilePosition; 
		public GameObject customTilePrefab; 
		public GameObject plowedTilePrefab;
		public bool isPlantable = false;
		public int nitrogen = 1000;
		public int phosphorus = 1000;
		public int potassium = 1000;
		public int humidity = 1000;
        public int rotationState = 0;

        // Construtor
        public CustomTileData(Vector3Int tilePosition, GameObject customTilePrefab, GameObject plowedTilePrefab, bool isPlantable, 
			int nitrogen, int phosphorus, int potassium, int humidity, int rotationState)
        {
			this.tilePosition = tilePosition;
            this.customTilePrefab = customTilePrefab;
            this.plowedTilePrefab = plowedTilePrefab;
            this.isPlantable = isPlantable;
            this.nitrogen = nitrogen;
            this.phosphorus = phosphorus;
            this.potassium = potassium;
            this.humidity = humidity;
            this.rotationState = rotationState;
        }
    }

    [Serializable]
    public class TreeTileData
    {
		public Vector3Int tilePosition;
        public GameObject treePrefab;
        public GameObject groundPrefab;
        public bool wasCuted;

        // Construtor
        public TreeTileData(Vector3Int tilePosition, GameObject treePrefab, GameObject groundPrefab, bool wasCuted)
		{
			this.tilePosition = tilePosition;
			this.treePrefab = treePrefab;
            this.groundPrefab = groundPrefab;
			this.wasCuted = wasCuted;
		}
    }

    [Serializable]
    public class PlantTileData
    {
		public Vector3Int tilePosition;
		public GameObject[] growthPrefabs;
		public GameObject rotPrefab;
		public float[] growthTimes;
        public Ambient.Temperature idealTemperature;
		public Ambient.Climate idealClimate;
		public int greenTolerance;
		public int yellowTolerance;
        public bool isPlanted;
		public bool isFullyGrown;
		public bool isRotten;
		public Item harvestedItem;
		public CustomTileBase soilTile;
        public int growthStage;
		public float totalGrowthTime;
		public float currentGrowthTime;
		public GameObject currentGrowthInstance;
        public int requiredNitrogen;
		public int requiredPhosphorus;
		public int requiredPotassium;
		public int returnNitrogen;
		public int returnPhosphorus;
		public int returnPotassium;

        // Construtor
        public PlantTileData(Vector3Int tilePosition, GameObject[] growthPrefabs, GameObject rotPrefab, float[] growthTimes,
			Ambient.Temperature idealTemperature, Ambient.Climate idealClimate, int greenTolerance, int yellowTolerance,
			bool isPlanted, bool isFullyGrown, bool isRotten, Item harvestedItem, CustomTileBase soilTile,
			int growthStage, float totalGrowthTime, float currentGrowthTime, GameObject currentGrowthInstance,
			int requiredNitrogen, int requiredPhosphorus, int requiredPotassium, int returnNitrogen, int returnPhosphorus, int returnPotassium)
		{
			this.tilePosition = tilePosition;
            this.growthPrefabs = growthPrefabs;
            this.rotPrefab = rotPrefab;
            this.growthTimes = growthTimes;
            this.idealTemperature = idealTemperature;
            this.idealClimate = idealClimate;
            this.greenTolerance = greenTolerance;
            this.yellowTolerance = yellowTolerance;
            this.isPlanted = isPlanted;
            this.isFullyGrown = isFullyGrown;
            this.isRotten = isRotten;
            this.harvestedItem = harvestedItem;
            this.soilTile = soilTile;
            this.growthStage = growthStage;
            this.totalGrowthTime = totalGrowthTime;
            this.currentGrowthTime = currentGrowthTime;
            this.currentGrowthInstance = currentGrowthInstance;
            this.requiredNitrogen = requiredNitrogen;
            this.requiredPhosphorus = requiredPhosphorus;
            this.requiredPotassium = requiredPotassium;
            this.returnNitrogen = returnNitrogen;
            this.returnPhosphorus = returnPhosphorus;
            this.returnPotassium = returnPotassium;
        }
    }

    [Serializable]
    public class HouseTileData
    {
        public Vector3Int tilePosition;
		public GameObject groundPrefab;

        // Construtor
        public HouseTileData(Vector3Int tilePosition, GameObject groundPrefab)
		{
			this.tilePosition = tilePosition;
            this.groundPrefab = groundPrefab;
        }
    }

    [Serializable]
	public class InventoryData
	{
		public List<InventoryItem> itens;

		// Construtor
		public InventoryData(List<InventoryItem> itens)
		{
			this.itens = itens;
		}

		/*public class InventoryItemData
		{
			public Item item;
			public int quantity;
		}*/
	}

    [Serializable]
    public class QuestData
	{
        public List<Quest> availableQuests;
        public List<Quest> activeQuests;

		// Construtor
		public QuestData(List<Quest> availableQuests, List<Quest> activeQuests)
		{
			this.availableQuests = availableQuests;
			this.activeQuests = activeQuests;
		}
    }
	



    #endregion

    #region // Save List Guide

    /*
		Config
		Tutorial
		Camera
		Money
		Ambient
		Ids
		Tile []
		Inventory
		Quests
	*/

    #region // Tile

    /*
		{CustomTileBase, PalntTile}
		Tile:
			position
			NPKh
			isPlantable
			isPanted
			PlantTile

			PlantTile:
				growthState
				totalGrowthTime
				currantGrowthTime
				currentGrowthInstance
	*/

    #endregion
    #region // IDS

    /*
		{IDS}
		IDS:
			ecologico
			economico
			social
			CalcularIDS()
	*/

    #endregion
    #region // Ambient

    /*
		{Ambient: Sriptableobject}
		Ambient:
			currentTemperature
			currentClimate
	*/

    #endregion
    #region // Money

    /*
		{MoneyManager}
		Money:
			currentMoney
	*/

    #endregion
    #region // Invetory

    /*
		{Inventory, InventoryItem}
		Inventory:
			items: List<InventoryItem>

			InventoryItem:
				item
				quantity
	*/

    #endregion
    #region // Quest

    /*
		{TaskManager, Task/Quest, Objective, Reward}
		Quests:
			quests: List<Quest>

			Quest:
				title
				description
				isCompleted
				objectives: List<Objective>
				Reward

				Objective:
					description
					isCompleted
					type: ObjectiveType
					targetAmount
					currentAmont

				Reward: 
					moneyAmount
					ecologicoValor
					economicoValor
					socialValor
	*/

    #endregion
    #region // Camera

    /*
		{CameraController}	
		Camera:
			position
			size
	*/

    #endregion
    #region // Tutorial

    /*
		{???}
		Tutorial:
			isDone
    */

    #endregion
    #region // Config

    /*
		{???}
		Config:
			//@
	*/

    #endregion

    #region // CompleteList

    /*
		Tile[]
		IDS
		Ambient
		Money
		Inventory
		Quests
		Camera
		Tutorial


		{CustomTileBase, PalntTile}
		Tile:
			position
			NPKh
			isPlantable
			isPanted
			PlantTile

			PlantTile:
				growthState
				totalGrowthTime
				currantGrowthTime
				currentGrowthInstance

		{IDS}
		IDS:
			ecologico
			economico
			social
			CalcularIDS()

		{Ambient: Sriptableobject}
		Ambient:
			currentTemperature
			currentClimate

		{MoneyManager}
		Money:
			currentMoney

		{Inventory, InventoryItem}
		Inventory:
			items: List<InventoryItem>

			InventoryItem:
				item
				quantity

		{TaskManager, Task/Quest, Objective, Reward}
		Quests:
			quests: List<Quest>

			Quest:
				title
				description
				isCompleted
				objectives: List<Objective>
				Reward

				Objective:
					description
					isCompleted
					type: ObjectiveType
					targetAmount
					currentAmont

				Reward: 
					moneyAmount
					ecologicoValor
					economicoValor
					socialValor

		{CameraController}	
		Camera:
			position
			size

		{???}
		Tutorial:
			isDone
	*/

    #endregion

    #endregion
}
