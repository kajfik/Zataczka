using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zataczka
{
    public partial class settings : Form
    {
        public settings()
        {
            InitializeComponent();
        }

        public static Color[] kolors = { Form1.kolor[0], Form1.kolor[1], Form1.kolor[2], Form1.kolor[3]};
        public static int rint1 = Convert.ToInt32(kolors[0].R);
        public static int gint1 = Convert.ToInt32(kolors[0].G);
        public static int bint1 = Convert.ToInt32(kolors[0].B);
        public static int rint2 = Convert.ToInt32(kolors[1].R);
        public static int gint2 = Convert.ToInt32(kolors[1].G);
        public static int bint2 = Convert.ToInt32(kolors[1].B);
        public static int rint3 = Convert.ToInt32(kolors[2].R);
        public static int gint3 = Convert.ToInt32(kolors[2].G);
        public static int bint3 = Convert.ToInt32(kolors[2].B);
        public static int rint4 = Convert.ToInt32(kolors[3].R);
        public static int gint4 = Convert.ToInt32(kolors[3].G);
        public static int bint4 = Convert.ToInt32(kolors[3].B);
        public static bool[] komps = { Form1.komp[0], Form1.komp[1], Form1.komp[2], Form1.komp[3] };
        //public static bool resetuj = false;

        public void settings_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = Form1.ilosckolek;
            numericUpDown2.Value = Form1.prumer[0];
            numericUpDown3.Value = Form1.unirychlost;
            numericUpDown6.Value = Form1.uniWielkoscOtoczki;
            numericUpDown4.Value = Form1.szyrzka;
            numericUpDown5.Value = Form1.wyszka;
            numericUpDown7.Value = Form1.maxPunkty;
            checkBox1.Checked = Form1.komp[0];
            checkBox2.Checked = Form1.komp[1];
            checkBox3.Checked = Form1.komp[2];
            checkBox4.Checked = Form1.komp[3];
            R1.Value = rint1;
            G1.Value = gint1;
            B1.Value = bint1;
            R2.Value = rint2;
            G2.Value = gint2;
            B2.Value = bint2;
            R3.Value = rint3;
            G3.Value = gint3;
            B3.Value = bint3;
            R4.Value = rint4;
            G4.Value = gint4;
            B4.Value = bint4;
            
            /*settings.kolor0 = Color.FromArgb(255, Convert.ToInt32(R1.Value), Convert.ToInt32(G1.Value), Convert.ToInt32(B1.Value));
            settings.kolor1 = Color.FromArgb(255, Convert.ToInt32(R2.Value), Convert.ToInt32(G2.Value), Convert.ToInt32(B2.Value));
            settings.kolor2 = Color.FromArgb(255, Convert.ToInt32(R3.Value), Convert.ToInt32(G3.Value), Convert.ToInt32(B3.Value));
            settings.kolor3 = Color.FromArgb(255, Convert.ToInt32(R4.Value), Convert.ToInt32(G4.Value), Convert.ToInt32(B4.Value));
            Form1.kolor[0] = settings.kolor0;
            Form1.kolor[1] = settings.kolor1;
            Form1.kolor[2] = settings.kolor2;
            Form1.kolor[3] = settings.kolor3;*/
            Bitmap bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Bitmap bmp2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Bitmap bmp3 = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            Bitmap bmp4 = new Bitmap(pictureBox4.Width, pictureBox4.Height);
            Graphics g1 = Graphics.FromImage(bmp1);
            Graphics g2 = Graphics.FromImage(bmp2);
            Graphics g3 = Graphics.FromImage(bmp3);
            Graphics g4 = Graphics.FromImage(bmp4);
            pictureBox1.Image = bmp1;
            pictureBox2.Image = bmp2;
            pictureBox3.Image = bmp3;
            pictureBox4.Image = bmp4;
            g1.Clear(Color.FromArgb(Convert.ToInt32(R1.Value), Convert.ToInt32(G1.Value), Convert.ToInt32(B1.Value)));
            g2.Clear(Color.FromArgb(Convert.ToInt32(R2.Value), Convert.ToInt32(G2.Value), Convert.ToInt32(B2.Value)));
            g3.Clear(Color.FromArgb(Convert.ToInt32(R3.Value), Convert.ToInt32(G3.Value), Convert.ToInt32(B3.Value)));
            g4.Clear(Color.FromArgb(Convert.ToInt32(R4.Value), Convert.ToInt32(G4.Value), Convert.ToInt32(B4.Value)));
        }

        public void zmjynKolor()
        {
            Bitmap bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Bitmap bmp2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Bitmap bmp3 = new Bitmap(pictureBox3.Width, pictureBox3.Height);
            Bitmap bmp4 = new Bitmap(pictureBox4.Width, pictureBox4.Height);
            Graphics g1 = Graphics.FromImage(bmp1);
            Graphics g2 = Graphics.FromImage(bmp2);
            Graphics g3 = Graphics.FromImage(bmp3);
            Graphics g4 = Graphics.FromImage(bmp4);
            pictureBox1.Image = bmp1;
            pictureBox2.Image = bmp2;
            pictureBox3.Image = bmp3;
            pictureBox4.Image = bmp4;
            rint1 = Convert.ToInt32(R1.Value);
            gint1 = Convert.ToInt32(G1.Value);
            bint1 = Convert.ToInt32(B1.Value);
            rint2 = Convert.ToInt32(R2.Value);
            gint2 = Convert.ToInt32(G2.Value);
            bint2 = Convert.ToInt32(B2.Value);
            rint3 = Convert.ToInt32(R3.Value);
            gint3 = Convert.ToInt32(G3.Value);
            bint3 = Convert.ToInt32(B3.Value);
            rint4 = Convert.ToInt32(R4.Value);
            gint4 = Convert.ToInt32(G4.Value);
            bint4 = Convert.ToInt32(B4.Value);
            kolors[0] = Color.FromArgb(255, Convert.ToInt32(R1.Value), Convert.ToInt32(G1.Value), Convert.ToInt32(B1.Value));
            kolors[1] = Color.FromArgb(255, Convert.ToInt32(R2.Value), Convert.ToInt32(G2.Value), Convert.ToInt32(B2.Value));
            kolors[2] = Color.FromArgb(255, Convert.ToInt32(R3.Value), Convert.ToInt32(G3.Value), Convert.ToInt32(B3.Value));
            kolors[3] = Color.FromArgb(255, Convert.ToInt32(R4.Value), Convert.ToInt32(G4.Value), Convert.ToInt32(B4.Value));
            g1.Clear(Color.FromArgb(Convert.ToInt32(R1.Value), Convert.ToInt32(G1.Value), Convert.ToInt32(B1.Value)));
            g2.Clear(Color.FromArgb(Convert.ToInt32(R2.Value), Convert.ToInt32(G2.Value), Convert.ToInt32(B2.Value)));
            g3.Clear(Color.FromArgb(Convert.ToInt32(R3.Value), Convert.ToInt32(G3.Value), Convert.ToInt32(B3.Value)));
            g4.Clear(Color.FromArgb(Convert.ToInt32(R4.Value), Convert.ToInt32(G4.Value), Convert.ToInt32(B4.Value)));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            zmjynKolor();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int a = 0; a < 4; a++)
            {
                Form1.iloscPunktow[a] = 0;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            komps[0] = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            komps[1] = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            komps[2] = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            komps[3] = checkBox4.Checked;
        }

        
    }
}
