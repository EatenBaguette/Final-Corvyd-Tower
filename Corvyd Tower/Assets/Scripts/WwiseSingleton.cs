using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseSingleton : MonoBehaviour
{

    public static WwiseSingleton WwiseInsance;

    void Awake()
    {
        if(WwiseInsance == null)
        {
            WwiseInsance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
