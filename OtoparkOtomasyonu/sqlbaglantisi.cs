using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtoparkOtomasyonu
{
    internal class sqlbaglantisi
    {
        public SqlConnection baglanti()
        {
            SqlConnection baglan = new SqlConnection("Data Source=DESKTOP-23LHLCO\\SQLEXPRESS;Initial Catalog=Otopark911;Integrated Security=True");
            baglan.Open();
            return baglan;
        }
    }
}
