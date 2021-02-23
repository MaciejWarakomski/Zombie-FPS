using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class Shooting : MonoBehaviour
{
    public Texture2D crosshairTexture;
    public AudioClip pistolShot;
    public AudioClip reloadSound;
    public int maxAmmo = 200;
    public int clipSize = 10;
    public TMP_Text ammoText;
    public TMP_Text reloadText;
    public float reloadTime = 2.0f;
    public int currentAmmo = 30;
    public bool automatic = false;
    public float shotDelay = 0.5f;
    public GameObject bulletHole;
    public GameObject bloodParticles;
    public float damage = 5.0f;
    public int gunType;

    private GameObject cameraVector;
    private int currentClip;
    private Rect position;
    private float range = 200.0f;
    private Vector3 fwd;
    private Vector3 bwd;
    private RaycastHit hit;
    private AudioSource audioSource;
    public bool isReloading = false;
    private float timer = 0.0f;
    private float shotDelayCounter = 0.0f;
    private float zoomFieldOfView = 40.0f;
    private float defaultFieldOfView = 60.0f;
    private Vector3 cameraPosition;
    private int canGetAmmo;
    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height - crosshairTexture.height) / 2, crosshairTexture.width, crosshairTexture.height);
        currentClip = clipSize;
        cameraVector = GameObject.Find("FirstPersonCharacter");
        CanGetAmmo();
        GetComponentInParent<GunsInventory>().SendMessage("CanAddAmmo", new Vector2(gunType, canGetAmmo));
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1f)
        {
            if (paused)
            {
                audioSource.UnPause();
                paused = false;
            }
            fwd = cameraVector.transform.TransformDirection(Vector3.forward);
            bwd = cameraVector.transform.TransformDirection(Vector3.back);
            cameraPosition = cameraVector.transform.position;
            if (currentClip > 0 && !isReloading)
            {
                if ((Input.GetButtonDown("Fire1") || (Input.GetButton("Fire1") && automatic)) && shotDelayCounter <= 0)
                {
                    shotDelayCounter = shotDelay;
                    Fire();
                    Collider[] collidersNoise = Physics.OverlapSphere(transform.position, 50);
                    foreach (Collider other in collidersNoise)
                    {
                        if (other.tag.Contains("Listening"))
                        {
                            other.GetComponent<EnemyAI>().SendMessage("MakeNoise");
                        }
                    }
                    if (Physics.Raycast(cameraPosition, fwd, out hit))
                    {
                        if (hit.transform.tag.Contains("Enemy") && hit.distance < range)
                        {
                            hit.transform.gameObject.SendMessage("takeHit", damage);
                            GameObject go;
                            go = Instantiate(bloodParticles, hit.point, Quaternion.LookRotation(bwd)) as GameObject;
                            Destroy(go, 0.5f);
                        }
                        else if (hit.distance < range && hit.transform.tag == "Wall")
                        {
                            GameObject go;
                            go = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
                            go.transform.LookAt(hit.point + hit.normal);
                            Destroy(go, 10);
                        }
                    }
                    currentClip--;
                }
            }

            if (shotDelayCounter > 0)
            {
                shotDelayCounter -= Time.deltaTime;
            }

            if (Input.GetButtonDown("Reload") && currentClip < clipSize)
            {
                if (currentAmmo > 0 && (timer > reloadTime || timer == 0))
                {
                    audioSource.PlayOneShot(reloadSound);
                    isReloading = true;
                }
            }

            if (isReloading)
            {
                timer += Time.deltaTime;
                if (timer >= reloadTime)
                {
                    int needAmmo = clipSize - currentClip;
                    if (currentAmmo >= needAmmo)
                    {
                        currentClip = clipSize;
                        currentAmmo -= needAmmo;
                    }
                    else
                    {
                        currentClip += currentAmmo;
                        currentAmmo = 0;
                    }
                    isReloading = false;
                    timer = 0.0f;
                    CanGetAmmo();
                    GetComponentInParent<GunsInventory>().SendMessage("CanAddAmmo", new Vector2(gunType, canGetAmmo));
                }
            }

            if (gameObject.GetComponentInParent<Camera>() is Camera)
            {
                Camera cam = gameObject.GetComponentInParent<Camera>();
                if (Input.GetButton("Fire2"))
                {
                    if (cam.fieldOfView > zoomFieldOfView)
                    {
                        cam.fieldOfView--;
                    }
                }
                else
                {
                    if (cam.fieldOfView < defaultFieldOfView)
                    {
                        cam.fieldOfView++;
                    }
                }
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                paused = true;
            }
        }
    }

    void Fire()
    {
        var systems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem system in systems)
        {
            system.Play();
        }

        audioSource.PlayOneShot(pistolShot);
    }

    void OnGUI()
    {
        if (Time.timeScale == 1f)
        {
            GUI.DrawTexture(position, crosshairTexture);
        }
        ammoText.text = currentClip.ToString() + " / " + currentAmmo.ToString() + "  |  " + GetComponentInParent<GrenadeThrow>().currentGrenades.ToString() + " / 2";
        if (currentAmmo > 0)
        {
            if (currentClip == 0 && !isReloading)
            {
                reloadText.text = "Press R to reload!";
            }
            else
            {
                reloadText.text = null;
            }
        }
        else if(currentClip == 0)
        {
            reloadText.text = "You need to find ammo!";
        }
    }

    void CanGetAmmo()
    {
        if (currentAmmo == maxAmmo)
        {
            canGetAmmo = 0;
        }
        else
        {
            canGetAmmo = 1;
        }
    }

    void addAmmo(Vector2 data)
    {
        int ammoToAdd = (int)data.x;

        if (maxAmmo - currentAmmo >= ammoToAdd)
        {
            currentAmmo += ammoToAdd;
        }
        else
        {
            currentAmmo = maxAmmo;
        }
        CanGetAmmo();
        GetComponentInParent<GunsInventory>().SendMessage("CanAddAmmo", new Vector2(gunType, canGetAmmo));
    }
}
