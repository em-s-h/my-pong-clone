using static Godot.GD;
using Godot;

public class Main : Node
{
    // We assign this in the editor, so we don't need the warning about not being assigned.
#pragma warning disable 649
    [Export]
    public PackedScene BotScene;

    [Export]
    public PackedScene PlayerScene;
#pragma warning restore 649

    public int GameMode = 0;


    public override void _Ready()
    {
        SetProcess(false);
        Demo();
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("main_menu"))
        {
            GetNode<HUD>("HUD").MainMenu();
            Demo();
            SetProcess(false);
        }
        else if (Input.IsActionJustPressed("restart") && GameMode == 1)
        {
            GetNode<HUD>("HUD").PrepareGame(1);
            SetProcess(false);
        }
        else if (Input.IsActionJustPressed("restart") && GameMode == 2)
        {
            GetNode<HUD>("HUD").PrepareGame(2);
            SetProcess(false);
        }
    }

    public void Demo()
    {
        foreach (var child in GetChildren())
        {
            if (child is Player player)
                player.QueueFree();

            if (child is Bot bot)
                bot.QueueFree();
        }
        GameMode = 0;

        var rightBot = (Bot) BotScene.Instance();
        var leftBot = (Bot) BotScene.Instance();

        rightBot.Name = "RightPaddle";
        leftBot.Name = "LeftPaddle";

        rightBot.FollowSpeed = (float) RandRange(3.0, 4.0);
        leftBot.FollowSpeed = (float) RandRange(3.0, 4.0);

        AddChild(rightBot);
        AddChild(leftBot);

        GetNode<Ball>("Ball").Reset(
            (Randi() % 2 == 0) ? "Left" : "Right"
        );
    }

    // Receives:
    // "StartGame1P" from HUD.
    public void NewGame1P(float botLevel)
    {
        foreach (var child in GetChildren())
        {
            if (child is Bot paddleL && paddleL.Name.Contains("Left"))
                paddleL.QueueFree();

            if (child is Bot paddleR && paddleR.Name.Contains("Right"))
                paddleR.FollowSpeed = botLevel;
        }
        GameMode = 1;

        var leftPlayer = (Player) PlayerScene.Instance();
        leftPlayer.Name = "LeftPaddle";
        AddChild(leftPlayer);

        GetNode<Ball>("Ball").Reset(
            (Randi() % 2 == 0) ? "Left" : "Right"
        );
    }

    // Receives:
    // "StartGame1P" from HUD.
    public void NewGame2P()
    {
        foreach (var child in GetChildren())
        {
            if (child is Player player)
                player.QueueFree();

            if (child is Bot bot)
                bot.QueueFree();
        }
        GameMode = 2;

        var rightPlayer = (Player) PlayerScene.Instance();
        var leftPlayer = (Player) PlayerScene.Instance();

        rightPlayer.Name = "RightPaddle";
        leftPlayer.Name = "LeftPaddle";

        AddChild(rightPlayer);
        AddChild(leftPlayer);

        GetNode<Ball>("Ball").Reset(
            (Randi() % 2 == 0) ? "Left" : "Right"
        );
    }

    // Receives:
    // "GameOver" from HUD.
    public void GameOver()
    {
        GetNode<Ball>("Ball").SetProcess(false);
        SetProcess(true);
    }
}
