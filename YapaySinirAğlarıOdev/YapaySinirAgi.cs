using System;

namespace YapaySinirAğlarıOdev
{
    public class YapaySinirAgi
    {
        private int girisSayisi = 35;
        private int gizliKatmanSayisi;
        private int cikisSayisi = 5;
        private double[,] girisGizliAgirlik;
        private double[,] gizliCikisAgirlik;
        private double[] gizliKatman;
        private double[] cikisKatman;
        private double ogrenmeKatsayisi = 0.1;

        public YapaySinirAgi(int gizliNoronSayisi)
        {
            gizliKatmanSayisi = gizliNoronSayisi;
            gizliKatman = new double[gizliKatmanSayisi];
            cikisKatman = new double[cikisSayisi];
            girisGizliAgirlik = new double[girisSayisi, gizliKatmanSayisi];
            gizliCikisAgirlik = new double[gizliKatmanSayisi, cikisSayisi];
            Random rastgele = new Random();
            for (int i = 0; i < girisSayisi; i++)
                for (int j = 0; j < gizliKatmanSayisi; j++)
                    girisGizliAgirlik[i, j] = rastgele.NextDouble() * 2 - 1;
            for (int j = 0; j < gizliKatmanSayisi; j++)
                for (int k = 0; k < cikisSayisi; k++)
                    gizliCikisAgirlik[j, k] = rastgele.NextDouble() * 2 - 1;
        }

        private double Sigmoid(double x) => 1.0 / (1.0 + Math.Exp(-x));
        private double SigmoidTurev(double x) => x * (1 - x);

        public double[] IleriBesleme(int[] girdi)
        {
            for (int i = 0; i < gizliKatmanSayisi; i++)
            {
                double toplam = 0;
                for (int j = 0; j < girisSayisi; j++)
                    toplam += girdi[j] * girisGizliAgirlik[j, i];
                gizliKatman[i] = Sigmoid(toplam);
            }
            for (int i = 0; i < cikisSayisi; i++)
            {
                double toplam = 0;
                for (int j = 0; j < gizliKatmanSayisi; j++)
                    toplam += gizliKatman[j] * gizliCikisAgirlik[j, i];
                cikisKatman[i] = Sigmoid(toplam);
            }
            return cikisKatman;
        }

        public double Egit(int[][] egitimVerileri, int[][] hedefler, int iterasyon, double hataLimiti)
        {
            double toplamHata = 0;
            for (int epok = 0; epok < iterasyon; epok++)
            {
                toplamHata = 0;
                for (int ornek = 0; ornek < egitimVerileri.Length; ornek++)
                {
                    double[] cikti = IleriBesleme(egitimVerileri[ornek]);
                    double[] cikisHatalari = new double[cikisSayisi];
                    for (int i = 0; i < cikisSayisi; i++)
                    {
                        cikisHatalari[i] = hedefler[ornek][i] - cikti[i];
                        toplamHata += Math.Pow(cikisHatalari[i], 2);
                    }
                    double[] gizliHatalar = new double[gizliKatmanSayisi];
                    for (int i = 0; i < gizliKatmanSayisi; i++)
                    {
                        gizliHatalar[i] = 0;
                        for (int j = 0; j < cikisSayisi; j++)
                            gizliHatalar[i] += cikisHatalari[j] * gizliCikisAgirlik[i, j];
                    }
                    for (int i = 0; i < gizliKatmanSayisi; i++)
                        for (int j = 0; j < cikisSayisi; j++)
                            gizliCikisAgirlik[i, j] += ogrenmeKatsayisi * cikisHatalari[j] * SigmoidTurev(cikisKatman[j]) * gizliKatman[i];
                    for (int i = 0; i < girisSayisi; i++)
                        for (int j = 0; j < gizliKatmanSayisi; j++)
                            girisGizliAgirlik[i, j] += ogrenmeKatsayisi * gizliHatalar[j] * SigmoidTurev(gizliKatman[j]) * egitimVerileri[ornek][i];
                }
                if (toplamHata < hataLimiti)
                    break;
            }
            return toplamHata;
        }

        public int[][] EgitimVerileriniGetir()
        {
            return new int[][]
            {
                new int[] { 0,1,1,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,1,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1 },
                new int[] { 0,0,1,0,0, 0,1,0,1,0, 1,0,0,0,1, 1,1,1,1,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1 },
                new int[] { 0,0,1,0,0, 0,1,0,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1 },
                new int[] { 0,0,1,0,0, 0,1,1,1,0, 1,0,1,0,1, 1,0,0,0,1, 1,1,1,1,1, 1,0,0,0,1, 1,0,0,0,1 },
                new int[] { 0,0,1,0,0, 0,1,0,1,0, 1,1,0,1,1, 1,0,1,0,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1 },
                new int[] { 1,1,1,1,1, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,1,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1 },
                new int[] { 0,0,1,0,0, 0,1,0,1,0, 1,0,0,0,1, 1,1,1,1,1, 1,0,0,0,1, 1,0,0,0,1, 0,1,1,1,0 },
                new int[] { 1,1,1,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,1,0 },
                new int[] { 1,1,1,0,0, 1,0,0,1,0, 1,0,0,0,1, 1,1,1,0,0, 1,0,0,1,0, 1,0,0,0,1, 1,1,1,0,0 },
                new int[] { 1,1,1,1,1, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,1,1, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,1,1 },
                new int[] { 1,1,1,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,0,0, 1,0,0,1,0, 1,0,0,0,1, 1,1,1,1,0 },
                new int[] { 1,1,1,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,1,1, 1,0,0,0,0, 1,0,0,0,0, 1,1,1,1,1 },
                new int[] { 1,1,1,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,1,0, 1,0,1,0,1, 1,0,0,0,1, 1,1,0,1,1 },
                new int[] { 1,1,1,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,1,0, 1,0,0,0,1, 1,0,0,0,1, 0,1,1,1,0 },
                new int[] { 0,1,1,1,1, 1,0,0,0,0, 1,0,0,0,0, 1,0,0,0,0, 1,0,0,0,0, 1,0,0,0,0, 0,1,1,1,1 },
                new int[] { 0,0,1,1,1, 0,1,0,0,0, 1,0,0,0,0, 1,0,0,0,0, 1,0,0,0,0, 0,1,0,0,0, 0,0,1,1,1 },
                new int[] { 1,1,1,1,1, 1,0,0,0,0, 1,0,0,0,0, 1,1,1,0,0, 1,0,0,0,0, 1,0,0,0,0, 1,1,1,1,1 },
                new int[] { 0,0,1,1,0, 0,1,0,0,1, 1,0,0,0,0, 1,0,0,0,0, 1,0,0,0,0, 0,1,0,0,1, 0,0,1,1,0 },
                new int[] { 1,1,1,1,1, 1,0,0,0,0, 1,0,0,0,0, 1,0,0,0,0, 1,0,0,0,0, 1,0,0,0,0, 1,1,1,1,1 },
                new int[] { 0,1,1,1,0, 1,0,0,0,1, 1,0,0,0,0, 1,0,1,1,1, 1,0,0,0,0, 1,0,0,0,1, 0,1,1,1,0 },
                new int[] { 0,0,1,1,1, 0,1,0,0,0, 1,0,0,0,0, 1,0,1,1,1, 1,0,0,0,0, 0,1,0,0,0, 0,0,1,1,1 },
                new int[] { 1,1,1,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1, 1,1,1,1,0 },
                new int[] { 1,1,1,0,0, 1,0,0,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,1,0, 1,1,1,0,0 },
                new int[] { 1,1,1,1,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1, 0,1,1,1,0 },
                new int[] { 1,1,1,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,1,0,0,1, 1,0,1,0,1, 1,0,0,1,0, 1,1,1,0,0 },
                new int[] { 1,1,1,0,0, 1,0,1,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,1,0, 1,1,1,0,0 },
                new int[] { 1,1,1,0,0, 1,0,0,1,0, 1,0,0,0,1, 1,1,0,0,1, 1,0,1,0,1, 1,0,0,1,0, 1,1,1,0,0 },
                new int[] { 1,1,1,0,0, 1,0,0,1,0, 1,0,0,0,1, 1,0,0,0,1, 1,0,0,0,1, 1,1,0,1,0, 1,0,1,0,0 },
                new int[] { 1,1,1,1,1, 1,0,0,0,0, 1,0,0,0,0, 1,1,1,1,1, 1,0,0,0,0, 1,0,0,0,0, 1,1,1,1,1 },
                new int[] { 1,1,1,1,1, 1,0,0,0,0, 1,0,0,0,0, 1,1,1,0,0, 1,0,0,0,0, 1,0,0,0,0, 1,1,1,1,1 },
                new int[] { 1,1,1,1,1, 1,0,0,0,0, 1,0,0,0,0, 1,1,1,1,0, 1,0,0,0,0, 1,0,0,0,0, 1,1,1,1,1 },
                new int[] { 1,1,1,1,1, 1,0,0,0,0, 1,1,1,0,0, 1,0,0,0,0, 1,1,1,0,0, 1,0,0,0,0, 1,1,1,1,1 },
                new int[] { 1,1,1,1,1, 1,0,1,0,0, 1,0,1,0,0, 1,1,1,1,1, 1,0,1,0,0, 1,0,0,0,0, 1,1,1,1,1 },
                new int[] { 1,1,1,1,1, 0,0,0,0,1, 0,0,0,0,1, 1,1,1,1,1, 0,0,0,0,1, 0,0,0,0,1, 1,1,1,1,1 },
                new int[] { 1,1,1,1,1, 1,0,0,0,0, 1,0,0,0,0, 1,1,1,1,1, 1,0,0,0,0, 1,0,0,0,0, 0,1,1,1,1 }
            };
        }

        public int[][] HedefleriGetir()
        {
            return new int[][]
            {
                new int[] { 1, 0, 0, 0, 0 }, new int[] { 1, 0, 0, 0, 0 }, new int[] { 1, 0, 0, 0, 0 },
                new int[] { 1, 0, 0, 0, 0 }, new int[] { 1, 0, 0, 0, 0 }, new int[] { 1, 0, 0, 0, 0 },
                new int[] { 1, 0, 0, 0, 0 }, new int[] { 0, 1, 0, 0, 0 }, new int[] { 0, 1, 0, 0, 0 },
                new int[] { 0, 1, 0, 0, 0 }, new int[] { 0, 1, 0, 0, 0 }, new int[] { 0, 1, 0, 0, 0 },
                new int[] { 0, 1, 0, 0, 0 }, new int[] { 0, 1, 0, 0, 0 }, new int[] { 0, 0, 1, 0, 0 },
                new int[] { 0, 0, 1, 0, 0 }, new int[] { 0, 0, 1, 0, 0 }, new int[] { 0, 0, 1, 0, 0 },
                new int[] { 0, 0, 1, 0, 0 }, new int[] { 0, 0, 1, 0, 0 }, new int[] { 0, 0, 1, 0, 0 },
                new int[] { 0, 0, 0, 1, 0 }, new int[] { 0, 0, 0, 1, 0 }, new int[] { 0, 0, 0, 1, 0 },
                new int[] { 0, 0, 0, 1, 0 }, new int[] { 0, 0, 0, 1, 0 }, new int[] { 0, 0, 0, 1, 0 },
                new int[] { 0, 0, 0, 1, 0 }, new int[] { 0, 0, 0, 0, 1 }, new int[] { 0, 0, 0, 0, 1 },
                new int[] { 0, 0, 0, 0, 1 }, new int[] { 0, 0, 0, 0, 1 }, new int[] { 0, 0, 0, 0, 1 },
                new int[] { 0, 0, 0, 0, 1 }, new int[] { 0, 0, 0, 0, 1 }
            };
        }
    }
}