using static Godot.GD;
using Godot;

public class Ball : Area2D
{
    private const int INIT_SPEED = 300;
    private Vector2 _initialPosition;
    public int Speed = INIT_SPEED;
    public Vector2 Direction;


    // Methods:
    public override void _Ready()
    {
        _initialPosition = GetViewportRect().Size / 2;
        Position = _initialPosition;
    }

    public override void _Process(float delta)
        => Position += Speed * delta * Direction;


    public void Reset(string newDirection)
    {
        Position = _initialPosition;
        Speed = INIT_SPEED;

        if (newDirection == "Left")
            Direction = Vector2.Left;

        else
            Direction = Vector2.Right;

        if ( !IsProcessing() )
            SetProcess(true);
    }

    public async void CheckIfOutBounds()
    {
        await ToSignal(GetTree().CreateTimer((float) 0.5), "timeout");

        // Stop the ball from getting out of bounds.
        if (Position.x < -25 || Position.x > 1049)
            Reset( (Randi() % 2 == 0) ? "Left" : "Right" );

        else if (Position.y < -25 || Position.y > 625)
            Reset( (Randi() % 2 == 0) ? "Left" : "Right" );
    }

    public void OnAreaEntered(Area2D area)
    {
        if ( ! area.Name.Contains("Goal") && GetNode<Main>("/root/Main").GameMode != 0 )
            GetNode<AudioStreamPlayer>("HitSound").Play();

        if ( area.Name.Contains("Wall") )
        {
            Speed += 5;
            CheckIfOutBounds();
        }
    }
}
