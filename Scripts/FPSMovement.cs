using Godot;
using System;

public partial class FPSMovement : CharacterBody3D {
	const float speed = 5f;
	const float gravity = 30f;
	const float jumpForce = 10f;
	const float acceleration = 0.5f;
	const float sensitivity = 0.01f;
    float cameraPos = 0;

	public Node3D head;
	public Camera3D camera;
	private Vector3 velocity = Vector3.Zero;

	public override void _Ready() {
		head = GetNode<Node3D>("Head");
		camera = GetNode<Camera3D>("Head/Camera3D");
        cameraPos = camera.Position.Y;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

    public override void _Process(double delta) {
        if (IsOnFloor() && Input.IsActionPressed("crouch")) {
			camera.Position = new Vector3(camera.Position.X, cameraPos-0.5f, camera.Position.Z);
		} else {
			camera.Position = new Vector3(camera.Position.X, cameraPos, camera.Position.Z);
        }
    }

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion) {
			InputEventMouseMotion mouseMotion = @event as InputEventMouseMotion;
			head.RotateY(-mouseMotion.Relative.X * sensitivity);
			camera.RotateX(-mouseMotion.Relative.Y * sensitivity);

			Vector3 cameraRot = camera.Rotation;
			cameraRot.X = Mathf.Clamp(cameraRot.X, Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
			camera.Rotation = cameraRot;
		}
	}

	public override void _PhysicsProcess(double delta) {
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        Vector3 forward = head.Basis.Z * new Vector3(1, 0, 1);
        Vector3 right = head.Basis.X * new Vector3(1, 0, 1);
		Vector3 Direction = (forward * inputDir.Y).Normalized()+(right * inputDir.X).Normalized();

		if (Direction != Vector3.Zero) {
			velocity.X = Direction.X * speed;
			velocity.Z = Direction.Z * speed;
		} else {
			velocity.X = Mathf.MoveToward(Velocity.X, 0, acceleration);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, acceleration);
		}

		if (!IsOnFloor()) {
			velocity.Y -= gravity * (float)delta;
		}

		if (IsOnFloor() && Input.IsActionJustPressed("jump")) {
			velocity.Y = jumpForce;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
