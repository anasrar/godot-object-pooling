using Godot;

public partial class BattleField : Node2D
{
    [Export]
    public NodePath PathPoolEnemies { get; set; }
    [Export]
    public Control GridBullets { get; set; }
    [Export]
    public Control GridEnemies { get; set; }

    private Timer TimerSpawnEnemy = new Timer();

    public override void _Ready()
    {
        Pool PoolEnemies = GetNode<Pool>(PathPoolEnemies);

        TimerSpawnEnemy.WaitTime = Mathf.Lerp(0.2, 1, GD.Randf());
        TimerSpawnEnemy.Timeout += () =>
        {
            Enemy enemy = PoolEnemies.GetItem() as Enemy;
            enemy.Spawn();
            TimerSpawnEnemy.WaitTime = Mathf.Lerp(0.2, 1, GD.Randf());
            TimerSpawnEnemy.Start();
        };
        AddChild(TimerSpawnEnemy, true);

        TimerSpawnEnemy.Start();
    }

    private void OnBulletsChildOrderChanged()
    {
        const int size = 24;
        const int gap = 8;
        Vector2 origin = new Vector2(377, 82);
        var panels = GridBullets.GetChildren();
        for (int i = 0; i < panels.Count; i++)
        {
            Panel panel = (Panel)panels[i];
            panel.Size = new Vector2(size, size);
            var column = i % 5;
            var row = i / 5;
            panel.Position = origin + new Vector2((size * column) + (gap * column), (size * row) + (gap * row));
        }
    }

    private void OnEnemiesChildOrderChanged()
    {
        const int size = 24;
        const int gap = 8;
        Vector2 origin = new Vector2(323 - size, 82);
        var panels = GridEnemies.GetChildren();
        for (int i = 0; i < panels.Count; i++)
        {
            Panel panel = (Panel)panels[i];
            panel.Size = new Vector2(size, size);
            var column = i % 5;
            var row = i / 5;
            panel.Position = origin + new Vector2(-((size * column) + (gap * column)), (size * row) + (gap * row));
        }
    }
}
