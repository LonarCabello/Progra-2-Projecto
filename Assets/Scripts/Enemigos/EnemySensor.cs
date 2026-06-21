/**
 * =============<< ********* >>=============
 * Author       : Guillermo KÖSTER
 * Email        : guillermo.fullstack@gmail.com
 * Organization : 
 * Created On   : 
 * Title        : EnemySensor
 * Description  : Sensor del enemigo para detectar al player.
 * Copyright (c) 2025 Guillermo KÖSTER.
 * =============<< ********* >>=============
 */

using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    [SerializeField]
    Transform target;
    // const float visionRange = 40f;
    // const float angleVisionH = 45f;
    // const float angleVisionV = 45f;
    // const float walkHearingRange = 8f;
    // const float runHearingRange = 15f;
    // const float hearingMessageRange = 10f;
    Rigidbody targetRB;
    EnemyData enemyData;
    
    public void Initialize(EnemyData data)
    {
        targetRB = target.GetComponent<Rigidbody>();
        enemyData = data;
    }

    public bool canSeeTarget(Transform target)
    {
        RaycastHit hit;
        Vector3 direction = target.position - transform.position;
        bool inAngleH = Vector3.Angle(transform.forward, new Vector3(direction.x, 0, direction.z)) <= enemyData.angleVisionH;
        Vector3 aux = (transform.forward * Mathf.Sqrt(Mathf.Pow(direction.magnitude, 2) - Mathf.Pow(direction.y, 2))) + (Vector3.up * direction.y);
        bool inAngleV = Vector3.Angle(transform.forward, aux) <= enemyData.angleVisionV;
        if (inAngleH && inAngleV)
        {
            if(Physics.Raycast(transform.position, direction,out hit, enemyData.visionRange))
            {
                return hit.transform == target;
            }
        }

        return false;
    }

    public bool canHearTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        float sqrDistance = direction.sqrMagnitude;
        if(target.tag == "Player")
        {
            if(sqrDistance <= targetRB.linearVelocity.sqrMagnitude * 5f)
            {
                return true;
            }
            // if(sqrDistance <= enemyData.walkHearingRange * enemyData.walkHearingRange && targetRB.linearVelocity.sqrMagnitude > 1f) // 8*8= 64 , 4 
            // {
            //     return true;
            // }
            // else if(sqrDistance <= enemyData.runHearingRange * enemyData.runHearingRange && targetRB.linearVelocity.sqrMagnitude >= 4f) // 15*15= 225 , 6,5
            // {
            //     return true;
            // }
            return false;
        }else
        {
            return sqrDistance <= enemyData.hearingMessageRange * enemyData.hearingMessageRange;
        }
    }
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Esfera para sonido suave (Caminata)
            Gizmos.color = Color.rebeccaPurple;
            Gizmos.DrawWireSphere(transform.position, enemyData.walkHearingRange);

            // Esfera para sonido fuerte (Correr)
            Gizmos.color = Color.greenYellow;
            Gizmos.DrawWireSphere(transform.position, enemyData.runHearingRange);

            // // Esfera para alertas de mensajes
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, enemyData.hearingMessageRange);

            // // Línea para visión
            
            Gizmos.color = !canSeeTarget(target) ? Color.green : Color.red;
            
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * enemyData.visionRange);
            Matrix4x4 oldMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawFrustum(Vector3.zero, enemyData.angleVisionH*2, enemyData.visionRange, 0.1f, 1f);
            Gizmos.matrix = oldMatrix;
        }

    }
}

