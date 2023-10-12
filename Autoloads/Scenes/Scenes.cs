using Godot;

public partial class Scenes : Node
{
    public void ChangeScene(string path)
    {
        foreach(Node child in GetChildren())
        {
          child.QueueFree();
        }
        AddChild(ResourceLoader.Load<PackedScene>(path).Instantiate());
    }
}
