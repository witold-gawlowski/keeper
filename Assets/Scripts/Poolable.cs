using System;
using UnityEngine;
public abstract class Poolable: MonoBehaviour
{
    public System.Action<Poolable> destroyed;
}
