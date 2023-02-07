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
           
            Point myPoint = new Point(1.2,0.6,0.25);


            //this syntax is a bit confusing
            List<double> valueXYZ = new List<double>() { 1.0, 0.0, 0.0 };
            
            PointVector pointVector = new PointVector(valueXYZ);

            List<double> vector = new List<double>() { 1.0, 1.0, 0 };

            GlobalCoordinateSystem globalCoordinateSystem = new GlobalCoordinateSystem(valueXYZ, vector);
            int x = 2;
            //MessageBox.Show(globalCoordinateSystem.R.ToString());




        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(name);
        }
    }
}
