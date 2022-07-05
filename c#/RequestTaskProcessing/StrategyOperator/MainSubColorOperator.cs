using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;
using System.Numerics;
using Accord.MachineLearning;
using System.Linq;

namespace RequestTaskProcessing.StrategyOperator
{
    public class MainSubColorOperator : IStrategyOperateAble
    {
        const double INTENSITY_IMPRESS = 2.55 * 3.0;
        const double SATUATION_IMPRESS = 2.55;
        const double BANDWIDTH = 30.0;
        const int MAN_ITERATIONS = 1000;

        public void ClearResource()
        {
            return;
        }

        public TaskMessage GetMessage()
        {
            TaskMessage taskMessage = new TaskMessage(requestMessage);
            taskMessage.type = MessageType.Receive_Container_Fashion;        //set
            taskMessage.productor = null;                                   //set
            taskMessage.resource = container;                               //set
            return taskMessage;
        }

        public void SetResource(TaskMessage message)
        {
            requestMessage = new TaskMessage(message);
            mainCategoryContainer = requestMessage.resource as MainCategoryContainer;
            container = new MainSubColorContainer();
        }
        protected TaskMessage requestMessage = null;
        protected MainCategoryContainer mainCategoryContainer = null;
        protected MainSubColorContainer container;

        public void Work()
        {
            //load crop img and crop mask
            using (Mat img = Cv2.ImRead(mainCategoryContainer.cropimgPath.Value))
            {
                using(Mat mask = Cv2.ImRead(mainCategoryContainer.cropmaskPath.Value))
                {
                    //img rgb -> hsi
                    //hsi -> nomal xyi
                    using (Mat points = GetPoints(img, mask))
                    {
                        double[][] input = new double[points.Cols][];

                        for (int i = 0; i < points.Cols; i++)
                        {
                            input[i] = new double[3];
                            Vec3d color = points.At<Vec3d>(0, i);
                            input[i][0] = (double)color.Item0;
                            input[i][1] = (double)color.Item1;
                            input[i][2] = (double)color.Item2;
                        }

                        // Create a new Mean-Shift algorithm for 3 dimensional samples

                        MeanShift meanShift = new MeanShift(3, new Accord.Statistics.Distributions.DensityKernels.EpanechnikovKernel(), BANDWIDTH);
                        meanShift.MaxIterations = MAN_ITERATIONS;
                        meanShift.Tolerance = 0.5;
                        meanShift.UseSeeding = true;
                        meanShift.Maximum = 5;

                        // Learn a data partitioning using the Mean Shift algorithm
                        MeanShiftClusterCollection clustering = meanShift.Learn(input);

                        // Predict group labels for each point
                        int[] labels = clustering.Decide(input);

                        //Count labels
                        List<float> labelsPropor = new List<float>();
                        {
                            for(int i=0; i<clustering.Modes.Length; i++)
                            {
                                int cnt = labels.Count<int>(n => n == i);
                                labelsPropor.Add((float)cnt / labels.Length);
                            }
                        }

                        //normalxyi -> hsi
                        using (Mat hsiColor = new Mat(1, clustering.Modes.Length, MatType.CV_8UC3))
                        {
                            int idx = 0;
                            foreach (double[] color in clustering.Modes)
                            {
                                hsiColor.Set(0, idx++, normalxyi2hsi(color[0], color[1], color[2]));
                            }

                            //hsi -> bgr
                            using(Mat rgbCenters = new Mat())
                            {
                                Cv2.CvtColor(hsiColor, rgbCenters, ColorConversionCodes.HSV2BGR);

                                int maxIdx = 0;
                                int secnIdx = 0;

                                //Print color Proporshion
                                for (int i = 0; i < labelsPropor.Count; i++)
                                {
                                    //지금 판별할 것
                                    idx = i;

                                    // 가장큰거 판별
                                    SwapOperator(ref maxIdx, labelsPropor[maxIdx], ref idx, labelsPropor[idx]);

                                    // 다음 큰거 판별
                                    SwapOperator(ref secnIdx, labelsPropor[secnIdx], ref idx, labelsPropor[idx]);

                                    //if(labelsPropor[i] * 100 > 1.0)
                                    //{ 
                                    //   Vec3b bgr = rgbCenters.At<Vec3b>(0, i);
                                    //    Console.WriteLine(i.ToString()
                                    //        + " : (char)" + bgr.Item2
                                    //        + " , (char)" + bgr.Item1
                                    //        + " , (char)" + bgr.Item0
                                    //        + " <= P : " + labelsPropor[i] * 100);

                                    //    using(Mat color = new Mat(500, 500, MatType.CV_8UC3))
                                    //    {
                                    //        color.SetTo(new Scalar(bgr.Item0, bgr.Item1, bgr.Item2));
                                    //        Cv2.NamedWindow(i.ToString()
                                    //        + ":" + bgr.Item2
                                    //        + "," + bgr.Item1
                                    //        + "," + bgr.Item0
                                    //       + " | " + labelsPropor[i] * 100);
                                            
                                    //        Cv2.ImShow(i.ToString()
                                    //        + ":" + bgr.Item2
                                    //        + "," + bgr.Item1
                                    //        + "," + bgr.Item0
                                    //        + " | " + labelsPropor[i] * 100,
                                    //        color);
                                    //    }
                                    //}
                                }
                                //Cv2.ImShow("input", img);
                                //Cv2.WaitKey();
                                Vec3b main = rgbCenters.At<Vec3b>(0, maxIdx);
                                container.main.SetRGB((char)(main[2]), (char)(main[1]), (char)(main[0]));
                                container.main.SetProportion((int)(labelsPropor[maxIdx] * 100));


                                Vec3b sub = rgbCenters.At<Vec3b>(0, secnIdx);
                                container.sub.SetRGB((char)(sub[2]), (char)(sub[1]), (char)(sub[0]));
                                container.sub.SetProportion((int)(labelsPropor[secnIdx]*100));
                            }
                        }
                    }
                }
            }
        }

        static public bool SwapOperator(ref int bigIdx, double bigerNum, ref int smallIdx, double samllNum)
        {
            if (bigerNum < samllNum)
            {
                int temp = bigIdx;
                bigIdx = smallIdx;
                smallIdx = temp;
                return true;
            }
            else return false;
        }
        static public Vec3d hsi2normalxyi(Vec3b hsi)
        {
            Vec3d normalxyi = new Vec3d();
            //seta = math.radians(2 * hsi_color[0])
            double seta = hsi.Item0 * Math.PI / 90.0;


            //z = cmath.rect((hsi_color[1] / SATUATION_IMPRESS), seta)
            Complex z = Complex.FromPolarCoordinates((hsi.Item1 / SATUATION_IMPRESS),seta);

            //normalized_xyi = [z.real, z.imag, hsi_color[2] / INTENSITY_IMPRESS]#intensity 2 divide
            normalxyi.Item0 = z.Real;                            // x
            normalxyi.Item1 = z.Imaginary;                       // y
            normalxyi.Item2 = ((double)hsi.Item2 / (INTENSITY_IMPRESS));   // i

            return normalxyi;
        }

        static public Vec3b normalxyi2hsi(double x, double y, double i)
        {
            Vec3b hsi = new Vec3b();

            //Set complex
            Complex z = new Complex(x, y);

            //Get phi
            double phi = z.Phase;
            phi = (((phi * 90.0 / Math.PI)) + 180.0);
            phi = (phi - (int)phi) + ((int)phi) % 180;

            //Get r
            double r = Complex.Abs(z);
            r = r * SATUATION_IMPRESS;

            //hsi = [phi1, r, min(normalized_xyi_color[2] * INTENSITY_IMPRESS, 255)]
            hsi.Item0 = (byte)phi;                                                  // h
            hsi.Item1 = (byte)r;                                                    // s
            hsi.Item2 = (byte)Math.Min(i * INTENSITY_IMPRESS*3, 255);   // i

            //Console.WriteLine("normalxyi : " + x + "\t" + y + "\t" + i + "\tto hsi :" + hsi.Item0 + "\t" + hsi.Item1 + "\t" + hsi.Item2);
            return hsi;
        }
        static public Vec3b normalxyi2hsi(Vec3d normalxyi)
        {
            Vec3b hsi = new Vec3b();

            //z = complex(normalized_xyi_color[0], normalized_xyi_color[1])
            Complex z = new Complex(normalxyi.Item0, normalxyi.Item1);

            //r, phi = cmath.polar(z)
            //phi1 = (((phi / math.pi) * 90) + 180) % 180
            double phi = z.Phase;
            phi = (((phi * 90 / Math.PI) ) + 180) % 180;

            //r = r * SATUATION_IMPRESS
            double r = Complex.Abs(z);
            r = r * SATUATION_IMPRESS;

            //hsi = [phi1, r, min(normalized_xyi_color[2] * INTENSITY_IMPRESS, 255)]
            hsi.Item0 = (byte)phi;                                                  // h
            hsi.Item1 = (byte)r;                                                    // s
            hsi.Item2 = (byte)Math.Min(normalxyi.Item2 * INTENSITY_IMPRESS, 255);   // i

            return hsi;
        }
        static public Mat GetPoints(Mat src, Mat mask)
        {
            
            const int threshold = 10;

            int clusterCount = 0;
            clusterCount = Math.Min(clusterCount, src.Cols * src.Rows);

            Mat nomalxyiPoints = null;
            //create points
            using (Mat points = Mat.Zeros(1, src.Rows*src.Cols, MatType.CV_8UC3))
            {
                int point_num = 0;
                //masked src & Pointsed src
                for (int y = 0; y < src.Rows; y++)
                {
                    int i = y * src.Cols;
                    for (int x = 0; x < src.Cols; x++)
                    {
                        int r = i + x;
                        if (mask.At<byte>(y, x) > threshold)
                        {
                            Vec3b bgr = src.At<Vec3b>(y, x);
                            points.Set<Vec3b>(0, point_num, new Vec3b(bgr[0], bgr[1], bgr[2]));

                            point_num++;//count point num
                        }
                    }
                }
                using (Mat hsi = new Mat())
                {
                    //img rgb -> hsi
                    Cv2.CvtColor(points, hsi, ColorConversionCodes.BGR2HSV);


                    nomalxyiPoints = new Mat(1, point_num, MatType.CV_64FC3);
                    for(int i=0; i<nomalxyiPoints.Cols; i++)
                    {
                        nomalxyiPoints.Set<Vec3d>(0, i,
                            hsi2normalxyi(hsi.At<Vec3b>(0, i)));
                        
                    }
                }
            }

            return nomalxyiPoints;
        }
        

    }//*/
}
