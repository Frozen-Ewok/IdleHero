using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUD_Info : MonoBehaviour 
{
	private GameObject m_goLevelSpawner;
	private Level_Data m_LevelScript;
	
	private GameObject m_goHero;
	private Hero_Data HeroScript;
	
	public Text m_textHeroStats;
	public Text m_textLevelInfo;
	
	public Text m_textEnemyLevel;
	public Text m_textEnemyAmount;
	
	public Text m_textAvailableStatPoints;

	public Text m_textFullHealth;
	public Text m_textFullMana;

	public Text m_textEnemyInfo;

	public Image m_imageStats;

	// Use this for initialization
	void Start () 
	{
		m_goHero = GameObject.FindGameObjectWithTag("Hero");
		HeroScript = m_goHero.GetComponent<Hero_Data>();
		
		m_goLevelSpawner = GameObject.FindGameObjectWithTag("Spawner");
		m_LevelScript = m_goLevelSpawner.GetComponent<Level_Data>();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!m_goHero)
		{
			m_goHero = GameObject.FindGameObjectWithTag("Hero");
			HeroScript = m_goHero.GetComponent<Hero_Data>();
		}
		
		if(!m_goLevelSpawner)
		{
			m_goLevelSpawner = GameObject.FindGameObjectWithTag("Spawner");
			m_LevelScript = m_goLevelSpawner.GetComponent<Level_Data>();
		}
		
		UpdateHUD();	
	}
	
	void UpdateHUD()
	{
		m_textEnemyLevel.text = "Increase Enemy Level: " + (m_LevelScript.m_fEnemy_Level_Cost * m_LevelScript.m_iEnemy_Level) + " Gold";
		m_textEnemyAmount.text = "Increase Enemy Amount: " + (m_LevelScript.m_fEnemy_Amount_Cost * m_LevelScript.m_iAmount_Enemy_Spawn) + " Gold";
			
		m_textLevelInfo.text = "Level Infomation \n";
		m_textLevelInfo.text += "Enemy Level: " + m_LevelScript.m_iEnemy_Level + "\n";
		m_textLevelInfo.text += "Enemies Per Wave: " + m_LevelScript.m_iAmount_Enemy_Spawn + "\n";

		//Fill in the hero info
		m_textHeroStats.text = "Hero Stats \n";
		m_textHeroStats.text += "Level: " + HeroScript.m_iLevel + "\n";
		m_textHeroStats.text += "Gold: " +  HeroScript.m_dGold.ToString("F1") + "\n";
		m_textHeroStats.text += "DPS: " +  (HeroScript.m_fDamage / HeroScript.m_fAttack_Speed) + "\n";
		m_textHeroStats.text += "HP " + HeroScript.m_fHealth.ToString("F1") + "/" + HeroScript.m_fMax_Health + "\n";
		m_textHeroStats.text += "HP Regen +" + HeroScript.m_fHealth_Regen.ToString("F1") + "/s \n";
		m_textHeroStats.text += "MP " + HeroScript.m_fMana.ToString("F1") + "/" + HeroScript.m_fMax_Mana +"\n";
		m_textHeroStats.text += "MP Regen +" + HeroScript.m_fMana_Regen.ToString("F1") + "/s \n";
		m_textHeroStats.text += "EXP: \n"+ HeroScript.m_iCurrnet_Exp.ToString("F0") + " / " + HeroScript.m_iExp_To_Level.ToString("F0") + "\n";
		
		m_textAvailableStatPoints.text = "Stat Points: " + HeroScript.m_iStatPoints;

		float fHealthCost = HeroScript.m_fMax_Health - HeroScript.m_fHealth;
		float fManaCost = HeroScript.m_fMax_Mana - HeroScript.m_fMana;

		m_textFullHealth.text = "Buy full health heal: " + fHealthCost.ToString("F1") + " Gold";
		m_textFullMana.text = "Buy full mana heal: " + fManaCost.ToString("F1") + " Gold";

		if(HeroScript.m_iStatPoints >= 1)
		{
			m_imageStats.gameObject.SetActive(true);
		}
		else
		{
			m_imageStats.gameObject.SetActive(false);
		}
	}
	
	public void EnemyUpgrade()
	{
		if(HeroScript.m_dGold >= (m_LevelScript.m_fEnemy_Level_Cost * m_LevelScript.m_iEnemy_Level))
		{
			HeroScript.m_dGold -= (m_LevelScript.m_fEnemy_Level_Cost * m_LevelScript.m_iEnemy_Level);
			++m_LevelScript.m_iEnemy_Level;
		}
	}
	
	public void EnemyAmountUpgrade()
	{
		if(HeroScript.m_dGold >= (m_LevelScript.m_fEnemy_Amount_Cost * m_LevelScript.m_iAmount_Enemy_Spawn))
		{
			HeroScript.m_dGold -= (m_LevelScript.m_fEnemy_Amount_Cost * m_LevelScript.m_iAmount_Enemy_Spawn);
			++m_LevelScript.m_iAmount_Enemy_Spawn;
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

	public void BuyFullHealth()
	{
		float fCost = HeroScript.m_fMax_Health - HeroScript.m_fHealth;
		if(HeroScript.m_dGold > fCost)
		{
			HeroScript.m_fHealth = HeroScript.m_fMax_Health;
			HeroScript.m_dGold -= fCost;
		}
	}
	public void BuyFullMana()
	{
		float fCost = HeroScript.m_fMax_Mana - HeroScript.m_fMana;
		if(HeroScript.m_dGold > fCost)
		{
			HeroScript.m_fMana = HeroScript.m_fMax_Mana;
			HeroScript.m_dGold -= fCost;
		}
	}
		
}
