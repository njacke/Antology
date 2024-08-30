using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool GettingKnockedBack { get; private set; }

    private Rigidbody2D _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Vector2 dir, float knockbackThrust, float knockbackTime){
        GettingKnockedBack = true;
        _rb.velocity = Vector2.zero;
        _rb.AddForce(dir * knockbackThrust, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine(knockbackTime));
    }

    private IEnumerator KnockRoutine(float knockbackTime){
        yield return new WaitForSeconds(knockbackTime);
        _rb.velocity = Vector2.zero;
        GettingKnockedBack = false;
    }
}