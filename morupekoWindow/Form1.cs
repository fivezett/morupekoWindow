using System.Text;
using System.Runtime.InteropServices;
using windowMG;

namespace morupekoWindow
{
    public partial class Form1 : Form
    {
        private WindowMG windowMg = new WindowMG();
        private Size originalSize = new Size();

        public Form1()
        {
            InitializeComponent();
        }

        private void apply(int x, int y)
        {
            windowMg.setWindowSize(comboBox1.SelectedIndex, x, y, checkBox1.Checked);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //IntPtr target = windowList[comboBox1.SelectedIndex];
        }

        private void windowSelectorFocus(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            var winList = windowMg.windowListUpdate();
            foreach (var win in winList)
            {
                comboBox1.Items.Add(win.Key);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            apply(640, 480);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var s = windowMg.getWindowSize(comboBox1.SelectedIndex, checkBox1.Checked);
            numericUpDown1.Value = s.Width;
            numericUpDown2.Value = s.Height;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            apply(1280, 720);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            apply(1920, 1080);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            apply(2560, 1440);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            this.Size = originalSize;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            apply(3840, 2160);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            apply((int)Math.Max(1, numericUpDown1.Value), (int)Math.Max(1, numericUpDown2.Value));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            windowSelectorFocus(new object(), new EventArgs());
            originalSize = new Size(this.Size.Width, this.Size.Height);
        }
    }
}