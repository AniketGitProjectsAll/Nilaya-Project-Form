using Nilaya;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nilaya_Project_Form
{
    public partial class Form1 : Form
    {
        Operations op = new Operations();
        Printutilities printUtil = new Printutilities();

        public Form1()
        {
            InitializeComponent();
            LoadNilayaGrid(); // It loads the grid every time the form opens or whenever this app is run

            //Safe preview (no DB mutation)
            
           
        }

                                                                            

        //private void ShowNextSerial(int serial)
        //{
        //    txtSerialNo.Text = serial.ToString();
        //}

        //private void ShowNextSerial(int startSerial)
        //{
        //    try
        //    {
        //        int nextSerial = op.GetNextSerialNumber();
        //        txtSerialNo.Text = nextSerial.ToString();
        //    }
        //    catch
        //    {
        //        txtSerialNo.Text = "";
        //    }
        //}

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            txtProductDesc.Text = row.Cells["ProductDesc"].Value?.ToString();
            txtDimensions.Text = row.Cells["Dimensions"].Value?.ToString();
            txtSKUCode.Text = row.Cells["SKUCode"].Value?.ToString();
            txtSKUDesc.Text = row.Cells["SKUDesc"].Value?.ToString();
            txtMRPRs.Text = row.Cells["MRPRs"].Value?.ToString();
            txtMonthandYearofImport.Text = row.Cells["MonthAndYearOfImport"].Value?.ToString();
            txtBatchCode.Text = row.Cells["BatchCode"].Value?.ToString();
            txtCountryofOrigin.Text = row.Cells["CountryOfOrigin"].Value?.ToString();
            txtPlantCode.Text = row.Cells["PlantCode"].Value?.ToString();
            txtQuantity.Text = row.Cells["Quantity"].Value?.ToString();
            txtImportedBy.Text = row.Cells["ImportedBy"].Value?.ToString();
        }

        private void btnDriverBasedPrinting_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                int copies = int.Parse(txtNoofCopies.Text);

                //Get next serial number automatically from database
                int dateCode = op.GetCurrentDateCode();
                int nextSerial = op.GetNextSerialNumberDateWise(dateCode);
                //int yearMonthCode = op.GetCurrentYearMonthCode();
                //int nextSerial = op.GetNextSerialNumber(yearMonthCode);



                //Preview only once
                //ShowNextSerial(nextSerial);

                //Printing multiple copies 

                for (int i = 1; i <= copies; i++)
                {
                    int currentSerial = nextSerial++;

                    // 🔹 FULL PRINT SERIAL (260200001)
                    string fullSerial =
     dateCode.ToString("000000") +
     currentSerial.ToString("0000");

                    //Preview
                    txtSerialNo.Text = fullSerial;

                    printUtil.DRIVERBASEDPRINTING(
                   txtProductDesc,
        txtDimensions,
        txtSKUCode,
        txtSKUDesc,
        txtMRPRs,
        txtMonthandYearofImport,
        txtBatchCode,
        txtCountryofOrigin,
        txtPlantCode,
       txtQuantity,
txtImportedBy,
        currentSerial,
        copies
    );

                    bool saved = op.SavePrintHistory(
                txtProductDesc.Text,
                txtDimensions.Text,
                txtSKUCode.Text,
                txtSKUDesc.Text,
                txtMRPRs.Text,
                txtMonthandYearofImport.Text,
                txtBatchCode.Text,
                txtCountryofOrigin.Text,
                txtPlantCode.Text,
                txtQuantity.Text,
                txtImportedBy.Text,
                copies,
                currentSerial,
                0,
                dateCode   
 );
                    //int previewSerial = op.GetNextSerialNumber(dateCode);
                    //txtSerialNo.Text = previewSerial.ToString();

                }

                // =============================================
                // PREVIEW NEXT SERIAL NUMBER LOGIC - INT
                // =============================================
                int newDateCode = op.GetCurrentDateCode();
                int nextPreview = op.GetNextSerialNumberDateWise(newDateCode);

                //string fullPreviewSerial =
                //    newDateCode.ToString();
                //nextPreviewSerial.ToString().PadLeft( 4, '0' );

                txtSerialNo.Text =
      newDateCode.ToString("000000") +
      nextPreview.ToString("0000");



                LoadNilayaGrid();

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Driver Print Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtProductDesc.Text) ||
                 string.IsNullOrWhiteSpace(txtDimensions.Text) ||
                 string.IsNullOrWhiteSpace(txtSKUCode.Text) ||
                 string.IsNullOrWhiteSpace(txtSKUDesc.Text) ||
                 string.IsNullOrWhiteSpace(txtMRPRs.Text) ||
                 string.IsNullOrWhiteSpace(txtMonthandYearofImport.Text) ||
                 string.IsNullOrWhiteSpace(txtBatchCode.Text) ||
                 string.IsNullOrWhiteSpace(txtCountryofOrigin.Text) ||
                 string.IsNullOrWhiteSpace(txtPlantCode.Text) ||
                 string.IsNullOrWhiteSpace(txtImportedBy.Text) ||
                string.IsNullOrWhiteSpace(txtImportedBy.Text) ||
                string.IsNullOrWhiteSpace(txtNoofCopies.Text))
            {
                MessageBox.Show(
                    "All fields are mandatory for saving your data., else you can leave it blank",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );

                return false;
            }
            if (!int.TryParse(txtQuantity.Text, out _))
            {
                MessageBox.Show(
                    "Quantity must be numeric",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                txtQuantity.Focus();
                return false;
            }



            //DateTime MonthandYearofImport;
            if (!DateTime.TryParseExact(
                txtMonthandYearofImport.Text.Trim(),
                "MM/yyyy", 
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _
                ))
            {
                MessageBox.Show(
    "Month and Year of Import must be in MM/YYYY format (e.g. 02/2026)",
    "Validation Error",
    MessageBoxButtons.OK,
    MessageBoxIcon.Error
);


                txtMonthandYearofImport.Focus();
                return false;


            }
            return true;

        }

        private void LoadNilayaGrid()
        {
            dataGridView1.AutoGenerateColumns = true;
            try
            {
                op.fillgrid(
    dataGridView1,
    "SELECT * FROM tblPrintHistory ORDER BY ID ASC"
);


                dataGridView1.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch
            {
                // DO NOTHING – app should still open. Here no catch exception is used.
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellContentClick(sender, e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            txtProductDesc.Clear();
            txtDimensions.Clear();
            txtSKUCode.Clear();
            txtSKUDesc.Clear();
            txtMRPRs.Clear();
            txtMonthandYearofImport.Clear();
            txtBatchCode.Clear();
            txtCountryofOrigin.Clear();
            txtPlantCode.Clear();
            txtImportedBy.Clear();
            txtQuantity.Clear();
            txtImportedBy.Clear();
            txtNoofCopies.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                int copies = int.Parse(txtNoofCopies.Text);

                int yearMonthCode = op.GetCurrentYearMonthCode();

                int datecode = op.GetCurrentDateCode();

                bool saved = op.SavePrintHistory(
     txtProductDesc.Text,
     txtDimensions.Text,
     txtSKUCode.Text,
     txtSKUDesc.Text,
     txtMRPRs.Text,
     txtMonthandYearofImport.Text,
     txtBatchCode.Text,
     txtCountryofOrigin.Text,
     txtPlantCode.Text,
     txtQuantity.Text,    
    txtImportedBy.Text,
     copies,
     0,              // SerialNo = 0 (not printed)
     yearMonthCode,
     datecode    ///Datecode
 );

                if (saved)
                {
                    LoadNilayaGrid();  //This refreshes the grid

                    MessageBox.Show(
                        "Record saved successfully", 
                        "Saved",
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information
                        );
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Save Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }

        //Update button
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show(
                    "Please select a record or a field to update.",
                    "Record Updation Message",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
                return;
            }

            if (!ValidateFields())
                return;

            try
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

                int copies = int.Parse(txtNoofCopies.Text);

                bool updated = op.UpdateLabel(
                    id,
                    txtProductDesc.Text,
                    txtDimensions.Text,
                    txtSKUCode.Text,
                    txtSKUDesc.Text,
                    txtMRPRs.Text,
                    txtMonthandYearofImport.Text,
                    txtBatchCode.Text,
                    txtCountryofOrigin.Text,
                    txtPlantCode.Text,
                    txtQuantity.Text,     
                    txtImportedBy.Text,  
                    copies
                    );

                if (updated)
                {
                    LoadNilayaGrid(); //refreshes grid

                    MessageBox.Show(
                        "Record Updated Successfully",
                        "Record updated",
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information
                        );
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    ex.Message, 
                    "Update Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           if (dataGridView1.CurrentRow == null)    
            {
                MessageBox.Show(
                    "Please select a record to delete", 
                    "Delete Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );
                return;
            }

            DialogResult dr = MessageBox.Show(
                "Do you really want to delete this field?", 
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );

            if (dr == DialogResult.No)
                return;

            try
            {
                //Get Id from selected grid row
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
                bool result = op.DeleteLabel(id);
                if (result)
                {
                    MessageBox.Show(
                        "Label deleted successfully", 
                        "Delete Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                        );
                    btnClearAll.PerformClick();
                }
                op.fillgrid(dataGridView1, "SELECT * FROM tblPrintHistory");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTextBasedPrinting_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            try
            {
                int copies = int.Parse(txtNoofCopies.Text);

                int yearMonthCode = op.GetCurrentYearMonthCode();

                for (int i = 1; i <= copies; i++)
                {
                    //1) Creating Output.txt from Input.txt
                    printUtil.filecreation(
                        txtProductDesc,
                        txtDimensions,
                        txtSKUCode,
                        txtSKUDesc,
                        txtMRPRs,
                        txtMonthandYearofImport,
                        txtBatchCode,
                        txtCountryofOrigin,
                        txtPlantCode,
                        txtImportedBy,
                        txtImportedBy
                        );

                    //2) Printing using spool.exe
                    printUtil.printbatch();

                    int datecode = op.GetCurrentDateCode();
                    //3)Save Print History table
                    op.SavePrintHistory(
      txtProductDesc.Text,
      txtDimensions.Text,
      txtSKUCode.Text,
      txtSKUDesc.Text,
      txtMRPRs.Text,
      txtMonthandYearofImport.Text,
      txtBatchCode.Text,
      txtCountryofOrigin.Text,
      txtPlantCode.Text,
     txtQuantity.Text,
txtImportedBy.Text,
      1,              // NoOfCopies
      0,              // SerialNo
      yearMonthCode,   // ✅ REQUIRED
      datecode 
      );
                }

                // 4) Refresh grid once after printing
                LoadNilayaGrid();

               MessageBox.Show(
                   "Text based printing completed successfully", 
                   "Print Success",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information
                   );
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, 
                    "Text based Printing Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("" +
                "⚠ Are you sure you want to delete all records?\n This action cannot be undone!", 
                "Confirm Delete All",
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning
                );

            if (dr == DialogResult.No)
                return;

            try
            {
                bool result = op.DeleteAllPrintHistory();

                if (result)
                {
                    MessageBox.Show(
                        "All records deleted successfully",
                        "Delete Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information
                        );

                    //Refresh Grid
                    op.fillgrid(
                        dataGridView1, 
                        "SELECT * FROM tblPrintHistory ORDER BY ID ASC"
                        );
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    ex.Message, 
                    "Delete Error", 
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
        }
    }
}
