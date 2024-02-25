using Challenge.BusinessLogicLayer;
using Challenge.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DevExpress.Data.Helpers.ExpressiveSortInfo;

namespace Challenge.UI
{
    public partial class LicenceUI : Form
    {

        BLL bll;

        public LicenceUI()
        {
            InitializeComponent();
            bll = new BLL();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDataGrid();
        }

        private void LoadDataGrid()
        {
            dataGridView1.DataSource = null;

            List<Entities.Licence> liste = bll.GetAllLicences();

            BindingSource bindingSource = new BindingSource();
            dataGridView1.DataSource = bindingSource;

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(string));
            dataTable.Columns.Add("Key", typeof(string));
            dataTable.Columns.Add("Create", typeof(DateTime));
            dataTable.Columns.Add("Expiry", typeof(DateTime));

            bindingSource.DataSource = dataTable;

            foreach (Entities.Licence licence in liste)
            {
                DataRow row = dataTable.NewRow();

                row["Id"] = licence.Id;
                row["Key"] = licence.Key;
                row["Create"] = licence.Create;
                row["Expiry"] = licence.Expiry;

                dataTable.Rows.Add(row);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 
                && dataGridView1.SelectedRows[0].Cells[0].Value != null)
            {
                int selectedRow = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                bll.DeleteLicence(selectedRow);
                LoadDataGrid();

            }
            else
            {
                MessageBox.Show("Hiçbir satır seçilmedi.");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            bool returnValues = bll.UpdateLicence(
                updateKeyId.Text, 
                updateKeyTxt.Text, 
                updateDateEdit.Text);
            if (returnValues)
            {
                updateKeyId.Text = "";
                updateKeyTxt.Text = "";
                updateDateEdit.Text = "";
                LoadDataGrid();
                MessageBox.Show("Güncelleme işlemi başarılı");
            }
            else
            {
                MessageBox.Show("Güncelleme işlemi hatalı");
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            bool returnValues = bll.AddNewLicence(newKeyTxt.Text, newDateEdit.Text);
            if (returnValues)
            {
                LoadDataGrid();
                MessageBox.Show("Yeni lisans ekleme başarılı");
            }
            else
            {
                MessageBox.Show("Yeni lisans eklenemedi");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0
                && dataGridView1.SelectedRows[0].Cells[0].Value != null)
            {
                string id = "", key = "", expiry = "";
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    id = row.Cells["Id"].Value.ToString();
                    key = row.Cells["Key"].Value.ToString();
                    expiry = row.Cells["Expiry"].Value.ToString();
                }
                updateKeyTxt.Text = key;
                updateDateEdit.Text = expiry;
                updateKeyId.Text = id;
            }
        }
    }
}
