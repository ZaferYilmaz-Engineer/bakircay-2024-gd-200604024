using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    [SerializeField] private BoxCollider leftCollider;
    [SerializeField] private BoxCollider rightCollider;
    [SerializeField] private BoxCollider bottomCollider;
    [SerializeField] private BoxCollider topCollider;
    
    public struct Boundaries
    {
        public float leftBoundary;
        public float rightBoundary;
        public float bottomBoundary;
        public float topBoundary;
    }
    
    public static GameArea Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public Boundaries GetBoundaries()
    {
        return new Boundaries
        {
            leftBoundary = leftCollider.transform.position.x,
            rightBoundary = rightCollider.transform.position.x,
            bottomBoundary = bottomCollider.transform.position.z,
            topBoundary = topCollider.transform.position.z
        };
    }
}
