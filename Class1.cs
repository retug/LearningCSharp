using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Text;


namespace WindowsFormsApp1
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        //This is the constructor, redefine the point?

        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class PointVector
    {
        public List<double> XYZ {get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }


        //This is the constructor, redefine the point?

        public PointVector(List<double> xyz)
        {
            X = xyz[0];
            Y = xyz[1];
            Z = xyz[2];
        }
    }
    public class GlobalCoordinateSystem
    {
        public List<double> XYZ { get; set; }
        public List<double> Vector { get; set; }
        public double hyp { get; set; }
        public double[,] R { get; set; }
        public string inverseMatrixText { get; set; }
        public Matrix<double> customMatrix { get; set; }
        //This is the constructor, redefine the point?
        public GlobalCoordinateSystem(List<double> xyz, List<double> vector)
        {
            hyp = Math.Sqrt((vector[0]* vector[0] + vector[1]* vector[1]));

            R = new double[,] { { vector[0] / hyp, -vector[1] / hyp, 0 }, { vector[1] / hyp, vector[0] / hyp, 0 }, { 0, 0, 1 } };

            //Matrix<double> customMatrix = Matrix<double>.Build.DenseOfArray(R);
            //Matrix<double> inverseMyCustomMatrix = customMatrix.Inverse();

            //string inverseMatrixText = customMatrix.ToString("F2");


        }
    }

}
    
