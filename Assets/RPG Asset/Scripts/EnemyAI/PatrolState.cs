using UnityEngine;

// เดินวนตาม enemy.PatrolPoints ไปเรื่อยๆ จนกว่าจะเจอ Player เข้าระยะมองเห็น
public class PatrolState : EnemyState
{
    private int currentPointIndex;

    public PatrolState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        if (enemy.PatrolPoints == null || enemy.PatrolPoints.Length == 0)
        {
            // ไม่ได้ตั้งจุด patrol ไว้ กันพังด้วยการกลับไปยืนเฉยๆ ที่จุดเกิดแทน
            Debug.LogWarning($"{enemy.name} is set to Patrol but has no PatrolPoints assigned.");
            enemy.TransitionTo(new ReturnState(enemy));
            return;
        }

        enemy.Agent.isStopped = false;
        enemy.SetMovementState(1);
        currentPointIndex = FindClosestPointIndex();
        enemy.Agent.SetDestination(enemy.PatrolPoints[currentPointIndex].position);
    }

    public override void Tick(float deltaTime)
    {
        if (enemy.Player != null)
        {
            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
            if (distanceToPlayer <= enemy.VisionRange)
            {
                enemy.TransitionTo(new ChaseState(enemy));
                return;
            }
        }

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
        {
            currentPointIndex = (currentPointIndex + 1) % enemy.PatrolPoints.Length;
            enemy.Agent.SetDestination(enemy.PatrolPoints[currentPointIndex].position);
        }
    }

    private int FindClosestPointIndex()
    {
        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < enemy.PatrolPoints.Length; i++)
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.PatrolPoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }
}
