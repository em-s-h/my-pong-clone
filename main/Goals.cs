using static Godot.GD;
using Godot;

public class Goals : Area2D
{
    public void OnGoalEntered(Area2D area)
    {
        if (area is Ball ball)
        {
            var name = (Name.Contains("Left")) ? "Left" : "Right";
            ball.Reset(name);

            if (GetNode<Main>("/root/Main").GameMode != 0)
                GetNode<HUD>("/root/Main/HUD").UpdateScore(name);
        }
    }
}
