using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Form1 : Form
    {

        private settingsDialog settings = new settingsDialog();

        public Form1()
        {
            InitializeComponent();

            this.Text = "Lab 6 - Noah Hathout";
            this.Size = new Size(635, 550); 
            this.StartPosition = FormStartPosition.CenterScreen; 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private ArrayList elements = new ArrayList();

        public class Elem
        {
            //base draw method 
            public virtual void Draw(Graphics g)
            {
        
            }
        }

        internal class Line : Elem
        {
            private Point lineStart;
            private Point lineEnd;

            private Pen linePen;

            //constructor for a line
            public Line(Point lineStart, Point lineEnd, Pen linePen)
            {
                this.lineStart = lineStart;
                this.lineEnd = lineEnd;
                this.linePen = linePen;

            }

            //overridden draw for lines
            public override void Draw(Graphics g)
            {
                g.DrawLine(linePen, lineStart, lineEnd);
            }
        }

        internal class Rectangle : Elem
        {
            private Point rectStart;
            private Point rectEnd;

            private Brush rectBrush;

            private Pen rectPen;

            //constructor for rectangle
            public Rectangle(Point rectStart, Point rectEnd, Brush rectBrush, Pen rectPen)
            {
                this.rectStart = rectStart;
                this.rectEnd = rectEnd;
                this.rectBrush = rectBrush;
                this.rectPen = rectPen;

            }

            //overridden draw for rectangles
            public override void Draw(Graphics g)
            {

                int width = Math.Abs(rectEnd.X - rectStart.X);
                int height = Math.Abs(rectEnd.Y - rectStart.Y);

                int topLeftX = Math.Min(rectStart.X, rectEnd.X);
                int topLeftY = Math.Min(rectStart.Y, rectEnd.Y);

                if(rectBrush != null)
                {
                    g.FillRectangle(rectBrush, topLeftX, topLeftY, width, height); //draw filled rectangle first
                }
                
                if(rectPen != null)
                {
                    g.DrawRectangle(rectPen, topLeftX, topLeftY, width, height); //overlay the outline on top of filled shape
                }
                
            }
        }

        internal class Ellipse : Elem
        {
            private Point ellipseStart;
            private Point ellipseEnd;

            private Brush ellipseBrush;
            private Pen ellipsePen;

            //Constructor of an ellipse
            public Ellipse(Point ellipseStart, Point ellipseEnd, Brush ellipseBrush, Pen ellipsePen)
            {
                this.ellipseStart = ellipseStart;
                this.ellipseEnd = ellipseEnd;
                this.ellipseBrush = ellipseBrush;
                this.ellipsePen = ellipsePen;
            }

            //overridden Draw for ellipses
            public override void Draw(Graphics g)
            {
                int width = Math.Abs(ellipseEnd.X - ellipseStart.X);
                int height = Math.Abs(ellipseEnd.Y - ellipseStart.Y);

                int topLeftX = Math.Min(ellipseStart.X, ellipseEnd.X);
                int topLeftY = Math.Min(ellipseStart.Y, ellipseEnd.Y);

                if (ellipseBrush != null)
                {
                    g.FillEllipse(ellipseBrush, topLeftX, topLeftY, width, height); //draw filled ellipse first
                }

                if (ellipsePen != null)
                {
                    g.DrawEllipse(ellipsePen, topLeftX, topLeftY, width, height); //overlay the outline on top of filled shape
                }
            } 
        }

        private Point start;
        private Point end;
        private bool firstClick = true; 

        //Mouse event handler for drawing panel
        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            if(firstClick)
            {
                firstClick = false;

                start = e.Location;
            }
            else if (!firstClick) //two points in coordinates, use to create respective object depending on radio buttons
            {
                end = e.Location;

                firstClick = true;

                Brush fillC = (Brush)null;
                Brush outlineC = (Brush)null;
                Pen pWidth = (Pen)null;

                switch (this.settings.listBox1.SelectedIndex)
                {
                    case 1:

                        outlineC = Brushes.Black; break;

                    case 2:

                        outlineC = Brushes.Red; break;

                    case 3:

                        outlineC = Brushes.Blue; break;

                    case 4:

                        outlineC = Brushes.Green; break;
                }

                if (this.settings.listBox2.SelectedIndex != 0)
                {
                    switch (this.settings.listBox2.SelectedIndex)
                    {
                        case 1:

                            fillC = Brushes.Black; break;

                        case 2:

                            fillC = Brushes.Red; break;

                        case 3:

                            fillC = Brushes.Blue; break;

                        case 4:

                            fillC = Brushes.Green; break;
                    }
                }

                if (outlineC == null && fillC == null)
                {
                    MessageBox.Show("Fill and or pen/outline must be selected.");
                }
                else
                {
                    if(outlineC != null)
                    {
                        pWidth = new Pen(outlineC, (int)this.settings.listBox3.SelectedItem);
                    }
                    if (pWidth != null)
                    {
                        if (radioButton1.Checked)
                        {
                            elements.Add(new Line(start, end, pWidth)); //make new line
                        }
                    }
                    if (radioButton2.Checked)
                    {
                        elements.Add(new Rectangle(start, end, fillC, pWidth)); //make new rectangle
                    }
                    if (radioButton3.Checked)
                    {
                        elements.Add(new Ellipse(start, end, fillC, pWidth)); //make new ellipse
                    }
                }
            }

            panel2.Invalidate();
        }

        //Paint event handler for drawing panel
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            const int WIDTH = 10;
            const int HEIGHT = 10;

            foreach (Elem elem in elements)
            {
                elem.Draw(g);
            }

            if (!firstClick) //only one point in coordinates, so should only print one small black circle
            {
                g.FillEllipse(Brushes.Black, start.X, start.Y, WIDTH, HEIGHT);
            }
        }

        //settings button clicked, should open settings dialog
        private void button1_Click(object sender, EventArgs e)
        {
            if (settings.ShowDialog() == DialogResult.OK)
            {
                settings.ShowDialog();
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(elements.Count > 0)
            {
                elements.RemoveAt(elements.Count - 1);
                panel2.Invalidate();
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            elements.Clear();
            panel2.Invalidate();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     }
}
