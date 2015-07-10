using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Equip_Items : MonoBehaviour 
{
	//only one hero
	public static Equip_Items Equip;

	public GameObject m_goEquip_Weapon;
	public GameObject m_goEquip_Helm;
	public GameObject m_goEquip_ChestPlate;
	public GameObject m_goEquip_Belt;
	public GameObject m_goEquip_PlateLegs;

	private Inventory InventoryScript;
	private Hero_Data HeroScript;

	private float m_fModHp;
	private float m_fModHpRegen;
	private float m_fModHpPercent;
	private float m_fModMp;
	private float m_fModMpRegen;
	private float m_fModMpPercent; 
	private float m_fModDmg;
	private float m_fModDmgPercent;

	private bool m_bShowEquip = false;

	private Vector3 m_transOrginalPos;
	private Vector3 m_transOffPos = new Vector3(-10000, -10000, 0);

	// Use this for initialization
	void Start () 
	{
		if(Equip == null)
		{
			Equip = this;
		}
		else if(Equip != this)
		{
			Destroy(gameObject);
		}

		GameObject m_goInventory = GameObject.FindGameObjectWithTag("inventory");
		InventoryScript = m_goInventory.GetComponent<Inventory>();

		m_transOrginalPos = gameObject.transform.localPosition;
		gameObject.transform.localPosition = m_transOffPos;

		Game_Info.GameInfo.LoadEquip();
	}

	public void HideShowEquip()
	{
		m_bShowEquip = !m_bShowEquip;

		if(m_bShowEquip)
		{
			gameObject.transform.localPosition = m_transOrginalPos;
		}
		else
		{
			gameObject.transform.localPosition = m_transOffPos;
		}
	}

	void ResetMods()
	{
		m_fModHp = 0.0f;
		m_fModHpRegen = 0.0f;
		m_fModHpPercent = 1.0f;
		m_fModMp = 0.0f;
		m_fModMpRegen = 0.0f;
		m_fModMpPercent = 1.0f;
		m_fModDmg = 0.0f;
		m_fModDmgPercent = 1.0f;
	}
	
	public void Equip_Item(GameObject _goItem)
	{
		Item_Data ItemScript;
		if(_goItem)
		{
			ItemScript = _goItem.GetComponent<Item_Data>();
			float fX = 55.0f;
			float fY = 0.0f;

			if(ItemScript.m_eItemType == Item_Data.ItemType.ItemType_Weapon)
			{
				if(m_goEquip_Weapon)
				{
					InventoryScript.AddExisingItem(m_goEquip_Weapon);
					m_goEquip_Weapon.GetComponent<Item_Data>().m_bEquip = false;
				}
				m_goEquip_Weapon = _goItem;

				fY = 198.0f;
			}
			else if(ItemScript.m_eItemType == Item_Data.ItemType.ItemType_Helm)
			{
				if(m_goEquip_Helm)
				{
					InventoryScript.AddExisingItem(m_goEquip_Helm);
					m_goEquip_Helm.GetComponent<Item_Data>().m_bEquip = false;
				}
				m_goEquip_Helm = _goItem;
				
				fY = 149.0f;
			}
			else if(ItemScript.m_eItemType == Item_Data.ItemType.ItemType_ChestPlate)
			{
				if(m_goEquip_ChestPlate)
				{
					InventoryScript.AddExisingItem(m_goEquip_ChestPlate);
					m_goEquip_ChestPlate.GetComponent<Item_Data>().m_bEquip = false;
				}
				m_goEquip_ChestPlate = _goItem;

				fY = 100.0f;
			}

			else if(ItemScript.m_eItemType == Item_Data.ItemType.ItemType_Belt)
			{
				if(m_goEquip_Belt)
				{
					InventoryScript.AddExisingItem(m_goEquip_Belt);
					m_goEquip_Belt.GetComponent<Item_Data>().m_bEquip = false;
				}
				m_goEquip_Belt = _goItem;
				
				fY = 51.0f;
			}
			else if(ItemScript.m_eItemType == Item_Data.ItemType.ItemType_PlateLegs)
			{
				if(m_goEquip_PlateLegs)
				{
					InventoryScript.AddExisingItem(m_goEquip_PlateLegs);
					m_goEquip_PlateLegs.GetComponent<Item_Data>().m_bEquip = false;
				}
				m_goEquip_PlateLegs = _goItem;

				fY = 2.0f;
			}
			_goItem.transform.SetParent(gameObject.transform);
			_goItem.transform.localPosition = new Vector2(fX, fY);
			ItemScript.m_bEquip = true;
			applyMods();
		}
	}
	void applyMods()
	{
		ResetMods();
		if(m_goEquip_Weapon)CalcTotalMod(m_goEquip_Weapon);
		if(m_goEquip_Helm)CalcTotalMod(m_goEquip_Helm);
		if(m_goEquip_ChestPlate)CalcTotalMod(m_goEquip_ChestPlate);
		if(m_goEquip_Belt)CalcTotalMod(m_goEquip_Belt);
		if(m_goEquip_PlateLegs)CalcTotalMod(m_goEquip_PlateLegs);

		HeroScript = Hero_Data.Hero.GetComponent<Hero_Data>();

		HeroScript.m_fDamage_Mod = m_fModDmg;
		HeroScript.m_fDamage_Percent_Mod = m_fModDmgPercent;
		HeroScript.m_fHealth_Mod = m_fModHp;
		HeroScript.m_fHealth_Percent_Mod = m_fModHpPercent;
		HeroScript.m_fHealth_Regen_Mod = m_fModHpRegen;
		HeroScript.m_fMana_Mod = m_fModMp;
		HeroScript.m_fMana_Percent_Mod = m_fModMpPercent;
		HeroScript.m_fMana_Regen_Mod = m_fModMpRegen;
	}

	void CalcTotalMod(GameObject _goItem)
	{
		Item_Data ItemScript;
		ItemScript = _goItem.GetComponent<Item_Data>();

		if(ItemScript.m_eItemModType == Item_Data.ItemMod.ItemMod_Dmg)
		{
			m_fModDmg += ItemScript.m_fItemMod;
		}
		else if(ItemScript.m_eItemModType == Item_Data.ItemMod.ItemMod_DmgPercent)
		{
			m_fModDmgPercent += ItemScript.m_fItemMod;
		}
		else if(ItemScript.m_eItemModType == Item_Data.ItemMod.ItemMod_Hp)
		{
			m_fModHp += ItemScript.m_fItemMod;
		}
		else if(ItemScript.m_eItemModType == Item_Data.ItemMod.ItemMod_HpPercent)
		{
			m_fModHpPercent += ItemScript.m_fItemMod;
		}
		else if(ItemScript.m_eItemModType == Item_Data.ItemMod.ItemMod_HpRegen)
		{
			m_fModHpRegen += ItemScript.m_fItemMod;
		}
		else if(ItemScript.m_eItemModType == Item_Data.ItemMod.ItemMod_Mp)
		{
			m_fModMp += ItemScript.m_fItemMod;
		}
		else if(ItemScript.m_eItemModType == Item_Data.ItemMod.ItemMod_MpPercent)
		{
			m_fModMpPercent += ItemScript.m_fItemMod;
		}
		else if(ItemScript.m_eItemModType == Item_Data.ItemMod.ItemMod_MpRegen)
		{
			m_fModMpRegen += ItemScript.m_fItemMod;
		}
	}


}
