using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseEnemy))]
public class EnemyEditor : Editor
{
    private void OnSceneGUI()
    {
        BaseEnemy enemy = (BaseEnemy)target;

        if (enemy == null || enemy.enemyData == null)
            return;

        Handles.color = Color.white;
        Handles.DrawWireArc(enemy.transform.position, Vector3.up, Vector3.forward, 360, enemy.enemyData.SeeingDistance);

        Vector3 viewAngle1 = DirectionFromAngle(enemy.transform.eulerAngles.y, -enemy.enemyData.SeeingAngle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(enemy.transform.eulerAngles.y, enemy.enemyData.SeeingAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(enemy.transform.position, enemy.transform.position + viewAngle1 * enemy.enemyData.SeeingDistance);
        Handles.DrawLine(enemy.transform.position, enemy.transform.position + viewAngle2 * enemy.enemyData.SeeingDistance);

        if (enemy.isSeeingPlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(enemy.transform.position, enemy.playerTarget.position);
        }
    }

    Vector3 DirectionFromAngle(float eulerY, float angleInDeg)
    {
        angleInDeg += eulerY;

        return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
    }
}
