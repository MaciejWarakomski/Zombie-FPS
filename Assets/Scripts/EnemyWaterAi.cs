using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaterAi : MonoBehaviour
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
    private float runSpeed;
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


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        animator.SetInteger("Transition", 0);
        this.gameObject.transform.Rotate(0, Random.Range(0, 361), 0);
        runSpeed = walkSpeed * 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        timerIdleSound = timerIdleSound > 0 ? timerIdleSound - Time.deltaTime : 0;
        timerAttackMouthSound = timerAttackMouthSound > 0 ? timerAttackMouthSound - Time.deltaTime : 0;
        if (timerAttackMouthSound == 0 && hp > 0) AttackMouth();
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
            Quaternion targetRotation = Quaternion.LookRotation(other.transform.position - transform.position);
            float oryginalX = transform.rotation.x;
            float oryginalZ = transform.rotation.z;
            Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, 3f * Time.deltaTime);
            finalRotation.x = oryginalX;
            finalRotation.z = oryginalZ;
            transform.rotation = finalRotation;

            if(distance <= 25)
            {
                runSpeed = walkSpeed * 1.5f;
            }
            else
            {
                runSpeed = walkSpeed * 2.5f;
            }

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
            else
            {
                timer -= Time.deltaTime;
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
    }

    private void Step()
    {
        int n = Random.Range(1, footSteps.Length);
        audioSource.clip = footSteps[n];
        audioSource.PlayOneShot(audioSource.clip);
        // move picked sound to index 0 so it's not picked next time
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
