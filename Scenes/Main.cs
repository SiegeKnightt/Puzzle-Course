using Godot;
using System;

namespace Game;

public partial class Main : Node2D {

	private Sprite2D cursor;

	// PackedScene containes the data required
	// to instantiate a scene
	private PackedScene buildingScene;

	private Button placeBuildingButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {

		buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");

		placeBuildingButton = GetNode<Button>("PlaceBuildingButton");

		cursor = GetNode<Sprite2D>("Cursor");
		cursor.Visible = false;

		// Both of these do the same thing
		// The first is the new recommended way (with some issues)
		// The second method is traditional way
		placeBuildingButton.Pressed += OnButtonPressed;
		// placeBuildingButton.Connect(Button.SignalName.Pressed, Callable.From(OnButtonPressed));
	}

	public override void _UnhandledInput(InputEvent @event) {

		if (@event.IsActionPressed("left_click") && cursor.Visible) {

			PlaceBuildingAtMousePosition();

			// Resets the ability to place a building
			cursor.Visible = false;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {

		var gridPosition = GetMouseGridCellPosition();

		// Multiply position by 64 to ensure 
		// pixel position is aligned with grid
		cursor.GlobalPosition = gridPosition * 64;
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

	private void OnButtonPressed() {

		cursor.Visible = true;
	}
}
