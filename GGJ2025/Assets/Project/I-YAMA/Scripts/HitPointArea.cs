using System;
using UnityEngine;

namespace Project.GGJ2025
{
    public class HitPointArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerStay2D(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            // Debug.Log($"Entry or Stay other.tag:{other.tag}");
            if (!other.CompareTag("Player"))
            {
                return;
            }
            // Player は collider の親が持っている
            var player = other.transform.parent.GetComponent<Player>();
            DataStore.Instance.HitChangePlayerState(player, gameObject.tag);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // Debug.Log($"Exit other.tag:{other.tag}");
            if (!other.CompareTag("Player"))
            {
                return;
            }
            // Player は collider の親が持っている
            var player = other.transform.parent.GetComponent<Player>();
            DataStore.Instance.HitChangePlayerState(player, AreaState.None.ToString());
        }
    }
}