using UnityEngine;

// หลุดระยะมองเห็นแล้วเดินกลับไปจุดเกิด (spawn position) แทนการ patrol
// อยากเปลี่ยนเป็น patrol ทีหลังก็แค่เขียน PatrolState ใหม่แล้วสลับใช้แทนตัวนี้ได้เลย โดยไม่กระทบ state อื่น
public class ReturnState : EnemyState
{
    public ReturnState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.Agent.isStopped = false;
        enemy.SetMovementState(1);
        enemy.Agent.SetDestination(enemy.SpawnPosition);
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
            enemy.TransitionTo(new IdleState(enemy));
        }
    }
}
