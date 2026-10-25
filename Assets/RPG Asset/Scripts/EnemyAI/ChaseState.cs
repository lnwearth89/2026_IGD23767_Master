using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.Agent.isStopped = false;
        enemy.SetMovementState(1);
    }

    public override void Tick(float deltaTime)
    {
        if (enemy.Player == null) return;

        Vector3 playerPosition = enemy.Player.transform.position;
        float distance = Vector3.Distance(enemy.transform.position, playerPosition);

        if (distance > enemy.VisionRange)
        {
            // ให้ Enemy เลือกเองว่าจะกลับไปยืนเฉยๆ หรือไป patrol ต่อ (ตั้งค่าได้ทีละตัวใน Inspector)
            enemy.TransitionTo(enemy.CreateLostSightState());
            return;
        }

        if (distance <= enemy.AttackRange)
        {
            enemy.TransitionTo(new AttackState(enemy));
            return;
        }

        // อัปเดตเป้าหมายทุกเฟรมเพื่อไล่ตาม Player ที่กำลังเคลื่อนที่
        enemy.Agent.SetDestination(playerPosition);
    }
}
