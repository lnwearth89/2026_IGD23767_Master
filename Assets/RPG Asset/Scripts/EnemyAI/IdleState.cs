using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.Agent.isStopped = true;
        enemy.SetMovementState(0);
    }

    public override void Tick(float deltaTime)
    {
        if (enemy.Player == null) return;

        float distance = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distance <= enemy.VisionRange)
        {
            enemy.TransitionTo(new ChaseState(enemy));
        }
    }
}
