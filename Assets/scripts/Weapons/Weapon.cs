using UnityEngine;

public class Weapon : MonoBehaviour {
    public WeaponData Data;

    public Weapon()
    {
    }

    public virtual void Activate() {
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public virtual void Fire()
    {

    }
}
