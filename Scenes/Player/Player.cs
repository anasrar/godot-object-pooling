using Godot;

public partial class Player : CharacterBody2D
{
    private bool CanFireBullet = true;
    private Timer TimerCooldownFireBullet = new Timer();

    [Export(PropertyHint.Range, "0.01,999,0.01,or_greater")]
    public float CooldownFireBullet = 0.2f;
    [Export]
    public NodePath PathPoolBullets;

    public override void _Ready()
    {
        TimerCooldownFireBullet.WaitTime = CooldownFireBullet;
        TimerCooldownFireBullet.Timeout += () =>
        {
            CanFireBullet = true;
        };
        AddChild(TimerCooldownFireBullet);
    }

    public override void _PhysicsProcess(double delta)
    {
        Rotation = (GetViewport().GetMousePosition() - GlobalPosition).Angle();

        if (Input.IsActionPressed("fire_bullet") && CanFireBullet)
        {
            Pool PoolBullets = GetNode<Pool>(PathPoolBullets);

            Bullet bullet = PoolBullets.GetItem() as Bullet;
            bullet.Spawn(Rotation);
            CanFireBullet = false;
            TimerCooldownFireBullet.Start();
        }
    }
}
