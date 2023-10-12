using Godot;

public interface IPoolItem
{
    public bool Active { get; set; }
}

public partial class Pool : Node2D
{
    [Export(PropertyHint.Range, "1,100,1,or_greater")]
    public int InitialSpawn = 10;
    [Export]
    public PackedScene Item { get; set; }

    public override void _Ready()
    {
        PoolingStatus status = GetNode<Envs>("/root/Envs").Pooling;

        if (status == PoolingStatus.ACTIVE)
        {
            for (int i = 0; i < InitialSpawn; i++)
            {
                Node2D item = Item.Instantiate<Node2D>();
                AddChild(item, true);
            }
        }
    }

    public Node2D GetItem()
    {
        PoolingStatus status = GetNode<Envs>("/root/Envs").Pooling;

        if (status == PoolingStatus.ACTIVE)
        {
            Node2D item = GetChild<Node2D>(0);
            IPoolItem check = item as IPoolItem;
            if (check.Active)
            {
                Node2D newItem = Item.Instantiate<Node2D>();
                AddChild(newItem, true);
                return newItem;
            }
            return item;
        }
        else
        {
            Node2D item = Item.Instantiate<Node2D>();
            AddChild(item, true);
            return item;
        }
    }
}
