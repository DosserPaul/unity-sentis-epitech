# MnistAI Class Explanation

## Overview

The `MnistAI` class is responsible for integrating the MNIST AI model into the Unity game. It processes the player's drawing on a virtual screen to predict the most likely handwritten digit using the trained model.

## Variables

- `inputTensor`: A tensor that holds the input data for the AI model.
- `mnistONNX`: A public `ModelAsset` variable to load the trained MNIST model.
- `ops`: An `Ops` object for performing operations on tensors.
- `engine`: An `IWorker` object that runs the AI model.
- `imageWidth`: The width of the input image for the AI model (28x28 pixels).
- `lookCamera`: The main camera in the scene.
- `backendType`: The backend type for the AI computations (GPUCompute).

## Methods

### `Start()`

- **Purpose**: Initializes the AI model and sets the main camera.
- **Actions**:
  - Calls `InitializeEngine()` to set up the AI model and engine.
  - Assigns the main camera to `lookCamera`.

### `InitializeEngine()`

- **Purpose**: Loads the MNIST model and creates the AI engine and ops.
- **Actions**:
  - Loads the MNIST model using `ModelLoader.Load()`.
  - Creates an `IWorker` (`engine`) and `Ops` (`ops`) for running and processing the AI model.

### `GetMostLikelyDigitProbability(Texture2D drawableTexture)`

- **Purpose**: Processes the player's drawing to predict the most likely digit.
- **Parameters**: 
  - `drawableTexture`: The player's drawing as a `Texture2D`.
- **Actions**:
  - Converts the texture to a tensor (`inputTensor`) suitable for the AI model.
  - Executes the AI model with the input tensor.
  - Processes the output tensor to find the most likely predicted digit and its probability.
- **Returns**: 
  - A tuple containing the probability and the predicted digit.

### `CleanupTensor()`

- **Purpose**: Frees up memory by disposing of the input tensor.

### `Update()`

- **Purpose**: Checks for mouse input.
- **Actions**:
  - Calls `HandleMouseInput()` to check for mouse button events.

### `HandleMouseInput()`

- **Purpose**: Handles mouse button down and hold events.
- **Actions**:
  - Checks for mouse button down and hold events.
  - Calls `ProcessMouseClick()` or `ProcessMouseHold()` accordingly.

### `ProcessMouseClick()`

- **Purpose**: Processes mouse click events.
- **Actions**:
  - Casts a ray from the mouse position in the scene.
  - Checks if the ray hits an object named "Screen".
  - Calls `ScreenMouseDown()` on the `Screen` component of the hit object.

### `ProcessMouseHold()`

- **Purpose**: Processes mouse hold events.
- **Actions**:
  - Similar to `ProcessMouseClick()` but calls `ScreenGetMouse()` while holding the mouse button.

### `OnDestroy()`

- **Purpose**: Cleans up resources when the object is destroyed.
- **Actions**:
  - Calls `CleanupResources()` to clean up tensors, engine, and ops.

### `CleanupResources()`

- **Purpose**: Cleans up tensors, engine, and ops.
- **Actions**:
  - Calls `CleanupTensor()` and disposes of the `engine` and `ops` objects.

## Summary

The `MnistAI` class integrates the MNIST AI model into the Unity game, providing methods for initializing the AI engine, processing player's drawing, and predicting the most likely digit. The AI model runs on a GPU backend for efficient computation. The script also handles mouse input to interact with the virtual screen where the player can draw digits.

