﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Pathfinding
{
    public class PathfindingAgent : MonoBehaviour
    {
        // Editor-assignable target for testing
        [SerializeField] private Transform _target;
        private Vector3 _pathTarget;

        private Pathfinder _pathfinder;
        private List<PathfindingNode> _path;

        void Awake()
        {
            _pathfinder = FindObjectOfType<Pathfinder>();
        }

        void Start()
        {

        }

        void Update()
        {
            if(_pathTarget != _target.position)
            {
                _pathTarget = _target.position;
                _path = _pathfinder.FindPath(transform.position, _pathTarget, GetComponent<CircleCollider2D>());
            }
        }

        void OnDrawGizmosSelected()
        {
            if(_path != null)
            {
                Gizmos.color = Color.white;
                foreach(var node in _path)
                {
                    Gizmos.DrawLine(node.WorldPosition, node.Parent.WorldPosition);
                    node.DrawGizmos(0.25f, true);
                }
            }
        }
    }
}