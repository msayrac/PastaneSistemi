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

namespace PastaneSistemi
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		SqlConnection connection = new SqlConnection(@"Data Source=msyc;Initial Catalog=TestMaliyet;Integrated Security=True");

		void Malzemeliste()
		{
			connection.Open();
			SqlDataAdapter da = new SqlDataAdapter("Select * From TBLMALZEMELER", connection);
			DataTable dt = new DataTable();
			da.Fill(dt);
			dataGridView1.DataSource = dt;
			connection.Close();
		}

		void UrunListe()
		{
			connection.Open();
			SqlDataAdapter da = new SqlDataAdapter("Select * From TBLURUNLER", connection);
			DataTable dt = new DataTable();
			da.Fill(dt);
			dataGridView1.DataSource = dt;
			connection.Close();
		}

		void Kasa()
		{
			connection.Open();
			SqlDataAdapter da = new SqlDataAdapter("Select * From TBLKASA", connection);
			DataTable dt = new DataTable();
			da.Fill(dt);
			dataGridView1.DataSource = dt;
			connection.Close();
		}

		void Urunler()
		{
			connection.Open();

			SqlDataAdapter da = new SqlDataAdapter("Select * From TBLURUNLER", connection);
			DataTable dt = new DataTable();
			da.Fill(dt);
			CmbUrunOlusturUrun.ValueMember = "URUNID";
			CmbUrunOlusturUrun.DisplayMember = "AD";
			CmbUrunOlusturUrun.DataSource = dt;
			connection.Close();
		}

		void Malzemeler()
		{
			connection.Open();
			SqlDataAdapter da = new SqlDataAdapter("Select * From TBLMALZEMELER", connection);
			DataTable dt = new DataTable();
			da.Fill(dt);
			CmbUrunOlusturMalzeme.ValueMember = "MALZEMEID";
			CmbUrunOlusturMalzeme.DisplayMember = "AD";
			CmbUrunOlusturMalzeme.DataSource = dt;
			connection.Close();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Malzemeliste();
			Urunler();
			Malzemeler();
		}

		private void BtnUrunListe_Click(object sender, EventArgs e)
		{
			UrunListe();
		}

		private void BtnMalzemeListe_Click(object sender, EventArgs e)
		{
			Malzemeliste();
		}

		private void BtnKasa_Click(object sender, EventArgs e)
		{
			Kasa();
		}

		private void BtnCikis_Click(object sender, EventArgs e)
		{

		}

		private void BtnMalzemeEkle_Click(object sender, EventArgs e)
		{
			connection.Open();

			SqlCommand command = new SqlCommand("insert into TBLMALZEMELER (AD, STOK, FIYAT, NOTLAR) values (@p1,@p2,@p3,@p4)", connection);

			command.Parameters.AddWithValue("@p1", TxtMalzemeAd.Text);
			command.Parameters.AddWithValue("@p2", TxtMalzemeStok.Text);
			command.Parameters.AddWithValue("@p3", TxtMalzemeFiyat.Text);
			command.Parameters.AddWithValue("@p4", TxtMalzemeNotlar.Text);
			command.ExecuteNonQuery();
			connection.Close();
			Malzemeliste();
		}

		private void BtnUrunEkle_Click(object sender, EventArgs e)
		{
			connection.Open();
			//SqlCommand command = new SqlCommand("insert into TBLURUNLER (AD,MFIYAT,SFIYAT,STOK) values (@p1,@p2,@p3,@p4)", connection);
			SqlCommand command = new SqlCommand("insert into TBLURUNLER (AD) values (@p1)", connection);

			command.Parameters.AddWithValue("@p1", TxtUrunAd.Text);
			//command.Parameters.AddWithValue("@p2",TxtUrunMFiyat.Text);
			//command.Parameters.AddWithValue("@p3",TxtUrunSFiyat.Text);
			//command.Parameters.AddWithValue("@p4",TxtUrunStok.Text);

			command.ExecuteNonQuery();

			connection.Close();
			UrunListe();
		}

		private void BtnUrunOlusturEkle_Click(object sender, EventArgs e)
		{
			connection.Open();
			SqlCommand command = new SqlCommand("insert into TBLFIRIN (URUNID,MALZEMEID,MIKTAR,MALIYET) values (@p1,@p2,@p3,@p4)", connection);


			command.Parameters.AddWithValue("@p1", CmbUrunOlusturUrun.SelectedValue.ToString());
			command.Parameters.AddWithValue("@p2", CmbUrunOlusturMalzeme.SelectedValue.ToString());
			command.Parameters.AddWithValue("@p3", TxtUrunOlusturMiktar.Text);
			command.Parameters.AddWithValue("@p4", TxtUrunOlusturMaliyet.Text);

			command.ExecuteNonQuery();

			connection.Close();


		}

		private void TxtUrunOlusturMiktar_TextChanged(object sender, EventArgs e)
		{
			double maliyet;

			if (TxtUrunOlusturMiktar.Text == "")
			{
				TxtUrunOlusturMiktar.Text = "0";
			}

			connection.Open();
			SqlCommand command = new SqlCommand("Select * from TBLMALZEMELER where MALZEMEID=@p1", connection);
			command.Parameters.AddWithValue("@p1", CmbUrunOlusturMalzeme.SelectedValue);

			SqlDataReader dr = command.ExecuteReader();
			while (dr.Read())
			{
				TxtUrunOlusturMaliyet.Text = dr[3].ToString();
			}
			connection.Close();

			maliyet = (Convert.ToDouble(TxtUrunOlusturMaliyet.Text) / 1000) * Convert.ToDouble(TxtUrunOlusturMiktar.Text);

			TxtUrunOlusturMaliyet.Text = maliyet.ToString();

			listBox1.Items.Add(CmbUrunOlusturMalzeme.Text + "-" + TxtUrunOlusturMaliyet.Text);
		}

		private void CmbUrunOlusturMalzeme_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int secilen = dataGridView1.SelectedCells[0].RowIndex;

			TxtUrunID.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();

			TxtUrunAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();

			connection.Open();

			SqlCommand command = new SqlCommand("Select sum(MALIYET) from TBLFIRIN Where URUNID=@p1", connection);

			command.Parameters.AddWithValue("@p1", TxtUrunID.Text);

			SqlDataReader dr = command.ExecuteReader();
			while (dr.Read())
			{
				TxtUrunMFiyat.Text = dr[0].ToString();
			}

			connection.Close();

		}
	}
}
