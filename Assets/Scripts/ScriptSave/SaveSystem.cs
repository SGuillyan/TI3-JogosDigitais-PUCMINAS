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

		Save("Assets/Scripts/ScriptSave/SaveData.json");
    }

	public static void Save(string path)
	{
		SaveData save = new SaveData();

		// TileData
        new IDS_Data(IDS.GetEcologico(), IDS.GetEconomico(), IDS.GetSocial());
		new AmbientData(Ambient.GetCurrentTemperature(), Ambient.GetCurrentClimate(), AmbientManager.GetCurrentSeason(), AmbientManager.GetSeasonalFactor());
		new MoneyData(moneyManager.GetCurrentMoney());
		new InventoryData(inventoryManager.playerInventory.items);
		// QuestData
		new CameraData(mainCamera.transform.position, mainCamera.orthographicSize);
		new TutorialData(false);

		string json = JsonUtility.ToJson(save, true);

		File.WriteAllText(path, json);
    }

    public static void Load(string path)
	{
		string json = File.ReadAllText(path);

		SaveData load = JsonUtility.FromJson<SaveData>(json);
    }

    #region // Data Classes

	[Serializable]
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

	/*implement
	private class TileData
	{

	}
	*/

	[Serializable]
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

	[Serializable]
	private class AmbientData
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
	[Serializable]
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

	[Serializable]
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

	[Serializable]
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
		Config
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
