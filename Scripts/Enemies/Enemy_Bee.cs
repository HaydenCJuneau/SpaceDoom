﻿using System;
using Godot;

using SpaceDoom.Library.Abstract;
using SpaceDoom.Systems.Combat;

namespace SpaceDoom.Enemies
{
    public class Enemy_Bee : Enemy
    {
        //Nodes
        private Tween PointLabelTween { get; set; }
        private Label PointsLabel { get; set; }
        //Data
        public bool Dead { get; protected set; } = false;

        //Godot methods
        public override void _Ready()
        {
            base._Ready();

            Sprite = GetNode<AnimatedSprite>("Sprite");
            PointLabelTween = GetNode<Tween>("PointLabel/Tween");
            PointsLabel = GetNode<Label>("PointLabel");
        }

        public void SpriteAnimationFinished()
        {
            if(Sprite.Animation == "death") 
            {
                QueueFree();
            }
        }

        //Behavior methods
        public override void ProcessCombatEvent(CombatEvent comEvent)
        {
            if (!Dead) { base.ProcessCombatEvent(comEvent); }
        }

        public override void TriggerDeath()
        {
            Dead = true;
            PointsLabel.Show();
            PointLabelTween.InterpolateProperty(PointsLabel, "rect_position", null, new Vector2(-16, 150), 4);
            Sprite.Play("death");
            _invokeDealthAction();
        }
    }
}
