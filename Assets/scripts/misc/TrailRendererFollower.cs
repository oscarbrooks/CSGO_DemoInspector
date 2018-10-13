using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailRendererFollower : MonoBehaviour {

    public Transform FollowTarget;
    public TrailRenderer TrailRenderer;

	private void Start () {
        TrailRenderer = GetComponent<TrailRenderer>();
	}
	
	private void Update () {
        if(FollowTarget != null) transform.position = FollowTarget.position;
	}
}
