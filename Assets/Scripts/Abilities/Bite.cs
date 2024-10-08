using UnityEngine;

public class Bite : Ability, IAbilityAction, IAbilityEnd
{
    private BoxCollider2D _myCollider;

    private void Awake() {
        _myCollider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        _myCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Checking for IDamagable");
        IDamagable damagable = other.GetComponentInParent<IDamagable>();
        Vector3 collisonPoint = other.ClosestPoint(this.transform.position);

        damagable?.TakeDamage(AbilityInfo.Damage, collisonPoint);
    }

    public void AbilityAction() {
        _myCollider.enabled = true;
    }

    public void AbilityEnd() {
        _myCollider.enabled = false;
    }
}
