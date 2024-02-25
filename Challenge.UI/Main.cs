using Challenge.BusinessLogicLayer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Challenge.UI
{
    public partial class Main : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        BLL bll;
        RichTextUI richTextUI;
        LicenceUI licenceUI;

        public Main()
        {
            InitializeComponent();
            bll = new BLL();
            richTextUI = new RichTextUI();
            licenceUI = new LicenceUI();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (!bll.CheckLicenceKey())
            {
                timer1.Stop();
                MessageBox.Show(
                    "Lisansınız geçersiz!",
                    "Lisans Geçersiz",
                    MessageBoxButtons.OK);
                Application.Exit();
            }
            
            richTextUI.MdiParent = this;
            richTextUI.Show();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Word Dosyaları|*.doc;*.docx|Tüm Dosyalar|*.*";
            openFileDialog1.Title = "Word Belgesi Seç";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;

                // Dosyayı kopyalamak için hedef dizini oluştur
                string destinationDirectory = Path.Combine(Application.StartupPath, "files");
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                // Dosyayı programın çalıştığı dizine kopyala
                string fileName = Path.GetFileName(selectedFileName);
                string destinationPath = Path.Combine(destinationDirectory, fileName);
                File.Copy(selectedFileName, destinationPath, true); // true, hedef dosya zaten varsa üzerine yazılmasını sağlar

                // Kopyalanan dosyanın yolunu ve adını al
                string copiedFileName = Path.GetFileName(destinationPath);
                string copiedFilePath = Path.GetFullPath(destinationPath);

                bll.AddNewDocument(copiedFileName, copiedFilePath);
                
                richTextUI.DestinationPath = destinationPath;
                richTextUI.LoadDocument();
            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!bll.CheckLicenceKey())
            {
                timer1.Stop();
                MessageBox.Show(
                    "Lisansınız geçersiz program kapatılacak!", 
                    "Lisans Geçersiz", 
                    MessageBoxButtons.OK);
                Application.Exit();
            }
        }

        private void licenceManagementBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            licenceUI.MdiParent = this;
            licenceUI.Show();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            richTextUI.saveDocument();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextUI.DestinationPath))
            {
                if (bll.DeleteDocumentByPath(richTextUI.DestinationPath))
                {
                    File.Delete(richTextUI.DestinationPath);

                    richTextUI.clearContent();

                    MessageBox.Show("Dosya silme başarılı");
                }
                else
                {
                    MessageBox.Show("Belirtilen dosya bulunamadı.");
                }
            }
            else
            {
                MessageBox.Show("Silinecek kayıtlı bir dosya bulunamadı");
            }
        }
    }
}
