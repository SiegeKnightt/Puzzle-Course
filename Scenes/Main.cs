using Godot;
using System;

namespace Game;

public partial class Main : Node2D {

	private Sprite2D sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {

		sprite = GetNode<Sprite2D>("Cursor");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {

		var mousePosition = GetGlobalMousePosition();

		// Divides the x and y component by 64
		var gridPosition = mousePosition / 64;

		// Floor rounds the component down
		gridPosition = gridPosition.Floor();

		// Multiply position by 64 to ensure 
		// pixel position is aligned with grid
		sprite.GlobalPosition = gridPosition * 64;

		GD.Print(gridPosition);
	}
}
