using UnityEngine;
using System.Collections;

public class Hero_Data : MonoBehaviour 
{
	//Hero Stats
	public int m_iLevel = 1;
	private int m_iMax_Level = 5000;

	public int m_iStatPoints = 0;

	private float m_fAttack_Range = 2.0f;

	public double m_iExp_To_Level = 20;
	public double m_iCurrnet_Exp = 0;

	public float m_fDamage = 2.0f;

	public float m_fMax_Health = 100f;
	public float m_fHealth = 100f;
	public float m_fHealth_Regen = 0.6f;

	public float m_fMax_Mana = 50f;
	public float m_fMana = 50f;
	public float m_fMana_Regen = 0.1f;

	public float m_fAttack_Speed = 1.0f;
	private float m_fAttack_Timer = 0.0f;

	public double m_dGold = 0;

	private float m_fRegenTimer = 0;
	private GameObject m_gEnemy;
	private Enemy_Data m_EnemyScript;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_iCurrnet_Exp >= m_iExp_To_Level) LevelUp();

		Regen();
		Attack();
	}

	//Do all the level upping and change the m_iExp_To_Level
	void LevelUp ()
	{
		if(m_iLevel <= m_iMax_Level)
		{
			double Exp_Overflow = m_iCurrnet_Exp - m_iExp_To_Level;
			m_iLevel++;

			m_iExp_To_Level = (m_iExp_To_Level + m_iLevel) * 1.10f; 

			m_iStatPoints += 2;

			m_iCurrnet_Exp = Exp_Overflow;
		}
	}

	//Apply all the regen
	void Regen()
	{
		m_fRegenTimer += Time.deltaTime;

		if(m_fRegenTimer >= 1.0f)
		{
			m_fRegenTimer = 0;

			if(m_fHealth + m_fHealth_Regen >= m_fMax_Health)
			{
				m_fHealth = m_fMax_Health;
			}
			else
			{
				m_fHealth += m_fHealth_Regen;
			}

			if(m_fMana + m_fMana_Regen >= m_fMax_Mana)
			{
				m_fMana = m_fMax_Mana;
			}
			else
			{
				m_fMana += m_fMana_Regen;
			}
		}
	}
	void Attack()
	{
		
		m_fAttack_Timer += Time.deltaTime;
		//if the enemy is vaild then attack it.
		if(m_gEnemy)
		{
			Vector3 Direction = m_gEnemy.transform.position - transform.position;

			//The enemy is in attack range
			if(Direction.magnitude < m_fAttack_Range)
			{
				Direction.Normalize();
				transform.forward = Direction;

				if(m_fAttack_Timer > m_fAttack_Speed)
				{
					m_fAttack_Timer = 0.0f;

					m_EnemyScript.m_fHealth -= m_fDamage;
				}
			}
		}
		else
		{
			float Nearest = 100000.0f;

			GameObject[] NearestEnemy = GameObject.FindGameObjectsWithTag("Enemy");

			for(int i = 0; i < NearestEnemy.Length; ++i)
			{
				Vector3 Direction = NearestEnemy[i].transform.position - transform.position;

				if(Direction.magnitude < Nearest)
				{
					m_gEnemy = NearestEnemy[i];
					Nearest = Direction.magnitude;
				}
			}
			if(m_gEnemy)
			{
				m_EnemyScript = m_gEnemy.GetComponent<Enemy_Data>();
			}
		}
	}
}
