using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(VineCoroutine());
    }

    private IEnumerator VineCoroutine()
    {
        Animator myAnimator = GetComponent<Animator>();
        myAnimator.SetTrigger("attack");
        yield return null;
        yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(this);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            collider.GetComponent<Enemy>().TakeDamage();
        }
    }
}
