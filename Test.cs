using Godot;

public class Test : Node
{
    public override void _Ready()
    {
        GD.Print("start");

        var tree = GetNode<Tree>("Tree");
        var root = tree.CreateItem();
        tree.HideRoot = true;

        var child1 = tree.CreateItem(root);
        child1.SetText(0, "child 1");
        var child2 = tree.CreateItem(root);
        child2.SetText(0, "child 2");
        var subchild1 = tree.CreateItem(child1);
        // subchild1.SetText(0, "subchild1");
        subchild1.SetText(1, "subchild1 cloluimn 2?");

    }


}