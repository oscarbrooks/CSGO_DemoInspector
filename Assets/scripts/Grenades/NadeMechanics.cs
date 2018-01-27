using UnityEngine;

public abstract class NadeMechanics {

    protected Vector3 Rotation;
    protected Vector3 Velocity;

    protected static Vector3 GetRandomRotation()
    {
        var min = 0.1f;
        var max = 1.5f;

        var speed = 10;

        return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max)) * speed;
    }

    public void UpdateVelocity(Vector3 velocity)
    {
        Velocity = velocity;
    }
}
