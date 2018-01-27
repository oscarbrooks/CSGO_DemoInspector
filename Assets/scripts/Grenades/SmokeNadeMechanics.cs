using UnityEngine;

public class SmokeNadeMechanics : NadeMechanics, INadeMechanics {

    private NadeProjectileGraphics _nadeProjectile;

    public void Initialize(NadeProjectileGraphics nadeProjectile)
    {
        _nadeProjectile = nadeProjectile;

        Rotation = GetRandomRotation();
    }
	
	public void Update () {
        if (Velocity.magnitude > 0) _nadeProjectile.Graphics.transform.Rotate(Rotation);


    }
}
