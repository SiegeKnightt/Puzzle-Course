using Godot;
using System;

namespace Game;

public partial class Main : Node2D {

	private Sprite2D sprite;

	// PackedScene containes the data required
	// to instantiate a scene
	private PackedScene buildingScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {

		buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");

		sprite = GetNode<Sprite2D>("Cursor");
	}

    public override void _UnhandledInput(InputEvent @event) {

		if (@event.IsActionPressed("left_click")) {

			PlaceBuildingAtMousePosition();
		}
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {

		var gridPosition = GetMouseGridCellPosition();

		// Multiply position by 64 to ensure 
		// pixel position is aligned with grid
		sprite.GlobalPosition = gridPosition * 64;

		GD.Print(gridPosition);
	}

	private Vector2 GetMouseGridCellPosition() {

		var mousePosition = GetGlobalMousePosition();

		// Divides the x and y component by 64
		var gridPosition = mousePosition / 64;

		// Floor rounds the component down
		gridPosition = gridPosition.Floor();

		return gridPosition;
	}

	private void PlaceBuildingAtMousePosition() {

		var building = buildingScene.Instantiate<Node2D>();

		// Adds a child of the class into the scene tree
		AddChild(building);

		var gridPosition = GetMouseGridCellPosition();

		building.GlobalPosition = gridPosition * 64;
	}
}
