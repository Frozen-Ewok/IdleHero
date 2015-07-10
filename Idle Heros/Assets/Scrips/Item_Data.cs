using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Item_Data : MonoBehaviour 
{

	public enum ItemType
	{
		ItemType_Weapon = 0,
		ItemType_Helm = 1,
		ItemType_ChestPlate = 2,
		ItemType_Belt = 3,
		ItemType_PlateLegs = 4,

		ItemType_Max = 50
	};
	public enum ItemRarity
	{
		ItemRarity_Common = 0,	
		ItemRarity_Uncommon = 1,	//one in 5
		ItemRarity_Rare = 2,		//one in 25
		ItemRarity_Epic = 3,			//one in 100
		ItemRarity_Legendary = 4,	//one in 500

		ItemRarity_MAX = 50
	};

	public enum ItemMod
	{
		ItemMod_Hp = 0,
		ItemMod_HpRegen = 1,
		ItemMod_HpPercent = 2,
		ItemMod_Mp = 3,
		ItemMod_MpRegen = 4,
		ItemMod_MpPercent = 5,
		ItemMod_Dmg = 6,
		ItemMod_DmgPercent = 7,

		ItemMod_MAX = 50

	};


	public float m_fItemMod;
	public ItemType m_eItemType = ItemType.ItemType_Max;
	public ItemMod m_eItemModType = ItemMod.ItemMod_MAX;
	public ItemRarity m_eRarity = ItemRarity.ItemRarity_MAX;	
	public int m_iItemLevel;

	public Button m_buttonItem;

	private bool m_bShowOptions = false;
	public GameObject m_OptionButtons;

	public Button m_buttonSell;
	public Button m_buttonEquip;

	public Sprite m_spSword;
	public Sprite m_spHelm;
	public Sprite m_spChest;
	public Sprite m_spBelt;
	public Sprite m_spLegs;

	public Text m_textItem_Des;
	public Image m_imageItem;

	private Hero_Data HeroScript;
	private Inventory InventoryScript;
	private Equip_Items Equip_ItemsScript;

	public bool m_bSold = false;
	public bool m_bEquip = false;

	// Use this for initialization
	void Start () 
	{

		GameObject m_goHero = GameObject.FindGameObjectWithTag("Hero");
		HeroScript = m_goHero.GetComponent<Hero_Data>();

		GameObject m_goInventory = GameObject.FindGameObjectWithTag("inventory");
		InventoryScript = m_goInventory.GetComponent<Inventory>();

		GameObject m_goEquip_Items = GameObject.FindGameObjectWithTag("Equipment");
		Equip_ItemsScript = m_goEquip_Items.GetComponent<Equip_Items>();

		m_buttonItem.onClick.AddListener(()=>{OnClickOptions();});

		m_buttonSell.onClick.AddListener(()=>{OnClickSell();});

		m_buttonEquip.onClick.AddListener(()=>{OnClickEquip();});

		m_OptionButtons.SetActive(m_bShowOptions);

	}
	
	// Update is called once per frame
	void Update () 
	{
				
	}

	public void OnClickOptions()
	{
		m_bShowOptions = !m_bShowOptions;
		m_OptionButtons.SetActive(m_bShowOptions);
	}

	public void OnClickSell()
	{
		float fSellValue = 20.0f * (m_iItemLevel + (float)m_eRarity);

		HeroScript.m_dGold += fSellValue;

		m_bSold = true;

		if(m_bEquip)
		{
			Destroy(gameObject);
		}

		InventoryScript.Rearange();
	}
	public void OnClickEquip()
	{
		if(!m_bEquip)
		{
			InventoryScript.RemoveItem(gameObject);
			Equip_ItemsScript.Equip_Item(gameObject);
			OnClickOptions();
		}

	}

	public void SetItemInfo(int _iItemLevel)
	{
		m_iItemLevel = _iItemLevel;

		m_textItem_Des.text = "Item level:" + m_iItemLevel;

		SetItemType();
		SetItemRarity();
		SetItemStats(true);
	}

	void SetItemType()
	{
		int iType = Random.Range(0,4);

		m_eItemType = (ItemType)iType;

		if(m_eItemType == ItemType.ItemType_Weapon)
		{
			m_imageItem.sprite = m_spSword;
		}
		else if(m_eItemType == ItemType.ItemType_Helm)
		{
			m_imageItem.sprite = m_spHelm;
		}
		else if(m_eItemType == ItemType.ItemType_ChestPlate)
		{
			m_imageItem.sprite = m_spChest;
		}
		else if(m_eItemType == ItemType.ItemType_Belt)
		{
			m_imageItem.sprite = m_spBelt;
		}
		else
		{
			m_imageItem.sprite = m_spLegs;
		}

	}

	void SetItemStats(bool _bNewItem)
	{
		int iType = 100;

		if(m_eItemType != ItemType.ItemType_Weapon)
		{
			if(_bNewItem)iType = Random.Range(0,5);

			if(iType == 0 || m_eItemModType == ItemMod.ItemMod_Hp) //Base Hp
			{
				m_eItemModType = ItemMod.ItemMod_Hp;
				m_fItemMod = 20 * (m_iItemLevel + (int)(m_eRarity) + 1.5f);

				m_textItem_Des.text += "\nHealth +" + m_fItemMod;
			}
			else if(iType == 1 || m_eItemModType == ItemMod.ItemMod_HpRegen) //Hp Regen
			{
				m_eItemModType = ItemMod.ItemMod_HpRegen;
				m_fItemMod = 0.3f * (m_iItemLevel + (int)(m_eRarity) + 1);

				m_textItem_Des.text += "\nHealth Regen +" + m_fItemMod;
			}
			else if(iType == 2 || m_eItemModType == ItemMod.ItemMod_HpPercent) //Hp %
			{
				m_eItemModType = ItemMod.ItemMod_HpPercent;
				m_fItemMod = 0.1f * (m_iItemLevel + (int)(m_eRarity));

				m_textItem_Des.text += "\nHealth + %" + m_fItemMod;
			}
			else if(iType == 3 || m_eItemModType == ItemMod.ItemMod_Mp) //Base Mana
			{
				m_eItemModType = ItemMod.ItemMod_Mp;
				m_fItemMod = 10 * (m_iItemLevel + (int)(m_eRarity) + 1.5f);

				m_textItem_Des.text += "\nMana +" + m_fItemMod;
			}
			else if(iType == 4 || m_eItemModType == ItemMod.ItemMod_MpRegen)
			{
				m_eItemModType = ItemMod.ItemMod_MpRegen;
				m_fItemMod = 0.1f * (m_iItemLevel + (int)(m_eRarity));

				m_textItem_Des.text += "\nMana Regen +" + m_fItemMod;
			}
			else if(iType == 5 || m_eItemModType == ItemMod.ItemMod_MpPercent)
			{
				m_eItemModType = ItemMod.ItemMod_MpPercent;
				m_fItemMod = 0.1f * (m_iItemLevel + (int)(m_eRarity));

				m_textItem_Des.text += "\nMana + %" + m_fItemMod;
			}
		}
		else
		{
			if(_bNewItem)iType = Random.Range(0, 5);

			if(iType == 0 || m_eItemModType == ItemMod.ItemMod_DmgPercent)
			{
				m_eItemModType = ItemMod.ItemMod_DmgPercent;
				m_fItemMod = (1.0f + m_iItemLevel * 0.1f) * (((int)m_eRarity + 1.0f) / 2.0f);
				
				m_textItem_Des.text += "\nDamage + %" + m_fItemMod;
			}
			else
			{
				m_eItemModType = ItemMod.ItemMod_Dmg;
				m_fItemMod = (2 * m_iItemLevel + 1.5f) * (((int)m_eRarity + 1.0f) / 2);
				
				m_textItem_Des.text += "\nDamage +" + m_fItemMod;
			}
		}
	}

	void SetItemRarity()
	{
		Image ItemImage = gameObject.GetComponent<Image>();
		int iRarity = Random.Range(0,500);

		if(iRarity == 500)
		{
			m_eRarity = ItemRarity.ItemRarity_Legendary;
			ItemImage.color = Color.yellow;

		}
		else if(iRarity % 100 == 0)
		{
			m_eRarity = ItemRarity.ItemRarity_Epic;
			ItemImage.color = Color.cyan;
		}
		else if(iRarity % 25 == 0)
		{
			m_eRarity = ItemRarity.ItemRarity_Rare;
			ItemImage.color = Color.magenta;
		}
		else if(iRarity % 5 == 0)
		{
			m_eRarity = ItemRarity.ItemRarity_Uncommon;
			ItemImage.color = Color.gray;
		}
		else
		{
			m_eRarity = ItemRarity.ItemRarity_Common;
		}
	}

	public void forceDisplay()
	{
		Image ItemImage = gameObject.GetComponent<Image>();
		if(m_eRarity == ItemRarity.ItemRarity_Legendary) ItemImage.color = Color.yellow;
		else if(m_eRarity == ItemRarity.ItemRarity_Epic) ItemImage.color = Color.cyan;
		else if(m_eRarity == ItemRarity.ItemRarity_Rare) ItemImage.color = Color.magenta;
		else if(m_eRarity == ItemRarity.ItemRarity_Uncommon) ItemImage.color = Color.gray;

		if(m_eItemType == ItemType.ItemType_Weapon)	m_imageItem.sprite = m_spSword;
		else if(m_eItemType == ItemType.ItemType_Helm) m_imageItem.sprite = m_spHelm;
		else if(m_eItemType == ItemType.ItemType_ChestPlate) m_imageItem.sprite = m_spChest;
		else if(m_eItemType == ItemType.ItemType_Belt) m_imageItem.sprite = m_spBelt;
		else m_imageItem.sprite = m_spLegs;

		m_textItem_Des.text = "Item level:" + m_iItemLevel;
		SetItemStats(false);

	}
}

