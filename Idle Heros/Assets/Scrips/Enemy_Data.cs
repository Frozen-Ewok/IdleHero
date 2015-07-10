using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy_Data : MonoBehaviour {

	private int m_iLevel;

	public float m_fDamage;

	public float m_fHealth;
	public float m_fMax_Health;

	public double m_fGold_Worth;
	public double m_fExp_Worth;

	//one in 10 chance at getting an item
	private int m_iItem_Drop_Rate = 10;

	private float m_fAttack_Timer = 0.0f;

	private Hero_Data HeroScript;

	private Inventory InventoryScript;

	// Use this for initialization
	void Start () 
	{
		HeroScript = Hero_Data.Hero.GetComponent<Hero_Data>();

		GameObject m_goInventory = GameObject.FindGameObjectWithTag("inventory");
		InventoryScript = m_goInventory.GetComponent<Inventory>();
	}

	public void SetBaseStats(int _iEnemyLevel)
	{
		m_iLevel = _iEnemyLevel;
		m_fGold_Worth = Random.Range((3 * m_iLevel),(5 * m_iLevel));
		m_fHealth = 10 * (m_iLevel * 0.65f);
		m_fExp_Worth = m_iLevel * 2.2f;

		m_fMax_Health = m_fHealth;

		m_fDamage = m_iLevel * 1.9f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_fHealth <= 0)
		{
			HeroScript.m_dGold += m_fGold_Worth;
			HeroScript.m_iCurrnet_Exp += m_fExp_Worth;

			float result = Random.Range(0, m_iItem_Drop_Rate);

			result = 1;

			if(result == 1)InventoryScript.SpawnItem();

			Destroy(gameObject);
		}

		AttackPlayer();
	}

	void AttackPlayer()
	{
		m_fAttack_Timer += Time.deltaTime;
		Vector3 Direction = Hero_Data.Hero.transform.position - transform.position;

		//move in to attack distance
		if(Direction.magnitude > 2)
		{
			Direction.Normalize();
			transform.forward = Direction;
			transform.position += transform.forward * Time.deltaTime * 10;
		}
		//now in range to attack
		else
		{
			if(m_fAttack_Timer > 1)
			{
				m_fAttack_Timer = 0.0f;

				HeroScript.m_fHealth -= m_fDamage;
			}
		}
	}

	public void SetEnemyLevel(int _iLevel)
	{
		m_iLevel = _iLevel;
	}
}
