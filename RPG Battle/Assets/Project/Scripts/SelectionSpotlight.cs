using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSpotlight : MonoBehaviour
{
    Vector3 targetPosition;
    float translateSpeed = 10f;

    // Update is called once per frame
    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f) {
            transform.position = targetPosition;
        } else {
            var vectorToTargetPosition = targetPosition - transform.position;
            transform.position += vectorToTargetPosition * translateSpeed * Time.deltaTime;
        }
    }

    public void SetTargetCharacter(CharacterBattle characterBattle)
    {
        targetPosition = characterBattle.transform.position;
        targetPosition.y += 5; 
    }
}
