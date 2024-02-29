using Godot;
using System;

public partial class TopDownPlayer : CharacterBody2D {
	public float Speed = 5;

    private Vector2 velocity = Vector2.Zero;
    public static TopDownPlayer Instance;

    public override void _Ready() {
        Instance = this;
    }

    public override void _Process(double delta) {
    }

    public override void _PhysicsProcess(double delta) {
        velocity = Vector2.Zero;
        if (Input.IsActionPressed("move_right")) {
            velocity.X += 1;
        }
        if (Input.IsActionPressed("move_left")) {
            velocity.X -= 1;
        }
        if (Input.IsActionPressed("move_down")) {
            velocity.Y += 1;
        }
        if (Input.IsActionPressed("move_up")) {
            velocity.Y -= 1;
        }
        velocity = velocity.Normalized()*Speed;
        Vector2 result = GlobalPosition+velocity*(float) delta;
        MoveAndCollide(velocity);
    }
}
