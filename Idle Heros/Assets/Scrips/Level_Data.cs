using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level_Data : MonoBehaviour 
{

	public static Level_Data Level_Info;
	public GameObject Enemy;
	public GameObject Hero;

	public int m_iAmount_Enemy_Spawn = 1;
	public int m_iEnemy_Level = 1;

	public float m_fEnemy_Level_Cost = 25;
	public float m_fEnemy_Amount_Cost = 25;

	public float m_fSpawn_Intervial = 20;

	public float m_fSpawn_Timer = 19.9f;

	private Hero_Data HeroScript;


	// Use this for initialization
	void Start () 
	{
		if(Level_Info == null)
		{
			Level_Info = this;
		}
		else if(Level_Info != this)
		{
			Destroy(gameObject);
		}

		HeroScript = Hero_Data.Hero.GetComponent<Hero_Data>();

		Game_Info.GameInfo.LoadLevel();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if the hero dies reset the level and give the hero full hp and mana
		if(HeroScript.m_fHealth < 0)
		{
			HeroScript.FullHeal();

			m_iAmount_Enemy_Spawn = 1;
			m_iEnemy_Level = 1;
		}

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
		if(Hero_Data.Hero.transform)
		{
			for(int i = 0; i < m_iAmount_Enemy_Spawn; ++i)
			{
				float fHeroX = Hero_Data.Hero.transform.position.x;
				float fHeroZ = Hero_Data.Hero.transform.position.z;

				Vector3 EnemyPos = Hero_Data.Hero.transform.position;

				EnemyPos.x = Random.Range (fHeroX - 50, fHeroX + 50);
				EnemyPos.z = Random.Range (fHeroZ - 50, fHeroZ + 50);

				Enemy_Data EnemyScript = Enemy.GetComponent<Enemy_Data>();
				EnemyScript.SetEnemyLevel(m_iEnemy_Level);

				Instantiate(Enemy,EnemyPos, Hero_Data.Hero.transform.rotation);

				EnemyScript.SetEnemyLevel(m_iEnemy_Level);
				EnemyScript.SetBaseStats(m_iEnemy_Level);
			}
		}
	}
}

















