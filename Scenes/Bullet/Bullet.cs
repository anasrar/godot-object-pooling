using Godot;

public partial class Bullet : Area2D, IPoolItem
{
    public static Theme InactivePoolItem = ResourceLoader.Load<Theme>("res://Themes/InactivePoolItem/InactivePoolItem.theme");

    private float Speed = 120f;
    private Vector2 Direction = Vector2.Right;
    private Panel PanelPoolItem = new Panel();
    private Timer TimerDestroy = new Timer();

    public bool Active { get; set; } = false;

    public override void _Ready()
    {
        Visible = false;
        Monitoring = false;
        Monitorable = false;
        SetProcess(false);

        PanelPoolItem.Theme = Enemy.InactivePoolItem;
        GetNode<Control>("/root/Scenes/BattleField/CanvasLayer/Control/Bullets").AddChild(PanelPoolItem, true);

        TimerDestroy.WaitTime = 350 / Speed;
        TimerDestroy.Timeout += () =>
        {
            Destroy();
        };
        AddChild(TimerDestroy, true);

        AreaEntered += (Area2D enemy) =>
        {
            ((Enemy)enemy).Destroy();
            Destroy();
        };
    }

    public override void _Process(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }

    public void SetDirection(float angle)
    {
        Position = Vector2.Zero;
        Direction = Vector2.Right.Rotated(angle);
    }

    public void Spawn(float angle)
    {
        Active = true;
        Visible = true;
        Monitoring = true;
        Monitorable = true;
        SetProcess(true);
        GetParent().MoveChild(this, -1);

        PanelPoolItem.Theme = null;
        SetDirection(angle);
        TimerDestroy.Start();
    }

    public void Destroy()
    {
        PoolingStatus status = GetNode<Envs>("/root/Envs").Pooling;

        switch (status)
        {
            case PoolingStatus.ACTIVE:
                Active = false;
                Visible = false;
                SetDeferred(Area2D.PropertyName.Monitoring, false);
                SetDeferred(Area2D.PropertyName.Monitorable, false);
                SetProcess(false);
                GetParent().MoveChild(this, 0);

                PanelPoolItem.Theme = Enemy.InactivePoolItem;
                TimerDestroy.Stop();
                break;
            case PoolingStatus.INACTIVE:
                PanelPoolItem.QueueFree();
                QueueFree();
                break;
        }
    }
}
