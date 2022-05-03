using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    //[SerializeField] private Transform debugTransform;
    [SerializeField] private Transform projectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private float bulletForce;
    [SerializeField] private float upForce;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image crossHair;
    [SerializeField] private GameObject gameOver;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;

    public float health;
    public float damage;

    Bomb bomb;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();

        bomb = GameObject.Find("Bomb").GetComponent<Bomb>();
        gameOver.SetActive(false);

        healthBar.maxValue = health;
        healthBar.value = health;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            //debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            crossHair.enabled = true;

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }

        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            crossHair.enabled = false;
        }

        if (starterAssetsInputs.shoot)
        {
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(projectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));

            starterAssetsInputs.shoot = false;
        }

        if (starterAssetsInputs.exitGame)
        {
            Application.Quit();
            Debug.Log("Quit!");
        }

        healthBar.value = health;

        if (health <= 0)
        {
            Destroy(gameObject);
            gameOver.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Fire"))
        {
            health -= bomb.fireDamage;
        }
    }
}
