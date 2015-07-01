using UnityEngine;
using System.Collections;

public class Enemy_Data : MonoBehaviour {

	private int m_iLevel;

	public float m_fDamage;

	public float m_fHealth;

	public double m_fGold_Worth;
	public double m_fExp_Worth;

	private GameObject m_goHero;
	private float m_fAttack_Timer = 0.0f;

	private Hero_Data HeroScript;

	// Use this for initialization
	void Start () 
	{
		m_goHero = GameObject.FindGameObjectWithTag("Hero");

		HeroScript = m_goHero.GetComponent<Hero_Data>();
	}

	public void SetBaseStats()
	{
		m_fGold_Worth = Random.Range(3,5) * 1.3; 
		m_fHealth = 10 * (m_iLevel * 0.65f);
		m_fExp_Worth = m_iLevel * 1.8f;
		
		m_fDamage = m_iLevel * 2.2f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_fHealth <= 0)
		{
			HeroScript.m_dGold += m_fGold_Worth;
			HeroScript.m_iCurrnet_Exp += m_fExp_Worth;

			Destroy(gameObject);
		}

		AttackPlayer();
	}

	void AttackPlayer()
	{
		m_fAttack_Timer += Time.deltaTime;
		Vector3 Direction = m_goHero.transform.position - transform.position;

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
