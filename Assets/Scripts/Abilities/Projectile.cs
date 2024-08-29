using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f; //TODO: add to ability info?
    public int ProjectileDamage { get; set; } = 0;

    private void Update() {
        MoveProjectile();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Checking for IDamagable");
        IDamagable damagable = other.GetComponentInParent<IDamagable>();
        Vector3 collisonPoint = other.ClosestPoint(this.transform.position);
        Destroy(gameObject);

        damagable?.TakeDamage(ProjectileDamage, collisonPoint);
    }

    private void MoveProjectile() {
        transform.Translate(_moveSpeed * Time.deltaTime * Vector3.right);
    }
}
