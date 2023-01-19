using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace EntityOgrenciNotlar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        #region Nesneler
        DbOgrenciNotlarEntities DbModel = new DbOgrenciNotlarEntities();

        #endregion




        SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-EPE6CU2L\MSSQLSERVER1;Initial Catalog=DbOgrenciNotlar;Integrated Security=True");


        #region Ders Listeleme
        private void button1_Click(object sender, EventArgs e)
        {

            connection.Open();
            SqlCommand command = new SqlCommand("select *from TBLDERSLER", connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            command.ExecuteNonQuery();
            connection.Close();
        }
        #endregion


        #region Öğrenci listeleme

        private void BtnOgrenciListele_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DbModel.TBLOGRENCI.ToList();

        }

        #endregion

        #region Not listesi
        private void BtnNotlistesi_Click(object sender, EventArgs e)
        {
            var query = from item in DbModel.TBLNOTLAR
                        select new
                        {
                            item.NOTID,
                            item.OGR,
                            item.SINAV1,
                            item.SINAV2,
                            item.SINAV3,
                            item.ORTALAMA
                        };
            dataGridView1.DataSource = query.ToList();

        }

        #endregion

        #region Kaydet Butonu
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            TBLOGRENCI tblOgrenci = new TBLOGRENCI();
            tblOgrenci.AD = TxtOgrenciAd.Text;
            tblOgrenci.SOYAD = TxtSoyad.Text;
            DbModel.TBLOGRENCI.Add(tblOgrenci);
            DbModel.SaveChanges();
            MessageBox.Show("Öğrenci kaydedildi");

        }
        #endregion


        #region Sil Butonu

        private void BtnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TxtOgrenciID.Text);
            var SilinecekSatir = DbModel.TBLOGRENCI.Find(id);
            DbModel.TBLOGRENCI.Remove(SilinecekSatir);
            DbModel.SaveChanges();
            MessageBox.Show("Öğrenci Sistemden Silindi");
        }



        #endregion




        #region Güncelle butonu
        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TxtOgrenciID.Text);
            var SatiriGuncelle = DbModel.TBLOGRENCI.Find(id);
            SatiriGuncelle.AD = TxtOgrenciAd.Text;
            SatiriGuncelle.SOYAD = TxtSoyad.Text;
            SatiriGuncelle.FOTOGRAF = TxtFotograf.Text;
            DbModel.SaveChanges();
            MessageBox.Show("Öğrenci bilgileri güncellendi");
        }

        #endregion


        #region BtnBul
        private void BtnBul_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DbModel.TBLOGRENCI.Where(x => x.AD == TxtOgrenciAd.Text).ToList();


         
        }

        #endregion


        #region TxtOgrenciAd_TextChanged
        private void TxtOgrenciAd_TextChanged(object sender, EventArgs e)
        {
            string aranan = TxtOgrenciAd.Text;
            var degerler = from item in DbModel.TBLOGRENCI
                           where item.AD.Contains(aranan)
                           select item;
            dataGridView1.DataSource = degerler.ToList();
           
            
        }
        #endregion

        private void BtnLinqEntity_Click(object sender, EventArgs e)
        {

            List<TBLOGRENCI> liste;

            #region A'dan Z'ye sıralama yapan kod
            if (radioButton1.Checked == true)
            {
                liste = DbModel.TBLOGRENCI.OrderBy(p => p.AD).ToList();
                dataGridView1.DataSource = liste;
            }
            #endregion

            #region Z den A'ya göre sıralama yapan kod
            if (radioButton2.Checked==true)
            {
                liste = DbModel.TBLOGRENCI.OrderByDescending(p => p.AD).ToList();
                dataGridView1.DataSource = liste;
            }
            #endregion


            #region A'dan Z'ye göre sıralama yapıp ilk 3 öğrenciyi getiren kod
            if (radioButton3.Checked==true)
            {
                liste = DbModel.TBLOGRENCI.OrderBy(p => p.AD).Take(3).ToList();
                dataGridView1.DataSource = liste;
                
            }
            #endregion

            #region ID'ye göre öğrenci getiren kod
            if (radioButton4.Checked==true)

            {
                int deger=int.Parse(TxtOgrenciID.Text);
                liste = DbModel.TBLOGRENCI.Where(p => p.ID == deger).ToList();
                dataGridView1.DataSource = liste;
            }
            #endregion

            #region Başında A olan öğrencileri getiren kod
            if (radioButton5.Checked==true)
            {

                liste = DbModel.TBLOGRENCI.Where(p => p.AD.StartsWith("a")).ToList();
                dataGridView1.DataSource = liste;
            }
            #endregion

            #region Sonunda A olan ogrencileri getiren kod
            if (radioButton6.Checked==true)
            {
                liste = DbModel.TBLOGRENCI.Where(p => p.AD.EndsWith("a")).ToList();
                dataGridView1.DataSource = liste;
            }
            #endregion

            #region Tabloda veri olup olmadığını kontrol eden kod
            if (radioButton7.Checked == true)
            {
                bool deger = DbModel.TBLOGRENCI.Any();
                MessageBox.Show(deger.ToString());
            }
            
            #endregion

            #region Toplam Ogrenci Sayısı
            if (radioButton8.Checked == true)
            {
                int toplam = DbModel.TBLOGRENCI.Count();
                MessageBox.Show(toplam.ToString());
            }
            #endregion

            #region SINAV1 notlarının toplamı
            if (radioButton9.Checked==true)
            {
                var toplam = DbModel.TBLNOTLAR.Sum(p => p.SINAV1);
                MessageBox.Show("Toplam Öğrenci Sayısı:" + toplam.ToString());
            }
            #endregion

            #region Ortalama bulma
            if (radioButton10.Checked==true)
            {
                var ortalama = DbModel.TBLNOTLAR.Average(p => p.SINAV1);
                MessageBox.Show("Sınav1 ortalama:" + ortalama.ToString());
            }
            #endregion

            #region Ortalamadan büyük olan notları getiren kod
            if (radioButton11.Checked == true)
            {
                var ortalama = DbModel.TBLNOTLAR.Average(p => p.SINAV1);
                var OrtBuyuk = DbModel.TBLNOTLAR.Where(x => x.SINAV1 > ortalama).ToList();
                dataGridView1.DataSource = OrtBuyuk;
            }

            #endregion

            #region En Yüksek Sınav1 i görüntüleyen kod
            if (radioButton12.Checked==true)
            {
                var enYuksek = DbModel.TBLNOTLAR.Max(x => x.SINAV1).ToString();
                MessageBox.Show("En Yüksek Not:" + enYuksek);

            }
#endregion

            #region En Düşük Sınav1 i görüntüleyen kod
            if (radioButton13.Checked == true)
            {
                var enDusuk = DbModel.TBLNOTLAR.Min(x => x.SINAV1).ToString();
                MessageBox.Show("En Düşük Not:" + enDusuk);

            }
            #endregion

            #region En Yüksek Nota Sahip Öğrenci
            if (radioButton14.Checked == true)
            {
                var yuksekNot = DbModel.TBLNOTLAR.Max(x => x.SINAV1);
                var ogrenciKim = from item in DbModel.NOTLISTESI()
                                 where item.SINAV1 == yuksekNot
                                 select new
                                 {
                                     item.AD_SOYAD,
                                     item.DERSAD,
                                     item.SINAV1
                                 };
                dataGridView1.DataSource = ogrenciKim.ToList();

            }

            #endregion


        }

        #region JOIN İŞLEMİ
        private void BtnSinavGuncelle_Click(object sender, EventArgs e)
        {
            var sorgu = from db1 in DbModel.TBLNOTLAR
                        join db2 in DbModel.TBLOGRENCI
                        on db1.OGR equals db2.ID
                        select new
                        {
                            ÖĞRENCİ_AD_SOYAD=db2.AD +" "+ db2.SOYAD,
                            s1=db1.SINAV1,
                            s2=db1.SINAV2,
                            s3=db1.SINAV3
                        };

            dataGridView1.DataSource = sorgu.ToList();

}
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 fr = new Form2();
            fr.Show();
            this.Hide();
        }
    }
}

