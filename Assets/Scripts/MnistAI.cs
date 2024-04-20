using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Sentis;

public struct Bounds
{
    public int left;
    public int right;
    public int top;
    public int bottom;
}

public class MnistAI : MonoBehaviour
{
    private TensorFloat inputTensor = null;
    public ModelAsset mnistONNX;
    private Ops ops;
    private IWorker engine;
    private const int imageWidth = 28;
    private Camera lookCamera;
    private static readonly Unity.Sentis.BackendType backendType = Unity.Sentis.BackendType.GPUCompute;

    private void Start()
    {
        InitializeEngine();
        lookCamera = Camera.main;
    }

    private void InitializeEngine()
    {
        Model model = ModelLoader.Load(mnistONNX);
        engine = WorkerFactory.CreateWorker(backendType, model);
        ops = WorkerFactory.CreateOps(backendType, null);
    }

    public (float, int) GetMostLikelyDigitProbability(Texture2D drawableTexture)
    {
        CleanupTensor();
        inputTensor = TextureConverter.ToTensor(drawableTexture, imageWidth, imageWidth, 1);
        engine.Execute(inputTensor);
        
        using (TensorFloat result = engine.PeekOutput() as TensorFloat)
        {
            var probabilities = ops.Softmax(result);
            var indexOfMaxProba = ops.ArgMax(probabilities, -1, false);
            probabilities.MakeReadable();
            indexOfMaxProba.MakeReadable();

            var predictedNumber = indexOfMaxProba[0];
            var probability = probabilities[predictedNumber];

            return (probability, predictedNumber);
        }
    }

    private void CleanupTensor()
    {
        inputTensor?.Dispose();
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ProcessMouseClick();
        }
        else if (Input.GetMouseButton(0))
        {
            ProcessMouseHold();
        }
    }

    private void ProcessMouseClick()
    {
        Ray ray = lookCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.name == "Screen")
        {
            Screen screen = hit.collider.GetComponentInParent<Screen>();
            screen?.ScreenMouseDown(hit);
        }
    }

    private void ProcessMouseHold()
    {
        Ray ray = lookCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.name == "Screen")
        {
            Screen screen = hit.collider.GetComponentInParent<Screen>();
            screen?.ScreenGetMouse(hit);
        }
    }

    private void OnDestroy()
    {
        CleanupResources();
    }

    private void CleanupResources()
    {
        CleanupTensor();
        engine?.Dispose();
        ops?.Dispose();
    }
}
