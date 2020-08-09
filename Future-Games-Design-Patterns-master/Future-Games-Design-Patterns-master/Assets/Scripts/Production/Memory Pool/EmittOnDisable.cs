using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmittOnDisable : MonoBehaviour
{
    public event Action<GameObject> OnDisableGameObject;

    private void OnDisable()
    {
        OnDisableGameObject?.Invoke(this.gameObject) ;
    }
}
