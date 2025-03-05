using Godot;
using System;
using System.Collections.Generic;

namespace Game;

public partial class Main : Node2D {

	private Sprite2D cursor;

	// PackedScene containes the data required
	// to instantiate a scene
	private PackedScene buildingScene;

	private Button placeBuildingButton;

	private TileMapLayer highlightTileMapLayer;

	private Vector2? hoveredGridCell;

	private HashSet<Vector2> occupiedCells = new HashSet<Vector2>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {

		buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");

		placeBuildingButton = GetNode<Button>("PlaceBuildingButton");

		highlightTileMapLayer = GetNode<TileMapLayer>("HighlightTileMapLayer");

		cursor = GetNode<Sprite2D>("Cursor");
		cursor.Visible = false;

		// Both of these do the same thing
		// The first is the new recommended way (with some issues)
		// The second method is traditional way
		placeBuildingButton.Pressed += OnButtonPressed;
		// placeBuildingButton.Connect(Button.SignalName.Pressed, Callable.From(OnButtonPressed));
	}

	public override void _UnhandledInput(InputEvent @event) {

		if (@event.IsActionPressed("left_click") && cursor.Visible && !occupiedCells.Contains(GetMouseGridCellPosition())) {

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

		if (cursor.Visible && (!hoveredGridCell.HasValue || hoveredGridCell.Value != gridPosition)) {

			hoveredGridCell = gridPosition;

			UpdateHighlightTileMapLayer();
		} 
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

		// Adds the position to the hash set if it doesn't exist
		occupiedCells.Add(gridPosition);

		// Reset hovered grid cell and the tilemap
		hoveredGridCell = null;
		UpdateHighlightTileMapLayer();
	}

	private void UpdateHighlightTileMapLayer() {

		highlightTileMapLayer.Clear();

		if (!hoveredGridCell.HasValue) {

			return;
		}

		for (float x = hoveredGridCell.Value.X - 3; x <= hoveredGridCell.Value.X + 3; x++) {
			for (float y = hoveredGridCell.Value.Y - 3; y <= hoveredGridCell.Value.Y + 3; y++) {

				highlightTileMapLayer.SetCell(new Vector2I((int)x, (int)y), 0, Vector2I.Zero);
			}
		}
	}

	private void OnButtonPressed() {

		cursor.Visible = true;
	}
}
