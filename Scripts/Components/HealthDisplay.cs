 using Godot;
using System;

using SpaceDoom.Library.Abstract;

namespace SpaceDoom.Components
{
    //The class that controls the behavior of an entities healthbar
    public class HealthDisplay : TextureProgress
    {
        //Nodes
        private Tween TweenAnimator { get; set; }

        //Health Data
        public double MaxHealth
        {
            get { return MaxValue; }
            set { MaxValue = value; }
        }

        public double CurrentHealth 
        { 
            get { return Value; } 
            set { Value = value; } //Now this is epic
        }

        //Visibility data
        private Color DefaultPTint { get; set; }
        private bool maxHidden { get; set; } //Use to hide the node before damage has been done

        //Godot methods
        public override void _Ready()
        {
            DefaultPTint = new Color(0, 210, 0, 255);
            TweenAnimator = GetNode<Tween>("Interpolater");

            base._Ready();
        }

        //InjectData (setup) Method
        public void InjectData(int maxHealth, int currentHealth, bool hide)
        {
            MaxValue = maxHealth;
            Value = currentHealth;
            if (hide) { Hide(); }
            maxHidden = hide;
        }

        public void InjectData(int maxHealth, int currentHealth, bool hide, Enemy pairedEnemy)
        {
            InjectData(maxHealth, currentHealth, hide); //Is this bad use case for method overloads?
            pairedEnemy.HealthChanged += HealthChanged;
            pairedEnemy.Died += EntityDied;
        }

        //Methods to change health
        public void HealthChanged(int difference)
        {
            if (maxHidden) { Show(); maxHidden = false; }

            var previous = Value;
            //If the change in health is less than 0, set to 0. Otherwise calculate difference.
            var current = (Value + difference < 0) ? 0 : Value + difference;
            //This tween creates a smooth animation for the health bar.
            TweenAnimator.InterpolateProperty(this, "value", previous, current, .3f);
            TweenAnimator.Start();
        }

        private void EntityDied(int d) => Hide();
    }
}