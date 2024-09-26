using System;
using Tensorflow;
using Tensorflow.NumPy;
using OpenCvSharp;
using System.Drawing;
using System.IO;
using static Tensorflow.Binding;

class Program
{
    static void Main(string[] args)
    {
        var recognizer = new ImageRecognition();
        recognizer.RecognizeFromImages(["img1.jpg"]);
    }
}