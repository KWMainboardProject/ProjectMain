using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using OpenCvSharp;
using System.Threading;

namespace RequestTaskProcessing.StrategyOperator.SubCategory
{
    class TopClassification : IStrategyOperateAble
    {
        private Mat imageFloat = null;
        public Size imgSize = new Size(224, 224);
        private Scalar padColor = new Scalar(114, 114, 114);

        private string result_label = null;
        private float result_conf = 0;

        InferenceSession session = null;

        protected TopClassification()
        {
            //plz set gpu device
            ClearResource();

            var option = new SessionOptions();
            option.GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL;
            option.ExecutionMode = ExecutionMode.ORT_SEQUENTIAL;
            //Set cuda (gpu) device
            //option.AppendExecutionProvider_CUDA(0);

            session = new InferenceSession(ShareWorkPath.GetInstance().WEIGHT_PATH + @"\Top_classification.onnx", option);
        }

        public void classification(string imagePath)
        {
            string imageFilePath = imagePath;
            imageFloat = new Mat();
            float ratio = 0.0f;
            Point diff1 = new Point();
            Point diff2 = new Point();
            bool isAuto = true;


            Mat img = Cv2.ImRead(imageFilePath);
            var letterimg = CreateLetterbox(img, imgSize, padColor, out ratio, out diff1, out diff2, auto: isAuto, scaleFill: !isAuto);

            //{
            var dW = imgSize.Width - letterimg.Width;
            var dH = imgSize.Height - letterimg.Height;
            var dW_h = (int)Math.Round((float)dW / 2);
            var dH_h = (int)Math.Round((float)dH / 2);
            Cv2.CopyMakeBorder(letterimg, letterimg, dH_h, dH_h, dW_h, dW_h, BorderTypes.Constant, padColor);
            //}
            Cv2.Resize(img, img, imgSize);


            letterimg.ConvertTo(imageFloat, MatType.CV_32FC3, (float)(1 / 255.0));
            Mat data = Mat.Zeros(letterimg.Size(), MatType.CV_32FC3);
            using (var rgbImage = new Mat())
            {
                Cv2.CvtColor(letterimg, rgbImage, ColorConversionCodes.BGR2RGB);
                rgbImage.ConvertTo(data, MatType.CV_32FC3, (float)(1 / 255.0));
                var channelData = Cv2.Split(data);
                channelData[0] = (channelData[0] - 0.485) / 0.229;
                channelData[1] = (channelData[1] - 0.456) / 0.224;
                channelData[2] = (channelData[2] - 0.406) / 0.225;
                Cv2.Merge(channelData, data);
            }
            var input = new DenseTensor<float>(MatToList(data), new[] { 1, 3, 224, 224 });
            var inputs = new List<NamedOnnxValue>
            { NamedOnnxValue.CreateFromTensor("input", input)};

            //var option = new SessionOptions();
            //option.GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL;
            //option.ExecutionMode = ExecutionMode.ORT_SEQUENTIAL;
            ////Set cuda (gpu) device
            ////option.AppendExecutionProvider_CUDA(0);

            //var session = new InferenceSession(modelFilePath, option);
            IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);
            IEnumerable<float> output = results.First().AsEnumerable<float>();
            float sum = output.Sum(x => (float)Math.Exp(x));
            IEnumerable<float> softmax = output.Select(x => (float)Math.Exp(x) / sum);

            IEnumerable<Prediction> top10 = softmax.Select((x, i) => new Prediction { Label = LabelMap.Tops[i], Confidence = x })
                                       .OrderByDescending(x => x.Confidence)
                                       .Take(1);

            Console.WriteLine("Top 1 predictions for ResNet50 v2...");
            Console.WriteLine("--------------------------------------------------------------");
            foreach (var t in top10)
            {
                Console.WriteLine($"Label: {t.Label}, Confidence: {t.Confidence}");
                this.result_label = t.Label;
                this.result_conf = t.Confidence;
            }
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

        public static Mat CreateLetterbox(Mat img, Size sz, Scalar color, out float ratio, out Point diff, out Point diff2,
            bool auto = true, bool scaleFill = false, bool scaleup = true)
        {
            Mat newImage = new Mat();
            Cv2.CvtColor(img, newImage, ColorConversionCodes.BGR2RGB);
            ratio = Math.Min((float)sz.Width / newImage.Width, (float)sz.Height / newImage.Height);
            if (!scaleup)
            {
                ratio = Math.Min(ratio, 1.0f);
            }
            var newUnpad = new OpenCvSharp.Size((int)Math.Round(newImage.Width * ratio),
                (int)Math.Round(newImage.Height * ratio));
            var dW = sz.Width - newUnpad.Width;
            var dH = sz.Height - newUnpad.Height;

            var tensor_ratio = sz.Height / (float)sz.Width;
            var input_ratio = img.Height / (float)img.Width;
            if (auto && tensor_ratio != input_ratio)
            {
                dW %= 32;
                dH %= 32;
            }
            else if (scaleFill)
            {
                dW = 0;
                dH = 0;
                newUnpad = sz;
            }
            var dW_h = (int)Math.Round((float)dW / 2);
            var dH_h = (int)Math.Round((float)dH / 2);
            var dw2 = 0;
            var dh2 = 0;
            if (dW_h * 2 != dW)
            {
                dw2 = dW - dW_h * 2;
            }
            if (dH_h * 2 != dH)
            {
                dh2 = dH - dH_h * 2;
            }

            if (newImage.Width != newUnpad.Width || newImage.Height != newUnpad.Height)
            {
                Cv2.Resize(newImage, newImage, newUnpad);
            }
            Cv2.CopyMakeBorder(newImage, newImage, dH_h + dh2, dH_h, dW_h + dw2, dW_h, BorderTypes.Constant, color);
            diff = new OpenCvSharp.Point(dW_h, dH_h);
            diff2 = new OpenCvSharp.Point(dw2, dh2);
            return newImage;
        }

        public string getLabel()
        {
            return this.result_label;
        }

        public float getConf()
        {
            return this.result_conf;
        }

        public void ClearResource()
        {
            container = new SubCategoryContainer();
        }


        public TaskMessage GetMessage()
        {
            //보내는 메세지
            lock (Holder.instance)
            {
                TaskMessage taskMessage = new TaskMessage(requestMessage);
                taskMessage.type = MessageType.Receive_Container_SubCategory_Top;        //set
                taskMessage.productor = null;                                   //set
                taskMessage.resource = container;
                return taskMessage;
            }
        }

        public void SetResource(TaskMessage message)
        {
            //받는 메세지
            Console.WriteLine("\tplz Set resource");
            requestMessage = new TaskMessage(message);  //메세지 열어서 내용 확인
        }
        protected TaskMessage requestMessage = null;
        protected SubCategoryContainer container;
        

        public void Work()
        {
            lock (Holder.instance)
            {
                //절대 경로
                string img_abs_path = requestMessage.resource.GetValue().ToString();

                Console.WriteLine("Top category img path : " + img_abs_path);
                classification(img_abs_path);

                //결과 (conf, class)
                container.confidenceContainer.SetConfidence(result_conf);
                container.classficationContainer.SetClassfication(result_label);
               
                Thread.Sleep(1000);

                //Set container
                return;
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// singleton pattern
        /// </summary>
        /// <returns></returns>
        public static TopClassification GetInstance()
        {
            return Holder.instance;
        }
        /// <summary>
        /// Lazy Initialization + holder
        /// </summary>
        private static class Holder
        {
            //객체 생성 시 모델 경로와 파일 경로 넣고 싶은데 How?
            public static TopClassification instance = new TopClassification();
        }
    }
}
