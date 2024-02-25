using Challenge.BusinessLogicLayer;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.RichTextBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;



namespace Challenge.UI
{
    public partial class RichTextUI : Form
    {

        BLL bll;
        public string DestinationPath { get; set; }

        public RichTextUI()
        {
            InitializeComponent();
            bll = new BLL();
        }

        private void RichTextUI_Load(object sender, EventArgs e)
        {

        }

        public void LoadDocument()
        {
            if (!string.IsNullOrEmpty(DestinationPath))
            {
                // Microsoft.Office.Interop.Word kütüphanesini kullanarak Word belgesini okuyun
                Microsoft.Office.Interop.Word.Application wordApp =
                    new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document wordDoc =
                    wordApp.Documents.Open(DestinationPath);

                // Belgedeki metni RichTextBox kontrolüne yükleyin
                richEditControl1.Text = wordDoc.Content.Text;

                // Word belgesini kapat
                wordDoc.Close();
                wordApp.Quit();
            }
            else
            {
                MessageBox.Show("Dosya açılırken bir sorun yaşandı");
            }
        }

        public void saveDocument()
        {

            if (!string.IsNullOrEmpty(DestinationPath))
            {
                // Öncelikle, RichEditControl içeriğini bir değişkende saklayın
                string richTextContent = richEditControl1.Text;

                // Word belgesini oluşturun veya mevcut bir belgeyi açın
                Word.Application wordApp = new Word.Application();
                Word.Document doc = wordApp.Documents.Add();

                // Word belgesinin içeriğini temizleyin veya yeni içerikle değiştirin
                doc.Content.Delete();

                // RichEditControl'den aldığınız içeriği Word belgesine ekleyin
                doc.Content.Text = richTextContent;

                // Word belgesini kaydedin veya işlemlerinizi tamamlayın
                doc.SaveAs2(DestinationPath);
                doc.Close();
                wordApp.Quit();
                MessageBox.Show("Dosya kaydedildi");
            }
            else
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "Word Belgesi|*.docx";
                saveFileDialog1.Title = "Kaydedilecek Yeri Seç";
                saveFileDialog1.FileName = "yeni_belge.docx";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string richTextContent = richEditControl1.Text;

                    Word.Application wordApp = new Word.Application();
                    Word.Document doc = wordApp.Documents.Add();

                    doc.Content.Delete();

                    doc.Content.Text = richTextContent;

                    string filePath = saveFileDialog1.FileName;
                    string copiedFileName = Path.GetFileName(saveFileDialog1.FileName);
                    string copiedFilePath = Path.GetFullPath(saveFileDialog1.FileName);

                    DestinationPath = filePath;
                    bll.AddNewDocument(copiedFileName, copiedFilePath);

                    doc.SaveAs2(filePath);
                    doc.Close();
                    wordApp.Quit();
                }
            }
            
        }


        public void clearContent()
        {
            richEditControl1.Text = string.Empty;
        }


    }
}
