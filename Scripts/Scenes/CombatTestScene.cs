using Godot;
using System;

using SpaceDoom.Scenes;
using SpaceDoom.Library.Abstract;

public class CombatTestScene : SceneBase
{
    //Scenes
    private PackedScene GUIScene { get; set; }
    private PackedScene EnemyScene { get; set; }
    //Nodes
    private Timer TestTimer { get; set; }
    private Label TestLabel { get; set; }
    private AudioStreamPlayer MusicBackground { get; set; }
    //Data
    private Random rng = new Random();
    private int EnemyCount { get; set; } = 0;
    [Export] private int EnemyMax { get; set; } = 15;

    //Godot methods
    public override void _Ready()
    {
        base._Ready();

        TestTimer = GetNode<Timer>("Timer");
        TestLabel = GetNode<Label>("TestLabel");
        MusicBackground = GetNode<AudioStreamPlayer>("BGMusic");

        EnemyScene = GD.Load<PackedScene>("res://Scenes/Entities/Enemy_Bee.tscn");

        LoadPlayer(new Vector2(1024, 800), true);

        for(var e = EnemyMax; e > 0; e--)
        {
            CreateEnemy();
        }

        MusicBackground.Play();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        TestLabel.Text = PlayerSingleton.PlayerScript.PlayerScore.ToString() + $"\n{EnemyCount}";

        if(EnemyCount == 0) 
        { 
            PlayerSingleton.PlayerScript.PlayerScore += 1000;

            for (var e = EnemyMax; e > 0; e--)
            {
                CreateEnemy();
            }
        }

        if (Input.IsActionJustPressed("Fullscreen")) { OS.WindowFullscreen = !OS.WindowFullscreen; }
    }

    //Spawning enemies for combat testing
    public void CreateEnemy()
    {
        if(EnemyCount >= EnemyMax) { return; }
        var offset = new Vector2(128 + rng.Next(0, 1664), 128 + rng.Next(0, 448));
        var instance = EnemyScene.Instance();
        var script = instance as Enemy;
        script.EnemyDied += EnemyDown;
        LoadChildAt(script, offset);
        EnemyCount++;
    }

    public void EnemyDown() => EnemyCount--;

    //Music!
    public void MusicBackgroundFinished()
    {
        MusicBackground.Play();
    }
}