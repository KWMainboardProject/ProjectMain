using System;
using System.Collections.Generic;
using System.Text;
//using Accord.MachineLearning;
using OpenCvSharp;
using System.Numerics;

namespace RequestTaskProcessing.StrategyOperator
{
    public class MainSubColorOperator : IStrategyOperateAble
    {
        const double INTENSITY_IMPRESS = 255.0 * 5.0;
        const double SATUATION_IMPRESS = 255.0;

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
            container = new FashionObjectContainer(mainCategoryContainer.GetKey());

            container.SetJObject(mainCategoryContainer.GetJObject());
        }
        protected TaskMessage requestMessage = null;
        protected MainCategoryContainer mainCategoryContainer = null;
        protected FashionObjectContainer container;


        public void Work()
        {
            //load crop img and crop mask
            using (Mat img = Cv2.ImRead(mainCategoryContainer.cropimgPath.Value))
            {
                using(Mat mask = Cv2.ImRead(mainCategoryContainer.cropmaskPath.Value))
                {
                    //img rgb -> hsi
                    //hsi -> nomal xyi
                    using (Mat input = GetPoints(img, mask))
                    {
                        int level = 2;
                        double space_radius = 0.1;
                        double color_radius = 0.1;
                        // mean shift : ouput = nomal xyi
                        using (Mat cluster = input.PyrMeanShiftFiltering(space_radius, color_radius, level,
                            new TermCriteria(CriteriaType.Eps | CriteriaType.Count, 1000, 0.001)))
                        {
                            // output nomal xyi -> rgb
                            using (Mat rgbCluster = new Mat(1, cluster.Cols, MatType.CV_8UC3))
                            {
                                for(int col = 0; col < cluster.Cols; col++)
                                {

                                    rgbCluster.Set<Vec3b>(0, col,
                                        normalxyi2hsi(cluster.At<Vec3f>(0, col)));

                                    //Test
                                    if (col % 10 == 0) Console.WriteLine();
                                    Console.Write(rgbCluster.At<Vec3b>(0, col).ToString() + " ");
                                }
                                // calc proporsion
                                container.color.main.SetDumi();
                                container.color.sub.SetDumi();
                            }
                        }
                    }
                }
            }
        }
        static public Vec3f hsi2normalxyi(Vec3b hsi)
        {
            Vec3f normalxyi = new Vec3f();
            //seta = math.radians(2 * hsi_color[0])
            double seta = 2 * hsi.Item0 * Math.PI / 180.0;


            //z = cmath.rect((hsi_color[1] / SATUATION_IMPRESS), seta)
            Complex z = Complex.FromPolarCoordinates((hsi.Item1 / SATUATION_IMPRESS),seta);


            //normalized_xyi = [z.real, z.imag, hsi_color[2] / INTENSITY_IMPRESS]#intensity 2 divide
            normalxyi.Item0 = (float)z.Real;                            // x
            normalxyi.Item1 = (float)z.Imaginary;                       // y
            normalxyi.Item2 = (float)(hsi.Item2 / INTENSITY_IMPRESS);   // i

            return normalxyi;
        }

        static public Vec3b normalxyi2hsi(Vec3f normalxyi)
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


                    nomalxyiPoints = new Mat(1, point_num, MatType.CV_32FC3);
                    for(int i=0; i<nomalxyiPoints.Cols; i++)
                    {
                        if (i > 301727)
                        {
                            Console.WriteLine(i.ToString() + " : " +
                                hsi.At<Vec3b>(0, i).Item0.ToString()
                                + " , " + hsi.At<Vec3b>(0, i).Item1.ToString()
                                + " , " + hsi.At<Vec3b>(0, i).Item2.ToString());
                        }
                        nomalxyiPoints.Set<Vec3f>(0, i,
                            hsi2normalxyi(hsi.At<Vec3b>(0, i)));
                        
                    }
                }
            }

            return nomalxyiPoints;
        }
        

    }//*/
}
