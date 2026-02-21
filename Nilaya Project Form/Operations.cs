using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Nilaya
{
    class Operations
    {
        private readonly string connectionString =
@"Server=DELL3400-5VPD10;
Database=Asian_Paints_Nilaya;
Trusted_Connection=True;
TrustServerCertificate=True;";



        // =========================
        // 🔹 YEAR MONTH CODE (YYMM)
        // =========================
        

        public void fillgrid(DataGridView dataGridView1, string sqlquery)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlDataAdapter da = new SqlDataAdapter(sqlquery, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Grid Load Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        public bool SavePrintHistory(
            string ProductDesc,
    string Dimensions,
    string SKUCode,
    string SKUDesc,
    string MRPRs,
    string MonthandYearofImport,
    string BatchCode,
    string CountryofOrigin,
    string PlantCode,
    string Quantity,
    string ImportedBy,
    int NoofCopies,
    int SerialNo,
   int yearMonthCode,
int dateCode)  //this must be int not string

        {
            try
            {
                if (!int.TryParse(Quantity, out int qty))
                {
                    MessageBox.Show("Quantity must be numeric", "Validation Error");
                    return false;
                }

                
                //This is Insert query
                string query = @"
INSERT INTO tblPrintHistory
(
    ProductDesc,
    Dimensions,
    SKUCode,
    SKUDesc,
    MRPRs,
    MonthAndYearOfImport,
    BatchCode,
    CountryOfOrigin,
    PlantCode,
    Quantity,
    ImportedBy,
    PrintDateTime,
    NoOfCopies,
    SerialNo,
    YearMonthCode,
    DateCode,
SerialNoData
)
VALUES
(
    @ProductDesc,
    @Dimensions,
    @SKUCode,
    @SKUDesc,
    @MRPRs,
    @MonthAndYearOfImport,
    @BatchCode,
    @CountryOfOrigin,
    @PlantCode,
    @Quantity,
    @ImportedBy,
    @PrintDateTime,
    @NoOfCopies,
    @SerialNo,
    @YearMonthCode,
    @DateCode,
    @SerialNoData
)";
                string serialNoData =
                    dateCode.ToString("000000") +
                    SerialNo.ToString("0000");

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductDesc", ProductDesc);
                    cmd.Parameters.AddWithValue("@Dimensions", Dimensions);
                    cmd.Parameters.AddWithValue("@SKUCode", SKUCode);
                    cmd.Parameters.AddWithValue("@SKUDesc", SKUDesc);
                    cmd.Parameters.AddWithValue("@MRPRs", MRPRs);
                    cmd.Parameters.AddWithValue("@MonthAndYearOfImport", MonthandYearofImport);
                    cmd.Parameters.AddWithValue("@BatchCode", BatchCode);
                    cmd.Parameters.AddWithValue("@CountryOfOrigin", CountryofOrigin);
                    cmd.Parameters.AddWithValue("@PlantCode", PlantCode);
                    cmd.Parameters.AddWithValue("@Quantity", qty);
                    cmd.Parameters.AddWithValue("@ImportedBy", ImportedBy);
                    cmd.Parameters.AddWithValue("@NoOfCopies", NoofCopies);
                    cmd.Parameters.AddWithValue("@SerialNo", SerialNo);
                    cmd.Parameters.AddWithValue("@YearMonthCode", yearMonthCode);
                    cmd.Parameters.AddWithValue("@PrintDateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateCode", dateCode);
                    cmd.Parameters.AddWithValue("@SerialNoData", serialNoData);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error saving print history: {ex.Message}",
                    "DB Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }
        }

        public bool DeleteLabel(int id)
        {
            
            try
            {
                using (SqlConnection con = new SqlConnection(@"Server=DELL3400-5VPD10;
Database=Asian_Paints_Nilaya;
Trusted_Connection=True;
TrustServerCertificate=True;"))
                {
                    string query = "DELETE FROM tblPrintHistory WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID",id);

                        con.Open();
                        int rows = cmd.ExecuteNonQuery();

                        return rows == 1; // This deletes exact one row
                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(
                    ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error
                    );
            }
            return false;
        }

        public bool UpdateLabel(
            int id, 
            string ProductDesc, 
            string Dimensions, 
            string SKUCode, 
            string SKUDesc,
            string MRPRs,
            string MonthandYearofImport, 
            string BatchCode, 
            string CountryofOrigin,
            string PlantCode,
            string Quantity,
            string ImportedBy, 
            int copies)
        {
            try
            {
                if (!int.TryParse(Quantity, out int quantity))
                {
                    MessageBox.Show("Quantity must be numeric", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string query = @"
UPDATE tblPrintHistory
SET
    ProductDesc = @ProductDesc,
    Dimensions = @Dimensions,
    SKUCode = @SKUCode,
    SKUDesc = @SKUDesc,
    MRPRs = @MRPRs,
    MonthandYearofImport = @MonthandYearofImport,
    BatchCode = @BatchCode,
    CountryofOrigin = @CountryofOrigin,
    PlantCode = @PlantCode,
    Quantity = @Quantity,
    ImportedBy = @ImportedBy,
    NoOfCopies = @NoOfCopies
WHERE ID = @ID";
                using (SqlConnection con = new SqlConnection(connectionString))
                using(SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@ProductDesc", ProductDesc);
                    cmd.Parameters.AddWithValue("@Dimensions", Dimensions);
                    cmd.Parameters.AddWithValue("@SKUCode", SKUCode);
                    cmd.Parameters.AddWithValue("@SKUDesc", SKUDesc);
                    cmd.Parameters.AddWithValue("@MRPRs", MRPRs);
                    cmd.Parameters.AddWithValue("@MonthandYearofImport", MonthandYearofImport);
                    cmd.Parameters.AddWithValue("@BatchCode", BatchCode);
                    cmd.Parameters.AddWithValue("@CountryofOrigin", CountryofOrigin);
                    cmd.Parameters.AddWithValue("@PlantCode", PlantCode);
                    cmd.Parameters.AddWithValue("@Quantity", Quantity);
                    cmd.Parameters.AddWithValue("@ImportedBy", ImportedBy);
                    cmd.Parameters.AddWithValue("@NoofCopies", copies);

                    con.Open();
                   return cmd.ExecuteNonQuery() == 1;
                }
                                     
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool DeleteAllPrintHistory()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(
                    @"Server=DELL3400-5VPD10;
Database=Asian_Paints_Nilaya;
Trusted_Connection=True;
TrustServerCertificate=True;"))
                {
                    string query = "DELETE FROM tblPrintHistory";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Delete Error",
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );
                return false;
            }
        }


        // ====================================|
        //  MONTHLY RESET SERIAL NUMBER LOGIC |  =>  MONHLY RESET BOOM
        // ====================================|
        public int GetNextSerialNumber(int yearMonthCode)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT ISNULL(MAX(SerialNo), 0)
          FROM tblPrintHistory
          WHERE YearMonthCode = @YearMonthCode", con))
            {
                cmd.Parameters.AddWithValue("@YearMonthCode", yearMonthCode);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            }
        }


        // =============================================
        //  DATEWISE SRNO  RESET  - >SERIAL NUMBER LOGIC
        // =============================================
        public int GetNextSerialNumberDateWise(int dateCode)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT ISNULL(MAX(SerialNo), 0)
          FROM tblPrintHistory
          WHERE DateCode = @DateCode", con))
            {
                cmd.Parameters.AddWithValue("@DateCode", dateCode);
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            }
        }

        // ==============================
        //  DATE CODE (YYMMDD) LOGIC
        // ==============================
        public int GetCurrentDateCode()
        {
            DateTime now = DateTime.Now;

            string year = (now.Year % 100).ToString("00");
            string month = now.Month.ToString("00");
            string day = now.Day.ToString("00");

            string dateCode = year + month + day;   // "260220"

            return int.Parse(dateCode);             // return as INT
        }


        // ==============================
        //  CURRENT YEAR MONTH CODE  LOGIC
        // ==============================
        public int GetCurrentYearMonthCode()
        {
            DateTime now = DateTime.Now;
            return (now.Year % 100) * 100 + now.Month; // 2026 Feb → 2602
        }
    }
}
