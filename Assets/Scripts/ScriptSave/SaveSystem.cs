using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

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
        public InventoryManager inventoryManager;
		public QuestManager questManager;
	}

	[SerializeField] private SaveAccesses accesses;

	private static VolumeSettings volumeSettings;
    private static TutorialManager tutorialManager;
    private static Camera mainCamera;
    private static MoneyManager moneyManager;
	private static Transform tilesParent;
	private static InventoryManager inventoryManager;
	private static QuestManager questManager;

	private void Awake()
	{
		volumeSettings = accesses.volumeSettings;
        tutorialManager = accesses.tutorialManager;
        mainCamera = accesses.mainCamera;
        moneyManager = accesses.moneyManager;
		tilesParent = accesses.tilesParent;
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
		volumeSettings.musicSlider.value = load.configData.musicVolume;
		volumeSettings.sfxSlider.value = load.configData.sfxVolume;
		#endregion
		#region // TutorialData
		tutorialManager.tutirialCompleted = load.tutorialData.isDone;
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
        #region // TileData

        #endregion
        #region // InventoryData
		for (int i = 0; i < load.inventoryData.itens.Count; i++)
		{
			inventoryManager.playerInventory.items.Add(load.inventoryData.itens[i]);
		}
        #endregion
        #region // QuestData
        for (int i = 0; i < load.questData.availableQuests.Count; i++)
        {
            questManager.availableQuests.Add(load.questData.availableQuests[i]);
        }
        for (int i = 0; i < load.questData.activeQuests.Count; i++)
        {
            questManager.activeQuests.Add(load.questData.activeQuests[i]);
        }
		#endregion
    }


    private static void Save(string path)
	{
		string json = JsonUtility.ToJson(GenerateSaveData(), true);

		File.WriteAllText(path, json);
		Debug.Log("Save concluido!");
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
		//save.tileData = new TileData(tilesParent);
        save.inventoryData = new InventoryData(inventoryManager.playerInventory.items);
		save.questData = new QuestData(questManager.availableQuests, questManager.activeQuests);

        Debug.Log("Relatório de salvamento gerado!");
        return save;
	}

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
        public TileData tileData;
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
    public class TileData
	{
		public List<GameObject> tiles;

		//Construtor
		public TileData(Transform tilesParent)
		{
			for(int i = 0; i < tilesParent.childCount; i++)
			{
				tiles.Add(tilesParent.GetChild(i).gameObject);
			}
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
