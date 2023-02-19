using static Godot.GD;
using Godot;

public class HUD : CanvasLayer
{
    [Signal]
    public delegate void StartGame1P(float aiLevel);

    [Signal]
    public delegate void StartGame2P();

    [Signal]
    public delegate void GameOver();

    private Label _rightScore;
    private Label _leftScore;
    private Label _message;


    // Methods:
    public override void _Ready()
    {
        _rightScore = GetNode<Label>("Game/RightScore");
        _leftScore =  GetNode<Label>("Game/LeftScore");
        _message = GetNode<Label>("Message");

        MainMenu();
    }

    public void MainMenu()
    {
        GetNode<CanvasLayer>("MainMenu").Show();
        GetNode<TextureRect>("Blur").Show();
        GetNode<CanvasLayer>("Game").Hide();

        _message.Text = "A Game by: Emilly S.H.";
        _message.Align = Label.AlignEnum.Left;
        _message.Uppercase = false;
        _message.Show();
    }

    // Receives:
    // "Pressed" from 1PButton
    // "Pressed" from 2PButton
    public void PrepareGame(int mode)
    {
        GetNode<Label>("Game/RightLabel").Hide();
        GetNode<Label>("Game/LeftLabel").Hide();
        GetNode<TextureRect>("Blur").Hide();

        _rightScore.Text = "0";
        _leftScore.Text = "0";
        _message.Hide();

        GetNode<CanvasLayer>("MainMenu").Hide();
        GetNode<CanvasLayer>("Game").Show();

        if (mode == 1)
        {
            EmitSignal("StartGame1P",
                GetNode<Slider>("MainMenu/BotLevel").Value
            );
        }
        else
            EmitSignal("StartGame2P");
    }

    public void UpdateScore(string side)
    {
        int scoreR = int.Parse(_rightScore.Text);
        int scoreL = int.Parse(_leftScore.Text);

        int scoreDiff = (scoreL > scoreR)
                        ? scoreL - scoreR
                        : scoreR - scoreL;

        if (side == "Left")
            _rightScore.Text = (++scoreR).ToString();

        else
            _leftScore.Text = (++scoreL).ToString();


        if (scoreL >= 10 && scoreDiff >= 3)
        {
            EmitSignal("GameOver");
            GameWon("Left");
        }
        else if (scoreR >= 10 && scoreDiff >= 3)
        {
            EmitSignal("GameOver");
            GameWon("Right");
        }
    }

    public async void GameWon(string winner)
    {
        if (winner == "Left")
        {
            GetNode<Label>("Game/RightLabel").Text = "Lose";
            GetNode<Label>("Game/LeftLabel").Text = "Win";
        }
        else
        {
            GetNode<Label>("Game/RightLabel").Text = "Win";
            GetNode<Label>("Game/LeftLabel").Text = "Lose";
        }

        GetNode<Label>("Game/RightLabel").Show();
        GetNode<Label>("Game/LeftLabel").Show();

        await ToSignal(GetTree().CreateTimer(1), "timeout");
        _message.Uppercase = true;

        _message.Text = "Press 'R' to Restart the game or press 'M' to return to the Main menu.";
        _message.Align = Label.AlignEnum.Center;
        _message.Show();
    }
}
