using Godot;

public enum PoolingStatus
{
    ACTIVE,
    INACTIVE,
}

public partial class Envs : Node
{
    [Export]
    public PoolingStatus Pooling = PoolingStatus.INACTIVE;
}
