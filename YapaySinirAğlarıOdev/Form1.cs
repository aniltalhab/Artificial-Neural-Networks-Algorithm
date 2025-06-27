using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace YapaySinirAğlarıOdev
{
    public partial class Form1 : Form
    {
        YapaySinirAgi ag;
        int[][] egitimVerileri;
        int[][] hedefler;
        private Button[,] matrisButonlari = new Button[7, 5];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ag = new YapaySinirAgi(15);
            egitimVerileri = ag.EgitimVerileriniGetir();
            hedefler = ag.HedefleriGetir();
            int butonBoyut = 40;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Button btn = new Button();
                    btn.Width = butonBoyut;
                    btn.Height = butonBoyut;
                    btn.Left = j * (butonBoyut + 2);
                    btn.Top = i * (butonBoyut + 2);
                    btn.Tag = 0;
                    btn.BackColor = Color.White;
                    btn.Click += MatrisButon_Click;
                    this.Controls.Add(btn);
                    matrisButonlari[i, j] = btn;
                }
            }
        }

        private int[] GirisVerisiniAl()
        {
            List<int> giris = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    giris.Add((int)matrisButonlari[i, j].Tag);
                }
            }
            return giris.ToArray();
        }

        private void HarfiButonlaraYaz(int[] harfVerisi)
        {
            int index = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrisButonlari[i, j].Tag = harfVerisi[index];
                    matrisButonlari[i, j].BackColor = (harfVerisi[index] == 1) ? Color.Black : Color.White;
                    index++;
                }
            }
        }

        private void MatrisButon_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if ((int)btn.Tag == 0)
            {
                btn.BackColor = Color.Black;
                btn.Tag = 1;
            }
            else
            {
                btn.BackColor = Color.White;
                btn.Tag = 0;
            }
        }

        private void btnEgit_Click(object sender, EventArgs e)
        {
            double hataLimiti;
            if (!double.TryParse(txtEpsilon.Text, out hataLimiti) || hataLimiti <= 0)
            {
                MessageBox.Show("Lütfen geçerli bir epsilon hata değeri girin (pozitif bir sayı).", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            double hata = ag.Egit(egitimVerileri, hedefler, 10000, hataLimiti);
            txtHata.Text = hata.ToString("F4");
        }

        private void btnTanimla_Click(object sender, EventArgs e)
        {
            int[] girdi = GirisVerisiniAl();
            double[] cikti = ag.IleriBesleme(girdi);
            txtA.Text = cikti[0].ToString("F8");
            txtB.Text = cikti[1].ToString("F8");
            txtC.Text = cikti[2].ToString("F8");
            txtD.Text = cikti[3].ToString("F8");
            txtE.Text = cikti[4].ToString("F8");
            double maxDeger = cikti.Max();
            int maxIndex = Array.IndexOf(cikti, maxDeger);
            string sonucHarfi;
            switch (maxIndex)
            {
                case 0: sonucHarfi = "A"; break;
                case 1: sonucHarfi = "B"; break;
                case 2: sonucHarfi = "C"; break;
                case 3: sonucHarfi = "D"; break;
                case 4: sonucHarfi = "E"; break;
                default: sonucHarfi = "Bilinmiyor"; break;
            }
            if (lblSonuc != null)
            {
                lblSonuc.Text = sonucHarfi;
            }
            else
            {
                MessageBox.Show("lblSonuc label'ı bulunamadı! Lütfen formda lblSonuc adında bir Label ekleyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrisButonlari[i, j].Tag = 0;
                    matrisButonlari[i, j].BackColor = Color.White;
                }
            }
            txtE.Clear();
            txtA.Clear();
            txtB.Clear();
            txtC.Clear();
            txtD.Clear();
            txtEpsilon.Clear();
            txtHata.Clear();
            lblSonuc.Text = "SONUC";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}