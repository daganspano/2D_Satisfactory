using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace _2D_Satisfactory.TitleScreenClasses;

/// <summary>
/// Represents a group of buttons, allowing navigation and selection using mouse, keyboard, or gamepad input.
/// </summary>
public class ButtonGroup
{
    private readonly List<Button> _buttons;
    private int _selectedButtonIndex;
    private bool _wasPreviouslyDown;
    private bool _wasPreviouslyUp;
    private Point _previousMousePosition;

    public ButtonGroup(Dictionary<string, Action> labelsAndActions, int gameWidth, float buttonStartingYPosition)
    {
        _selectedButtonIndex = -1;

        float buttonScale = 0.7f;
        float buttonHeightScaled = 88 * buttonScale;
        int buttonPadding = 8;

        _buttons = new List<Button>();
        int i = 0;
        foreach (var label in labelsAndActions.Keys)
        {
            _buttons.Add(new Button(label, buttonScale, gameWidth, buttonStartingYPosition + i * (buttonPadding + buttonHeightScaled), labelsAndActions[label]));
            i++;
        }
    }

    public void LoadContent(ContentManager content)
    {
        foreach (var button in _buttons) button.LoadContent(content);
    }

    public void Update()
    {
        Point currentMousePosition = Mouse.GetState().Position;

        if (currentMousePosition != _previousMousePosition) UpdateSelectedButtonWithMouse(currentMousePosition);
        else UpdateSelectedButtonWithoutMouse();

        _previousMousePosition = currentMousePosition;

        for (int i = 0; i < _buttons.Count; i++) _buttons[i].Update(_selectedButtonIndex == i);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var button in _buttons) button.Draw(spriteBatch);
    }

    private void UpdateSelectedButtonWithMouse(Point currentMousePosition)
    {
        _selectedButtonIndex = -1;

        for (int i = 0; i < _buttons.Count; i++)
        {
            var button = _buttons[i];
            if (!(currentMousePosition.X < button.ButtonPosition.X 
                || currentMousePosition.X > button.ButtonPosition.X + button.ButtonWidthScaled 
                || currentMousePosition.Y < button.ButtonPosition.Y 
                || currentMousePosition.Y > button.ButtonPosition.Y + button.ButtonHeightScaled
            ))
            {
                _selectedButtonIndex = i;
                break;
            }
        }
    }

    private void UpdateSelectedButtonWithoutMouse()
    {
        // Get whether the user is pressing up or down on the keyboard
        (bool isKeyboardDown, bool isKeyboardUp) = GetKeyboardInputState();

        // Get whether the user is pressing up or down on the gamepad
        (bool isGamePadDown, bool isGamePadUp) = GetGamePadInputState();

        // Get whether to move the selection up or down based on keyboard or gamepad input
        bool isDown = isKeyboardDown || isGamePadDown;
        bool isUp = isKeyboardUp || isGamePadUp;

        // Update the selected button index based on input, wrapping around if necessary
        if (isDown && !_wasPreviouslyDown)
            _selectedButtonIndex = (_selectedButtonIndex + 1) % _buttons.Count;
        else if (isUp && !_wasPreviouslyUp)
            _selectedButtonIndex = (_selectedButtonIndex - 1 + _buttons.Count) % _buttons.Count;

        // Set previous state for next update
        _wasPreviouslyDown = isDown;
        _wasPreviouslyUp = isUp;
    }

    private (bool, bool) GetKeyboardInputState()
    {
        KeyboardState keyboardState = Keyboard.GetState();
        bool isTabDown = keyboardState.IsKeyDown(Keys.Tab);
        bool isLeftShiftDown = keyboardState.IsKeyDown(Keys.LeftShift);
        bool isKeyboardDown = (isTabDown && !isLeftShiftDown) || keyboardState.IsKeyDown(Keys.Down);
        bool isKeyboardUp = (isTabDown && isLeftShiftDown) || keyboardState.IsKeyDown(Keys.Up);
        return (isKeyboardDown, isKeyboardUp);
    }

    private (bool, bool) GetGamePadInputState()
    {
        GamePadState currentGamePadState = GamePad.GetState(PlayerIndex.One);
        Vector2 thumbStickDirection = currentGamePadState.ThumbSticks.Left;
        bool isGamePadDown = currentGamePadState.IsButtonDown(Buttons.DPadDown) || (thumbStickDirection.Y < 0);
        bool isGamePadUp = currentGamePadState.IsButtonDown(Buttons.DPadUp) || (thumbStickDirection.Y > 0);
        return (isGamePadDown, isGamePadUp);
    }
}