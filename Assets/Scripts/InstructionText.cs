using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionText : MonoBehaviour
{
    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
        
        StartCoroutine(PlayAnimation());
    }

    //play animation for floating instruction text
    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(20);
        anim.Play();
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
