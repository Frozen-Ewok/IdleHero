using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level_Data : MonoBehaviour 
{
	public GameObject Enemy;
	public GameObject Hero;

	private int m_iCurrent_Level = 1;
	private int m_iAmount_Enemy_Spawn = 1;
	private int m_iEnemy_Level = 1;

	public float m_fEnemy_Level_Cost = 50;
	public float m_fEnemy_Amount_Cost = 50;

	private float m_fSpawn_Intervial = 20;

	private float m_fSpawn_Timer = 18.0f;

	private GameObject m_goHero;
	private Hero_Data HeroScript;
	public Text m_textHeroStats;
	public Text m_textHeroGold;
	public Text m_textLevelInfo;

	public Text m_textEnemyLevel;
	public Text m_textEnemyAmount;

	public Text m_textAvailableStatPoints;

	public Button m_buttonUpDmg;
	public Button m_buttonUpHp;
	public Button m_buttonUpHpRe;
	public Button m_buttonUpMp;
	public Button m_buttonUpMpRe;

	// Use this for initialization
	void Start () 
	{
		Vector3 HeroPos;
		HeroPos.x = 0;
		HeroPos.y = 0;
		HeroPos.z = 0;

		Instantiate(Hero, HeroPos, transform.rotation);

		m_goHero = GameObject.FindGameObjectWithTag("Hero");
		HeroScript = m_goHero.GetComponent<Hero_Data>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateHUD();
		m_fSpawn_Timer += Time.deltaTime;
		if(m_fSpawn_Timer > m_fSpawn_Intervial)
		{
			SpawnEnemies();
			m_fSpawn_Timer = 0.0f;
		}
	}

	void SpawnEnemies()
	{
		//make sure the hero has a transform
		if(m_goHero.transform)
		{
			for(int i = 0; i < m_iAmount_Enemy_Spawn; ++i)
			{
				float fHeroX = m_goHero.transform.position.x;
				float fHeroZ = m_goHero.transform.position.z;

				Vector3 EnemyPos = m_goHero.transform.position;

				EnemyPos.x = Random.Range (fHeroX - 50, fHeroX + 50);
				EnemyPos.z = Random.Range (fHeroZ - 50, fHeroZ + 50);

				Enemy_Data EnemyScript = Enemy.GetComponent<Enemy_Data>();

				Instantiate(Enemy,EnemyPos, m_goHero.transform.rotation);

				EnemyScript.SetEnemyLevel(m_iEnemy_Level);
				EnemyScript.SetBaseStats();
			}
			++m_iCurrent_Level;
		}
	}

	void UpdateHUD()
	{
		m_textEnemyLevel.text = "Increase Enemy Level: " + (m_fEnemy_Level_Cost * m_iEnemy_Level) + " Gold";

		m_textEnemyAmount.text = "Increase Enemy Amount: " + (m_fEnemy_Amount_Cost * m_iAmount_Enemy_Spawn) + " Gold";

		m_textHeroGold.text = "Gold: " + HeroScript.m_dGold;

		m_textLevelInfo.text = "Enemy Level: " + m_iEnemy_Level + "\n";
		m_textLevelInfo.text += "Enemy Amount: " + m_iAmount_Enemy_Spawn + "\n";

		m_textHeroStats.text = "Level: " + HeroScript.m_iLevel + "\n";
		m_textHeroStats.text += "DPS: " +  (HeroScript.m_fDamage / HeroScript.m_fAttack_Speed) + "\n";
		m_textHeroStats.text += "HP " + HeroScript.m_fHealth.ToString("F1") + "/" + HeroScript.m_fMax_Health + "\n";
		m_textHeroStats.text += "MP " + HeroScript.m_fMana.ToString("F1") + "/" + HeroScript.m_fMax_Mana + "\n";
		m_textHeroStats.text += "EXP: "+ HeroScript.m_iCurrnet_Exp.ToString("F1") + "/" + HeroScript.m_iExp_To_Level.ToString("F1") + "\n";

		m_textAvailableStatPoints.text = "Stat Points: " + HeroScript.m_iStatPoints;

		if(HeroScript.m_iStatPoints >= 1)
		{
			m_buttonUpDmg.gameObject.SetActive(true);
			m_buttonUpHp.gameObject.SetActive(true);
			m_buttonUpHpRe.gameObject.SetActive(true);
			m_buttonUpMp.gameObject.SetActive(true);
			m_buttonUpMpRe.gameObject.SetActive(true);
			m_textAvailableStatPoints.gameObject.SetActive(true);
		}
		else
		{
			m_buttonUpDmg.gameObject.SetActive(false);
			m_buttonUpHp.gameObject.SetActive(false);
			m_buttonUpHpRe.gameObject.SetActive(false);
			m_buttonUpMp.gameObject.SetActive(false);
			m_buttonUpMpRe.gameObject.SetActive(false);
			m_textAvailableStatPoints.gameObject.SetActive(false);

		}
	}

	public void EnemyUpgrade()
	{
		if(HeroScript.m_dGold >= (m_fEnemy_Level_Cost * m_iEnemy_Level))
		{
			HeroScript.m_dGold -= (m_fEnemy_Level_Cost * m_iEnemy_Level);
			++m_iEnemy_Level;
		}
	}

	public void EnemyAmountUpgrade()
	{
		if(HeroScript.m_dGold >= (m_fEnemy_Amount_Cost * m_iAmount_Enemy_Spawn))
		{
			HeroScript.m_dGold -= (m_fEnemy_Amount_Cost * m_iAmount_Enemy_Spawn);
			++m_iAmount_Enemy_Spawn;
		}
	}

	public void LevelHeroDMG()
	{
		HeroScript.m_fDamage += 1.0f;
		--HeroScript.m_iStatPoints;
	}

	public void LevelHeroHP()
	{
		HeroScript.m_fHealth += 50.0f;
		HeroScript.m_fMax_Health += 50.0f;
		--HeroScript.m_iStatPoints;
	}

	public void LevelHeroHPRegen()
	{
		HeroScript.m_fHealth_Regen += 0.3f;
		--HeroScript.m_iStatPoints;
	}
	public void LevelHeroMP()
	{
		HeroScript.m_fMana += 10.0f;
		HeroScript.m_fMax_Mana += 10.0f;
		--HeroScript.m_iStatPoints;
	}
	public void LevelHeroMPRegen()
	{
		HeroScript.m_fMana_Regen += 0.1f;
		--HeroScript.m_iStatPoints;
	}



}

















