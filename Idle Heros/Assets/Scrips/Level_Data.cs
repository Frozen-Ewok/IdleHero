using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level_Data : MonoBehaviour 
{
	public GameObject Enemy;
	public GameObject Hero;

	public int m_iAmount_Enemy_Spawn = 1;
	public int m_iEnemy_Level = 1;

	public float m_fEnemy_Level_Cost = 25;
	public float m_fEnemy_Amount_Cost = 25;

	private float m_fSpawn_Intervial = 20;

	private float m_fSpawn_Timer = 19.9f;

	private GameObject m_goHero;
	private Hero_Data HeroScript;


	// Use this for initialization
	void Start () 
	{
		m_goHero = GameObject.FindGameObjectWithTag("Hero");
		HeroScript = m_goHero.GetComponent<Hero_Data>();

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
				EnemyScript.SetEnemyLevel(m_iEnemy_Level);

				Instantiate(Enemy,EnemyPos, m_goHero.transform.rotation);

				EnemyScript.SetEnemyLevel(m_iEnemy_Level);
				EnemyScript.SetBaseStats(m_iEnemy_Level);
			}
		}
	}
}

















