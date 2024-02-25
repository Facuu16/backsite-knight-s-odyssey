using System.Collections.Generic;
using UnityEngine;

public class HitCheck : MonoBehaviour
{

    private readonly HashSet<GameObject> _collisions = new HashSet<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Damageable"))
            _collisions.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _collisions.Remove(collision.gameObject);
    }

    public HashSet<GameObject> Collisions { 
        get {  return _collisions; } 
    }

}
