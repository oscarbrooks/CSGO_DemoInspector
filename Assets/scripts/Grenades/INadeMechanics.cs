using UnityEngine;

public interface INadeMechanics {
    void Initialize(NadeProjectileGraphics nadeGraphics);
    void Update();
    void UpdateVelocity(Vector3 velocity);
}
