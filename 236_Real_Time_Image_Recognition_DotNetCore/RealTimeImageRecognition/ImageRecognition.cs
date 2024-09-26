using System;
using Tensorflow;
using Tensorflow.NumPy;
using OpenCvSharp;
using System.Drawing;
using System.IO;
using static Tensorflow.Binding;

public class ImageRecognition
{
    private readonly string baseDir = AppDomain.CurrentDomain.BaseDirectory;
    // Model file and labels file paths
    private readonly string modelFile; // Path to the frozen MobileNetV2 model
    private readonly string labelsFile; // Path to the labels file
    private readonly int inputHeight = 224; // Height of the input image
    private readonly int inputWidth = 224; // Width of the input image
    private readonly float inputMean = 128f; // Mean value for normalization
    private readonly float inputStd = 128f; // Standard deviation for normalization

    private Graph graph; // Graph to hold the model
    private Session session; // Session to run the model

    // Constructor to initialize the model and session
    public ImageRecognition()
    {
        modelFile = Path.Combine(baseDir, "models", "mobilenet_v2_1.4_224_frozen.pb");
        labelsFile = Path.Combine(baseDir, "models", "labels.txt");
        graph = new Graph().as_default(); // Create a new graph and set it as default
        graph.Import(modelFile); // Import the model into the graph
        session = tf.Session(graph); // Create a new session with the graph
    }

    // Load and preprocess the image for prediction
    public Tensor LoadImage(string filePath)
    {
        var bitmap = new Bitmap(filePath); // Load the image as a Bitmap
        // Resize the image to the required input dimensions
        var resized = new Bitmap(bitmap, new System.Drawing.Size(inputWidth, inputHeight));
        // Create a zero-initialized tensor for the input
        var matrix = np.zeros(new Shape(1, inputHeight, inputWidth, 3), np.float32);

        // Loop through each pixel in the resized image to normalize and store it in the tensor
        for (int y = 0; y < inputHeight; y++)
        {
            for (int x = 0; x < inputWidth; x++)
            {
                var color = resized.GetPixel(x, y); // Get the pixel color
                // Normalize the pixel values and store them in the tensor
                matrix[0, y, x, 0] = (color.R - inputMean) / inputStd; // Red channel
                matrix[0, y, x, 1] = (color.G - inputMean) / inputStd; // Green channel
                matrix[0, y, x, 2] = (color.B - inputMean) / inputStd; // Blue channel
            }
        }
        return matrix; // Return the preprocessed tensor
    }

    // Read the labels from the labels file
    public string[] ReadLabels()
    {
        return File.ReadAllLines(labelsFile); // Read all lines from the labels file and return as an array
    }

    // Predict the label for a given image path
    public string Predict(string imagePath)
    {
        var tensor = LoadImage(imagePath); // Load and preprocess the image

        // Retrieve the input and output tensors from the graph using their names
        var inputTensor = graph.OperationByName("input").output; // Input tensor
        var outputTensor = graph.OperationByName("MobilenetV2/Predictions/Reshape_1").output; // Output tensor

        // Run the session with the input tensor and get the prediction results
        var results = session.run(
            new[] { outputTensor }, // Specify the output tensor to retrieve
            new[] { new FeedItem(inputTensor, tensor) } // Feed the preprocessed tensor into the input tensor
        );

        // Convert the output results to an array of probabilities
        var probabilities = results[0].ToArray<float>();
        var labels = ReadLabels(); // Get the labels
        var bestIndex = Array.IndexOf(probabilities, probabilities.Max()); // Get the index of the highest probability

        // Check if the index is within the bounds of the labels array
        if (bestIndex >= labels.Length)
        {
            Console.WriteLine("Prediction index out of bounds. Check the number of labels."); // Log a warning if out of bounds
            return "Unknown"; // Return 'Unknown' if the prediction is invalid
        }
        return labels[bestIndex]; // Return the predicted label
    }

    // Process multiple images for recognition
    public void RecognizeFromImages(string[] imagePaths)
    {
        // Loop through each image path to perform recognition
        foreach (var imagePath in imagePaths)
        {
            Console.WriteLine($"Processing image: {imagePath}"); // Log the image being processed
            var label = Predict(imagePath); // Get the prediction for the image
            Console.WriteLine($"Prediction for {imagePath}: {label}"); // Log the prediction result
        }

        Cv2.DestroyAllWindows(); // Close all OpenCV windows after processing
    }
}