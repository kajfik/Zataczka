using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Zataczka
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        public static int szyrzka = 1366, wyszka = 705;
        public static int maxPunkty = 10;
        public static int ilosckolek = 3;
        public static int unirychlost = 4;
        public static int uniWielkoscOtoczki = 8;
        public static int[] prumer = { 5, 5, 5, 5 };
        public static bool[] komp = { false, true, true, true };

        public bool niekliknote = true;
        public bool namaluj = true;
        public int odpoczitawaniNamaluj = 0;
        public static int iloscbonusow = 2;
        public int odpoczitawaniSpawnyciaBonusu = 0;
        public int[] odpoczitawaniTrwaniaBonusu = new int[iloscbonusow];
        public bool[] bonusNamalowany = new bool[iloscbonusow];
        public int[] xbonus = new int[iloscbonusow];
        public int[] ybonus = new int[iloscbonusow];
        public int[] kolkoNaKierymBonusJe = new int[iloscbonusow];
        public int[] odpoczitawani90 = new int[4];
        public static int tick = 0;
        public static int terazBonus = 1;
        public static Bitmap bmp = new Bitmap(1, 1);
        public Graphics g;
        public int[,] xypole = new int[3000, 2000];
        public int[,] xpole = new int[101, 4];
        public int[,] ypole = new int[101, 4];
        public int[] pozycjaWline = new int[4];
        public static int[] iloscPunktow = new int[4];
        public int[] kieryumrzil = new int[4];
        public int[] zmianakierunku = new int[4];
        public int[] kierunek = new int[4];
        public int[] xkolko = new int[4];
        public int[] ykolko = new int[4];
        public int[] iloscBonusowNaKolku = new int[4];
        public int iloscUmrzitych;
        public static int[] wielkoscOtoczki = new int[4];
        public static int[] rychlost = new int[4];
        public static Color[] kolor = { Color.Green, Color.Blue, Color.Red, Color.Yellow };
        public settings st = new settings();

        /*public void namalujObdelnik(int x0, int y0, int x1, int y1)
        {

        }*/

        public double Deg2Rad(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public double Rad2Deg(double angle)
        {
            return angle * 180.0 / Math.PI;
        }

        public void line(int numerKolka)
        {
            int x0 = xkolko[numerKolka];
            int y0 = ykolko[numerKolka];
            int delka = 200;
            double angleInDegrees = kierunek[numerKolka];
            int x1 = Convert.ToInt32(x0 + delka * Math.Cos(Deg2Rad(angleInDegrees)));
            int y1 = Convert.ToInt32(y0 - delka * Math.Sin(Deg2Rad(angleInDegrees)));
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            for (int m = 0; m <= 100; m++)
            {
                xpole[m, numerKolka] = x0;
                ypole[m, numerKolka] = y0;
                if (x0 == x1 + 10 && y0 == y1 + 10) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }
        }

        public void wymazKierunek(Color kolor, int x0, int y0, int x1, int y1)
        {
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = (dx > dy ? dx : -dy) / 2, e2;
            for (; ; )
            {
                if (x0 >= 0 && x0 < szyrzka && y0 >= 0 && y0 < wyszka && xypole[x0, y0] == 0)
                {
                    bmp.SetPixel(x0, y0, kolor);
                }
                if (x0 == x1 && y0 == y1) break;
                e2 = err;
                if (e2 > -dx) { err -= dy; x0 += sx; }
                if (e2 < dy) { err += dx; y0 += sy; }
            }
        }

        public void lineWszystki()
        {
            for (int a = 0; a < ilosckolek; a++)
            {
                line(a);
            }
        }

        public int Max(int a, int b, int c)
        {
            int max = a;
            if(b == Math.Max(a, Math.Max(b, c)))
            {
                max = b;
            }
            if(c == Math.Max(a, Math.Max(b, c)))
            {
                max = c;
            }
            return max;
        }

        public int Min(int a, int b, int c)
        {
            int min = a;
            if (b == Math.Min(a, Math.Min(b, c)))
            {
                min = b;
            }
            if (c == Math.Min(a, Math.Min(b, c)))
            {
                min = c;
            }
            return min;
        }

        public int Srodek(int a, int b, int c)
        {
            if (a != b && b != c)
            {
                int srodek = a;
                if (a == Math.Max(a, Math.Max(b, c)) || a == Math.Min(a, Math.Min(b, c)))
                {
                    srodek = b;
                    if (b == Math.Max(a, Math.Max(b, c)) || b == Math.Min(a, Math.Min(b, c)))
                    {
                        srodek = c;
                    }
                }

                return srodek;
            }
            else return -1000;
        }

        public void namalujKolko(int numerKolka)
        {
            for (int a = xkolko[numerKolka] - prumer[numerKolka] - 2; a <= xkolko[numerKolka] + prumer[numerKolka] + 2; a++)
            {
                for (int b = ykolko[numerKolka] - prumer[numerKolka] - 2; b <= ykolko[numerKolka] + prumer[numerKolka] + 2; b++)
                {
                    if(Math.Sqrt((a - xkolko[numerKolka]) * (a - xkolko[numerKolka]) + (b - ykolko[numerKolka]) * (b - ykolko[numerKolka])) < prumer[numerKolka])
                    {
                        if (a < szyrzka && b < wyszka && a > 0 && b > 0)
                        {
                            if (namaluj || (odpoczitawaniNamaluj == 20 && namaluj == false))
                            {
                                bmp.SetPixel(a, b, Form1.kolor[numerKolka]);
                                xypole[a, b] = numerKolka + 1;
                            }
                        }
                    }
                    if (iloscBonusowNaKolku[numerKolka] > 0 && Math.Sqrt((a - xkolko[numerKolka]) * (a - xkolko[numerKolka]) + (b - ykolko[numerKolka]) * (b - ykolko[numerKolka])) < prumer[numerKolka] - 2)
                    {
                        if (a < szyrzka && b < wyszka && a > 0 && b > 0)
                        {
                            if (namaluj)
                            {
                                bmp.SetPixel(a, b, Color.White);
                            }
                        }
                    }
                }
            }
        }

        public void namalujWszystkiKolka()
        {
            for (int a = 0; a < ilosckolek; a++)
            {
                namalujKolko(a);
            }
        }

        public void zmianaKierunku(int numerKolka)
        {
            if (rychlost[numerKolka] > 0)
            {
                if ((zmianakierunku[numerKolka] == 6 || zmianakierunku[numerKolka] == 4) && odpoczitawani90[numerKolka] == 0 && wielkoscOtoczki[numerKolka] > 25 && prumer[numerKolka] > 3)
                {
                    int delka = prumer[numerKolka] + 4;
                    //Pen mypen = new Pen(Color.FromArgb(255, 0, 0, 0));
                    Color czorny = Color.FromArgb(255, 0, 0, 0);
                    double angleInDegrees = kierunek[numerKolka];
                    int x1 = Convert.ToInt32(xkolko[numerKolka] + prumer[numerKolka] * Math.Cos(Deg2Rad(angleInDegrees)));
                    int y1 = Convert.ToInt32(ykolko[numerKolka] - prumer[numerKolka] * Math.Sin(Deg2Rad(angleInDegrees)));
                    int x2 = Convert.ToInt32(xkolko[numerKolka] + delka * Math.Cos(Deg2Rad(angleInDegrees)));
                    int y2 = Convert.ToInt32(ykolko[numerKolka] - delka * Math.Sin(Deg2Rad(angleInDegrees)));
                    //int x3 = Convert.ToInt32(x2 + 2 * Math.Cos(Deg2Rad(angleInDegrees + 90)));
                    //int y3 = Convert.ToInt32(y2 - 2 * Math.Sin(Deg2Rad(angleInDegrees + 90)));
                    /*g.DrawLine(mypen, x1, y1, x2, y2);
                    g.DrawLine(mypen, x1 + 1, y1, x2 + 1, y2);
                    g.DrawLine(mypen, x1 - 1, y1, x2 - 1, y2);
                    g.DrawLine(mypen, x1, y1 + 1, x2, y2 + 1);
                    g.DrawLine(mypen, x1, y1 - 1, x2, y2 - 1);*/
                    wymazKierunek(czorny, x1, y1, x2, y2);
                    wymazKierunek(czorny, x1 + 1, y1, x2 + 1, y2);
                    wymazKierunek(czorny, x1 - 1, y1, x2 - 1, y2);
                    wymazKierunek(czorny, x1, y1 + 1, x2, y2 + 1);
                    wymazKierunek(czorny, x1, y1 - 1, x2, y2 - 1);
                }
                if (zmianakierunku[numerKolka] == 4)
                {
                    if (odpoczitawani90[numerKolka] == 0)
                    {
                        kierunek[numerKolka] += wielkoscOtoczki[numerKolka];
                        pozycjaWline[numerKolka] = 0;
                    }
                }
                if (zmianakierunku[numerKolka] == 6)
                {
                    if (odpoczitawani90[numerKolka] == 0)
                    {
                        kierunek[numerKolka] -= wielkoscOtoczki[numerKolka];
                        pozycjaWline[numerKolka] = 0;
                    }
                }
            }
        }

        public void zmianyKierunku()
        {
            for (int a = 0; a < ilosckolek; a++)
            {
                zmianaKierunku(a);
            }
        }

        public void posunyciKolka(int numerKolka)
        {
            h1:
            if(pozycjaWline[numerKolka] == 0)
            {
                line(numerKolka);
                for (int a = 0; a < rychlost[numerKolka]; a++)
                {
                    pozycjaWline[numerKolka]++;
                    xkolko[numerKolka] = xpole[pozycjaWline[numerKolka], numerKolka];
                    ykolko[numerKolka] = ypole[pozycjaWline[numerKolka], numerKolka];
                    namalujKolko(numerKolka);
                    if (pozycjaWline[numerKolka] == 99)
                    {
                        //pozycjaWline[numerKolka] = 0;
                        goto h1;
                    }
                }

            }
            else if (pozycjaWline[numerKolka] > 0 && pozycjaWline[numerKolka] < 99)
            {
                for (int a = 0; a < rychlost[numerKolka]; a++)
                {
                    pozycjaWline[numerKolka]++;
                    xkolko[numerKolka] = xpole[pozycjaWline[numerKolka], numerKolka];
                    ykolko[numerKolka] = ypole[pozycjaWline[numerKolka], numerKolka];
                    namalujKolko(numerKolka);
                    if (pozycjaWline[numerKolka] == 99)
                    {
                        pozycjaWline[numerKolka] = 0;
                        goto h1;
                    }
                }
            }
            else if(pozycjaWline[numerKolka] == 99)
            {
                pozycjaWline[numerKolka] = 0;
            }
        }

        public void posunyciKolek()
        {
            for (int a = 0; a < ilosckolek; a++)
            {
                posunyciKolka(a);
            }
        }

        public void naburali()
        {
            for (int a = 0; a < ilosckolek; a++)
            {
                int delka = (namaluj ? prumer[a] + 1 : 1);
                bool umrzil = false;
                if (rychlost[a] != 0)
                {
                    for (int b = (namaluj ? (wielkoscOtoczki[a] == 90 ? -10 : -70) : -30); b < (namaluj ? (wielkoscOtoczki[a] == 90 ? 10 : 70) : 30); b++)
                    {
                        int x2 = Convert.ToInt32(xkolko[a] + delka * Math.Cos(Deg2Rad(kierunek[a] + b)));
                        int y2 = Convert.ToInt32(ykolko[a] - delka * Math.Sin(Deg2Rad(kierunek[a] + b)));
                        if (x2 >= szyrzka || y2 >= wyszka || x2 < 0 || y2 < 0)
                        {
                            rychlost[a] = 0;
                            umrzil = true;
                        }
                        else if (xypole[x2, y2] != 0 && namaluj && xypole[x2, y2] < 5)
                        {
                            rychlost[a] = 0;
                            umrzil = true;
                        }
                        else if (xypole[x2, y2] == 0 + 5)
                        {
                            for(int c = 0; c < ilosckolek; c++)
                            {
                                if(c != a)
                                {
                                    wielkoscOtoczki[c] = 90;
                                }
                            }
                            wymazBonus(0);
                            kolkoNaKierymBonusJe[0] = a;
                            iloscBonusowNaKolku[a]++;
                            odpoczitawaniTrwaniaBonusu[0] = 250;
                        }
                        else if (xypole[x2, y2] == 1 + 5)
                        {
                            wielkoscOtoczki[a] = 90;
                            wymazBonus(1);
                            kolkoNaKierymBonusJe[1] = a;
                            iloscBonusowNaKolku[a]++;
                            odpoczitawaniTrwaniaBonusu[1] = 250;
                        }
                    }
                }
                if(umrzil)
                {
                    if (iloscUmrzitych < ilosckolek)
                    {
                        iloscUmrzitych++;
                    }
                    kieryumrzil[a] = iloscUmrzitych;
                }
            }
        }

        public void namalujKierunki()
        {
            g = Graphics.FromImage(bmp);
            for (int a = 0; a < ilosckolek; a++)
            {
                if (prumer[a] > 3)
                {
                    //var my = new SolidBrush(kolor[a]);
                    int delka = namaluj == true ? prumer[a] + 4 : prumer[a] + 1;
                    int R = iloscBonusowNaKolku[a] > 0 ? 255 : Form1.kolor[a].R;
                    int G = iloscBonusowNaKolku[a] > 0 ? 255 : Form1.kolor[a].G;
                    int B = iloscBonusowNaKolku[a] > 0 ? 255 : Form1.kolor[a].B;
                    Pen mypen = new Pen(Color.FromArgb(namaluj == true ? 210 : 100, R, G, B));
                    double angleInDegrees = kierunek[a];
                    int x1 = Convert.ToInt32(xkolko[a] + delka * Math.Cos(Deg2Rad(angleInDegrees)));
                    int y1 = Convert.ToInt32(ykolko[a] - delka * Math.Sin(Deg2Rad(angleInDegrees)));
                    int x2 = Convert.ToInt32(xkolko[a] + delka * Math.Cos(Deg2Rad(angleInDegrees)));
                    int y2 = Convert.ToInt32(ykolko[a] - delka * Math.Sin(Deg2Rad(angleInDegrees)));
                    //int x3 = Convert.ToInt32(x2 + 2 * Math.Cos(Deg2Rad(angleInDegrees + 90)));
                    //int y3 = Convert.ToInt32(y2 - 2 * Math.Sin(Deg2Rad(angleInDegrees + 90)));
                    g.DrawLine(mypen, xkolko[a], ykolko[a], x1, y1);
                }
            }
        }

        public bool koniec()
        {
            bool koniec = false;
            if(iloscUmrzitych == ilosckolek && ilosckolek != 1)
            {
                koniec = true;
            }
            if(ilosckolek == 1)
            {
                koniec = true;
                for(int a = 0; a < 4; a++)
                {
                    if(rychlost[a] > 0)
                    {
                        koniec = false;
                    }
                }
            }
            return koniec;
        }

        public void restartnabidni()
        {
            //g = Graphics.FromImage(bmp);
            
            for (int b = 0; b < ilosckolek; b++)
            {
                if (ilosckolek == 1 ? rychlost[b] == 0 : iloscUmrzitych == ilosckolek - 1)
                {
                    tick = 0;
                    timer1.Stop();
                    resetWszystkigo();
                    for (int a = 0; a < ilosckolek; a++)
                    {
                        if (kieryumrzil[a] == ilosckolek || rychlost[a] != 0)
                        {
                            iloscPunktow[a]++;
                        }
                    }
                    namalujIlosciPuntkow();
                    Image newImage2 = Image.FromFile(".\\icons\\settings.png");
                    Rectangle rect2 = new Rectangle(20, 20, 50, 50);
                    g.DrawImage(newImage2, rect2);
                    Font drawFont3 = new Font("Comic Sans MS", 20);
                    SolidBrush drawBrush3 = new SolidBrush(Color.White);
                    g.DrawString("Press space", drawFont3, drawBrush3, new PointF(80, 25));
                    for (int a = 0; a < ilosckolek; a++)
                    {
                        if (iloscPunktow[a] == maxPunkty)
                        {
                            iloscUmrzitych = ilosckolek;
                            Font drawFont = new Font("Comic Sans MS", 32);
                            Font drawFont2 = new Font("Comic Sans MS", 20);
                            Pen my = new Pen(Color.White);
                            SolidBrush drawBrush = new SolidBrush(Color.White);
                            SolidBrush drawBrush4 = new SolidBrush(Color.Black);
                            g.DrawString("Restart?", drawFont, drawBrush, new PointF(550, 250));
                            g.DrawString("Yes", drawFont2, drawBrush, new PointF(570, 305));
                            g.FillRectangle(drawBrush4, 80, 25, 200, 50);
                            g.DrawRectangle(my, 570, 307, 55, 35);
                        }
                    }
                }
            }
        }

        public void namalujIlosciPuntkow()
        {
            g = Graphics.FromImage(bmp);
            for (int a = 0; a < ilosckolek; a++)
            {
                for (int b = Convert.ToInt32(szyrzka / 2.355) + 100 * a; b < 100 * a + (iloscPunktow[a] >= 10 ? (iloscPunktow[a] >= 100 ? 640 + 100 : 640 + 50) : 640 + 0); b++)
                {
                    for (int c = 10; c < 60; c++)
                    {
                        if (Math.Sqrt((b - (Convert.ToInt32(szyrzka / 2.355) + 29 + 100 * a + (iloscPunktow[a] >= 10 ? (iloscPunktow[a] >= 100 ? 12 : 7) : 0))) * (b - (Convert.ToInt32(szyrzka / 2.355) + 29 + 100 * a + (iloscPunktow[a] >= 10 ? (iloscPunktow[a] >= 100 ? 12 : 7) : 0))) + (c - 31) * (c - 31)) < 12 + (iloscPunktow[a] >= 10 ? (iloscPunktow[a] >= 100 ? 10 : 4) : 0))
                        {
                            bmp.SetPixel(b, c, Form1.kolor[a]);
                        }
                    }
                }
                SolidBrush drawBrush = new SolidBrush(Form1.kolor[a]);
                SolidBrush drawBrush2 = new SolidBrush(Color.Black);
                PointF drawPoint = new PointF(Convert.ToInt32(szyrzka / 2.355) + 20 + 100 * a, 20);
                Font drawFont = new Font("Arial", 16);
                g.DrawString(Convert.ToString(iloscPunktow[a]), drawFont, drawBrush2, drawPoint);
            }
        }

        public void resetWszystkigo()
        {
            tick = 0;
            terazBonus = 1;
            iloscUmrzitych = 0;
            for(int a = 0; a < szyrzka; a++)
            {
                for (int b = 0; b < wyszka; b++)
                {
                    xypole[a, b] = 0;
                }
            }
            for(int c = 0; c < ilosckolek; c++)
            {
                xkolko[c] = 0;
                ykolko[c] = 0;
                pozycjaWline[c] = 0;
                kieryumrzil[c] = 0;
                wielkoscOtoczki[c] = uniWielkoscOtoczki;
                odpoczitawani90[c] = 0;
                zmianakierunku[c] = 0;
                iloscBonusowNaKolku[c] = 0;
            }
            for (int c = 0; c < iloscbonusow; c++)
            {
                xbonus[c] = 0;
                ybonus[c] = 0;
                bonusNamalowany[c] = false;
                odpoczitawaniSpawnyciaBonusu = 0;
                odpoczitawaniTrwaniaBonusu[c] = 0;
                kolkoNaKierymBonusJe[c] = -1;
            }
        }

        public void kompX()
        {
            for (int a = 0; a < ilosckolek; a++)
            {
                if (komp[a] && rychlost[a] != 0)
                {
                    double nalewo = 0, naprawo = 0;
                    for (double b = -80; b <= -1; b += 2)
                    {
                        for (int delka = prumer[a] + 1; delka < 150; delka++)
                        {
                            int x2 = Convert.ToInt32(xkolko[a] + delka * Math.Cos(Deg2Rad(kierunek[a] + b)));
                            int y2 = Convert.ToInt32(ykolko[a] - delka * Math.Sin(Deg2Rad(kierunek[a] + b)));
                            if (x2 >= szyrzka || y2 >= wyszka || x2 < 0 || y2 < 0)
                            {
                                naprawo += 1000000 / delka;
                                b++;
                                break;
                            }
                            else if (xypole[x2, y2] != 0 && xypole[x2, y2] < 5)
                            {
                                naprawo += 1000000 / delka;
                                b++;
                                break;
                            }
                            else if(xypole[x2, y2] != 0 && xypole[x2, y2] == 5)
                            {
                                naprawo -= 1000000 / delka;
                                b++;
                                break;
                            }
                            else if (xypole[x2, y2] != 0 && xypole[x2, y2] == 6)
                            {
                                naprawo += 1000000 / delka;
                                b++;
                                break;
                            }
                        }
                    }
                    for (double b = 1; b <= 80; b += 2)
                    {
                        for (int delka = prumer[a] + 1; delka < 150; delka++)
                        {
                            int x2 = Convert.ToInt32(xkolko[a] + delka * Math.Cos(Deg2Rad(kierunek[a] + b)));
                            int y2 = Convert.ToInt32(ykolko[a] - delka * Math.Sin(Deg2Rad(kierunek[a] + b)));
                            if (x2 >= szyrzka || y2 >= wyszka || x2 < 0 || y2 < 0)
                            {
                                nalewo += 1000000 / delka;
                                b++;
                                break;
                            }
                            else if (xypole[x2, y2] != 0 && xypole[x2, y2] < 5)
                            {
                                nalewo += 1000000 / delka;
                                b++;
                                break;
                            }
                            else if (xypole[x2, y2] != 0 && xypole[x2, y2] == 5)
                            {
                                nalewo -= 1000000 / delka;
                                b++;
                                break;
                            }
                            else if (xypole[x2, y2] != 0 && xypole[x2, y2] == 6)
                            {
                                nalewo += 1000000 / delka;
                                b++;
                                break;
                            }
                        }
                    }
                    if (nalewo > naprawo * 1.2)
                    {
                        zmianakierunku[a] = 6;
                    }
                    else if(nalewo < naprawo)
                    {
                        zmianakierunku[a] = 4;
                    }
                    else zmianakierunku[a] = 6;
                }
            }
        }

        public void kompX2()
        {
            for (int a = 0; a < ilosckolek; a++)
            {
                if (komp[a] && rychlost[a] != 0)
                {
                    double nalewo = 0, naprawo = 0;
                    double nejlewo = 0, nejprawo = 0;
                    for (int b = -179; b <= -1; b++)
                    {
                        for (int delka = prumer[a] + 20; delka < 2000; delka++)
                        {
                            int x2 = Convert.ToInt32(xkolko[a] + delka * Math.Cos(Deg2Rad(kierunek[a] + b)));
                            int y2 = Convert.ToInt32(ykolko[a] - delka * Math.Sin(Deg2Rad(kierunek[a] + b)));
                            int lewox = Convert.ToInt32(x2 + prumer[a] * Math.Cos(Deg2Rad(kierunek[a] + 90)));
                            int lewoy = Convert.ToInt32(y2 - prumer[a] * Math.Sin(Deg2Rad(kierunek[a] + 90)));
                            int prawox = Convert.ToInt32(x2 + prumer[a] * Math.Cos(Deg2Rad(kierunek[a] - 90)));
                            int prawoy = Convert.ToInt32(y2 - prumer[a] * Math.Sin(Deg2Rad(kierunek[a] - 90)));
                            if (x2 >= szyrzka || y2 >= wyszka || x2 < 0 || y2 < 0)
                            {
                                if (b >= -70 && delka < 100)
                                {
                                    naprawo += 1000000 / delka;
                                }
                                if (nejprawo < delka)
                                {
                                    nejprawo = delka;
                                }
                                b++;
                                break;
                            }
                            else if (xypole[x2, y2] != 0 && lewox <= szyrzka && lewox >= 0 && lewoy <= wyszka && lewoy >= 0
                                                       && prawox <= szyrzka && prawox >= 0 && prawoy <= wyszka && prawoy >= 0 && namaluj)
                            {
                                if (xypole[lewox, lewoy] != 0 && xypole[prawox, prawoy] != 0)
                                {
                                    if (b >= -70 && delka < 100)
                                    {
                                        naprawo += 1000000 / delka;
                                    }
                                    if (nejprawo < delka)
                                    {
                                        nejprawo = delka;
                                    }
                                    b++;
                                    break;
                                }
                            }
                        }
                    }
                    for (int b = 1; b <= 179; b++)
                    {
                        for (int delka = prumer[a] + 20; delka < 2000; delka++)
                        {
                            int x2 = Convert.ToInt32(xkolko[a] + delka * Math.Cos(Deg2Rad(kierunek[a] + b)));
                            int y2 = Convert.ToInt32(ykolko[a] - delka * Math.Sin(Deg2Rad(kierunek[a] + b)));
                            int lewox = Convert.ToInt32(x2 + prumer[a] * Math.Cos(Deg2Rad(kierunek[a] + 90)));
                            int lewoy = Convert.ToInt32(y2 - prumer[a] * Math.Sin(Deg2Rad(kierunek[a] + 90)));
                            int prawox = Convert.ToInt32(x2 + prumer[a] * Math.Cos(Deg2Rad(kierunek[a] - 90)));
                            int prawoy = Convert.ToInt32(y2 - prumer[a] * Math.Sin(Deg2Rad(kierunek[a] - 90)));
                            if (x2 >= szyrzka || y2 >= wyszka || x2 < 0 || y2 < 0)
                            {
                                if (b <= 70 && delka < 100)
                                {
                                    nalewo += 1000000 / delka;
                                }
                                if (nejlewo < delka)
                                {
                                    nejlewo = delka;
                                }
                                b++;
                                break;
                            }
                            else if (xypole[x2, y2] != 0 && lewox <= szyrzka && lewox >= 0 && lewoy <= wyszka && lewoy >= 0
                                                       && prawox <= szyrzka && prawox >= 0 && prawoy <= wyszka && prawoy >= 0 && namaluj)
                            {
                                if (xypole[lewox, lewoy] != 0 && xypole[prawox, prawoy] != 0)
                                {
                                    if (b <= 70 && delka < 100)
                                    {
                                        nalewo += 1000000 / delka;
                                    }
                                    if (nejlewo < delka)
                                    {
                                        nejlewo = delka;
                                    }
                                    b++;
                                    break;
                                }
                            }
                        }
                    }
                    if (nejlewo > nejprawo)
                    {
                        if (nalewo > naprawo * 2)
                        {
                            zmianakierunku[a] = 6;
                        }
                        else if (nalewo < naprawo)
                        {
                            zmianakierunku[a] = 4;
                        }
                        else zmianakierunku[a] = 4;
                    }
                    else if (nejlewo < nejprawo)
                    {
                        if (nalewo > naprawo)
                        {
                            zmianakierunku[a] = 6;
                        }
                        else if (nalewo * 2 < naprawo)
                        {
                            zmianakierunku[a] = 4;
                        }
                        else zmianakierunku[a] = 6;
                    }
                    else
                    {
                        zmianakierunku[a] = 0;
                    }
                    
                }
            }
        }

        public void odpoczitej90()
        {
            for(int a = 0; a < ilosckolek; a++)
            {
                if (wielkoscOtoczki[a] == 90 && odpoczitawani90[a] == 0 && zmianakierunku[a] != 0)
                {
                    if (komp[a] == false)
                    {
                        if (rychlost[a] == 1)
                        {
                            odpoczitawani90[a] = 10;
                        }
                        else if (rychlost[a] == 2)
                        {
                            odpoczitawani90[a] = 8;
                        }
                        else if (rychlost[a] == 3)
                        {
                            odpoczitawani90[a] = 6;
                        }
                        else if (rychlost[a] == 4)
                        {
                            odpoczitawani90[a] = 5;
                        }
                        else if (rychlost[a] == 5)
                        {
                            odpoczitawani90[a] = 4;
                        }
                        else if (rychlost[a] >= 5)
                        {
                            odpoczitawani90[a] = 3;
                        }
                    }
                    else
                    {
                        odpoczitawani90[a] = 3;
                    }
                }
                if (odpoczitawani90[a] > 0)
                {
                    odpoczitawani90[a]--;
                }
            }
        }

        public void namalujBonus(int numerBonusu)
        {
            Random rnd = new Random();
            Random rnd2 = new Random();
            bool nimaPuste = true;
            int x = 0, y = 0;
            int h = 0;
            while (nimaPuste && h < 1000000000)
            {
                x = rnd2.Next(100, szyrzka - 100);
                y = rnd.Next(100, wyszka - 100);
                for (int a = x; a <= x + 20; a++)
                {
                    for (int b = y; b <= y + 20; b++)
                    {
                        if (xypole[a, b] != 0)
                        {
                            goto h2;
                        }
                    }
                }
                nimaPuste = false;
                break;
            h2:
                {
                    nimaPuste = true;
                    h++;
                }
            }
            if (h < 1000000000)
            {
                Image newImage = Image.FromFile(".\\icons\\90blue.png");
                if (numerBonusu == 0)
                {
                    newImage = Image.FromFile(".\\icons\\90blue.png");
                }
                else if (numerBonusu == 1)
                {
                    newImage = Image.FromFile(".\\icons\\90red.png");
                }
                Rectangle rect = new Rectangle(x, y, 20, 20);
                for (int a = x; a <= x + 20; a++)
                {
                    for (int b = y; b <= y + 20; b++)
                    {
                        xypole[a, b] = numerBonusu + 5;
                    }
                }
                g.DrawImage(newImage, rect);
                bonusNamalowany[numerBonusu] = true;
                xbonus[numerBonusu] = x;
                ybonus[numerBonusu] = y;
            }
        }

        public void namalujDalszyBonus()
        {
            if (!bonusNamalowany[terazBonus - 1])
            {
                namalujBonus(terazBonus - 1);
            }
            terazBonus++;
            if(terazBonus == iloscbonusow + 1)
            {
                terazBonus = 1;
            }
        }

        public void wymazBonus(int numerBonusu)
        {
            int x = xbonus[numerBonusu];
            int y = ybonus[numerBonusu];
            Rectangle rect = new Rectangle(x, y, 20, 20);
            for (int a = x; a <= x + 20; a++)
            {
                for (int b = y; b <= y + 20; b++)
                {
                    xypole[a, b] = 0;
                }
            }
            g.FillRectangle(Brushes.Black, rect);
            bonusNamalowany[numerBonusu] = false;
        }

        public void zruszyniUczinkuBonusow()
        {
            for (int numerBonusu = 0; numerBonusu < iloscbonusow; numerBonusu++)
            {
                if (numerBonusu == 0 && odpoczitawaniTrwaniaBonusu[0] == 0 && kolkoNaKierymBonusJe[0] > -1)
                {
                    for (int c = 0; c < ilosckolek; c++)
                    {
                        if (c != kolkoNaKierymBonusJe[0])
                        {
                            wielkoscOtoczki[c] = uniWielkoscOtoczki;
                        }
                    }
                    if (iloscBonusowNaKolku[kolkoNaKierymBonusJe[0]] > 0)
                    {
                        iloscBonusowNaKolku[kolkoNaKierymBonusJe[0]]--;
                    }
                    kolkoNaKierymBonusJe[0] = -1;
                }
                if (numerBonusu == 1 && odpoczitawaniTrwaniaBonusu[1] == 0 && kolkoNaKierymBonusJe[1] > -1)
                {
                    wielkoscOtoczki[kolkoNaKierymBonusJe[1]] = uniWielkoscOtoczki;
                    if (iloscBonusowNaKolku[kolkoNaKierymBonusJe[1]] > 0)
                    {
                        iloscBonusowNaKolku[kolkoNaKierymBonusJe[1]]--;
                    }
                    kolkoNaKierymBonusJe[1] = -1;
                }
            }
        }

        public void inicjalizacja()
        {
            timer1.Stop();
            tick = 0;
            pictureBox1.Width = Convert.ToInt32(st.numericUpDown4.Value);
            pictureBox1.Height = Convert.ToInt32(st.numericUpDown5.Value);
            szyrzka = pictureBox1.Width;
            wyszka = pictureBox1.Height;
            ilosckolek = Convert.ToInt32(st.numericUpDown1.Value);
            maxPunkty = Convert.ToInt32(st.numericUpDown7.Value);
            unirychlost = Convert.ToInt32(st.numericUpDown3.Value);
            uniWielkoscOtoczki = Convert.ToInt32(st.numericUpDown6.Value);
            Zataczka.Form1.kolor[0] = Color.FromArgb(255, Zataczka.settings.rint1, Zataczka.settings.gint1, Zataczka.settings.bint1);
            Zataczka.Form1.kolor[1] = Color.FromArgb(255, Zataczka.settings.rint2, Zataczka.settings.gint2, Zataczka.settings.bint2);
            Zataczka.Form1.kolor[2] = Color.FromArgb(255, Zataczka.settings.rint3, Zataczka.settings.gint3, Zataczka.settings.bint3);
            Zataczka.Form1.kolor[3] = Color.FromArgb(255, Zataczka.settings.rint4, Zataczka.settings.gint4, Zataczka.settings.bint4);
            Random rnd = new Random();
            for (int a = 0; a < ilosckolek; a++)
            {
                prumer[a] = Convert.ToInt32(st.numericUpDown2.Value);
                rychlost[a] = Convert.ToInt32(st.numericUpDown3.Value);
                wielkoscOtoczki[a] = Convert.ToInt32(st.numericUpDown6.Value);
                komp[a] = Zataczka.settings.komps[a];
                int kier = rnd.Next(0, 360);
                kierunek[a] = kier;
                if (a == 0)
                {
                    int x = rnd.Next(80, (szyrzka - 80));
                    int y = rnd.Next(80, (wyszka - 80));
                    xkolko[0] = x;
                    ykolko[0] = y;
                }
                else if (a == 1)
                {
                    int[] x = { rnd.Next(60, xkolko[0] - 20), rnd.Next(xkolko[0] + 20, (szyrzka - 60)) };
                    int[] y = { rnd.Next(60, ykolko[0] - 20), rnd.Next(ykolko[0] + 20, (wyszka - 60)) };
                    int kierex = rnd.Next(0, 2);
                    int kierey = rnd.Next(0, 2);
                    xkolko[1] = x[kierex];
                    ykolko[1] = y[kierey];
                }
                else if (a == 2)
                {
                    int t0 = xkolko[0];
                    int t1 = xkolko[1];
                    int z0 = ykolko[0];
                    int z1 = ykolko[1];
                    int[] x = { rnd.Next(40, (Math.Min(t0, t1) - 10)), rnd.Next((Math.Min(t0, t1) + 10), (Math.Max(t0, t1) - 10)), rnd.Next((Math.Max(t0, t1) + 10), (szyrzka - 40)) };
                    int[] y = { rnd.Next(40, (Math.Min(z0, z1) - 10)), rnd.Next((Math.Min(z0, z1) + 10), (Math.Max(z0, z1) - 10)), rnd.Next((Math.Max(z0, z1) + 10), (wyszka - 40)) };
                    int kierex = rnd.Next(0, 3);
                    int kierey = rnd.Next(0, 3);
                    xkolko[2] = x[kierex];
                    ykolko[2] = y[kierey];
                }
                else
                {
                    int t0 = xkolko[0];
                    int t1 = xkolko[1];
                    int t2 = xkolko[2];
                    int z0 = ykolko[0];
                    int z1 = ykolko[1];
                    int z2 = ykolko[2];
                    int[] x = { rnd.Next(20, (Min(t0, t1, t2) - 5)), rnd.Next((Min(t0, t1, t2) + 5), (Srodek(t0, t1, t2) - 5)), rnd.Next((Srodek(t0, t1, t2) + 5), (Max(t0, t1, t2) - 5)), rnd.Next((Max(t0, t1, t2) + 5), (szyrzka - 20)) };
                    int[] y = { rnd.Next(20, (Min(z0, z1, z2) - 5)), rnd.Next((Min(z0, z1, z2) + 5), (Srodek(z0, z1, z2) - 5)), rnd.Next((Srodek(z0, z1, z2) + 5), (Max(z0, z1, z2) - 5)), rnd.Next((Max(z0, z1, z2) + 5), (wyszka - 20)) };
                    int kierex = rnd.Next(0, 4);
                    int kierey = rnd.Next(0, 4);
                    xkolko[3] = x[kierex];
                    ykolko[3] = y[kierey];
                }
            }
            bmp = new Bitmap(szyrzka, wyszka);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            namalujIlosciPuntkow();
            Image newImage = Image.FromFile(".\\icons\\startbutton.png");
            Rectangle rect = new Rectangle(pictureBox1.Width / 3, pictureBox1.Height / 3, pictureBox1.Width / 3, pictureBox1.Height / 4);
            g.DrawImage(newImage, rect);
            Image newImage2 = Image.FromFile(".\\icons\\settings.png");
            Rectangle rect2 = new Rectangle(20, 20, 50, 50);
            g.DrawImage(newImage2, rect2);
            pictureBox1.Image = bmp;
            niekliknote = true;
        }

        public void start(MouseEventArgs e)
        {
            tick = 0;
            bmp = new Bitmap(szyrzka, wyszka);
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pictureBox1.Image = bmp;
            if (niekliknote && e.X >= pictureBox1.Width / 3 && e.X <= pictureBox1.Width / 3 * 2 && e.Y >= pictureBox1.Height / 3 && e.Y <= pictureBox1.Width / 12 * 7)
            {
                g.Clear(Color.Black);
                Image newImage = Image.FromFile(".\\icons\\3.png");
                inicjalizacja();
                namalujWszystkiKolka();
                namalujKierunki();
                namalujIlosciPuntkow();
                for (int a = 0; a < 50; a++)
                {
                    g.Clear(Color.Black);
                    Rectangle rect = new Rectangle(Convert.ToInt32(szyrzka / 2.355) + a, Convert.ToInt32(wyszka / 2.82) + a, Convert.ToInt32(szyrzka / 6.9) - 2 * a, Convert.ToInt32(szyrzka / 6.9) - 2 * a);
                    g.DrawImage(newImage, rect);
                    namalujWszystkiKolka();
                    namalujKierunki();
                    namalujIlosciPuntkow();
                    pictureBox1.Image = bmp;
                    pictureBox1.Update();
                    pictureBox1.Show();
                }
                newImage = Image.FromFile(".\\icons\\2.png");
                for (int a = 0; a < 50; a++)
                {
                    g.Clear(Color.Black);
                    Rectangle rect = new Rectangle(Convert.ToInt32(szyrzka / 2.355) + a, Convert.ToInt32(wyszka / 2.82) + a, Convert.ToInt32(szyrzka / 6.9) - 2 * a, Convert.ToInt32(szyrzka / 6.9) - 2 * a);
                    g.DrawImage(newImage, rect);
                    namalujWszystkiKolka();
                    namalujKierunki();
                    namalujIlosciPuntkow();
                    pictureBox1.Image = bmp;
                    pictureBox1.Update();
                    pictureBox1.Show();
                }
                newImage = Image.FromFile(".\\icons\\1.png");
                for (int a = 0; a < 50; a++)
                {
                    g.Clear(Color.Black);
                    Rectangle rect = new Rectangle(Convert.ToInt32(szyrzka / 2.355) + a, Convert.ToInt32(wyszka / 2.82) + a, Convert.ToInt32(szyrzka / 6.9) - 2 * a, Convert.ToInt32(szyrzka / 6.9) - 2 * a);
                    g.DrawImage(newImage, rect);
                    namalujWszystkiKolka();
                    namalujKierunki();
                    namalujIlosciPuntkow();
                    pictureBox1.Image = bmp;
                    pictureBox1.Update();
                    pictureBox1.Show();
                }
                g.Clear(Color.Black);
                namalujWszystkiKolka();
                namalujKierunki();
                namalujIlosciPuntkow();
                timer1.Start();
                pictureBox1.Image = bmp;
                niekliknote = false;
                tick = 0;
            }
            else if (koniec() && e.X >= 570 && e.X <= 625 && e.Y >= 307 && e.Y <= 342)
            {
                for (int a = 0; a < 4; a++)
                {
                    iloscPunktow[a] = 0;
                }
                inicjalizacja();
                tick = 0;
                //start(e);
            }
            else if (e.X >= 20 && e.X <= 70 && e.Y >= 20 && e.Y <= 70)
            {
                g.Clear(Color.Black);
                Image newImage = Image.FromFile(".\\icons\\startbutton.png");
                Rectangle rect = new Rectangle(pictureBox1.Width / 3, pictureBox1.Height / 3, pictureBox1.Width / 3, pictureBox1.Height / 4);
                g.DrawImage(newImage, rect);
                Image newImage2 = Image.FromFile(".\\icons\\settings.png");
                Rectangle rect2 = new Rectangle(20, 20, 50, 50);
                g.DrawImage(newImage2, rect2);
                namalujIlosciPuntkow();
                st = new settings();
                st.Show();
                niekliknote = true;
            }
            else
            {
                g.Clear(Color.Black);
                Image newImage = Image.FromFile(".\\icons\\startbutton.png");
                Rectangle rect = new Rectangle(pictureBox1.Width / 3, pictureBox1.Height / 3, pictureBox1.Width / 3, pictureBox1.Height / 4);
                g.DrawImage(newImage, rect);
                Image newImage2 = Image.FromFile(".\\icons\\settings.png");
                Rectangle rect2 = new Rectangle(20, 20, 50, 50);
                g.DrawImage(newImage2, rect2);
                namalujIlosciPuntkow();
                niekliknote = true;
            }
        }

        public void startSpace()
        {
            tick = 0;
            bmp = new Bitmap(szyrzka, wyszka);
            g = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
            g.Clear(Color.Black);
            Image newImage = Image.FromFile(".\\icons\\3.png");
            inicjalizacja();
            namalujWszystkiKolka();
            namalujKierunki();
            namalujIlosciPuntkow();
            for (int a = 0; a < 50; a++)
            {
                g.Clear(Color.Black);
                Rectangle rect = new Rectangle(580 + a, 250 + a, 200 - 2 * a, 200 - 2 * a);
                g.DrawImage(newImage, rect);
                namalujWszystkiKolka();
                namalujKierunki();
                namalujIlosciPuntkow();
                pictureBox1.Image = bmp;
                pictureBox1.Update();
                pictureBox1.Show();
            }
            newImage = Image.FromFile(".\\icons\\2.png");
            for (int a = 0; a < 50; a++)
            {
                g.Clear(Color.Black);
                Rectangle rect = new Rectangle(580 + a, 250 + a, 200 - 2 * a, 200 - 2 * a);
                g.DrawImage(newImage, rect);
                namalujWszystkiKolka();
                namalujKierunki();
                namalujIlosciPuntkow();
                pictureBox1.Image = bmp;
                pictureBox1.Update();
                pictureBox1.Show();
            }
            newImage = Image.FromFile(".\\icons\\1.png");
            for (int a = 0; a < 50; a++)
            {
                g.Clear(Color.Black);
                Rectangle rect = new Rectangle(580 + a, 250 + a, 200 - 2 * a, 200 - 2 * a);
                g.DrawImage(newImage, rect);
                namalujWszystkiKolka();
                namalujKierunki();
                namalujIlosciPuntkow();
                pictureBox1.Image = bmp;
                pictureBox1.Update();
                pictureBox1.Show();
            }
            g.Clear(Color.Black);
            namalujWszystkiKolka();
            namalujKierunki();
            namalujIlosciPuntkow();
            timer1.Start();
            pictureBox1.Image = bmp;
            niekliknote = false;
            tick = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Stop();
            st.Show();
            st.Close();
            pictureBox1.Width = Convert.ToInt32(st.numericUpDown4.Value);
            pictureBox1.Height = Convert.ToInt32(st.numericUpDown5.Value);
            inicjalizacja();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!timer1.Enabled)
            {
                start(e);
            }
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            //bmp = new Bitmap(szyrzka, wyszka);
            tick++;
            //namalujIlosciPuntkow();
            for (int i = 0; i < iloscbonusow; i++ )
            {
                if(odpoczitawaniTrwaniaBonusu[i] > 0)
                {
                    odpoczitawaniTrwaniaBonusu[i]--;
                }
            }
            if (tick % 300 == 0 && tick != 0)
            {
                odpoczitawaniNamaluj = 20;
            }
            if (odpoczitawaniNamaluj > 0)
            {
                namaluj = false;
                namalujWszystkiKolka();
                odpoczitawaniNamaluj--;
            }
            else
            {
                namaluj = true;
            }
            g = Graphics.FromImage(bmp);
            kompX();
            zruszyniUczinkuBonusow();
            zmianyKierunku();
            odpoczitej90();
            naburali();
            posunyciKolek();
            namalujKierunki();
            namalujWszystkiKolka();
            if (odpoczitawaniSpawnyciaBonusu > 0)
            {
                odpoczitawaniSpawnyciaBonusu--;
            }
            Random rnd = new Random();
            int a = rnd.Next(0, 1000);
            if (/*a > 992 && */odpoczitawaniSpawnyciaBonusu == 0)
            {
                namalujDalszyBonus();
                odpoczitawaniSpawnyciaBonusu = 260;
            }
            restartnabidni();
            pictureBox1.Image = bmp;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                zmianakierunku[0] = 4;
            }
            if (e.KeyCode == Keys.Right)
            {
                zmianakierunku[0] = 6;
            }
            if (e.KeyCode == Keys.A)
            {
                zmianakierunku[1] = 4;
            }
            if (e.KeyCode == Keys.D)
            {
                zmianakierunku[1] = 6;
            }
            if (e.KeyCode == Keys.NumPad4)
            {
                zmianakierunku[2] = 4;
            }
            if (e.KeyCode == Keys.NumPad6)
            {
                zmianakierunku[2] = 6;
            }
            if (e.KeyCode == Keys.B)
            {
                zmianakierunku[3] = 4;
            }
            if (e.KeyCode == Keys.M)
            {
                zmianakierunku[3] = 6;
            }
            if (e.KeyCode == Keys.Space && !timer1.Enabled && (ilosckolek > 1 ? iloscPunktow[0] != 0 || iloscPunktow[1] != 0 || iloscPunktow[2] != 0 || iloscPunktow[3] != 0 : true))
            {
                startSpace();
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                zmianakierunku[0] = 0;
            }
            if (e.KeyCode == Keys.Right)
            {
                zmianakierunku[0] = 0;
            }
            if (e.KeyCode == Keys.A)
            {
                zmianakierunku[1] = 0;
            }
            if (e.KeyCode == Keys.D)
            {
                zmianakierunku[1] = 0;
            }
            if (e.KeyCode == Keys.NumPad4)
            {
                zmianakierunku[2] = 0;
            }
            if (e.KeyCode == Keys.NumPad6)
            {
                zmianakierunku[2] = 0;
            }
            if (e.KeyCode == Keys.B)
            {
                zmianakierunku[3] = 0;
            }
            if (e.KeyCode == Keys.M)
            {
                zmianakierunku[3] = 0;
            }
        }
    }
}
