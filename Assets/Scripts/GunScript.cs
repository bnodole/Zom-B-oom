using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    public int damage;
    public int range;
    public int bullets;
    int defaultbullets;
    Camera camera;

    public Text currentBullets;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem blood;
    [SerializeField] ParticleSystem hole;
    [SerializeField] AudioClip fire;
    [SerializeField] AudioClip reload;
    private void OnEnable()
    {
        Debug.Log(bullets);
        UpdateBulletUI(bullets);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
        defaultbullets = bullets;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPaused) return;
        if (PlayerScript.Instance.isDead) return;
        Shoot();
        Reload();
    }
    
    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && bullets >0)
        {
            GameManager.Instance.ShootSound(fire);
            muzzleFlash.Play();
            bullets--;
            UpdateBulletUI(bullets);
            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, range))
            {
                Debug.Log(hit.collider.name);
                ZombieScript zombie = hit.collider.GetComponentInParent<ZombieScript>();
                if (zombie != null)
                {
                    zombie.GetDamage(damage);
                    ParticleSystem bloodInstance = Instantiate(blood, hit.point, Quaternion.LookRotation(hit.normal));
                    if(bloodInstance != null)
                    {
                        bloodInstance.Play();
                        Destroy(bloodInstance.gameObject, 2f);
                    }
                }
                else
                {
                    ParticleSystem holeInstance = Instantiate(hole, hit.point, Quaternion.LookRotation(hit.normal));
                    if(holeInstance != null)
                    {
                        holeInstance.Play();
                        Destroy(holeInstance.gameObject, 5f);
                    }
                }
            }
        }
        Debug.DrawRay(camera.transform.position, camera.transform.forward *range, Color.green);
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && bullets < defaultbullets)
        {
            GameManager.Instance.ReloadSound(reload);
            bullets = defaultbullets;
            UpdateBulletUI(bullets);
        }
    }

    void UpdateBulletUI(int bullets)
    {
        currentBullets.text = bullets.ToString();
    }

    
}
