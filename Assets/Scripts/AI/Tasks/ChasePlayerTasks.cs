﻿using EventManagers;
using Panda;
using UnityEngine;
using UnityEngine.Events;
using Events;

namespace Enemy
{
    public class ChasePlayerTasks : MonoBehaviour
    {
        private static readonly Vector2 NullVector = Constants.NullVector;

        private Vector2 playerLastKnownPosition = NullVector;
        private Vector2 playerLastKnownHeading = NullVector;

        private GameObjectEventManager eventManager;
        private Rigidbody2D playerRigidbody;
        private new Rigidbody2D rigidbody;

        private UnityAction<ParamsObject> onPlayerSendRigidbodyUnityAction;

        void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();

            eventManager = GetComponent<GameObjectEventManager>();
            onPlayerSendRigidbodyUnityAction = new UnityAction<ParamsObject>(OnPlayerSendRigidbody);
            eventManager.StartListening(EnemyEvents.OnPlayerSendRigidbody, onPlayerSendRigidbodyUnityAction);
            eventManager.StartListening(EnemyEvents.OnSetPlayerLastKnownLocation, new UnityAction<ParamsObject>(OnSetPlayerLastKnownLocation));
        }

        [Task]
        bool SetMovementTargetToPlayerPosition()
        {
            eventManager.TriggerEvent(EnemyEvents.OnSetMovementTarget, new ParamsObject(playerRigidbody.position));
            return true;
        }

        [Task]
        bool SetRotationToPlayer()
        {
            Vector2 direction = playerRigidbody.position - rigidbody.position;
            rigidbody.rotation = direction.AngleFromZero();
            return true;
        }

        [Task]
        bool SetMovementTargetToPlayerLastKnownPosition()
        {
            eventManager.TriggerEvent(EnemyEvents.OnSetMovementTarget, new ParamsObject(playerLastKnownPosition));
            return true;
        }

        [Task]
        bool SetPlayerLastKnownPosition()
        {
            eventManager.TriggerEvent(EnemyEvents.OnSetPlayerLastKnownLocation, new ParamsObject(playerRigidbody.position));
            return true;
        }

        [Task]
        bool IsPlayerLastPositionKnown()
        {
            return playerLastKnownPosition != NullVector;
        }

        [Task]
        bool UnSetPlayerLastKnownPosition()
        {
            playerLastKnownPosition = NullVector;
            return true;
        }

        [Task]
        bool SetRotationToPlayerLastKnownHeading()
        {
            rigidbody.rotation = playerLastKnownHeading.AngleFromZero();
            return true;
        }

        [Task]
        bool SetPlayerLastKnownHeading()
        {
            Vector2 direction = playerRigidbody.velocity;
            playerLastKnownHeading = direction;
            return true;
        }

        [Task]
        bool IsPlayerLastHeadingKnown()
        {
            return playerLastKnownHeading != NullVector;
        }

        [Task]
        bool UnSetPlayerLastKnownHeading()
        {
            playerLastKnownHeading = NullVector;
            return true;
        }

        private void OnPlayerSendRigidbody(ParamsObject paramsObj)
        {
            playerRigidbody = paramsObj.Rigidbody;
            eventManager.StopListening(EnemyEvents.OnPlayerSendRigidbody, onPlayerSendRigidbodyUnityAction);
        }

        private void OnSetPlayerLastKnownLocation(ParamsObject paramsObj)
        {
            playerLastKnownPosition = paramsObj.Vector2;
        }

        private void OnSetPlayerLastKnownHeading(ParamsObject paramsObj)
        {
            playerLastKnownHeading = paramsObj.Vector2;
        }

        void OnDrawGizmos()
        {
            if(IsPlayerLastPositionKnown())
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(playerLastKnownPosition, new Vector3(.3f, .3f, .3f));
            }
        }
    }
}
