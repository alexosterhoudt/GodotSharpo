using Godot;
using System;

public partial class player : CharacterBody2D
{

	const float SPEED = 300f;
	public AnimatedSprite2D animatedSprite;
	bool isAttacking = false;

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
		var pos = Velocity.X + direction;

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

		if(isAttacking)
		{
			Velocity = direction * Vector2.Zero;
		}

		if(direction != 0)
		{
			Velocity = new Vector2(direction, 0) * SPEED;
		}
		else
		{
			Velocity = Vector2.Zero;
		}
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



