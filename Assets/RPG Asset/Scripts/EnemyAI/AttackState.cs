using UnityEngine;

public class AttackState : EnemyState
{
    private float cooldownTimer;

    public AttackState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.Agent.isStopped = true;
        enemy.SetMovementState(0);
        cooldownTimer = 0f;
        enemy.PerformAttack(); // โจมตีทันทีตอนเข้าระยะ
    }

    public override void Tick(float deltaTime)
    {
        if (enemy.Player == null) return;

        float distance = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
        if (distance > enemy.AttackRange)
        {
            enemy.TransitionTo(new ChaseState(enemy));
            return;
        }

        cooldownTimer += deltaTime;
        if (cooldownTimer >= enemy.AttackCooldown)
        {
            enemy.PerformAttack();
            cooldownTimer = 0f;
        }
    }
}
