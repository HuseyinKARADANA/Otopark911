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
namespace OtoparkOtomasyonu
{
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
        }
        int kapasite = 20;
        
        int islemId=-1;
        DateTime girisTarih = DateTime.Now;
        sqlbaglantisi bgl=new sqlbaglantisi();
        
        public int DbDoluluk()
        { int dbDoluluk = 0;
            SqlCommand komut = new SqlCommand("SELECT COUNT(*) AS CikisYapmamisKullaniciSayisi FROM TblOtoparkKayit WHERE CikisTarih IS NULL", bgl.baglanti());
            
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                dbDoluluk =int.Parse( dr[0].ToString());
            }
            bgl.baglanti().Close();
            return dbDoluluk;
        }
        public void Temizle()
        {
            txtAd.Text = "";
            txtAdres.Text = "";
            txtMarka.Text = "";
            txtModel.Text = "";
            txtPlaka.Text = "";
            txtSoyad.Text = "";
            txtTel.Text = "";
            islemId = -1;
            girisTarih = DateTime.Now;
            lblUcret.Visible = false;
            lblUcretBilgi.Visible = false;
        }
        public void YuzdeDolu()
        {
            int bos=(kapasite - DbDoluluk())*5;
            int dolu = 100 - bos;
            lblYuzde.Text = "%" + dolu;
            if (DbDoluluk() >= 20)
            {
                btnAracGiris.Enabled = false;
            }
            else
            {
                btnAracGiris.Enabled = true;
            }
        }
        public void listele()
        {
            //Sistemde giriş yapmış ama çıkmamış araçları listeleme
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT ID,MAd as Ad,MSoyad as Soyad,MTelNo as 'Tel No',MAdres as Adres,AMarka as Marka,AModel as Model,APlaka as Plaka,GirisTarih as Giriş FROM TblOtoparkKayit WHERE CikisTarih IS NULL", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        private void HomePage_Load(object sender, EventArgs e)
        {
           listele(); 
           YuzdeDolu();
            lblUcret.Visible = false;
            lblUcretBilgi.Visible = false;
        }

        //Araç Girişi
        private void button1_Click(object sender, EventArgs e)
        {
            if (!txtAd.Text.Trim().Equals(""))
            {
                if (!txtSoyad.Text.Trim().Equals(""))
                {
                    string tel=txtTel.Text.Trim();
                    tel=tel.Replace("(", "");
                    tel=tel.Replace(")", "");
                    tel=tel.Replace("-", "");
                    tel=tel.Replace("_", "");

                    if (!tel.Trim().Equals(""))
                    {
                        if (!txtAdres.Text.Trim().Equals(""))
                        {
                            if (!txtMarka.Text.Trim().Equals(""))
                            {
                                if (!txtModel.Text.Trim().Equals(""))
                                {
                                    if (!txtPlaka.Text.Trim().Equals(""))
                                    {
                                        SqlCommand komut = new SqlCommand("INSERT INTO TblOtoparkKayit (MAd,MSoyad,MTelNo,MAdres,AMarka,AModel,APlaka,GirisTarih) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)", bgl.baglanti());
                                        komut.Parameters.AddWithValue("@p1",txtAd.Text.Trim()); 
                                        komut.Parameters.AddWithValue("@p2",txtSoyad.Text.Trim()); 
                                        komut.Parameters.AddWithValue("@p3",txtTel.Text.Trim()); 
                                        komut.Parameters.AddWithValue("@p4",txtAdres.Text.Trim()); 
                                        komut.Parameters.AddWithValue("@p5",txtMarka.Text.Trim()); 
                                        komut.Parameters.AddWithValue("@p6",txtModel.Text.Trim()); 
                                        komut.Parameters.AddWithValue("@p7",txtPlaka.Text.Trim()); 
                                        komut.Parameters.AddWithValue("@p8",DateTime.Now); 
                                        komut.ExecuteNonQuery();
                                        bgl.baglanti().Close();
                                        
                                        MessageBox.Show("Araç kaydı gerçekleşmiştir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        listele();
                                        YuzdeDolu();
                                        Temizle();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Araç plakasını boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Araç modelini boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Araç markasını boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Müşteri adresini boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Müşteri telefon numarasını boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Müşteri soyadını boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Müşteri adını boş bırakmayınız.","Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime cikisTarih=DateTime.Now;
            TimeSpan kalmaSuresi =cikisTarih - girisTarih;
            int ucret = ((int)kalmaSuresi.TotalHours) * 50;
            ucret += 25;
            
            SqlCommand komut = new SqlCommand("UPDATE TblOtoparkKayit SET CikisTarih=@p1,OdenenUcret=@p2  WHERE ID=@p3", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", cikisTarih);
            komut.Parameters.AddWithValue("@p2", ucret);
            komut.Parameters.AddWithValue("@p3", islemId);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Araç başarıyla çıkış yapmıştır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            listele();
            YuzdeDolu();
            Temizle();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            islemId =int.Parse(dataGridView1.Rows[secilen].Cells[0].Value.ToString());
            txtAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            txtSoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            txtTel.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            txtAdres.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            txtMarka.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            txtModel.Text = dataGridView1.Rows[secilen].Cells[6].Value.ToString();
            txtPlaka.Text = dataGridView1.Rows[secilen].Cells[7].Value.ToString();
            girisTarih =DateTime.Parse(dataGridView1.Rows[secilen].Cells[8].Value.ToString());
            DateTime cikisTarih = DateTime.Now;
            TimeSpan kalmaSuresi = cikisTarih - girisTarih;
            int ucret = ((int)kalmaSuresi.TotalHours) * 50;
            ucret += 25;
            lblUcret.Visible = true;
            lblUcretBilgi.Visible = true;   
            lblUcret.Text = ucret.ToString()+" TL";

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (islemId != -1)
            {
                if (!txtAd.Text.Trim().Equals(""))
                {
                    if (!txtSoyad.Text.Trim().Equals(""))
                    {
                        string tel = txtTel.Text.Trim();
                        tel = tel.Replace("(", "");
                        tel = tel.Replace(")", "");
                        tel = tel.Replace("-", "");
                        tel = tel.Replace("_", "");

                        if (!tel.Trim().Equals(""))
                        {
                            if (!txtAdres.Text.Trim().Equals(""))
                            {
                                if (!txtMarka.Text.Trim().Equals(""))
                                {
                                    if (!txtModel.Text.Trim().Equals(""))
                                    {
                                        if (!txtPlaka.Text.Trim().Equals(""))
                                        {
                                            SqlCommand komut = new SqlCommand("UPDATE TblOtoparkKayit SET MAd=@p1,MSoyad=@p2,MTelNo=@p3,MAdres=@p4,AMarka=@p5,AModel=@p6,APlaka=@p7 WHERE ID=@p8", bgl.baglanti());
                                            komut.Parameters.AddWithValue("@p1", txtAd.Text.Trim());
                                            komut.Parameters.AddWithValue("@p2", txtSoyad.Text.Trim());
                                            komut.Parameters.AddWithValue("@p3", txtTel.Text.Trim());
                                            komut.Parameters.AddWithValue("@p4", txtAdres.Text.Trim());
                                            komut.Parameters.AddWithValue("@p5", txtMarka.Text.Trim());
                                            komut.Parameters.AddWithValue("@p6", txtModel.Text.Trim());
                                            komut.Parameters.AddWithValue("@p7", txtPlaka.Text.Trim());
                                            komut.Parameters.AddWithValue("@p8", islemId);

                                            komut.ExecuteNonQuery();
                                            bgl.baglanti().Close();
                                            MessageBox.Show("Araç bilgileri başarıyla güncellenmiştir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            listele();
                                            Temizle();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Araç plakasını boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Araç modelini boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Araç markasını boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Müşteri adresini boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Müşteri telefon numarasını boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Müşteri soyadını boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Müşteri adını boş bırakmayınız.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Güncelleme işlemi için bir araç seçiniz.", "Uyarı!!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GecmisIslemler frm=new GecmisIslemler();
            frm.Show();
            this.Hide();
        }
    }
}
