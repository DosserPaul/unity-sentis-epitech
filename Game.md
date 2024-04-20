# Handwritten Digit Recognition Game

## Overview

This project is a Unity-based game that combines handwriting with machine learning to create a fun and interactive experience. The player is presented with arithmetic problems to solve by writing the numbers on a virtual screen. The game uses an AI model trained on the MNIST dataset to recognize the handwritten digits and validate the player's answers.

## Components

### 1. Player Script (`Player.cs`)

- **Movement**: The player can move in different directions using the arrow keys.
- **Jumping**: The player can jump by pressing the spacebar.
- **Pushing Objects**: The player can push objects when colliding with them.

### 2. Game Manager Script (`Game.cs`)

- **Player Control**: Allows the player to control the character's movement and view.
- **Game Modes**: Supports two modes: WALK and CONTROL. In the CONTROL mode, the player can use the mouse to interact with the environment.

### 3. MNIST AI Script (`MnistAI.cs`)

- **TensorFlow Integration**: Uses the Unity.Sentis library to load and run an ONNX model for digit recognition.
- **Mouse Input**: Handles mouse input to trigger digit recognition on a virtual screen.

### 4. Screen Script (`Screen.cs`)

- **Drawing Interface**: Provides a canvas where the player can draw digits using the mouse.
- **Arithmetic Problems**: Generates arithmetic problems for the player to solve.
- **Inference**: Calls the MNIST AI to recognize the handwritten digits and validate the player's answers.

## Workflow

1. **Player Movement**: The player moves to the screen and selects a location to write.
2. **Handwriting**: The player draws the digits corresponding to the arithmetic problem presented.
3. **Digit Recognition**: The MNIST AI script processes the handwritten digits and provides a prediction.
4. **Validation**: The predicted digit is compared with the expected result of the arithmetic problem.
5. **Feedback**: If the prediction is correct, the player is notified, and a new problem is presented.

## Game Modes

- **WALK Mode**: Standard movement controls with keyboard and mouse.
- **CONTROL Mode**: Allows the player to use the mouse to interact with the game environment, including drawing on the screen.

## Dependencies

- Unity Engine
- Unity.Sentis for TensorFlow integration
- ONNX model for MNIST digit recognition

## Future Improvements

- Implementing a scoring system to track the player's progress.
- Adding levels with increasing difficulty.
- Enhancing the drawing interface for better user experience.


