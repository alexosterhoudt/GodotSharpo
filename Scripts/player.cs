using Godot;
using System;

public partial class player : CharacterBody2D
{

	const float SPEED = 300f;
	public AnimatedSprite2D animatedSprite;
	bool isAttacking = false;
	[Export]
	public int JumpImpulse {get; set;} = -400;
	[Export]
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	private Vector2 targetVelocity = Vector2.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		
		if(Input.IsKeyPressed(Key.Escape))
		{
			GetTree().Quit();

		}

		var direction = Input.GetAxis("move_left", "move_right");

		if(direction > 0)
		{
			animatedSprite.FlipH = false;
		}
		else if(direction < 0)
		{
			animatedSprite.FlipH = true;
		}

		if(isAttacking == false)
		{

			if(direction == 0 && IsOnFloor())
			{
				animatedSprite.Play("Idle");
			}
			else if(direction != 0 && IsOnFloor())
			{
				animatedSprite.Play("Run");
			}
			else if(!IsOnFloor())
			{
				animatedSprite.Play("Jump");
			}
		}
		
		if(isAttacking && IsOnFloor())
		{
			targetVelocity.X = 0;
		}

		targetVelocity.X = direction * SPEED;
		
		if(!IsOnFloor())
		{
			targetVelocity.Y += Gravity * (float)delta;
		}
		
		horizontal_movement();
		Velocity = targetVelocity;
		MoveAndSlide();
	}

	public void horizontal_movement()
	{
		var input = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
		
		targetVelocity.X = input * SPEED;
	}

	public override void _Input(InputEvent @event)
	{
		if(Input.IsActionJustPressed("attack"))
		{
			isAttacking = true;
			Velocity = Vector2.Zero;
			
			animatedSprite.Play("Attack");
		}
		if(IsOnFloor() && Input.IsActionJustPressed("move_up"))
		{
			targetVelocity.Y = JumpImpulse;
		}
		base._Input(@event);
	}

	private void _on_animated_sprite_2d_animation_finished()
	{
		isAttacking = false;
	}

}



