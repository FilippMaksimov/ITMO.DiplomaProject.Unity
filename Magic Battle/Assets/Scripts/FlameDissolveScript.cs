using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameDissolveScript : MonoBehaviour
{
    private Material mat;
    private void Start()
    {
        mat = gameObject.GetComponent<ParticleSystemRenderer>().material;
    }
    public void Disolve(float speed)
    {
        mat.SetFloat("_Progress", mat.GetFloat("_Progress") - speed);
        if (mat.GetFloat("_Progress") <= 0)
        {
            Destroy(gameObject, 1f);
        }
    }
}
