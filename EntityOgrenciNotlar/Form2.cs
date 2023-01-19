using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityOgrenciNotlar
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        DbOgrenciNotlarEntities DbModel = new DbOgrenciNotlarEntities();

        private void BtnLinqEntity_Click(object sender, EventArgs e)
        {
            


            #region 1. Sınavı 50'den küçük notlar
            if (radioButton1.Checked == true)
            {

                var degerler = DbModel.TBLNOTLAR.Where(x => x.SINAV1 < 50);
                dataGridView1.DataSource = degerler.ToList();

            }
            #endregion




            #region Adını veya Soyadını Textten Al

            if (radioButton2.Checked == true)
            {
                var degerler = DbModel.TBLOGRENCI.Where(x => x.AD == "Ali");
                dataGridView1.DataSource = degerler.ToList();
            }
            #endregion


            #region Adı veya Soyadı texten al

            if (radioButton3.Checked == true)
            {
                var degerler = DbModel.TBLOGRENCI.Where(x => x.AD == textBox1.Text || x.SOYAD == textBox1.Text);
                dataGridView1.DataSource = degerler.ToList();
            }
            #endregion


            #region Öğrenci Soyadları

            if (radioButton4.Checked == true)
            {
                var degerler = DbModel.TBLOGRENCI.Select(x =>
                new
                { soyadi = x.SOYAD });
                dataGridView1.DataSource = degerler.ToList();
            }

            #endregion


            #region Ad Büyük Soyad Küçük

            if (radioButton5.Checked == true)
            {
                var degerler = DbModel.TBLOGRENCI.Select(x => new { Ad = x.AD.ToUpper(), Soyad = x.SOYAD.ToLower() });
                dataGridView1.DataSource = degerler.ToList();

            }
            #endregion


            #region Şartlı Seçim 

            if (radioButton6.Checked == true)
            {
                var degerler = DbModel.TBLOGRENCI.Select(x =>
                new
                {
                    Ad = x.AD.ToUpper(),
                    Soyad = x.SOYAD.ToLower()
                }).Where(x => x.Ad != "ali");
                dataGridView1.DataSource = degerler.ToList();

            }

            #endregion


            #region Geçti mi Kaldı mı?
            if (radioButton7.Checked == true)
            {
                var degerler = DbModel.TBLNOTLAR.Select(x => new
                {
                    Adı = x.OGR,
                    OgrenciOrtalaması = x.ORTALAMA,
                    OgrenciDurumu = x.DURUM == true ? "Geçti" : "Kaldı"
                });

                dataGridView1.DataSource = degerler.ToList();

            }

            #endregion


            #region Birleştir
            if (radioButton8.Checked == true)
            {
                var degerler = DbModel.TBLNOTLAR.SelectMany(x => DbModel.TBLOGRENCI.Where(y => y.ID == x.OGR), (x, y) => new
                {
                    OGRENCİAD = y.AD,
                    OGRENCİORTALAMA = x.ORTALAMA,
                    DURUM = x.DURUM == true ? "Geçti" : "Kaldı"
                });


                dataGridView1.DataSource = degerler.ToList();

            }

            #endregion

            #region İlk 3 değer
            if (radioButton9.Checked == true)
            {
                var degerler = DbModel.TBLOGRENCI.OrderBy(x => x.ID).Take(3);
                dataGridView1.DataSource = degerler.ToList();
            }

            #endregion

            #region Son 3 değer
            if (radioButton10.Checked == true)
            {
                var degerler = DbModel.TBLOGRENCI.OrderByDescending(x => x.ID).Take(3);
                dataGridView1.DataSource = degerler.ToList();
            }
            #endregion


            #region Ada Göre Sırala
            if (radioButton10.Checked == true)
            {
                var degerler = DbModel.TBLOGRENCI.OrderBy(x => x.AD).Take(3);
                dataGridView1.DataSource = degerler.ToList();
            }
            #endregion

            #region Ada Göre Sırala
            if (radioButton11.Checked == true)
            {
                var degerler = DbModel.TBLOGRENCI.OrderBy(x => x.AD).Skip(5);
                dataGridView1.DataSource = degerler.ToList();
            }
            #endregion

            #region Grupla
            if(radioButton13.Checked==true)
            {
                var degerler = DbModel.TBLOGRENCI.OrderBy(x => x.SEHIR).GroupBy(y => y.SEHIR).Select(z => new { Şehir = z.Key, Toplam = z.Count() });

                dataGridView1.DataSource = degerler.ToList();
            }
            #endregion

            #region En Yüksek Ortalama
            if(radioButton14.Checked==true)
            {
                label3.Text = DbModel.TBLNOTLAR.Max(x => x.ORTALAMA).ToString();
               
            }
            #endregion

            #region Sınav1 En Düşük Not
            if(radioButton15.Checked==true)
            {
                label5.Text = DbModel.TBLNOTLAR.Min(x => x.SINAV1).ToString();
            }
            #endregion

            #region Sınav1 En Düşük Not
            if (radioButton16.Checked == true)
            {
                label6.Text = DbModel.TBLNOTLAR.Where(x => x.DURUM == false).Max(y => y.ORTALAMA).ToString();
            
            }
            #endregion


            #region Toplam Stok
            if (radioButton17.Checked==true)
            {
                label9.Text = DbModel.TBLURUN.Count().ToString();

            }
            #endregion

            #region Toplam Stok
            if (radioButton18.Checked == true)
            {
                label10.Text = DbModel.TBLURUN.Sum(x => x.STOK).ToString();
            
            }
            #endregion

            #region Buzdolabı Fiyat Ortalaması
            if (radioButton19.Checked == true)
            {
                label15.Text = DbModel.TBLURUN.Where(x => x.AD == "Buzdolabı").Average(y => y.STOK).ToString();
            
            
            }


            #endregion

            #region En Çok Stoğa Sahip Ürün

            if(radioButton20.Checked==true)
            {
                label17.Text = (from x in DbModel.TBLURUN
                                orderby x.STOK descending
                                select x.AD).First();

            }
            #endregion

            if(radioButton21.Checked==true)
            {
                var degerler = DbModel.OgrenciKulup();
                dataGridView1.DataSource = degerler.ToList();
            }


        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
            this.Hide();
        }
    }
}