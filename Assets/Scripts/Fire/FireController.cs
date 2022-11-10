using System.Collections;
using System.Collections.Generic;
using System.Linq;   
using UnityEngine;

public class FireController : MonoBehaviour
{
    private int totalLitFires = 10;
    private IFireDependant[] fireDependants;

    void Awake() 
    {
        this.fireDependants = FindObjectsOfType<MonoBehaviour>().OfType<IFireDependant>().ToArray();
    }

    void NotifyAllFireDependants()
    {
        foreach (IFireDependant fd in this.fireDependants)
        {
            fd.NotifyFireChange(this.totalLitFires);
        }
    }
}
