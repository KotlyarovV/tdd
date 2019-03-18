using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public partial class Form1 : Form
    {
        private CircularCloudLayouter circularCloud;
        
        public Form1()
        {
            InitializeComponent();
            Size = new Size(1000, 1000);
            Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            circularCloud = new CircularCloudLayouter(Size.GetCenter());
            var formGraphics = e.Graphics;
                
            var visualisator  = new Visualisator(circularCloud);
            visualisator.GenerateRandomRectangles(200);
            visualisator.SetRectanglesOnGraphics(formGraphics);

            visualisator.SaveBitmap("tag_cloud.jpg");
        }
    }
}
