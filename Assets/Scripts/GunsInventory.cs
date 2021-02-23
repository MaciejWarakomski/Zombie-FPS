using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunsInventory : MonoBehaviour
{
    public GameObject[] gunsList = new GameObject[10];
    public TMP_Text grabWeapon;
    public int[] canAddAmmo = new int[3];
    public bool[] guns = new bool[] { false, true, false, false, false, false, false, false, false, false };
    public int maxGuns = 1;
    public int currentGun = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(maxGuns == 2)
        {
            maxGuns = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && guns[1])
        {
            changeWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && guns[2])
        {
            changeWeapon(2);
        }
    }

    public void addGun(int number)
    {
        maxGuns++;
        guns[number] = true;
        grabWeapon.text = null;
        changeWeapon(number);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Grab"))
        {
            grabWeapon.text = "Press E to grab";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Grab"))
        {
            grabWeapon.text = null;
        }
    }

    public void addAmmo(Vector2 param)
    {
        GameObject gun = gunsList[(int)param.y];

        gun.SetActive(true);
        gun.SendMessage("addAmmo", param);
        if (param.y != currentGun)
        {
            gun.SetActive(false);
        }
    }

    void CanAddAmmo(Vector2 addAmmo)
    {
        canAddAmmo[(int)addAmmo.x] = (int)addAmmo.y;
    }

    void changeWeapon(int num)
    {
        currentGun = num;
        for (int i = 1; i < maxGuns; i++)
        {
            if(i == num)
            {
                gunsList[i].SetActive(true);
            }
            else
            {
                gunsList[i].SetActive(false);
            }
        }
    }
}
