using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;
using System.Text.RegularExpressions; //Regex kütüphanesi-Güvenli parola oluşturulmasını sağlar
using System.IO; //Giriş çıkış işlemlerine erişkin kütüphane - klasör işlemleri oluşturmak için

namespace PersonelTakip
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }


        //veritabanı bağlantı dosya yolu ve provider nesnesinin belirlenmesi
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OleDb.12.0; Data Source = personel.accdb");
        int parola_skoru = 0;

        //Method tanımlıyoruz.
        private void kullanicilari_goster()
        { 
           //veri rabanındaki kullanıcıları guncel olarak görmemi sağlıcak
            try
            {
                baglantim.Open();
                OleDbDataAdapter kullanicilari_listele = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO],ad AS[ADI],soyad AS[SOYADI],yetki AS[YETKİ],kullaniciadi AS[KULLANICI ADI],parola AS[PAROLA] from kullanicilar Order By ad ASC",baglantim);
                DataSet ds = new DataSet();
                kullanicilari_listele.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                baglantim.Close();        
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        }
        private void personelleri_goster()
        {
            //veri rabanındaki kullanıcıları guncel olarak görmemi sağlıcak
            try
            {
                baglantim.Open();
                OleDbDataAdapter personelleri_listele = new OleDbDataAdapter("select tcno AS[TC KİMLİK NO],ad AS[ADI],soyad AS[SOYADI],cinsiyet AS[CİNSİYET],mezuniyet AS[MEZUNİYETİ],dogumtarihi AS[DOĞUM TARİHİ], gorevi AS[GÖREVİ], gorevyeri AS[GÖREV YERİ], maasi AS[MAAŞI] from personeller Order By ad ASC", baglantim);
                DataSet ds = new DataSet();
                personelleri_listele.Fill(ds);
                dataGridView2.DataSource = ds.Tables[0];
                baglantim.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            //Form2 Ayarları
            pictureBox1.Height = 150;
            pictureBox1.Width = 150;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            try
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath+"\\kullaniciresimler\\"+Form1.tcno + ".png");
            }
            catch 
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\kullaniciresimler\\resimyok.png");
            }
            //Kullanıcı İşlemleri Sekme işlermleri
            this.Text = "YÖNETİCİ İŞLEMLERİ";
            label20.ForeColor = Color.DarkRed;
            label20.Text = Form1.adi + " " + Form1.soyadi;
            txt_tc.MaxLength = 11;
            txt_kullanici.MaxLength = 8;
            toolTip1.SetToolTip(this.txt_tc,"TC Kimlik No 11 Karakterli Olmalı"); //mause üzerine gelince uyarı vericek
            radioButton1.Checked = true;
            txt_ad.CharacterCasing = CharacterCasing.Upper; //Büyük harfe çevirir
            txt_soyad.CharacterCasing = CharacterCasing.Upper;
            txt_parola.MaxLength = 10;
            txt_parolat.MaxLength = 10;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            //Methodları çağırıyoruz
            kullanicilari_goster();
            //Personel İşlemleri 
            pictureBox2.Height = 100;
            pictureBox2.Width = 100;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            maskedTextBox1.Mask = "00000000000";
            maskedTextBox2.Mask = "LL????????????????????";
            maskedTextBox3.Mask = "LL????????????????????";
            maskedTextBox4.Mask = "0000";
            maskedTextBox2.Text.ToUpper();//büyük harf
            maskedTextBox3.Text.ToUpper();

            comboBox1.Items.Add("İlköğretim");
            comboBox1.Items.Add("Ortaöğretim");
            comboBox1.Items.Add("Lise");
            comboBox1.Items.Add("Üniversite");

            comboBox3.Items.Add("Yönetici");
            comboBox3.Items.Add("Memur");
            comboBox3.Items.Add("Şoför");
            comboBox3.Items.Add("İşciler");

            comboBox4.Items.Add("Arge");
            comboBox4.Items.Add("Bilgi İşlem");
            comboBox4.Items.Add("Muhasebe");
            comboBox4.Items.Add("Üretim");
            comboBox4.Items.Add("Paketleme");
            comboBox4.Items.Add("Nakliye");
            //Date time kullanımı
            DateTime zaman = DateTime.Now;
            int yil = int.Parse(zaman.ToString("yyyy"));
            int ay = int.Parse(zaman.ToString("MM"));
            int gun = int.Parse(zaman.ToString("dd"));
            dateTimePicker1.MinDate = new DateTime(1960,1,1);
            dateTimePicker1.MaxDate = new DateTime(yil-18,ay,gun);
            dateTimePicker1.Format =  DateTimePickerFormat.Short;

            //Cinsiyet 
            radioButton3.Checked = true;

            personelleri_goster();
        }
        

        //kullanıcı bilgileri
        private void txt_tc_TextChanged(object sender, EventArgs e)
        {
            // Tc kimlik  numarası sayılardan oluşur. Bu yüzden harf yazılmaz.
            if (txt_tc.Text.Length < 11)

                errorProvider1.SetError(txt_tc, "TC Kimlik No 11 karakter olmalı..");
            else
                errorProvider1.Clear();
        }
        private void txt_tc_KeyPress(object sender, KeyPressEventArgs e)
        {
            //KLAVYE KISITLAMLARI
            //Textboxa yalnızca rakam yazabiliriz. Harf yazılmaması için ;
            //Kullanicinin harfe basılırken harf yazılmıcak. sadece raham sayı yazılıcak
            if (((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57) || (int)e.KeyChar == 8)
                e.Handled = false; //Tuşa basılmasına engelledik
            else
                e.Handled = true; //tuşa basılmasını aktifleştirdik
        }
        private void txt_ad_KeyPress(object sender, KeyPressEventArgs e)
        {
            //KLAVYE KISITLAMLARI
            //Yalnızca harf,boşluk girilecek. Onun dışındakiler kapanacak.
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void txt_soyad_KeyPress(object sender, KeyPressEventArgs e)
        {
            //KLAVYE KISITLAMLARI
            //Yalnızca harf,boşluk girilecek. Onun dışındakiler kapanacak.
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void txt_kullanici_TextChanged(object sender, EventArgs e)
        {
            if (txt_kullanici.Text.Length != 8)
                errorProvider1.SetError(txt_kullanici, "Kullanıcı Adı 8 karakter olamlı.");
            else
                errorProvider1.Clear();
        }
        private void txt_kullanici_KeyPress(object sender, KeyPressEventArgs e)
        {
            //KLAVYE KISITLAMLARI
            //Yalnızca harf,boşluk girilecek. Onun dışındakiler kapanacak.
            if (char.IsLetter(e.KeyChar) == true || char.IsControl(e.KeyChar) == true || char.IsSeparator(e.KeyChar) == true)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void txt_parola_TextChanged(object sender, EventArgs e)
        {
            string parola_seviyesi = "";
            int kucuk_harf_skoru = 0, buyuk_harf_skoru = 0, rakam_skoru=0, sembol_skoru = 0;
            string sifre = txt_parola.Text;
            //Ragex kütüphanesi ingilizce karakterleri baz aldığından
            //türkçe karakterlerle sorun yasamaması için
            //sifre string ifadesindeki türkçe-ingilizce karakterleri dönştürmemiz gerekiyor
            string duzeltilmis_sifre = "";
            duzeltilmis_sifre = sifre;
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('İ','I'); //yer değiştir
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ı', 'i');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ç', 'C'); 
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ç', 'c'); 
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ş', 'S'); 
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ş', 's');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ğ', 'G'); 
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ğ', 'g'); 
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ü', 'U');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ü', 'u');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('Ö', 'O');
            duzeltilmis_sifre = duzeltilmis_sifre.Replace('ö', 'o');
            if (sifre != duzeltilmis_sifre)
            {
                sifre = duzeltilmis_sifre;
                txt_parola.Text = sifre;
                MessageBox.Show("Paroldaki Türkçe karakterler İngilizce karakterlere dönüştürülmüştür.");
            }
            //Parola Puanlama -- 1 küçük harf 10 puan 2 ve üzeri 20 puan
            int az_karakter_sayisi = sifre.Length - Regex.Replace(sifre,"[a-z]","").Length; //küçük harf 
            kucuk_harf_skoru = Math.Min(2,az_karakter_sayisi)*10;
            //Parola Puanlama -- 1 büyük harf 10 puan 2 ve üzeri 20 puan
            int AZ_karakter_sayisi = sifre.Length - Regex.Replace(sifre, "[A-Z]", "").Length; //büyük harf 
            buyuk_harf_skoru = Math.Min(2, AZ_karakter_sayisi) * 10;
            //Parola Puanlama -- 1 rakam harf 10 puan 2 ve üzeri 20 puan
            int rakam_sayisi = sifre.Length - Regex.Replace(sifre, "[0-9]", "").Length; //Rakam
            rakam_skoru = Math.Min(2, rakam_sayisi) * 10;
           //Parola Puanlama -- 1 sembol harf 10 puan 2 ve üzeri 20 puan
            int sembol_sayisi = sifre.Length - az_karakter_sayisi - AZ_karakter_sayisi - rakam_sayisi;
            sembol_skoru = Math.Min(2, sembol_sayisi) * 10;

            parola_skoru = kucuk_harf_skoru + buyuk_harf_skoru + rakam_skoru + sembol_skoru;

            if (sifre.Length == 9)
                parola_skoru += 10;
            else if (sifre.Length == 10)
                parola_skoru += 20;

            if (kucuk_harf_skoru == 0 || buyuk_harf_skoru == 0 || rakam_skoru == 0 || sembol_skoru == 0)
                label30.Text = "Büyük-Küçük harf, Rakam ve Sembol Kullanmalısınız.";
            if (kucuk_harf_skoru != 0 && buyuk_harf_skoru != 0 && rakam_skoru != 0 && sembol_skoru != 0)
                label30.Text = "";

            //Parola seviye belirleme
            if (parola_skoru < 70)
                parola_seviyesi = "Zayıf";
            else if (parola_skoru == 70 || parola_skoru ==80)
                parola_seviyesi = "Güçlü";
            else if (parola_skoru == 90 || parola_skoru == 100)
                parola_seviyesi = "Çok Güçlü";
            label17.Text = "%" + Convert.ToString(parola_skoru);
            label18.Text = parola_seviyesi;
            progressBar1.Value = parola_skoru;
        }   
        private void txt_parolat_TextChanged(object sender, EventArgs e)
        {
            if (txt_parolat.Text != txt_parola.Text)
                errorProvider1.SetError(txt_parolat, "Parola Eşleşmiyor");
            else
                errorProvider1.Clear();
        }
       
       
        //methodlar
        private void topPage1_temizle()
        {
            txt_tc.Clear();
            txt_ad.Clear();
            txt_soyad.Clear();
            txt_kullanici.Clear();
            txt_parola.Clear();
            txt_parolat.Clear();
        }
        private void topPage2_temizle()
        {
            pictureBox2.Image = null;
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            maskedTextBox3.Clear();
            maskedTextBox4.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;        
        }
       
        //birinci sekme butonları
        private void btn_kaydet_Click(object sender, EventArgs e)
        {
            string yetki = "";
            bool kayitkontrol = false; //böyle bir kullanıcı kaydı var mı diye kontrol edicez.

            baglantim.Open();
            OleDbCommand selectsorgu = new OleDbCommand("select * from kullanicilar where tcno = '"+ txt_tc.Text+"'",baglantim);
            OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
            while (kayitokuma.Read())
            {
                kayitkontrol = true;
                break;
            }
            baglantim.Close();

            if (kayitkontrol == false)
            {
                //TC Kimlik No veri kontorlü yapıyoruz
                if (txt_tc.Text.Length < 11 || txt_tc.Text == " " )
                    label9.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label9.ForeColor = Color.Black; //tc k.no yazısı siyah olucak
                //Adı veri Kontorlü-- 
                if (txt_ad.Text.Length < 2 || txt_ad.Text == " ")
                    label10.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label10.ForeColor = Color.Black; //tc k.no yazısı siyah olucak
                //Soyadı veri Kontorlü-- 
                if (txt_soyad.Text.Length < 2 || txt_soyad.Text == " ")
                    label11.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label11.ForeColor = Color.Black; //tc k.no yazısı siyah olucak
                //Kullanıcı adı veri Kontorlü-- 
                if (txt_kullanici.Text.Length < 8 || txt_kullanici.Text == " ")
                    label13.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label13.ForeColor = Color.Black; //tc k.no yazısı siyah olucak
                //Parola veri Kontorlü-- 
                if (  txt_parola.Text == " "|| parola_skoru<70)
                    label14.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label14.ForeColor = Color.Black; //tc k.no yazısı siyah olucak
                //Parola tekrar veri Kontorlü-- 
                if (txt_parolat.Text == " " || txt_parola.Text != txt_parolat.Text)
                    label15.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label15.ForeColor = Color.Black; //tc k.no yazısı siyah olucak
           
                //kayıt işlemlerine başlıyoruz. Hiç bir sorun yoksa
                if (txt_tc.Text.Length == 11 && txt_tc.Text != "" && 
                    txt_ad.Text !="" && txt_ad.Text.Length>1 &&
                    txt_soyad.Text != "" && txt_soyad.Text.Length>1 &&
                    txt_kullanici.Text != "" && txt_parola.Text !="" && txt_parolat.Text != "" &&
                    txt_parola.Text == txt_parolat.Text && parola_skoru >=70)
                {
                    if (radioButton1.Checked == true)
                        yetki = "Yönetici";
                    else if (radioButton2.Checked == true)
                        yetki = "Kullanıcı";

                    //kayıt işlemi
                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekomutu = new OleDbCommand("insert into kullanicilar values ('" + txt_tc.Text + "', '" + txt_ad.Text + "','" + txt_soyad.Text + "','" + yetki + "','" + txt_kullanici.Text + "','" + txt_parola.Text + "')", baglantim);
                        eklekomutu.ExecuteNonQuery(); //Access tablosuna işle komutu
                        baglantim.Close();
                        MessageBox.Show("Yeni kullanıcı kaydı oluşturuldu!","Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        topPage1_temizle();                
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        baglantim.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Yazı rengi kırmızı olan alanları yeniden gözden geçiriniz..!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Bu TC Kimlik No zaten kayıtlıdır..!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        
        private void btn_ara_Click(object sender, EventArgs e)
        {
            bool kayit_arama_durumu = false;
            if (txt_tc.Text.Length == 11)
            {
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from kullanicilar where tcno = '" + txt_tc.Text + "'", baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    txt_ad.Text = kayitokuma.GetValue(1).ToString();
                    txt_soyad.Text = kayitokuma.GetValue(2).ToString();
                    if (kayitokuma.GetValue(3).ToString() == "Yönetici")
                        radioButton1.Checked = true;
                    else
                        radioButton2.Checked = true;
                    txt_kullanici.Text = kayitokuma.GetValue(4).ToString();
                    txt_parola.Text = kayitokuma.GetValue(5).ToString();
                    txt_parolat.Text = kayitokuma.GetValue(5).ToString();
                    break;
                }
                if (kayit_arama_durumu == false)
                    MessageBox.Show("Aranan kayıt bulunamadı !. ", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                baglantim.Close();
            }
            else
            {
                MessageBox.Show("Lütfen 11 haneli bir TC Kimlik No Giriniz. !. ", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                topPage1_temizle(); 
            }
        }
       
        private void btn_guncelle_Click(object sender, EventArgs e)
        {
            //Güncelle ve Ekle(Kaydet) birbirine çok benzer
            string yetki = "";

                //TC Kimlik No veri kontorlü yapıyoruz
                if (txt_tc.Text.Length < 11 || txt_tc.Text == " ")
                    label9.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label9.ForeColor = Color.Black; //tc k.no yazısı siyah olucak
                //Adı veri Kontorlü-- 
                if (txt_ad.Text.Length < 2 || txt_ad.Text == " ")
                    label10.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label10.ForeColor = Color.Black; //tc k.no yazısı siyah olucak
                //Soyadı veri Kontorlü-- 
                if (txt_soyad.Text.Length < 2 || txt_soyad.Text == " ")
                    label11.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label11.ForeColor = Color.Black; //tc k.no yazısı siyah olucak
                //Kullanıcı adı veri Kontorlü-- 
                if (txt_kullanici.Text.Length < 8 || txt_kullanici.Text == " ")
                    label13.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label13.ForeColor = Color.Black; //tc k.no yazısı siyah olucak
                //Parola veri Kontorlü-- 
                if (txt_parola.Text == " " || parola_skoru < 70)
                    label14.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label14.ForeColor = Color.Black; //tc k.no yazısı siyah olucak
                //Parola tekrar veri Kontorlü-- 
                if (txt_parolat.Text == " " || txt_parola.Text != txt_parolat.Text)
                    label15.ForeColor = Color.Red; //tc k.no yazısı kırmızı olucak
                else
                    label15.ForeColor = Color.Black; //tc k.no yazısı siyah olucak


                //kayıt işlemlerine başlıyoruz. Hiç bir sorun yoksa
                if (txt_tc.Text.Length == 11 && txt_tc.Text != "" &&
                    txt_ad.Text != "" && txt_ad.Text.Length > 1 &&
                    txt_soyad.Text != "" && txt_soyad.Text.Length > 1 &&
                    txt_kullanici.Text != "" && txt_parola.Text != "" && txt_parolat.Text != "" &&
                    txt_parola.Text == txt_parolat.Text && parola_skoru >= 70)
                {
                    if (radioButton1.Checked == true)
                        yetki = "Yönetici";
                    else if (radioButton2.Checked == true)
                        yetki = "Kullanıcı";

                    //güncelle işlemi
                    try
                    {
                        baglantim.Open();
                        OleDbCommand guncellekomutu = new OleDbCommand("update kullanicilar set  ad = '"+txt_ad.Text+"', soyad = '"+txt_soyad.Text+"' , yetki = '"+yetki+"', kullaniciadi = '"+txt_kullanici.Text+"',  parola = '"+txt_parola.Text+"' where tcno = '"+txt_tc.Text+"'",baglantim);
                        guncellekomutu.ExecuteNonQuery(); //Access tablosuna işle komutu
                        baglantim.Close();
                        MessageBox.Show("Kullanıcı bilgileri güncellendi..!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        kullanicilari_goster();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Yazı rengi kırmızı olan alanları yeniden gözden geçiriniz..!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }
        
        private void btn_sil_Click(object sender, EventArgs e)
        {
            //Önce kaydın seçilmesini sağlıyoruz..
            if (txt_tc.Text.Length == 11)
            {
                bool kayit_arama_durumu = false;
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from kullanicilar where tcno = '"+txt_tc.Text+"'",baglantim);
                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit_arama_durumu = true;//böyle bir kayıt mevcuttur 
                    //Silme işlemi
                    OleDbCommand silkomutu = new OleDbCommand("Delete from kullanicilar where tcno = '" + txt_tc.Text + "' ", baglantim);
                    silkomutu.ExecuteNonQuery();
                    MessageBox.Show("Kullanıcı kaydı silindi","Personel Takip Programı",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    baglantim.Close();
                    kullanicilari_goster();
                    topPage1_temizle();
                    break;
                }
                if (kayit_arama_durumu == false)
                    MessageBox.Show("Silinecek kayıt bulunamadı !", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
                topPage1_temizle();
            }
            else
            {
                MessageBox.Show("Lütfen 11 karakterli bir sayı giriniz..!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }         
        }
       
        private void btn_temizle_Click(object sender, EventArgs e)
        {
            topPage1_temizle();
        }

       

        //personel bilgileri
        private void btn_gozat_Click(object sender, EventArgs e)
        {
            //Butona tıklandığında kullanıcı resim seçecek.
            OpenFileDialog resimsec = new OpenFileDialog();
            resimsec.Title = "Personel Resmi Seçiniz";
            resimsec.Filter = "JPG Dosyalar (*.jpg) | *.jpg";
            if (resimsec.ShowDialog() == DialogResult.OK)
            {
                this.pictureBox2.Image = new Bitmap(resimsec.OpenFile());
            }
        }

        private void btn_kaydett_Click(object sender, EventArgs e)
        {
            string cinsiyet = "";
            bool kayitkontrol = false;

            baglantim.Open();
            OleDbCommand selectsorgu = new OleDbCommand("select * from personeller where tcno = '"+maskedTextBox1.Text+"'",baglantim);
            OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();
            while (kayitokuma.Read())
            {
                kayitkontrol = true;
                break;
            }
            baglantim.Close();

            if (kayitkontrol == false)
            {
                //Veriler girilmediği takdirde uyarı vericek
                if (pictureBox2.Image == null)
                    btn_gozat.ForeColor = Color.Red;
                else
                    btn_gozat.ForeColor = Color.Black;
                //
                if (maskedTextBox1.MaskCompleted == false)
                    label21.ForeColor = Color.Red;
                else
                    label21.ForeColor = Color.Black;
                //
                if (maskedTextBox2.MaskCompleted == false)
                    label22.ForeColor = Color.Red;
                else
                    label22.ForeColor = Color.Black;
                //
                if (maskedTextBox3.MaskCompleted == false)
                    label23.ForeColor = Color.Red;
                else
                    label23.ForeColor = Color.Black;
                //
                if (comboBox1.Text == "")
                    label25.ForeColor = Color.Red;
                else
                    label25.ForeColor = Color.Black;
                //
                if (comboBox3.Text == "")
                    label27.ForeColor = Color.Red;
                else
                    label27.ForeColor = Color.Black;
                //
                if (comboBox4.Text == "")
                    label28.ForeColor = Color.Red;
                else
                    label28.ForeColor = Color.Black;
                //
                if (maskedTextBox4.MaskCompleted == false)
                    label29.ForeColor = Color.Red;
                else
                    label29.ForeColor = Color.Black;
                //
                if(int.Parse(maskedTextBox4.Text)<1000)
                    label29.ForeColor = Color.Red;
                else
                    label29.ForeColor = Color.Black;


                if (pictureBox2.Image != null && maskedTextBox1.MaskCompleted != false && maskedTextBox2.MaskCompleted != false &&
                    maskedTextBox3.MaskCompleted != false && comboBox1.Text != "" && comboBox3.Text != ""&& comboBox4.Text != "" &&   
                    maskedTextBox4.MaskCompleted != false)
                {
                    if (radioButton3.Checked == true)
                        cinsiyet = "Kadın";
                    else if (radioButton4.Checked == true)
                        cinsiyet = "Erkek";

                    //kayıt işlemi
                    try
                    {
                        baglantim.Open();
                        OleDbCommand ekle2komutu = new OleDbCommand("insert into personeller values ('" + maskedTextBox1.Text + "', '" + maskedTextBox2.Text + "','" + maskedTextBox3.Text+ "','" + cinsiyet+ "','" + dateTimePicker1.Text+ "','" + comboBox1.Text + "','" + comboBox3.Text + "','" + comboBox4.Text + "','" + maskedTextBox4.Text + "')", baglantim);
                        ekle2komutu.ExecuteNonQuery(); //Access tablosuna işle komutu
                        baglantim.Close();
                        if (!Directory.Exists(Application.StartupPath +"\\personelresimler")) //bindeki debug içerisinde peronelresimler var mı ?
		                    Directory.CreateDirectory(Application.StartupPath+"\\personelresimler");
                        else
                            pictureBox2.Image.Save(Application.StartupPath+"\\personelresimler\\"+maskedTextBox1.Text+".jpg");

                        MessageBox.Show("Yeni personel kaydı oluşturuldu!","Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        personelleri_goster();
                        topPage2_temizle();                
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Yazı rengi kırmızı olan alanları yeniden gözden geçiriniz..!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Girilen TC Kimlik No daha önceden kayıtlıdır..!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }
        
        private void btn_araa_Click(object sender, EventArgs e)
        {
             bool kayit_arama_durumu2 = false;
            if (maskedTextBox1.Text.Length == 11)
            {
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from personeller where tcno = '" + maskedTextBox1.Text + "'", baglantim);
                OleDbDataReader kayit_okuma = selectsorgu.ExecuteReader();
                while (kayit_okuma.Read())
                {
                    kayit_arama_durumu2 = true;
                   try 
	               {	        
		               pictureBox2.Image = Image.FromFile(Application.StartupPath+"\\personelresimler\\"+kayit_okuma.GetValue(0).ToString()+".jpg");
	               }
	               catch 
	               {
		                 pictureBox2.Image = Image.FromFile(Application.StartupPath+"\\personelresimler\\resimyok.jpg");
	               }             
                 
                    maskedTextBox2.Text = kayit_okuma.GetValue(1).ToString();
                    maskedTextBox3.Text = kayit_okuma.GetValue(2).ToString();
                    if (kayit_okuma.GetValue(3).ToString() == "Kadın")
                        radioButton3.Checked = true;
                    else
                        radioButton4.Checked = true;
                    dateTimePicker1.Text = kayit_okuma.GetValue(4).ToString();
                    comboBox1.Text = kayit_okuma.GetValue(5).ToString();
                    comboBox3.Text = kayit_okuma.GetValue(6).ToString();
                    comboBox4.Text = kayit_okuma.GetValue(7).ToString();
                    maskedTextBox4.Text = kayit_okuma.GetValue(8).ToString();
                  
                    break;
                }
                if (kayit_arama_durumu2 == false)
                    MessageBox.Show("Aranan kayıt bulunamadı !. ", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                baglantim.Close();
            }
            else
            {
                MessageBox.Show("Lütfen 11 haneli bir TC Kimlik No Giriniz. !. ", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                topPage2_temizle(); 
            }
        }

        private void btn_guncel_Click(object sender, EventArgs e)
        {
               string cinsiyet = "";
           
                //Veriler girilmediği takdirde uyarı vericek
                if (pictureBox2.Image == null)
                    btn_gozat.ForeColor = Color.Red;
                else
                    btn_gozat.ForeColor = Color.Black;
                //
                if (maskedTextBox1.MaskCompleted == false)
                    label21.ForeColor = Color.Red;
                else
                    label21.ForeColor = Color.Black;
                //
                if (maskedTextBox2.MaskCompleted == false)
                    label22.ForeColor = Color.Red;
                else
                    label22.ForeColor = Color.Black;
                //
                if (maskedTextBox3.MaskCompleted == false)
                    label23.ForeColor = Color.Red;
                else
                    label23.ForeColor = Color.Black;
                //
                if (comboBox1.Text == "")
                    label25.ForeColor = Color.Red;
                else
                    label25.ForeColor = Color.Black;
                //
                if (comboBox3.Text == "")
                    label27.ForeColor = Color.Red;
                else
                    label27.ForeColor = Color.Black;
                //
                if (comboBox4.Text == "")
                    label28.ForeColor = Color.Red;
                else
                    label28.ForeColor = Color.Black;
                //
                if (maskedTextBox4.MaskCompleted == false)
                    label29.ForeColor = Color.Red;
                else
                    label29.ForeColor = Color.Black;
                //
                if(int.Parse(maskedTextBox4.Text)<1000)
                    label29.ForeColor = Color.Red;
                else
                    label29.ForeColor = Color.Black;


                if (pictureBox2.Image != null && maskedTextBox1.MaskCompleted != false && maskedTextBox2.MaskCompleted != false &&
                    maskedTextBox3.MaskCompleted != false && comboBox1.Text != "" && comboBox3.Text != ""&& comboBox4.Text != "" &&   
                    maskedTextBox4.MaskCompleted != false)
                {
                    if (radioButton3.Checked == true)
                        cinsiyet = "Kadın";
                    else if (radioButton4.Checked == true)
                        cinsiyet = "Erkek";

                    //guncelle işlemi
                    try
                    {
                        baglantim.Open();
                        OleDbCommand guncelle2komutu = new OleDbCommand("update personeller set  ad = '" + maskedTextBox2.Text + "' , soyad = '" + maskedTextBox3.Text+ "',  cinsiyet='" + cinsiyet+ "', dogumtarihi= '" + dateTimePicker1.Text+ "', mezuniyet='" + comboBox1.Text + "', gorevi='" + comboBox3.Text + "', gorevyeri='" + comboBox4.Text + "', maasi='" + maskedTextBox4.Text + "' where tcno = '"+maskedTextBox1.Text+"'", baglantim);
                        guncelle2komutu.ExecuteNonQuery(); //Access tablosuna işle komutu
                        baglantim.Close();                       
                        personelleri_goster();
                        topPage2_temizle();      
                        //maskedTextBox4.Text = "0";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Yazı rengi kırmızı olan alanları yeniden gözden geçiriniz..!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        private void btn_sill_Click(object sender, EventArgs e)
        {
            //Önce kaydın seçilmesini sağlıyoruz..
            if (maskedTextBox1.MaskCompleted == true)
            {
                bool kayit_arama_durumu2 = false;
                baglantim.Open();
                OleDbCommand selectsorgu = new OleDbCommand("select * from personeller where tcno = '" + maskedTextBox1.Text + "'", baglantim);
                OleDbDataReader kayit_okuma = selectsorgu.ExecuteReader();
                while (kayit_okuma.Read())
                {
                    kayit_arama_durumu2 = true;//böyle bir kayıt mevcuttur 
                    //Silme işlemi
                    OleDbCommand sil_komutu = new OleDbCommand("Delete from personeller where tcno = '" + maskedTextBox1.Text + "' ", baglantim);
                    sil_komutu.ExecuteNonQuery();       
                    break;
                }
                if (kayit_arama_durumu2 == false)
                    MessageBox.Show("Silinecek kayıt bulunamadı !", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
                personelleri_goster();
                topPage2_temizle();
            }
            else
            {
                MessageBox.Show("Lütfen 11 karakterli bir sayı giriniz..!", "Personel Takip Programı", MessageBoxButtons.OK, MessageBoxIcon.Error);
                topPage2_temizle();
            }         
        }

        private void btn_temizlee_Click(object sender, EventArgs e)
        {
            topPage2_temizle();
        }

         
        }
    }

