using Godot;

public class Walls : Area2D
{
    public void OnWallEntered(Area2D area)
    {
        if (area is Ball ball)
        {
            var bounceDirection = (Name.Contains("Top")) ? 1 : -1;

            ball.Direction = (ball.Direction + new Vector2(0, bounceDirection)).Normalized();
        }
    }
}
