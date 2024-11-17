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

		Save("Assets/SaveData.json");
    }

	public static void Save(string path)
	{
        string json = JsonUtility.ToJson(GenerateSaveJsonFiles(), true);

		File.WriteAllText(path, json);
	}

	public static void Load(string path)
	{
		SaveData save = GenerateSaveData(path);
    }

	#region // Métodos Privados

	private static SaveJsonFiles GenerateSaveJsonFiles()
	{
        SaveJsonFiles jsonFiles = new SaveJsonFiles();

        // TileData
        //@
        jsonFiles.idsJson = JsonUtility.ToJson(new IDS_Data(IDS.GetEcologico(), IDS.GetEconomico(), IDS.GetSocial()));
        jsonFiles.ambientJson = JsonUtility.ToJson(new AmbientData(Ambient.currentTemperature, Ambient.currentClimate));
        jsonFiles.moneyJson = JsonUtility.ToJson(new MoneyData(moneyManager.GetCurrentMoney()));
        jsonFiles.inventoryJson = JsonUtility.ToJson(new InventoryData(inventoryManager.playerInventory.items));
        // QuestData
        //@
        jsonFiles.cameraJson = JsonUtility.ToJson(new CameraData(mainCamera.transform.position, mainCamera.orthographicSize));
        jsonFiles.tutorialJson = JsonUtility.ToJson(new TutorialData(false));

		return jsonFiles;
    }

	private static SaveData GenerateSaveData(string path)
	{
        SaveData save = new SaveData();

        string json = File.ReadAllText(path);
        SaveJsonFiles jsonFiles = JsonUtility.FromJson<SaveJsonFiles>(json);

        //save.tileData = JsonUtility.FromJson<TileData>(jsonFiles.tileJson);
        save.idsData = JsonUtility.FromJson<IDS_Data>(jsonFiles.idsJson);
        save.ambientData = JsonUtility.FromJson<AmbientData>(jsonFiles.ambientJson);
        save.moneyData = JsonUtility.FromJson<MoneyData>(jsonFiles.moneyJson);
        save.inventoryData = JsonUtility.FromJson<InventoryData>(jsonFiles.inventoryJson);
        //save.questData = JsonUtility.FromJson<QuestData>(jsonFiles.questJson);
        save.cameraData = JsonUtility.FromJson<CameraData>(jsonFiles.cameraJson);
        save.tutorialData = JsonUtility.FromJson<TutorialData>(jsonFiles.tutorialJson);

		return save;
    }

    #endregion

    #region // Data Classes

    private class SaveData
    {
		//public TileData tileData;
		public IDS_Data idsData;
		public AmbientData ambientData;
		public MoneyData moneyData;
		public InventoryData inventoryData;
		//public QuestData questData;
		public CameraData cameraData;
		public TutorialData tutorialData;
    }

	private class SaveJsonFiles
	{
		//public string tileJson;
		public string idsJson;
		public string ambientJson;
		public string moneyJson;
		public string inventoryJson;
		//public string questJson;
		public string cameraJson;
		public string tutorialJson;
	}

	/*implement
	private class TileData
	{

	}
	*/

	private class IDS_Data
	{
		public int ecologico;
		public int economico;
		public int social;

		// Construtor
		public IDS_Data(int ecologico, int economico, int social)
		{
			this.ecologico = ecologico;
			this.economico = economico;
			this.social = social;
		}
	}

	private class AmbientData
	{
		public Ambient.Temperature currentTemperature;
		public Ambient.Climate currentClimate;

		// Construtor
		public AmbientData(Ambient.Temperature currentTemperature, Ambient.Climate currentClimate)
		{
			this.currentTemperature = currentTemperature;
			this.currentClimate = currentClimate;
		}
	}

	private class MoneyData
	{
		public int money;

		// Construtor
		public MoneyData(int money)
		{
			this.money = money;
		}
	}

	//implement
	private class InventoryData
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
	private class QuestData
	{
		
	}
	*/

	private class CameraData
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

    private class TutorialData
    {
		public bool isDone;

		// Costrutor
		public TutorialData(bool isDone)
		{
			this.isDone = isDone;
		}
    }

	#endregion

	#region // Save List Guide

	/*
		Tile[]
		IDS
		Ambient
		Money
		Inventory
		Quests
		Camera
		Tutorial

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
