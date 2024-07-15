using Godot;
using System;

public partial class player : CharacterBody2D
{

	const float SPEED = 300f;
	public AnimatedSprite2D animatedSprite;
	bool isAttacking = false;
	[Export]
	public int JumpImpulse {get; set;} = 20;
	[Export]
	public int FallAccel{get; set;} = 75;
	private Vector2 targetVelocity = Vector2.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
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

			if(direction == 0)
			{
				animatedSprite.Play("Idle");
			}
			else
			{
				animatedSprite.Play("Run");
			}
		}

	}
	
	public override void _PhysicsProcess(double delta)
	{
		var direction = Input.GetAxis("move_left", "move_right");
		
		if(isAttacking)
		{
			targetVelocity = Vector2.Zero;
		}

		if(direction != 0)
		{
			targetVelocity = new Vector2(direction, 0) * SPEED;
		}
		else
		{
			targetVelocity = Vector2.Zero;
		}
		
		if(!IsOnFloor())
		{
			targetVelocity.Y += FallAccel * (float)delta;
		}
		
		if(IsOnFloor() && Input.IsActionJustPressed("move_up"))
		{
			targetVelocity.Y = JumpImpulse;
			GD.Print("Jumping");
		}
		Velocity = targetVelocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if(Input.IsActionJustPressed("attack"))
		{
			isAttacking = true;
			Velocity = Vector2.Zero;
			
			animatedSprite.Play("Attack");
		}
		base._Input(@event);
	}

	private void _on_animated_sprite_2d_animation_finished()
	{
		isAttacking = false;
	}

}



