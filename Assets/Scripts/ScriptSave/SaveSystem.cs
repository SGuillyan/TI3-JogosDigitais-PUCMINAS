using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{

	[Serializable]
	public class SaveAccesses
	{
		public MoneyManager moneyManager;
		public InventoryManager inventoryManager;
		public Camera mainCamera;
		public TutorialManager tutorialManager;

	}

	[SerializeField] private SaveAccesses accesses;

	private static MoneyManager moneyManager;
	private static InventoryManager inventoryManager;
	private static Camera mainCamera;
	private static TutorialManager tutorialManager;

	private void Start()
	{
		moneyManager = accesses.moneyManager;
		inventoryManager = accesses.inventoryManager;
		mainCamera = accesses.mainCamera;
		tutorialManager = accesses.tutorialManager;

		//Save("Assets/Scripts/ScriptSave/SaveData.json");
	}

	public static void Save(string path)
	{
		string json = JsonUtility.ToJson(GenerateSaveData(), true);

		File.WriteAllText(path, json);
	}

	public static SaveData Load(string path)
	{
		string json = File.ReadAllText(path);

		return JsonUtility.FromJson<SaveData>(json);
	}

	public static SaveData GenerateSaveData()
	{
		SaveData save = new SaveData();

		save.configData = new ConfigData("nome", "lingua", 5.3f, 8.1f);
        save.tutorialData = new TutorialData(false);
        save.cameraData = new CameraData(mainCamera.transform.position, mainCamera.orthographicSize);
        save.moneyData = new MoneyData(moneyManager.GetCurrentMoney());
        save.ambientData = new AmbientData(Ambient.GetCurrentTemperature(), Ambient.GetCurrentClimate(), AmbientManager.GetCurrentSeason(), AmbientManager.GetSeasonalFactor());
        save.idsData = new IDS_Data(IDS.GetIDS(), IDS.GetEcologico(), IDS.GetEconomico(), IDS.GetSocial());
        // TileData       
        save.inventoryData = new InventoryData(inventoryManager.playerInventory.items);
        // QuestData

        return save;
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
        //public TileData tileData;
        public InventoryData inventoryData;
		//public QuestData questData;		
	}

    [Serializable]
    public class ConfigData
    {
        public string name;
        public string lengague;
        public float musicVolume;
        public float sfxVolume;

        // Construtor
        public ConfigData(string name, string lenguage, float musicVolume, float sfxVolume)
        {
            this.name = name;
            this.lengague = lenguage;
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

    /*implement
	public class TileData
	{

	}
	*/

    //implement
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

	/*implement
	public class QuestData
	{
		
	}
	*/



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
