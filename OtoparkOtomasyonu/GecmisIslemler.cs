using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtoparkOtomasyonu
{
    public partial class GecmisIslemler : Form
    {
        public GecmisIslemler()
        {
            InitializeComponent();
        }
      
        
        sqlbaglantisi bgl = new sqlbaglantisi();
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
           
            lblAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            lblSoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            lblTel.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            lblAdres.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            lblMarka.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            lblModel.Text = dataGridView1.Rows[secilen].Cells[6].Value.ToString();
            lblPlaka.Text = dataGridView1.Rows[secilen].Cells[7].Value.ToString();
            DateTime girisTarih = DateTime.Parse(dataGridView1.Rows[secilen].Cells[8].Value.ToString());
            DateTime cikisTarih = DateTime.Now;
            TimeSpan kalmaSuresi = cikisTarih - girisTarih;
            int ucret = ((int)kalmaSuresi.TotalHours) * 50 ;
            ucret += 25;
            lblKalmaSuresi.Text=((int) kalmaSuresi.TotalHours).ToString()+" Saat";
            lblUcret.Text = ucret.ToString() + " TL";
        }
        public void Temizle()
        {
            lblAd.Text = "*****";
            lblAdres.Text = "*****";
            lblKalmaSuresi.Text = "0 Saat";
            lblMarka.Text = "*****";
            lblModel.Text = "*****";
            lblPlaka.Text = "*****";
            lblSoyad.Text = "*****";
            lblTel.Text = "*****";
            lblUcret.Text = "0 TL";
        }

        private void GecmisIslemler_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TblOtoparkKayit WHERE GirisTarih IS NOT NULL AND CikisTarih IS NOT NULL ORDER BY GirisTarih DESC, CikisTarih DESC", bgl.baglanti());
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            Temizle();
        }

        private void btnAracGiris_Click(object sender, EventArgs e)
        {
            HomePage frm= new HomePage();
            frm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Temizle();
            

        }
    }
}
