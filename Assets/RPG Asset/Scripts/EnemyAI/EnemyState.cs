// abstract class รูปแบบเดียวกับ TrafficLightState/DoorState: Enter() ทำครั้งเดียวตอนเข้าสถานะ, Tick() ทำทุกเฟรม
public abstract class EnemyState
{
    protected Enemy enemy;

    public EnemyState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public abstract void Enter();
    public abstract void Tick(float deltaTime);
}
