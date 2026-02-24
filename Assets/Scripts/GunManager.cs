using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] Transform gunHolder;
    Transform currentWeapon;
    bool canPickup;
    int selectedWeapon;
    int activeWeaponIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectedWeapon = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canPickup)
        {
            if (gunHolder.childCount < 2)
            {
                PickUpWeapon();
            }
            else
            {
                SwapWeapon();
            }
        }

        SwitchWeapon();

        SwitchWeapon();
    }

    void PickUpWeapon()
    {
        currentWeapon.SetParent(gunHolder, false);
        currentWeapon.localPosition = new Vector3(0.0507964492f, 0f, -0.0487245023f);
        currentWeapon.localRotation = Quaternion.Euler(0, 180, 0);
        currentWeapon.gameObject.SetActive(false);

        Collider col = currentWeapon.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    void SwapWeapon()
    {
        // Drop currently active weapon
        Transform weaponToDrop = gunHolder.GetChild(activeWeaponIndex);
        weaponToDrop.SetParent(null);

        // Enable its collider again so it can be picked up later
        Collider col = weaponToDrop.GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        // Pick up the new weapon
        PickUpWeapon();
    }

    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedWeapon = 1;

        if(gunHolder.childCount >= 2)
        {
            switch (selectedWeapon)
            {
                case 0:
                    gunHolder.GetChild(0).gameObject.SetActive(true);
                    gunHolder.GetChild(1).gameObject.SetActive(false);
                    gunHolder.GetChild(0).gameObject.GetComponent<GunScript>().enabled = true;
                    gunHolder.GetChild(1).gameObject.GetComponent<GunScript>().enabled = false;
                    activeWeaponIndex = 0;
                    break;
                case 1:
                    gunHolder.GetChild(1).gameObject.SetActive(true);
                    gunHolder.GetChild(0).gameObject.SetActive(false);
                    gunHolder.GetChild(1).gameObject.GetComponent<GunScript>().enabled = true;
                    gunHolder.GetChild(0).gameObject.GetComponent<GunScript>().enabled = false;
                    activeWeaponIndex = 1;
                    break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            currentWeapon = other.transform;
            canPickup = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        currentWeapon = null;
        canPickup = false;
    }
}
