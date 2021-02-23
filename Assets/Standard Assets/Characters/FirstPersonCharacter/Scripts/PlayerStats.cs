using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float maxHealth = 100;
    public float currentHealth = 100;
    private float maxArmour = 100;
    public float currentArmour = 100;
    private float maxStamina = 100;
    private float currentStamina = 100;

    private float barWidth = 100;
    private float barHeight = 100;

    private float canHeal = 0.0f;
    private float canRegenerate = 0.0f;

    private CharacterController chCont;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsC;
    private Vector3 lastPosition;

    public Texture2D healthTexture;
    public Texture2D armourTexture;
    public Texture2D staminaTexture;
    public GameObject sendGameOver;

    private void Awake()
    {
        barHeight = Screen.height * 0.02f;
        barWidth = barHeight * 10.0f;

        chCont = GetComponent<CharacterController>();
        fpsC = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();

        lastPosition = transform.position;
    }


    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10, Screen.height - barHeight - 10, currentHealth * barWidth / maxHealth, barHeight), healthTexture);
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10, Screen.height - barHeight * 2 - 20, currentArmour * barWidth / maxArmour, barHeight), armourTexture);
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10, Screen.height - barHeight * 3 - 30, currentStamina * barWidth / maxStamina, barHeight), staminaTexture);
    }

    void takeHit(float damage)
    {
        
        if (currentArmour > 0)
        {
            currentArmour -= damage;
            if (currentArmour < 0)
            {
                currentHealth += currentArmour;
                currentArmour = 0;
            }
        }
        else
        {
            currentHealth -= damage;
        }

        if (currentHealth < maxHealth)
        {
            canHeal = 5.0f;
        }

        currentArmour = Mathf.Clamp(currentArmour, 0, maxArmour);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    void regenerate(ref float currentStat, float maxStat)
    {
        currentStat += maxStat * 0.002f;
        Mathf.Clamp(currentStat, 0, maxStat);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            sendGameOver.SendMessage("GameOver");
        }
        if(canHeal > 0.0f)
        {
            canHeal -= Time.deltaTime;
        }
        if (canRegenerate > 0.0f)
        {
            canRegenerate -= Time.deltaTime;
        }

        if (canHeal <= 0.0f && currentHealth < maxHealth)
        {
            regenerate(ref currentHealth, maxHealth);
        }
        if(canRegenerate <= 0.0f && currentStamina < maxStamina)
        {
            regenerate(ref currentStamina, maxStamina);
        }
    }

    void FixedUpdate()
    {

        if(Input.GetKey(KeyCode.LeftShift) && lastPosition != transform.position && currentStamina > 0)
        {
            lastPosition = transform.position;
            currentStamina -= 0.3f;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            canRegenerate = 5.0f;
        }

        if (currentStamina > 0)
        {
            fpsC.CanRun = true;
        }
        else
        {
            fpsC.CanRun = false;
        }
    }
    
    public void addKevlar()
    {
        if(currentArmour < 50)
        {
            currentArmour += 50;
        }
        else
        {
            currentArmour = maxArmour;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Water"))
        {
            canHeal = 60;
            currentHealth = 0;
        }

        if (other.tag.Contains("Win"))
        {
            sendGameOver.SendMessage("Win");
        }
    }
}
