using static Godot.GD;
using Godot;

public class Player : Area2D
{
    private Vector2 _newBallDirection;
    private Vector2 _screenSize;
    private string _moveDown;
    private string _moveUp;

    [Export]
    public int Speed = 375;


    public override void _Ready()
    {
        var side = (Name.Contains("Left")) ? "left" : "right";

        _newBallDirection.x = (side == "left") ? 1 : -1;
        _screenSize = GetViewportRect().Size;

        _moveDown = $"{side}_move_down";
        _moveUp = $"{side}_move_up";

        if (side == "left")
        {
            Position = new Vector2(
                _screenSize.x / 10, _screenSize.y / 2
            );
        }
        else
        {
            Position = new Vector2(
                _screenSize.x - (_screenSize.x / 10),
                _screenSize.y / 2
            );
        }
    }

    public override void _Process(float delta)
    {
        // This is done to modify the Position.y value.
        Vector2 tmpPosition = Position;
        var verticalVelocity = 0;

        if (Input.IsActionPressed($"{_moveUp}"))
            verticalVelocity -= 1;

        if (Input.IsActionPressed($"{_moveDown}"))
            verticalVelocity += 1;

        tmpPosition.y += verticalVelocity * delta * Speed;
        tmpPosition.y = Mathf.Clamp(
            tmpPosition.y, 48, _screenSize.y -48
        );

        Position = tmpPosition;
    }

    public void OnAreaEntered(Area2D area)
    {
        if (area is Ball ball)
        {
            if (Input.IsActionPressed(_moveUp))
                _newBallDirection.y = (float) -RandRange(0.25, 0.75);

            else if (Input.IsActionPressed(_moveDown))
                _newBallDirection.y = (float) RandRange(0.25, 0.75);

            else
                _newBallDirection.y = (float) RandRange(-0.10, 0.10);

            ball.Direction = _newBallDirection;
            ball.Speed += 10;
        }
    }
}
