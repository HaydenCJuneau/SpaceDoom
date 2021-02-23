using Godot;
using System;

using SpaceDoom.Scenes;
using SpaceDoom.Library.Abstract;

public class TestScene01 : SceneBase
{
    //Scenes
    private PackedScene GUIScene { get; set; }
    private PackedScene EnemyScene { get; set; }
    //Nodes
    private Timer TestTimer { get; set; }
    //Data
    private Random rng = new Random();
    private int EnemyCount { get; set; } = 0;

    public override void _Ready()
    {
        base._Ready();

        EnemyScene = ResourceLoader.Load<PackedScene>("res://Scenes/Entities/Enemy_Bee.tscn");
        GUIScene = ResourceLoader.Load<PackedScene>("res://Scenes/Player/WeaponOverlay.tscn");

        HitscanLayer = GetNode<YSort>("FriendlyProjectiles");
        TestTimer = GetNode<Timer>("Timer");

        LoadPlayer(new Vector2(600, 400));

        //Load GUI
        AddChild(GUIScene.Instance());
    }

    

    //Code for spawning enemy at a testing nodes
    public void EnemyTimeout()
    {
        if(EnemyCount < 7) { SpawnEnemy(); }
    }

    private void EnemyDown() => EnemyCount--;

    private void SpawnEnemy()
    {
        Node2D spawner = GetNode<Node2D>($"EnemySpawnPositons/node{rng.Next(1,5)}");
        Vector2 offset = new Vector2(rng.Next(-100, 100), rng.Next(0, 100));
        var eInst = EnemyScene.Instance();
        var eScript = eInst as Enemy;
        eScript.EnemyDied += EnemyDown;
        LoadChildAt((KinematicBody2D)eInst, spawner.Position + offset);
        EnemyCount++;
    }
}
