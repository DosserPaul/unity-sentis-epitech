using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Sentis;

public class Screen : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject screen;
    public Text probabilityText;
    public Text calcule;

    [Header("AI Settings")]
    public MnistAI mnist;
    public System.Action<int, float> callback;

    [Header("Drawing Settings")]
    private const int imageWidth = 50;
    private Texture2D drawableTexture;
    private byte[] zeroes = new byte[imageWidth * imageWidth * 3];
    private Vector3 lastCoord;

    [Header("Calculation Settings")]
    private int result;
    private int predictedNumber;
    private float probability;

    [Header("Timing Settings")]
    private float timeOfLastEntry = float.MaxValue;
    private const float clearTime = 0.5f;

    private Camera lookCamera;

    private void Start()
    {
        lookCamera = Camera.main;
        InitializeTexture();
        GenerateCalcule();
    }

    private void InitializeTexture()
    {
        drawableTexture = new Texture2D(imageWidth, imageWidth, TextureFormat.RGB24, false)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Point
        };

        ClearTexture();
        screen.GetComponent<Renderer>().material.SetTexture("_EmissionMap", drawableTexture);
    }

    private void GenerateCalcule()
    {
        System.Random rand = new System.Random();
        
        int num1 = rand.Next(0, 5);
        int num2 = rand.Next(1, 4);
        char[] operators = { '+', '-', '*' };
        char op = operators[rand.Next(operators.Length)];

        ValidateOperation(ref num1, ref num2, ref op);

        result = CalculateResult(num1, num2, op);
        calcule.text = $"Find the result of {num1} {op} {num2} = ?";
    }

    private void ValidateOperation(ref int num1, ref int num2, ref char op)
    {
        if ((op == '-' || op == '*') && (num1 * num2 > 9 || num1 - num2 < 0))
        {
            op = '+';
        }

        if (op == '-')
        {
            SwapNumbers(ref num1, ref num2);
        }
    }

    private int CalculateResult(int num1, int num2, char op)
    {
        switch (op)
        {
            case '+':
                return num1 + num2;
            case '-':
                return num1 - num2;
            case '*':
                return num1 * num2;
            default:
                return 0;
        }
    }

    private void SwapNumbers(ref int num1, ref int num2)
    {
        if (num1 < num2)
        {
            int temp = num1;
            num1 = num2;
            num2 = temp;
        }
    }

    private void ClearTexture()
    {
        drawableTexture.LoadRawTextureData(zeroes);
        drawableTexture.Apply();
    }

    private void Infer()
    {
        var probabilityAndIndex = mnist.GetMostLikelyDigitProbability(drawableTexture);
        probability = probabilityAndIndex.Item1;
        predictedNumber = probabilityAndIndex.Item2;

        UpdateProbabilityText();

        Debug.Log("Predicted Number: " + predictedNumber + " with probability: " + probability);

        if (probability > 0.9f && predictedNumber == result)
        {
            Debug.Log("Correct");
            GenerateCalcule();
            ClearTexture();
        }
    }

    private void UpdateProbabilityText()
    {
        if (probabilityText) 
        {
            probabilityText.text = Mathf.Floor(probability * 100) + "%";
        }
    }

    private void DrawLine(Vector3 startp, Vector3 endp)
    {
        int steps = (int)((endp - startp).magnitude * 2 + 1);
        
        for (float a = 0; a <= steps; a++)
        {
            float t = a * 1f / steps;
            DrawPoint(Vector3.Lerp(startp, endp, t), 2, Color.white);
        }
    }

    private void DrawPoint(Vector3 coord, int thickness, Color color)
    {
        int x = Mathf.Clamp((int)coord.x, thickness, imageWidth - thickness);
        int y = Mathf.Clamp((int)coord.y, thickness, imageWidth - thickness);

        for (int i = x - thickness; i <= x + thickness; i++)
        {
            for (int j = y - thickness; j <= y + thickness; j++)
            {
                DrawPixel(i, j, color);
            }
        }
    }

    private void DrawPixel(int x, int y, Color color)
    {
        drawableTexture.SetPixel(x, y, color);
    }

    public void ScreenMouseDown(RaycastHit hit)
    {
        Vector2 uv = hit.textureCoord;
        lastCoord = uv * imageWidth;
        timeOfLastEntry = Time.time;
    }

    public void ScreenGetMouse(RaycastHit hit)
    {
        Vector2 uv = hit.textureCoord;
        Vector3 coords = uv * imageWidth;

        DrawLine(lastCoord, coords);
        lastCoord = coords;
        drawableTexture.Apply();

        timeOfLastEntry = Time.time;
        Infer();
    }

    private void Update()
    {
        CheckForClearing();
    }

    private void CheckForClearing()
    {
        if ((Time.time - timeOfLastEntry) > clearTime)
        {
            if (callback != null) callback(predictedNumber, probability);
            ClearTexture();
            timeOfLastEntry = float.MaxValue;
        }
    }
}
