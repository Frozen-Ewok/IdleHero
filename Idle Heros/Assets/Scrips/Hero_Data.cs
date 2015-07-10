using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hero_Data : MonoBehaviour 
{
	//only one hero
	public static Hero_Data Hero;

	//Hero levels
	public int m_iLevel = 1;
	private int m_iMax_Level = 5000;

	//heros available stat points
	public int m_iStatPoints = 0;

	//Exp of the hero
	public double m_iExp_To_Level = 10;
	public double m_iCurrnet_Exp = 0;
	
	//Heros health stats
	public float m_fMax_Health = 100f;
	public float m_fHealth = 100f;
	public float m_fHealth_Regen = 0.6f;

	public float m_fHealth_Mod = 0.0f;
	public float m_fHealth_Percent_Mod = 1.0f;
	public float m_fHealth_Regen_Mod = 0.0f;

	//heros mana stats
	public float m_fMax_Mana = 50f;
	public float m_fMana = 50f;
	public float m_fMana_Regen = 0.1f;

	public float m_fMana_Percent_Mod = 1.0f;
	public float m_fMana_Mod = 0f;
	public float m_fMana_Regen_Mod = 0.0f;

	//heros attack stats
	public float m_fDamage_Percent_Mod = 1.0f;
	public float m_fDamage_Mod = 0.0f;

	public float m_fDamage = 2.0f;
	private float m_fAttack_Range = 2.0f;
	public float m_fAttack_Speed = 1.0f;
	private float m_fAttack_Timer = 0.0f;

	//the heros gold
	public double m_dGold = 50;

	//the timer used to do the regen
	private float m_fRegenTimer = 0;

	//the enemy the hero is attacking
	private GameObject m_gEnemy;
	private Enemy_Data m_EnemyScript;

	//display the enemies stats
	public Text m_textEnemyInfo;

	// Use this for initialization
	void Start () 
	{
		if(Hero == null)
		{
			Hero = this;
		}
		else if(Hero != this)
		{
			Destroy(gameObject);
		}

		Game_Info.GameInfo.LoadHero();
	}
	void OnDestroy()
	{
		Game_Info.GameInfo.SaveHero();
		Game_Info.GameInfo.SaveEquip();
		Game_Info.GameInfo.SaveInvenroty();
		Game_Info.GameInfo.SaveLevel();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_iCurrnet_Exp >= m_iExp_To_Level) LevelUp();

		Regen();
		Attack();
		UpdateEnemyInfo();
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

			if(m_fHealth + m_fHealth_Regen >= ((m_fMax_Health + m_fHealth_Mod) * m_fHealth_Percent_Mod))
			{
				m_fHealth = (m_fMax_Health + m_fHealth_Mod) * m_fHealth_Percent_Mod;
			}
			else
			{
				m_fHealth += m_fHealth_Regen + m_fHealth_Regen_Mod;
			}

			if(m_fMana + m_fMana_Regen >= ((m_fMax_Mana + m_fMana_Mod) * m_fMana_Percent_Mod))
			{
				m_fMana = (m_fMax_Mana + m_fMana_Mod) * m_fMana_Percent_Mod;

			}
			else
			{
				m_fMana += m_fMana_Regen + m_fMana_Regen_Mod;
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

					m_EnemyScript.m_fHealth -= (m_fDamage + m_fDamage_Mod) * m_fDamage_Percent_Mod;
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
	void UpdateEnemyInfo()
	{
		if(m_gEnemy)
		{
			float fEnemyDamage = m_EnemyScript.m_fDamage;
			float fEnemyHp = m_EnemyScript.m_fHealth;
			float fEnemyGold = (float)m_EnemyScript.m_fGold_Worth; 
			float fEnemyExp = (float)m_EnemyScript.m_fExp_Worth;

			if(fEnemyHp <= 0)
			{
				fEnemyHp = 0;
			}

			m_textEnemyInfo.text = "Enemy Info \n";
			m_textEnemyInfo.text += "DPS: " + fEnemyDamage.ToString("F1") + "\n";
			m_textEnemyInfo.text += "Health: " + fEnemyHp.ToString("F1") + "/" + m_EnemyScript.m_fMax_Health.ToString("F1") + "\n";
			m_textEnemyInfo.text += "Gold: " + fEnemyGold.ToString("F1") + "\n";
			m_textEnemyInfo.text += "Exp: " + fEnemyExp.ToString("F1") + "\n";
		}
	}

	public void FullHeal()
	{
		m_fHealth = m_fMax_Health;
		m_fMana = m_fMax_Mana;
	}
}
