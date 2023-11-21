using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    [Header("Player Health and Damage")]
    private float PlayerHealth = 120f;
    private float presentHealth;
    public float giveDamage = 5f;
    public float PlayerSpeed;


    [Header("Player Things")]
    public UnityEngine.AI.NavMeshAgent PlayerAgent;
    public Transform LookPoint;
    public GameObject ShootingRaycastArea;
    public Transform enemyBody;
    public LayerMask enemyLayer;
    public Transform Spawn;
    public Transform PlayerCharacter;



    [Header("Player Shooting Var")]
    public float timebtwShoot;
    bool previouslyShoot;


    [Header("Player Animation and Spark effect")]
    public Animator anim;
    public ParticleSystem muzzleSpark;

    [Header("Player States")]
    public float visionRadius;
    public float shootingRadius;
    public bool enemyInvisionRadius;
    public bool enemyInshootingRadius;


    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootingSound;


    public  ScoreManager scoreManager;


    private void Awake()
    {
        PlayerAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        presentHealth = PlayerHealth;
    }

    private void Update()
    {
        enemyInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, enemyLayer);
        enemyInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, enemyLayer);

        if(enemyInvisionRadius && !enemyInshootingRadius) PursueEnemy();
        if(enemyInvisionRadius && enemyInshootingRadius) ShootEnemy();
    }

    private void PursueEnemy()
    {
        if(PlayerAgent.SetDestination(enemyBody.position))
        {
            anim.SetBool("Running",true);
            anim.SetBool("Shooting",false);
        }
        else
        {
            anim.SetBool("Running",false);
            anim.SetBool("Shooting",false);
        }
    }

    private void ShootEnemy()
    {
        PlayerAgent.SetDestination(transform.position);

        transform.LookAt(LookPoint);

        if(!previouslyShoot)
        {
            muzzleSpark.Play();
            audioSource.PlayOneShot(shootingSound);

            RaycastHit hit;

            if(Physics.Raycast(ShootingRaycastArea.transform.position, ShootingRaycastArea.transform.forward, out hit, shootingRadius))
            {
                Debug.Log("Shooting" + hit.transform.name);

                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if(enemy != null)
                {
                    enemy.enemyHitDamage(giveDamage);
                }
                anim.SetBool("Running",true);
                anim.SetBool("Shooting",false);
            }
            previouslyShoot = true;
            Invoke(nameof(ActiveShooting), timebtwShoot);
        }
    
    
    }

    private void ActiveShooting()
    {
        previouslyShoot = false;
    }

    // Player hit damage
    public void PlayerAIHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        if(presentHealth <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        PlayerAgent.SetDestination(transform.position);
        PlayerSpeed = 0f;
        shootingRadius = 0f;
        visionRadius = 0f;
        enemyInvisionRadius = false;
        enemyInshootingRadius = false;
        anim.SetBool("Die",true);
        anim.SetBool("Running",false);
        anim.SetBool("Shooting",false);

        // animations
        Debug.Log("Dead");
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        scoreManager.enemyKills += 1;

        yield return new WaitForSeconds(5f);
 
        Debug.Log("Spawn");
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        presentHealth = 120f; 
        PlayerSpeed = 2f;
        shootingRadius = 10f;
        visionRadius = 100f;
        enemyInvisionRadius = true;
        enemyInshootingRadius = false;

        // animations
        anim.SetBool("Die",false);
        anim.SetBool("Running",true);
        // spawn point
        PlayerCharacter.transform.position = Spawn.transform.position;
        PursueEnemy();
    }
}
