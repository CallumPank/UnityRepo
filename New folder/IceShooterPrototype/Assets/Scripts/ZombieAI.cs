using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombieAI : MonoBehaviour
{
	public float speed;
    private float maxSpeed;
    NavMeshAgent agent;
    GameObject[] Players;
    bool Attacked;
    public float AttackTime = 1f;
    private float InitAttackTime;
    ZombieManager zombieManager;

    // Use this for initialization
    void Start()
	{
        zombieManager = GameObject.Find("ZombieSpawns").GetComponent<ZombieManager>();
		agent = GetComponent<NavMeshAgent>();
        maxSpeed = 5f;
        agent.speed = speed;       
        InitAttackTime = AttackTime;
        Attacked = false;
        if (speed > maxSpeed) { speed = maxSpeed; }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !Attacked)
        {
            Attacked = true;
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
            playerHealth.TakeDamage(15.0f);
            
        }
    }

    GameObject ClosestPlayer()
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        for (int i = 0; i < Players.Length; i++)
        {
            // Find the distance to this player
            Vector3 diff = Players[i].transform.position - transform.position;
            float playerDistance = diff.sqrMagnitude;

            // if the distance to this player is less than the previous player checked set this player as the closest
            if (playerDistance < distance)
            {
                closest = Players[i];
                distance = playerDistance;
            }
        }
        return closest;
    }

    // Update is called once per frame
    void Update()
	{
        Players = GameObject.FindGameObjectsWithTag("Player");
        agent.SetDestination(ClosestPlayer().transform.position);
        print(speed);
        // Attacking delay
        if (Attacked)
        {
            AttackTime -= Time.deltaTime;
        }
        if (AttackTime <= 0.0f)
        {
            Attacked = false;
            AttackTime = InitAttackTime;
        }
        //print("Attacked = " + Attacked);
    }

    public void OnDestroy()
    {
        //zombieManager.EnemyCount--;
    }
}
