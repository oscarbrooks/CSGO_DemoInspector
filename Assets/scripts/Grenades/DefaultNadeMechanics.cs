using UnityEngine;

public class DefaultNadeMechanics : NadeMechanics, INadeMechanics {

    private NadeProjectileGraphics _nadeProjectile;

    private GameObject _visuals;

    public DefaultNadeMechanics(NadeProjectileGraphics nadeProjectile, GameObject visuals)
    {
        _nadeProjectile = nadeProjectile;

        _visuals = visuals;

        Rotation = GetRandomRotation();
    }

    public void Update()
    {
        //Debug.Log($"Updating {_nadeProjectile.name}");
        if (Velocity.magnitude > 0) _visuals.transform.Rotate(Rotation);
    }
}
