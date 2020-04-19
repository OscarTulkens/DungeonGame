using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 0;
    [SerializeField] private float _movementSpeedMultiplier = 0;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (ControlScript.Instance.DesiredPositions.Count >= 2)
        {
            if (Vector3.Distance(transform.position, ControlScript.Instance.DesiredPositions[0]) >= 0.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, ControlScript.Instance.DesiredPositions[0], _movementSpeed * _movementSpeedMultiplier * Time.deltaTime);
            }
            else
            {
                ControlScript.Instance.DesiredPositions.RemoveAt(0);
            }
        }
        else if (ControlScript.Instance.DesiredPositions.Count == 1)
        {
            if (Vector3.Distance(transform.position, ControlScript.Instance.DesiredPositions[0]) >= 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, ControlScript.Instance.DesiredPositions[0], _movementSpeed * Time.deltaTime);
            }
            else
            {
                ControlScript.Instance.DesiredPositions.RemoveAt(0);
            }
        }
    }
}
