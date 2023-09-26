using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisolveElementsScript : MonoBehaviour
{
    public SkinnedMeshRenderer[] skinnedMeshRenderers;
    public GameObject dissolveEffect;
    public void Disolve(float speed)
    {
        //Create disolving effect;
        foreach (var skin in skinnedMeshRenderers)
        {
            skin.material.SetFloat("_Progress", skin.material.GetFloat("_Progress") - speed);
            Debug.Log("Minus");
            if (skin.material.GetFloat("_Progress") <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void DisolveEffect()
    {
        GameObject dissolve = Instantiate(dissolveEffect, transform.position, transform.rotation);
        Destroy(dissolve, 2.3f);
    }
}
