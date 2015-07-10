using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
class Hero_Info
{
	public int m_iStatPoints;

	public double m_iExp_To_Level;
	public double m_iCurrnet_Exp;

	//Heros health stats
	public float m_fMax_Health;
	public float m_fHealth;
	public float m_fHealth_Regen;

	public float m_fMax_Mana;
	public float m_fMana;
	public float m_fMana_Regen;

	public float m_fDamage;
	private float m_fAttack_Range;
	public float m_fAttack_Speed;

	public double m_dGold;
}

[Serializable]
class Item_Info
{
	public float m_fItemMod = 0.0f;
	public int m_iItemLevel = 0;
	public int m_eItemType = 0;
	public int m_eItemModType = 0;
	public int m_eRarity = 0;

	public bool m_bIsNull = true;
}

[Serializable]
class Equipt_Item_Info
{
	public Item_Info m_goEquip_Weapon = new Item_Info();
	public Item_Info m_goEquip_Helm = new Item_Info();
	public Item_Info m_goEquip_ChestPlate = new Item_Info();
	public Item_Info m_goEquip_PlateLegs = new Item_Info();
	public Item_Info m_goEquip_Belt = new Item_Info();
}

[Serializable]
class Inventory_Item_Info
{
	public List<Item_Info> AllItems = new List<Item_Info>();
}


[Serializable]
class Level_Info
{
	public int m_iAmount_Enemy_Spawn;
	public int m_iEnemy_Level;
	public float m_fSpawn_Intervial;
	public float m_fSpawn_Timer;

}

public class Game_Info : MonoBehaviour 
{
	public static Game_Info GameInfo;

	public Hero_Data HeroScript;
	public GameObject m_goItem;

	// Use this for initialization
	void Start () 
	{
		if(GameInfo == null)
		{
			GameInfo = this;
		}
		else if(GameInfo != this)
		{
			Destroy(gameObject);
		}
	}

	public void SaveInvenroty()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "Inventory.dat");
		
		Inventory_Item_Info Inventory_Info = new Inventory_Item_Info();

		for(int i = 0; i < Inventory.Inven.m_goAllItems.Count; ++i)
		{
			Item_Info NewItem = new Item_Info();

			SaveItem(NewItem, Inventory.Inven.m_goAllItems[i]);
			Inventory_Info.AllItems.Add(NewItem);
		}

		bf.Serialize(file, Inventory_Info);
		
		file.Close();
	}

	public void LoadInventory()
	{
		if(File.Exists(Application.persistentDataPath + "Inventory.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file_loc = File.Open(Application.persistentDataPath + "Inventory.dat",FileMode.Open);
			
			Inventory_Item_Info Inven = (Inventory_Item_Info)bf.Deserialize(file_loc);
			
			file_loc.Close();
			print (Inven.AllItems.Count);

			for(int i = 0; i < Inven.AllItems.Count; ++i)
			{
				print ("item Loaded:" + i);
				if(LoadItem(Inven.AllItems[i]))
				{
					Inventory.Inven.AddExisingItem(LoadItem(Inven.AllItems[i]));
				}
				else
				{

				}
			}
		}
	}

	public void SaveEquip()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "Equip.dat");
		
		Equipt_Item_Info Equip = new Equipt_Item_Info();

		SaveItem(Equip.m_goEquip_Belt, Equip_Items.Equip.m_goEquip_Belt);
		SaveItem(Equip.m_goEquip_ChestPlate, Equip_Items.Equip.m_goEquip_ChestPlate);
		SaveItem(Equip.m_goEquip_Helm , Equip_Items.Equip.m_goEquip_Helm);
		SaveItem(Equip.m_goEquip_PlateLegs, Equip_Items.Equip.m_goEquip_PlateLegs);
		SaveItem(Equip.m_goEquip_Weapon, Equip_Items.Equip.m_goEquip_Weapon);

		bf.Serialize(file, Equip);
		
		file.Close();
	}

	public void LoadEquip()
	{
		if(File.Exists(Application.persistentDataPath + "Equip.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file_loc = File.Open(Application.persistentDataPath + "Equip.dat",FileMode.Open);
			
			Equipt_Item_Info Equip = (Equipt_Item_Info)bf.Deserialize(file_loc);
			
			file_loc.Close();
			
			Equip_Items.Equip.Equip_Item(LoadItem(Equip.m_goEquip_Belt));
			Equip_Items.Equip.Equip_Item(LoadItem(Equip.m_goEquip_ChestPlate));
			Equip_Items.Equip.Equip_Item(LoadItem(Equip.m_goEquip_Helm));
			Equip_Items.Equip.Equip_Item(LoadItem(Equip.m_goEquip_PlateLegs));
			Equip_Items.Equip.Equip_Item(LoadItem(Equip.m_goEquip_Weapon));			
		}
	}
	

	void SaveItem(Item_Info NewItem, GameObject _Item)
	{
		if(_Item)
		{
			Item_Data Item = _Item.GetComponent<Item_Data>();

			NewItem.m_eItemModType = (int)Item.m_eItemModType;
			NewItem.m_eItemType = (int)Item.m_eItemType;
			NewItem.m_eRarity = (int)Item.m_eRarity;
			NewItem.m_fItemMod = Item.m_fItemMod;
			NewItem.m_iItemLevel = Item.m_iItemLevel;

			NewItem.m_bIsNull = false;
		}
		else
		{
			NewItem.m_bIsNull = true;
		}
	}

	GameObject LoadItem(Item_Info _Item)
	{
		GameObject tNewItem = null;

		if(_Item.m_bIsNull)
		{

		}
		else
		{
			tNewItem = Instantiate(m_goItem) as GameObject;

			tNewItem.transform.SetParent(Inventory.Inven.transform);

			Item_Data ItemScript =  tNewItem.GetComponent<Item_Data>();
			
			ItemScript.m_eItemModType = (Item_Data.ItemMod)_Item.m_eItemModType;
			ItemScript.m_eItemType = (Item_Data.ItemType)_Item.m_eItemType;
			ItemScript.m_eRarity = (Item_Data.ItemRarity)_Item.m_eRarity;
			ItemScript.m_fItemMod = _Item.m_fItemMod;
			ItemScript.m_iItemLevel = _Item.m_iItemLevel;

			ItemScript.forceDisplay();
		}
		return(tNewItem);
	}

	public void SaveLevel()
	{
		Level_Data Level = Level_Data.Level_Info.GetComponent<Level_Data>();

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "LevelInfo.dat");
		
		Level_Info NewLevel = new Level_Info();

		NewLevel.m_fSpawn_Intervial = Level.m_fSpawn_Intervial;
		NewLevel.m_fSpawn_Timer = Level.m_fSpawn_Timer;
		NewLevel.m_iAmount_Enemy_Spawn = Level.m_iAmount_Enemy_Spawn;
		NewLevel.m_iEnemy_Level = Level.m_iEnemy_Level;
		
		bf.Serialize(file, NewLevel);
		file.Close();
	}

	public void LoadLevel()
	{
		Level_Data Level = Level_Data.Level_Info.GetComponent<Level_Data>();
		if(File.Exists(Application.persistentDataPath + "LevelInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file_loc = File.Open(Application.persistentDataPath + "LevelInfo.dat",FileMode.Open);
			
			Level_Info NewLevel = (Level_Info)bf.Deserialize(file_loc);
			
			file_loc.Close();

			Level.m_fSpawn_Intervial = NewLevel.m_fSpawn_Intervial;
			Level.m_fSpawn_Timer = NewLevel.m_fSpawn_Timer;
			Level.m_iAmount_Enemy_Spawn = NewLevel.m_iAmount_Enemy_Spawn;
			Level.m_iEnemy_Level = NewLevel.m_iEnemy_Level;
		}
	}

	public void SaveHero()
	{
		HeroScript = Hero_Data.Hero.GetComponent<Hero_Data>();

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "HeroInfo.dat");
		
		Hero_Info Hero = new Hero_Info();

		Hero.m_dGold = HeroScript.m_dGold;
		Hero.m_fAttack_Speed = HeroScript.m_fAttack_Speed;
		Hero.m_fDamage = HeroScript.m_fDamage;
		Hero.m_fHealth = HeroScript.m_fHealth;
		Hero.m_fHealth_Regen = HeroScript.m_fHealth_Regen;
		Hero.m_fMana = HeroScript.m_fMana;
		Hero.m_fMana_Regen = HeroScript.m_fMana_Regen;
		Hero.m_fMax_Health = HeroScript.m_fMax_Health;
		Hero.m_fMax_Mana = HeroScript.m_fMax_Mana;
		Hero.m_iCurrnet_Exp = HeroScript.m_iCurrnet_Exp;
		Hero.m_iExp_To_Level = HeroScript.m_iExp_To_Level;
		Hero.m_iStatPoints = HeroScript.m_iStatPoints;

		bf.Serialize(file, Hero);
		
		file.Close();
	}

	public void LoadHero()
	{
		HeroScript = Hero_Data.Hero.GetComponent<Hero_Data>();
		if(File.Exists(Application.persistentDataPath + "HeroInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file_loc = File.Open(Application.persistentDataPath + "HeroInfo.dat",FileMode.Open);
			
			Hero_Info Hero = (Hero_Info)bf.Deserialize(file_loc);
			
			file_loc.Close();

			HeroScript.m_dGold = Hero.m_dGold;
			HeroScript.m_fAttack_Speed = Hero.m_fAttack_Speed;
			HeroScript.m_fDamage = Hero.m_fDamage;
			HeroScript.m_fHealth = Hero.m_fHealth;
			HeroScript.m_fHealth_Regen = Hero.m_fHealth_Regen;
			HeroScript.m_fMana = Hero.m_fMana;
			HeroScript.m_fMana_Regen = Hero.m_fMana_Regen;
			HeroScript.m_fMax_Health = Hero.m_fMax_Health;
			HeroScript.m_fMax_Mana = Hero.m_fMax_Mana;
			HeroScript.m_iCurrnet_Exp = Hero.m_iCurrnet_Exp;
			HeroScript.m_iExp_To_Level = Hero.m_iExp_To_Level;
			HeroScript.m_iStatPoints = Hero.m_iStatPoints;
		}
		else
		{

		}
	}
}
