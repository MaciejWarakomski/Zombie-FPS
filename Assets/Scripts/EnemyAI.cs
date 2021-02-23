using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float attackDistance = 2.0f;
    public float attackDamage = 10.0f;
    public float attackDelay = 1.0f;
    public float hp = 20.0f;
    public Transform[] transforms;
    public float distance;

    private float timerKill = 0;
    private float timer = 0.0f;
    private Animator animator;
    private bool isPlayer = false;
    private float timerDestroy = 0;
    private bool isDie = false;
    private bool isRage = false;
    private float timerRage = 0;
    private float runSpeed;
    private bool isListening = false;
    private float timerListening = 0;
    private AudioSource audioSource;
    public AudioClip[] footSteps;
    public AudioClip[] attackSounds;
    public AudioClip fallForward;
    public AudioClip fallBack;
    public AudioClip[] hurtSounds;
    public AudioClip[] idleSounds;
    public AudioClip[] attackMouthSounds;
    private float timerIdleSound = 0;
    private float timerAttackMouthSound = 0;
    public GameObject spawnPoint;
    private bool created = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        animator.SetInteger("Transition", 0);
        this.gameObject.transform.Rotate(0, Random.Range(0, 361), 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timerIdleSound = timerIdleSound > 0 ? timerIdleSound - Time.deltaTime : 0;
        timerAttackMouthSound = timerAttackMouthSound > 0 ? timerAttackMouthSound - Time.deltaTime : 0;
        if (timerRage > 0)
        {
            timerRage -= Time.deltaTime;
            if (timerRage < 0)
            {
                timerRage = 0;
                isRage = false;
            }
        }
        if (timerListening > 0)
        {
            timerListening -= Time.deltaTime;
            if (timerListening < 0)
            {
                timerListening = 0;
                isListening = false;
            }
        }
        if (isRage || isListening)
        {
            runSpeed = walkSpeed * 1.5f;
        }
        else
        {
            runSpeed = walkSpeed;
        }
        if (timerKill > 0)
        {
            timerKill -= Time.deltaTime;
            if (timerKill < 0)
            {
                Destroy(GetComponent<Rigidbody>());
                GetComponent<SphereCollider>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                isDie = true;
                timerDestroy = 300;
            }
        }
        if (isDie)
        {
            timerDestroy -= Time.deltaTime;
            isPlayer = false;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 100);
            foreach (Collider other in colliders)
            {
                if (other.tag.Contains("Player"))
                {
                    isPlayer = true;
                }
            }
            if (!isPlayer && !this.gameObject.GetComponent<Renderer>().isVisible)
            {
                Destroy(gameObject);
            }
        }
        if (timerDestroy < 0 && !this.gameObject.GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Contains("Player") && hp > 0)
        {
            distance = Vector3.Distance(transform.position, other.transform.position);
            if (distance > 15 && !isRage && !isListening)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle0"))
                {
                    animator.SetInteger("Transition", 0);
                }
                if(timerIdleSound == 0) Idle();
            }
            if (distance <= 15 || (isListening && distance <= 50) || (isRage && distance <= 100))
            {
                if(timerAttackMouthSound == 0) AttackMouth(); 
                Quaternion targetRotation = Quaternion.LookRotation(other.transform.position - transform.position);
                float oryginalX = transform.rotation.x;
                float oryginalZ = transform.rotation.z;
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, 3f * Time.deltaTime);
                finalRotation.x = oryginalX;
                finalRotation.z = oryginalZ;
                transform.rotation = finalRotation;

                if (distance > attackDistance)
                {
                    animator.SetInteger("Transition", 1);
                    transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
                }
                else if (timer <= 0 && distance < attackDistance - 0.5f)
                {
                    animator.SetInteger("Transition", 2);
                    other.SendMessage("takeHit", attackDamage);
                    other.GetComponentInChildren<EfektTrafienia>().SendMessage("BloodEffect");
                    timer = attackDelay;
                }
                else if (timer <= 0)
                {
                    transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
                }


                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
            }
        }
    }

    void takeHit(float damage)
    {
        Hurt();
        hp -= damage;
        if (hp <= 0)
        {
            animator.SetInteger("Transition", 3);
            timerKill = 0.01f;
        }
        if (hp > 0)
        {
            isRage = true;
            timerRage = 30;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Player") && !isDie && !created)
        {
            created = true;
            GameObject ZombieStaying = Instantiate(spawnPoint, gameObject.transform.position, new Quaternion(0, 0, 0, 0));
            ZombieStaying.transform.parent = gameObject.transform.parent;
            Destroy(gameObject);
        }
    }

    public void MakeNoise()
    {
        isListening = true;
        timerListening = 30;
    }

    private void Step()
    {
        int n = Random.Range(1, footSteps.Length);
        audioSource.clip = footSteps[n];
        audioSource.PlayOneShot(audioSource.clip);
        footSteps[n] = footSteps[0];
        footSteps[0] = audioSource.clip;
    }

    private void Attack()
    {
        int n = Random.Range(1, attackSounds.Length);
        audioSource.clip = attackSounds[n];
        audioSource.PlayOneShot(audioSource.clip);
        attackSounds[n] = attackSounds[0];
        attackSounds[0] = audioSource.clip;
    }

    private void FallBack()
    {
        audioSource.clip = fallBack;
        audioSource.PlayOneShot(audioSource.clip);
    }

    private void FallForward()
    {
        audioSource.clip = fallForward;
        audioSource.PlayOneShot(audioSource.clip);
    }

    private void Hurt()
    {
        int n = Random.Range(1, hurtSounds.Length);
        audioSource.clip = hurtSounds[n];
        audioSource.PlayOneShot(audioSource.clip);
        hurtSounds[n] = hurtSounds[0];
        hurtSounds[0] = audioSource.clip;
    }

    private void Idle()
    {
        timerIdleSound = 10;
        int n = Random.Range(1, idleSounds.Length);
        audioSource.clip = idleSounds[n];
        audioSource.PlayOneShot(audioSource.clip);
        idleSounds[n] = idleSounds[0];
        idleSounds[0] = audioSource.clip;
    }

    private void AttackMouth()
    {
        timerAttackMouthSound = 5;
        int n = Random.Range(1, attackMouthSounds.Length);
        audioSource.clip = attackMouthSounds[n];
        audioSource.PlayOneShot(audioSource.clip);
        attackMouthSounds[n] = attackMouthSounds[0];
        attackMouthSounds[0] = audioSource.clip;
    }
}
