using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ColliderToEvent {
    public Collider collider;
    public UnityEvent onTrigger;
}

public class HandTrigger : MonoBehaviour
{
    public List<ColliderToEvent> colliderAndEvents;
    public List<ColliderToEvent> colliderLeave;

    void OnTriggerEnter(Collider collidingItem)
    {
        if (colliderAndEvents != null) {
            colliderAndEvents.ForEach((colliderToEvent) => {
                if (colliderToEvent.collider == collidingItem) {
                    colliderToEvent.onTrigger.Invoke();
                }
            });
        }
    }

    void OnTriggerExit(Collider collidingItem) {
        if (colliderLeave != null) {
            colliderLeave.ForEach((colliderToEvent) => {
                if (colliderToEvent.collider == collidingItem) {
                    colliderToEvent.onTrigger.Invoke();
                }
            });
        }
    }
}
