using UnityEngine;

public class SmokeNadeMechanics : NadeMechanics, INadeMechanics {

    private NadeProjectileGraphics _nadeProjectile;

    private GameObject _visuals;

    public SmokeNadeMechanics(NadeProjectileGraphics nadeProjectile, GameObject visuals)
    {
        _nadeProjectile = nadeProjectile;

        _visuals = visuals;

        Rotation = GetRandomRotation();
    }
	
	public void Update () {
        if (Velocity.magnitude > 0) _visuals.transform.Rotate(Rotation);
    }
}
