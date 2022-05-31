using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private const int imsz = 320;
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
        public List<Mat> objectSegmentation(Mat img)
        {
            List<Mat> masks = new List<Mat>();

            using(Mat image = new Mat())
            {
                Size originSize = img.Size();
                Cv2.Resize(img, image, imgSize);

                using (var data = DataPreprocessing(image))
                {
                    var input = new DenseTensor<float>(MatToList(data), new[] { 1, 3, imgSize.Height, imgSize.Width });
                    // Setup inputs and outputs
                    var inputs = new List<NamedOnnxValue>
                    {
                        NamedOnnxValue.CreateFromTensor("input.1", input)
                    };
                    using (var results = sess.Run(inputs))
                    {
                        //Postprocessing
                        var resultsArray = results.ToArray();
                        var pred_value = resultsArray[0].AsEnumerable<float>().ToArray();
                        var pred_dim = resultsArray[0].AsTensor<float>().Dimensions.ToArray();
                        var output = ConvertSegmentationResult(pred_value, pred_dim, MinConfidence);
                        for (int i = 0; i < output[0].Count; i++)
                        {
                            Mat resizeMat = new Mat();
                            Cv2.Resize(output[0][i], resizeMat, originSize, 0, 0, InterpolationFlags.Cubic);
                            //Cv2.NamedWindow("Mask"+i.ToString(), WindowMode.Normal);
                            //Cv2.ImShow("Mask" + i.ToString(), resizeMat);
                            masks.Add(resizeMat);
                        }
                        output.Clear();
                    }
                }
            }
            return masks;
        }
        private Mat DataPreprocessing(Mat image)
        {
            Mat data = Mat.Zeros(image.Size(), MatType.CV_32FC3);
            using (var rgbImage = new Mat())
            {
                Cv2.CvtColor(image, rgbImage, ColorConversionCodes.BGR2RGB);
                rgbImage.ConvertTo(data, MatType.CV_32FC3, (float)(1 / 255.0));
                var channelData = Cv2.Split(data);
                channelData[0] = (channelData[0] - 0.485) / 0.229;
                channelData[1] = (channelData[1] - 0.456) / 0.224;
                channelData[2] = (channelData[2] - 0.406) / 0.225;
                Cv2.Merge(channelData, data);
            }
            return data;
        }
        public static List<List<Mat>> ConvertSegmentationResult(float[] pred, int[] pred_dim, float threshold = 0.25f)
        {
            List<List<Mat>> dataList = new List<List<Mat>>();
            for (int batch = 0; batch < pred_dim[0]; batch++)
            {
                List<Mat> masks = new List<Mat>();
                for (int cls = 0; cls < pred_dim[1]; cls++)
                {
                    List<byte> subData = new List<byte>();
                    for (int h = 0; h < pred_dim[2]; h++)
                    {
                        for (int w = 0; w < pred_dim[3]; w++)
                        {
                            int idx = (batch * pred_dim[1] * pred_dim[2] * pred_dim[3]) +
                                (cls * pred_dim[2] * pred_dim[3]) + (h * pred_dim[3]) + w;
                            if (pred[idx] < threshold)
                            {
                                subData.Add(0);
                            }
                            else
                            {
                                subData.Add(255);
                            }
                        }
                    }
                    using (var mask = new Mat(new int[] { pred_dim[2], pred_dim[3] }, MatType.CV_8UC1, subData.ToArray()))
                    {
                        masks.Add(mask.Clone());
                    }
                }
                dataList.Add(masks);
            }
            return dataList;
        }
        private unsafe static float[] Create(float* ptr, int ih, int iw, int chn)
        {
            float[] array = new float[ih * iw * chn];

            for (int y = 0; y < ih; y++)
            {
                for (int x = 0; x < iw; x++)
                {
                    for (int c = 0; c < chn; c++)
                    {
                        var idx = (y * chn) * iw + (x * chn) + c;
                        var idx2 = (c * iw) * ih + (y * iw) + x;
                        array[idx2] = ptr[idx];
                    }
                }
            }
            return array;
        }
        private static float[] MatToList(Mat mat)
        {
            var ih = mat.Height;
            var iw = mat.Width;
            var chn = mat.Channels();
            unsafe
            {
                return Create((float*)mat.DataPointer, ih, iw, chn);
            }
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
