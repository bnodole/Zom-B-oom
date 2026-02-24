using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieScript : MonoBehaviour
{
    public int zombieHealth;
    public int zombieDamage;
    public int zombieSpeed;
    [SerializeField] Transform player;
    Animator zombieController;
    NavMeshAgent agent;
    bool isDead;
    bool isPlayerDead;

    public Slider healthBar;

    public GameObject[] zombieSpawnPoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        zombieSpawnPoints = GameObject.FindGameObjectsWithTag("ZombieSpawnPoints");
        zombieController = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        healthBar.maxValue = zombieHealth;
        healthBar.value = zombieHealth;
        zombieController.applyRootMotion = false;
        int randomSpawnPoints = Random.Range(0, zombieSpawnPoints.Length);
        this.transform.position = zombieSpawnPoints[randomSpawnPoints].transform.position;
        isPlayerDead = PlayerScript.Instance.isDead;
        zombieSpeed = Random.Range(1,5);
        agent.speed = zombieSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        ZombieMovement();
        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        transform.rotation = Quaternion.LookRotation(dir);
        if (isPlayerDead)
            this.GetComponent<ZombieScript>().enabled = false;
    }

    void ZombieMovement()
    {
        if(Vector3.Distance(player.position,this.transform.position) < 2 && !isDead)
        {
            agent.transform.LookAt(player.position);
            zombieController.SetBool("canPunch", true);
            zombieController.SetBool("canRun", false);
        }
        if(Vector3.Distance(player.position, this.transform.position) >= 2 && !isDead)
        {
            zombieController.SetBool("canPunch", false);
            zombieController.SetBool("canRun", true);
            agent.destination = player.position;
        }
        if (isDead)
        {
            agent.destination = this.transform.position;
        }
    }

    public void GetDamage(int damage)
    {
        zombieHealth -= damage;
        healthBar.value = zombieHealth;
        if(zombieHealth <= 0)
        {
            zombieController.SetBool("isDead", true);
            zombieController.SetBool("canRun", false);
            zombieController.SetBool("canPunch", false);
            isDead = true;
            this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            healthBar.gameObject.SetActive(false);
            WaveManager.Instance.remainingZombies--;
        }
    }

    public void DoDamage()
    {
        PlayerScript.Instance.GetDamage(zombieDamage);
    }

    /*private void OnAnimatorMove()
    {
        if(!isDead && !zombieController.GetBool("canPunch"))
        {
            agent.speed = (zombieController.deltaPosition / Time.deltaTime *4).magnitude;
        }
    }*/
}
