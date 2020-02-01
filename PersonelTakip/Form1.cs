using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb; //veritabanı kütüphane ekledik.

namespace PersonelTakip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //LOGIN EKRAN TASARIMI


        //veritabanı bağlantısı 
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OleDb.12.0; Data Source = personel.accdb");


        //Formlar arası veri aktarımında kullanılacak değişkenlerimizi tanımlıyoruz .
        public static string tcno, adi, soyadi, yetki;

        //yerel yani yalnızca bu formda geçerli olan değişkenleri tanımlıyoruz.
        int hak = 3;
        bool durum = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            //tasarım özellikleri
            this.Text = "KULLANICI GİRİŞİ"; //başlık
            this.AcceptButton = btn_giris; //Enter tuşuna basılınca giriş butonuna tıklansın
            this.CancelButton = btn_cikis; //ESC tuşuna basılınca çıkış butonu
            label6.Text = Convert.ToString(hak); //label6 ya yazdırma işlemi
            radioButton1.Checked = true; //rb1 seçiliyken
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow; //Formun simge durumunda küçült ve tam ekran yap kısmı pasif oldu
        }
        private void btn_giris_Click(object sender, EventArgs e)
        {
            if (hak != 0)
            {
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("Select * from kullanicilar",baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader(); //Veriokuyucu tanımladık. Tablodaki verileri getiricek
                while (kayitokuma.Read())
                {
                    if (radioButton1.Checked == true)//rb1 seçili ise kaç kayıt vrsa okadar çalışacak
                    {
                        if (kayitokuma["kullaniciadi"].ToString() == txt_kullanici.Text && kayitokuma["parola"].ToString() == txt_parola.Text && kayitokuma["yetki"].ToString() == "Yönetici")
                        {
                            durum = true; //başarılı giriş 
                            //Bilgiler form2ye alınacak
                            tcno = kayitokuma.GetValue(0).ToString(); //kaydın 0.alanını (tcno) alıyoruz. 
                            adi = kayitokuma.GetValue(1).ToString();
                            soyadi = kayitokuma.GetValue(2).ToString();
                            yetki = kayitokuma.GetValue(3).ToString();
                            this.Hide(); //Bu form gizlendii
                            //Form2 ye geçiş kodu
                            Form2 frm2 = new Form2();
                            frm2.Show();
                            break; 
                        }
                    }
                    if (radioButton2.Checked == true)//rb2 seçili ise kaç kayıt vrsa okadar çalışacak
                    {
                        if (kayitokuma["kullaniciadi"].ToString() == txt_kullanici.Text && kayitokuma["parola"].ToString() == txt_parola.Text && kayitokuma["yetki"].ToString() == "Kullanıcı")
                        {
                            durum = true; //başarılı giriş 
                            //Bilgiler form2ye alınacak
                            tcno = kayitokuma.GetValue(0).ToString(); //kaydın 0.alanını (tcno) alıyoruz. 
                            adi = kayitokuma.GetValue(1).ToString();
                            soyadi = kayitokuma.GetValue(2).ToString();
                            yetki = kayitokuma.GetValue(3).ToString();
                            this.Hide(); //Bu form gizlendii
                            //Form3 ye geçiş kodu
                            Form3 frm3 = new Form3();
                            frm3.Show();
                            break;
                        }
                    }
                }
                //Durum false ise hak azalır
                if (durum == false)   
                    hak--;               
                baglantim.Close();
               
            }
            label6.Text = Convert.ToString(hak);
            if (hak == 0)
            {
                btn_giris.Enabled = false;
                MessageBox.Show("Giriş Hakkı Kalmadı..","Personel Takip Programı",MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btn_cikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
