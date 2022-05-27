using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.Text;
using RequestTaskProcessing.StrategyOperator;
using RequestTaskProcessing;

namespace RequestTaskProcessing.StrategyOperator.U2Net
{
    public class RembgU2Net : IDisposable
    {
        private InferenceSession sess = null;
        private Mat imageFloat = null;
        private Mat debugImage = null;
        public float MinConfidence { get; set; }
        public float NmsThresh { get; set; }
        private float maxWH = 4096;
        public Size imgSize = new Size(IMG_SIZE, IMG_SIZE);
        private Scalar padColor = new Scalar(pclr, pclr, pclr);

        private const int imsz = 1200;
        private const int pclr = 114;

        public RembgU2Net(string model_path, int device_num = 0)
        {
            var option = new SessionOptions();
            option.GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL;
            option.ExecutionMode = ExecutionMode.ORT_SEQUENTIAL;
            //Set cuda (gpu) device
            //option.AppendExecutionProvider_CUDA(0);

            sess = new InferenceSession(model_path, option);

            InitResource();
            MinConfidence = 0.2f;
            NmsThresh = 0.4f;
        }
        public void InitResource()
        {
            imageFloat = new Mat();
            debugImage = new Mat();
        }
        public static int IMG_SIZE
        {
            get { return imsz; }
        }
        public static int PAD_COLOR
        {
            get { return pclr; }
        }

        public void Dispose()
        {
            debugImage?.Dispose();
            debugImage = null;
            imageFloat?.Dispose();
            imageFloat = null;
            sess?.Dispose();
            sess = null;
        }
    }
}
