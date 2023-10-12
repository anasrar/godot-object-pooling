using Godot;

public partial class Main : Node
{
    [Export(PropertyHint.File, "*.tscn,")]
    public string Scene { get; set; }

    public override void _Ready()
    {
        GetNode<Scenes>("/root/Scenes").ChangeScene(Scene);
        QueueFree();
    }
}
