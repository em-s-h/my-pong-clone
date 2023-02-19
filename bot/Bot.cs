using static Godot.GD;
using Godot;

public class Bot : Area2D
{
    private Vector2 _newBallDirection;
    private Vector2 _initialPosition;
    private Vector2 _screenSize;

    [Export]
    public float FollowSpeed;


    // Methods:
    public override void _Ready()
    {
        _newBallDirection.x = (Name.Contains("Left")) ? 1 : -1;
        _screenSize = GetViewportRect().Size;

        if (Name.Contains("Left"))
        {
            _initialPosition = new Vector2(
                _screenSize.x / 10, _screenSize.y / 2
            );
        }
        else
        {
            _initialPosition = new Vector2(
                _screenSize.x - (_screenSize.x / 10),
                _screenSize.y / 2
            );
        }

        Position = _initialPosition;
    }

    public override void _Process(float delta)
    {
        var ball = GetNode<Ball>("/root/Main/Ball");
        // This is done to modify the Position.x & y values.
        Vector2 tmpPosition = Position;

        tmpPosition = tmpPosition.LinearInterpolate(
            ball.Position, (FollowSpeed * delta)
        );

        tmpPosition.y = Mathf.Clamp(
            tmpPosition.y, 48, _screenSize.y - 48
        );

        tmpPosition.x = _initialPosition.x;
        Position = tmpPosition;
    }

    public void OnAreaEntered(Area2D area, string collision)
    {
        if (area is Ball ball)
        {
            if (collision.Contains("Top"))
                _newBallDirection.y = (float) -RandRange(0.25, 0.75);

            else if (collision.Contains("Bottom"))
                _newBallDirection.y = (float) RandRange(0.25, 0.75);

            else
                _newBallDirection.y = (float) RandRange(-0.15, 0.15);

            ball.Direction = _newBallDirection;
            ball.Speed += 10;
        }
    }
}
