using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Instance;
    Rigidbody playerRigidbody;
    Vector3 velocity;
    public int speed;
    public bool canJump = true;

    public float health = 200f;
    public Image healthBar;
    public RawImage bloodScreen;
    public Image death;
    public bool isDead;

    public Text timerText;
    float survivalTime = 0f;
    float highScore;

    private void Awake()
    {
        Instance = this;    
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 3;
        playerRigidbody = GetComponent<Rigidbody>();
        highScore = PlayerPrefs.GetFloat("Highscore");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Timer();
    }

    void Timer()
    {
        if (!isDead)
        {
            survivalTime += Time.deltaTime;

            timerText.text = "Time: " + survivalTime.ToString("F1") + "s";
        }
    }

    void Movement()
    {
        //Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = horizontal*transform.right + vertical * transform.forward;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            move *= 2;
        }
        velocity = move * speed;
        velocity.y = playerRigidbody.linearVelocity.y;
        playerRigidbody.linearVelocity = velocity;

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            playerRigidbody.AddForce(Vector3.up * 5, ForceMode.Impulse);
            canJump = false;
        }
    }

    public void GetDamage(int damage)
    {
        health -= damage;
        UpdateHealthBar(health);
        StartCoroutine(FlashBloodScreen());
        if(health <= 0)
        {
            Death();
        }
    }

    void UpdateHealthBar(float health)
    {
        health = Mathf.Clamp(health, 0f, 200f);
        healthBar.fillAmount = health / 200;
    }

    void Death()
    {
        isDead = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("You Died");
        if(survivalTime > highScore)
        {
            highScore = survivalTime;
            PlayerPrefs.SetFloat("Highscore", highScore);
            PlayerPrefs.Save();
        }
        StartCoroutine(AfterDeath());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boundary"))
            Death();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            canJump = true;
    }
    IEnumerator FlashBloodScreen()
    {
        bloodScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        bloodScreen.gameObject.SetActive(false);
    }
    IEnumerator AfterDeath()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 90f), 3);
        death.GetComponent<Animator>().enabled = true;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 0f;
    }

}
