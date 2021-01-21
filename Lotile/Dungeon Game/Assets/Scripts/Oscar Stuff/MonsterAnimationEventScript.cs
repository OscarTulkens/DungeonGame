using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationEventScript : MonoBehaviour
{
    [SerializeField] private GameObject _objectToDestroy = null;

    void Die()
    {
        Destroy(_objectToDestroy);
    }
}
