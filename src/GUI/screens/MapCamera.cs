using Godot;
using System;

public class MapCamera : Camera2D
{
    bool isPressed; // if middle click is pressed or not 
    [Export] Vector2 zoomConstant = new Vector2(0.05f, 0.05f); // how much we zoom in and out at a time
    [Export] float scrollConstant = 1.5f; // how much we scroll at a time

    public override void _Ready()
    {
        
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("zoom_in"))
        {
            // don't zoom in past 0
            if (Zoom > zoomConstant)
            {
                Zoom -= zoomConstant;
            }
        }
        else if (@event.IsActionPressed("zoom_out"))
        {
            // don't zoom out past 1
            if (Zoom <= Vector2.One - zoomConstant)
            {
                Zoom += zoomConstant;
            }
        }

        if (@event is InputEventMouseMotion)
        {
            if (isPressed)
            {
                // event.relative is what direction the mouse is moving
			    // multiply by zoom so that it isn't hard to control when we're zoomed in a lot
			    // multiply by -1 to reverse direction; when the mouse goes left, the camera goes right   
                Position += ((InputEventMouseMotion)@event).Relative * Zoom * -1;
            }
        }

        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton mEvent = (InputEventMouseButton)@event;
            if (mEvent.ButtonIndex == 3 || mEvent.ButtonIndex == 2)
            {
                isPressed = mEvent.Pressed;
            }
        }
    }

    public override void _Process(float delta)
    {
        float scrollAmt = scrollConstant * Zoom.x;

        if (Input.IsActionPressed("up"))
            Position = new Vector2(Position.x, Position.y - scrollAmt);
        if (Input.IsActionPressed("down"))
            Position = new Vector2(Position.x, Position.y + scrollAmt);
        if (Input.IsActionPressed("left"))
            Position = new Vector2(Position.x - scrollAmt, Position.y);
        if (Input.IsActionPressed("right"))
            Position = new Vector2(Position.x + scrollAmt, Position.y);
    }
}