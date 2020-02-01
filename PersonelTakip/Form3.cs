﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace PersonelTakip
{
    public partial class Form3 : Form
    {
        //veritabanı bağlantı dosya yolu ve provider nesnesinin belirlenmesi
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OleDb.12.0; Data Source = personel.accdb");
        public Form3()
        {
            InitializeComponent();
        }
        private void personelleri_goster()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter parametreleri_listele = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO], ad AS[AD], soyad AS[SOYAD], cinsiyet AS[CİNSİYET], dogumtarihi AS[DOĞUM TARİHİ], mezuniyet AS[MEZUNİYETİ], gorevi AS[GÖREVİ], gorevyeri AS[GÖREV YERİ], maasi AS[MAAŞI] from personeller Order By ad ASC ",baglantim);
                DataSet ds = new DataSet(); //bellekte bir alan oluşturacak
                parametreleri_listele.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                baglantim.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            personelleri_goster();
            this.Text = "KULLANICI İŞLEMLERİ";
            label19.Text = Form1.adi+" "+Form1.soyadi;

            pictureBox1.Height = 150;
            pictureBox1.Width = 150;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;

            pictureBox2.Height = 150;
            pictureBox2.Width = 150;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;

            try
            {
                pictureBox2.Image = Image.FromFile(Application.StartupPath+"\\kullaniciresimler\\" +Form1.tcno+".jpg");

            }
            catch 
            {
                pictureBox2.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\resimyok.png");
            }

            maskedTextBox1.Mask = "00000000000";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;
            if (maskedTextBox1.Text.Length == 11)
            {
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from personeller where tcno = '" + maskedTextBox1.Text + "'", baglantim);
                OleDbDataReader kayit_okuma = selectsorgu.ExecuteReader();
                while (kayit_okuma.Read())
                {
                    kayit_arama_durumu = true;
                    try
                    {
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\" + kayit_okuma.GetValue(0).ToString() + ".jpg");
                    }
                    catch
                    {
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\personelresimler\\resimyok.jpg");
                    }

                    label6.Text = kayit_okuma.GetValue(1).ToString();
                    label7.Text = kayit_okuma.GetValue(2).ToString();
                    if (kayit_okuma.GetValue(3).ToString() == "Kadın")
                        label8.Text = "Kadın";
                    else
                        label8.Text = "Erkek";
                   
                    label9.Text = kayit_okuma.GetValue(4).ToString();
                    label14.Text = kayit_okuma.GetValue(5).ToString();
                    label15.Text = kayit_okuma.GetValue(6).ToString();
                    label16.Text = kayit_okuma.GetValue(7).ToString();
                    label17.Text = kayit_okuma.GetValue(8).ToString();
                    break;
                }
                if (kayit_arama_durumu == false)
                    MessageBox.Show("Aranan kayıt bulunamadı !. ", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                baglantim.Close();
            }
            else
            {
                MessageBox.Show("Lütfen 11 haneli bir TC Kimlik No Giriniz. !. ", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }
    }
}
