using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponHolder : MonoBehaviour {
    public List<Weapon> Weapons;
    public Weapon ActiveWeapon;

    private const string WEAPONS_FOLDER = "prefabs/weapons";

	private void Start () {
        Weapons = Resources.LoadAll<GameObject>(WEAPONS_FOLDER)
            .Select(w => Instantiate(w).GetComponent<Weapon>())
            .ToList();

        Weapons.ForEach(w => {
            w.transform.parent = transform;
            w.transform.localPosition = Vector3.zero;
            w.gameObject.SetActive(false);
        });
    }

	private void Update () {
		
	}

    public void SetWeapon(PartialWeapon weapon)
    {
        if (ActiveWeapon?.Data.Name == weapon.Name) return;

        ActiveWeapon?.Deactivate();

        var newWeapon = Weapons.SingleOrDefault(w => w.Data.Name == weapon.Name);

        if (newWeapon == null) {
            ActiveWeapon?.Activate();
            return;
        }

        ActiveWeapon = newWeapon;

        ActiveWeapon.Activate();
    }
}
