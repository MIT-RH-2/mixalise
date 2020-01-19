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
}
