using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    int waveNumber;
    public GameObject zombies;
    public Transform zombieHolder;
    int zombieCount;
    public int remainingZombies;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SpawnZombies();
    }

    void SpawnZombies()
    {
        if(remainingZombies <= 0)
        {
            waveNumber++;
            zombieCount += 10;
            remainingZombies = zombieCount;
            for (int i = 0; i < zombieCount; i++)
            {
                GameObject zombie = Instantiate(zombies, zombieHolder);

                ZombieScript zs = zombie.GetComponent<ZombieScript>();

                // Scale stats per wave
                zs.zombieHealth += waveNumber * 100;
                zs.zombieDamage += waveNumber * 10;
            }
        }
    }
}
