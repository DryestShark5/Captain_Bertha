using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RangedAttack : MonoBehaviour
{
    //Weapon

    //Bullet
    public GameObject bullet;

    //Bullet force
    public float shootForce, upwardForce;

    //Gun Stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletPerTap;
    public bool allowButtonHold;

    int bulletLeft, bulletsShot;

    //Bools
    bool shooting, readyToShoot, reloading;

    //Refrence
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammonitionDisplay;

    //Bug fixing
    public bool allowInvoke = true;

    private void Awake()
    {
        //Make sure magazine is full
        bulletLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //Set ammo display
        if (ammonitionDisplay != null)
            ammonitionDisplay.SetText(bulletLeft / bulletPerTap + " / " + magazineSize / bulletPerTap);
    }

    private void MyInput()
    {
        //Check if allows to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletLeft < magazineSize && !reloading) Reload();
        //Auto reload when magazine empty
        if (readyToShoot && shooting && !reloading && bulletLeft <= 0) Reload();

        //shooting
        if (readyToShoot && shooting && !reloading && bulletLeft > 0)
        {
            //Set bullets shots to 0
            bulletsShot = 0;

            shoot();
        }

    }
    private void shoot()
    {
        readyToShoot = false;

        //find the exact hiy position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Ray to middle of screen
        RaycastHit hit;

        //check if ray hits something 
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Hit point far away from player

        //Calculate direction from attackPoint to tragetPoint
        Vector3 directionWithoutSpreak = targetPoint - attackPoint.position;

        //Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpreak + new Vector3(x, y, 0);

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        //Rotate bullet to shoot direction
        currentBullet.transform.position = directionWithSpread.normalized;

        //Add force to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, is you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletLeft--;
        bulletsShot++;

        //Invoke resetShot function
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //If more than one bulletPerTap
        if (bulletsShot < bulletPerTap && bulletLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        //Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletLeft = magazineSize;
        reloading = false;
    }
}
