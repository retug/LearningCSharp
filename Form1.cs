using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string name = "test";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = "John Doe";
           
            //Point myPoint = new Point(1.2,0.6,0.25);

            //this syntax is a bit confusing
            List<double> valueXYZ = new List<double>() { 1.0, 0.0, 0.0 };
            List<double> refPoint1 = new List<double>() { 0.0, 0.0, 0.0 };


            MyPoint myPoint = new MyPoint(valueXYZ);

            //this is the vector that this point lives in local coordinate system
            List<double> vector = new List<double>() { 1.0, 1.0, 0 };
            GlobalCoordinateSystem globalCoordinateSystem = new GlobalCoordinateSystem(refPoint1, vector);
            myPoint.glo_to_loc(globalCoordinateSystem);

            MessageBox.Show("LOCAL - Your X value is = " + myPoint.LocalCoords[0].ToString() + " Your Y value is = " + myPoint.LocalCoords[1].ToString()+ " Your Z value is = " + myPoint.LocalCoords[2].ToString());


            //This line of code converts from the local coordinate system of 1.414, with a 45 degree angle back to 1,1,0
            List<double> valueXYZ1 = new List<double>() { Math.Cos(Math.PI/4), -Math.Cos(Math.PI / 4), 0.0 };

            MyPoint myPoint1 = new MyPoint(valueXYZ1);

            //this is the vector that this point lives in local coordinate system
            List<double> vector1 = new List<double>() { 1.0, 1.0, 0 };

            List<double> refPoint2 = new List<double>() { 0.0, 0.0, 0.0 };

            GlobalCoordinateSystem globalCoordinateSystem1 = new GlobalCoordinateSystem(refPoint2, vector1);
            
            myPoint1.loc_to_glo(globalCoordinateSystem1);



            MessageBox.Show("GLOBAL - Your X value is = " + myPoint1.GlobalCoords[0].ToString() + " Your Y value is = " + myPoint1.GlobalCoords[1].ToString() + " Your Z value is = " + myPoint1.GlobalCoords[2].ToString());
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(name);
        }
    }
}
