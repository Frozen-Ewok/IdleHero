using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour 
{
	public static Inventory Inven;

	public int m_iMaxItems = 10;
	private int m_iTotalItems = 0;

	public Text m_textTotalItems;

	public GameObject m_goItem;
	private RectTransform RecTrans;

	public List<GameObject> m_goAllItems = new List<GameObject>();

	private bool m_bShowInventory = false;

	public GameObject m_goMainInventory;

	private Vector3 m_transOrginalPos;
	private Vector3 m_transOffPos = new Vector3(-10000, -10000, 0);

	// Use this for initialization
	void Start () 
	{
		if(Inven == null)
		{
			Inven = this;
		}
		else if(Inven != this)
		{
			Destroy(gameObject);
		}

		m_transOrginalPos = m_goMainInventory.gameObject.transform.localPosition;
		m_goMainInventory.gameObject.transform.localPosition = m_transOffPos;

		RecTrans = gameObject.GetComponent<RectTransform>();

		Game_Info.GameInfo.LoadInventory();

		m_textTotalItems.text = "Items: " + m_iTotalItems + "/" + m_iMaxItems;

	}
	
	// Update is called once per frame
	void Update () 
	{
		//SpawnItem();
	}
	public void HideShowInventory()
	{
		m_bShowInventory = !m_bShowInventory;

		if(m_bShowInventory)
		{
			m_goMainInventory.gameObject.transform.localPosition = m_transOrginalPos;
		}
		else
		{
			m_goMainInventory.gameObject.transform.localPosition = m_transOffPos;
		}
	}

	public void SpawnItem()
	{
		GameObject m_goLevelSpawner = GameObject.FindGameObjectWithTag("Spawner");
		Level_Data level_script = m_goLevelSpawner.GetComponent<Level_Data>();

		AddItem(level_script.m_iEnemy_Level);
	}

	public void RemoveItem(GameObject _goItem)
	{
		m_goAllItems.Remove(_goItem);
		--m_iTotalItems;

		Rearange();
	}

	public void Rearange()
	{
		Item_Data ItemScript;

		for(int i = 0; i < m_goAllItems.Count; ++i)
		{
			if(m_goAllItems[i])
			{
				ItemScript = m_goAllItems[i].GetComponent<Item_Data>();

				if(ItemScript.m_bSold)
				{
					--m_iTotalItems;
					Destroy(m_goAllItems[i]);
					m_goAllItems.RemoveAt(i);
				}
			}
		}
		
		for(int i = 0; i < m_goAllItems.Count; ++i)
		{

			float RowCount = (i / 2);

			RectTransform itemRec = m_goAllItems[i].GetComponent<RectTransform>();

			float fX = 0.0f;
			float fY = 0.0f;

			if(i % 2 == 0)
			{
				//even num of items so place left
				fX = 2.0f;
				fY = (itemRec.rect.height * RowCount) + 2.0f;
			}
			else
			{
				//odd num of items so place right
				fX = RecTrans.rect.width / 2;
				fY = (itemRec.rect.height * RowCount) + 2.0f;
			}
			itemRec.transform.localPosition = new Vector2(fX, fY);
		}

		m_textTotalItems.text = "Items: " + m_iTotalItems + "/" + m_iMaxItems;
	}

	public void AddExisingItem(GameObject _goItem)
	{
		RectTransform itemRec = _goItem.GetComponent<RectTransform>();

		float RowCount = (m_iTotalItems / 2);

		//set the size of the inventory
		RecTrans.sizeDelta = new Vector2(RecTrans.rect.width, (itemRec.rect.height + 20) * RowCount);

		_goItem.transform.SetParent(gameObject.transform);

		float fX = 0.0f;
		float fY = 0.0f;
		
		if(m_iTotalItems % 2 == 0)
		{
			//even num of items so place left
			fX = 2.0f;
			fY = (itemRec.rect.height * RowCount) + 2.0f;
		}
		else
		{
			//odd num of items so place right
			fX = RecTrans.rect.width / 2;
			fY = (itemRec.rect.height * RowCount) + 2.0f;
		}
		_goItem.transform.localPosition = new Vector2(fX, fY);

		m_goAllItems.Add(_goItem);
		
		++m_iTotalItems;
		m_textTotalItems.text = "Items: " + m_iTotalItems + "/" + m_iMaxItems;
	}

	void AddItem(int _iEnemyLevel)
	{
		if(m_iTotalItems < m_iMaxItems)
		{
			int iColumn = 2;
			RectTransform itemRec = m_goItem.GetComponent<RectTransform>();

			//inventory width with space for scroll bar
			float RowCount = (m_iTotalItems / iColumn);

			//set the size of the inventory
			RecTrans.sizeDelta = new Vector2(RecTrans.rect.width, (itemRec.rect.height + 20) * RowCount);

			GameObject NewItem = Instantiate(m_goItem) as GameObject;

			NewItem.transform.SetParent(gameObject.transform);

			RectTransform newItemRec = NewItem.GetComponent<RectTransform>();

			float fX = 0.0f;
			float fY = 0.0f;

			if(m_iTotalItems % 2 == 0)
			{
				//even num of items so place left
				fX = 2.0f;
				fY = (newItemRec.rect.height * RowCount) + 2.0f;
			}
			else
			{
				//odd num of items so place right
				fX = RecTrans.rect.width / 2;
				fY = (newItemRec.rect.height * RowCount) + 2.0f;
			}
			NewItem.transform.localPosition = new Vector2(fX, fY);

			Item_Data ItemScript = NewItem.GetComponent<Item_Data>();
			ItemScript.SetItemInfo(_iEnemyLevel);

			m_goAllItems.Add(NewItem);

			++m_iTotalItems;
			m_textTotalItems.text = "Items: " + m_iTotalItems + "/" + m_iMaxItems;

		}
	}
}
