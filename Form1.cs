
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void PlotButton_Click(object sender, EventArgs e)
        {
            List<PointF> points = new List<PointF>();
            //List<int> rowIndex = new();
            string outPut = "Некорректные значения в строках: ";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    float x, y;
                    if (float.TryParse(row.Cells[0].Value?.ToString(), out x) &&
                        float.TryParse(row.Cells[1].Value?.ToString(), out y))
                    {
                        points.Add(new PointF(x, y));
                    }
                    else
                    {
                        outPut += $"{row.Index + 1 } ";
                    }
                        
                   
                }
            }
            if(dataGridView1.Rows.Count == 1)
            {
                MessageBox.Show("Введите данные");
                return;
            }
            if (points.Count == 0 || points.Count == 1)
            {

                MessageBox.Show(outPut + ".");
            }
            grafik.PlotPoints(points.ToArray());
        }


    }
}

