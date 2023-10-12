using Godot;

public partial class Enemy : Area2D, IPoolItem
{
    public static Theme InactivePoolItem = ResourceLoader.Load<Theme>("res://Themes/InactivePoolItem/InactivePoolItem.theme");

    private float Speed = 30f;
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
        GetNode<Control>("/root/Scenes/BattleField/CanvasLayer/Control/Enemies").AddChild(PanelPoolItem, true);

        TimerDestroy.WaitTime = 320 / Speed;
        TimerDestroy.Timeout += () =>
        {
            Destroy();
        };
        AddChild(TimerDestroy, true);
    }

    public override void _Process(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }

    public void SetRandomPoision()
    {
        Direction = -Vector2.Right.Rotated(Mathf.Lerp(0, Mathf.Pi * 2, GD.Randf()));
        Position = Direction * -350;
    }

    public void Spawn()
    {
        Active = true;
        Visible = true;
        Monitoring = true;
        Monitorable = true;
        SetProcess(true);
        GetParent().MoveChild(this, -1);

        PanelPoolItem.Theme = null;
        SetRandomPoision();
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
