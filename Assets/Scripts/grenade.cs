using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{
    public float toExplode = 2.0f;
    public GameObject explosion;
    public AudioClip explosionSound;
    public AudioSource explosionHolder;
    public float maximumDamage = 20;
    private bool created = false;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        toExplode -= Time.deltaTime;

        if (toExplode < 0)
        {
            if (!created)
            {
                gameObject.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
                GameObject clone = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
                AudioSource explosionS = Instantiate(explosionHolder, transform.position, transform.rotation, clone.transform) as AudioSource;
                explosionS.clip = explosionSound;
                explosionS.Play();
                StartCoroutine(DestroyObject(clone));
                StartCoroutine(DestroyObject(gameObject));
                Collider[] colliders = Physics.OverlapSphere(transform.position, 13);
                foreach (Collider other in colliders)
                {
                    if (other.tag.Contains("Enemy"))
                    {
                        distance = Vector3.Distance(transform.position, other.transform.position);
                        if (distance > 0 && distance <= 3)
                        {
                            other.transform.gameObject.SendMessage("takeHit", maximumDamage);
                        }
                        else if (distance > 3 && distance <= 6)
                        {
                            other.transform.gameObject.SendMessage("takeHit", 0.75 * maximumDamage);
                        }
                        else if (distance > 6 && distance <= 9)
                        {
                            other.transform.gameObject.SendMessage("takeHit", 0.5 * maximumDamage);
                        }
                        else if (distance > 9 && distance <= 12)
                        {
                            other.transform.gameObject.SendMessage("takeHit", 0.25 * maximumDamage);
                        }
                    }
                    if (other.tag.Contains("Player") && other.GetComponent<PlayerStats>().currentHealth > 0)
                    {
                        distance = Vector3.Distance(transform.position, other.transform.position);
                        if (distance > 0 && distance <= 3)
                        {
                            other.transform.gameObject.SendMessage("takeHit", maximumDamage);
                            other.transform.gameObject.GetComponentInChildren<EfektTrafienia>().SendMessage("BloodEffect");
                        }
                        else if (distance > 3 && distance <= 6)
                        {
                            other.transform.gameObject.SendMessage("takeHit", 0.75 * maximumDamage);
                            other.transform.gameObject.GetComponentInChildren<EfektTrafienia>().SendMessage("BloodEffect");
                        }
                        else if (distance > 6 && distance <= 9)
                        {
                            other.transform.gameObject.SendMessage("takeHit", 0.5 * maximumDamage);
                            other.transform.gameObject.GetComponentInChildren<EfektTrafienia>().SendMessage("BloodEffect");
                        }
                        else if (distance > 9 && distance <= 12)
                        {
                            other.transform.gameObject.SendMessage("takeHit", 0.25 * maximumDamage);
                            other.transform.gameObject.GetComponentInChildren<EfektTrafienia>().SendMessage("BloodEffect");
                        }
                    }
                }
                Collider[] collidersNoise = Physics.OverlapSphere(transform.position, 50);
                foreach (Collider other in collidersNoise)
                {
                    if (other.tag.Contains("Listening"))
                    {
                        other.GetComponent<EnemyAI>().SendMessage("MakeNoise");
                    }
                }
                created = true;
            }
        }
    }
    IEnumerator DestroyObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
    }
}
