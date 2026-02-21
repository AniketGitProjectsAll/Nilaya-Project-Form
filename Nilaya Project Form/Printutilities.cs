using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
//using Microsoft.Graph;

namespace Nilaya
{
    class Printutilities
    {
        public void filecreation(
            TextBox txtProductDesc,
            TextBox txtDimensions,
            TextBox txtSKUCode,
            TextBox txtSKUDesc,
            TextBox txtMRPRs,
            TextBox txtMonthandYearofImport,
            TextBox txtBatchCode,
            TextBox txtCountryofOrigin,
            TextBox txtPlantCode,
            TextBox txtQuantity,
            TextBox txtImportedby)
        {
            string filepath = Application.StartupPath + @"\TextFiles";
            string inputpath = Path.Combine(Application.StartupPath, "TextFiles", "Input.txt");

            using (StreamReader inLine = new StreamReader(filepath + @"\Input.txt"))
            using (StreamWriter outLine = new StreamWriter(filepath + @"\Output.txt", false))
            {
                string aLine;

                while ((aLine = inLine.ReadLine()) != null)
                {
                    switch (aLine.Trim().ToUpper())
                    {
                        case "~PRODUCTDESC":
                            outLine.WriteLine(aLine);
                            outLine.WriteLine(inLine.ReadLine() + txtProductDesc.Text); //data
                            continue;

                        case "~DIMENSIONS":
                            outLine.WriteLine(aLine);
                            outLine.WriteLine(inLine.ReadLine() + txtDimensions.Text); //data
                            continue;

                        case "~SKUCODE":
                            outLine.WriteLine(aLine);
                            outLine.WriteLine(inLine.ReadLine() + txtSKUCode.Text); //data
                            continue;

                        case "~SKUDESC":
                            outLine.WriteLine(aLine);
                            outLine.WriteLine(inLine.ReadLine() + txtSKUDesc.Text); //data
                            continue;

                        case "~MRPRS":
                            outLine.WriteLine(aLine);
                            outLine.WriteLine(inLine.ReadLine() + txtMRPRs.Text); //data
                            continue;

                        case "~MONTHYEARIMPORT":
                            outLine.WriteLine(aLine);
                            outLine.WriteLine(inLine.ReadLine() + txtMonthandYearofImport.Text); //data
                            continue;

                        case "~BATCHCODE":
                            outLine.WriteLine(aLine);
                            outLine.WriteLine(inLine.ReadLine() + txtBatchCode.Text); //data
                            continue;

                        case "~COUNTRYOFORIGIN":
                            outLine.WriteLine(aLine);
                            outLine.WriteLine(inLine.ReadLine() + txtCountryofOrigin.Text); //data
                            continue;

                        case "~PLANTCODE":
                            outLine.WriteLine(aLine);
                            outLine.WriteLine(inLine.ReadLine() + txtPlantCode.Text); //data
                            continue;

                        case "~QUANTITY":
                            outLine.WriteLine(aLine);
                            outLine.WriteLine(inLine.ReadLine() + txtQuantity.Text); //data
                            continue;

                        case "~IMPORTEDBY":
                            outLine.WriteLine(aLine);
                            outLine.WriteLine(inLine.ReadLine() + txtImportedby.Text); //data
                            continue;

                        default:
                            outLine.WriteLine(aLine);
                            break;


                    }
                }
            }
        }

        public string SelectedPrinter()
        {
            string filepath = Application.StartupPath + @"\TextFiles\Driver.txt";

            if (!File.Exists(filepath))
                throw new Exception("Driver txt not found at " + filepath);
            return File.ReadAllText(filepath).Trim();
        }

        public void printbatch()
        {
            try
            {
                string basePath = Application.StartupPath;
                string textFilesPath = Path.Combine(basePath, "TextFiles");

                //1) Ensure textfile folder exists
                if (!Directory.Exists(textFilesPath))
                    Directory.CreateDirectory(textFilesPath);

                //2)Required Paths
                string spoolPath = Path.Combine(basePath, "spool.exe");
                string outputPath = Path.Combine(textFilesPath, "Output.txt");
                string batchPath = Path.Combine(textFilesPath, "printbatch.bat");

                //3) Validate required files
                if (!File.Exists(spoolPath))
                    throw new Exception("spool.exe NOT FOUND at : \n" + spoolPath);

                if (!File.Exists(outputPath))
                    throw new Exception("Output.txt NOT FOUND at: \n" + outputPath);

                string drivername = SelectedPrinter();

                //4) Create batchfile once
                File.WriteAllText(batchPath, $"\"{spoolPath}\" \"{outputPath}\" \"{drivername}\"");

                //5) Execute Batch File on once
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = batchPath,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                Process.Start(psi);



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in print_parallelPort", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        public void DRIVERBASEDPRINTING(
            TextBox txtProductDesc,
    TextBox txtDimensions,
    TextBox txtSKUCode,
    TextBox txtSKUDesc,
    TextBox txtMRPRs,
    TextBox txtMonthandYearofImport,
    TextBox txtBatchCode,
    TextBox txtCountryofOrigin,
    TextBox txtPlantCode,
    TextBox txtQuantity,
    TextBox txtImportedBy,
    int SerialNo,
    int totalCopies)
        //int copies)
        {
            string layoutFile = Path.Combine(Application.StartupPath, "TextFiles", "Layout.txt");
            //string codelogo1 = Path.Combine(Application.StartupPath, @"/images/ExtraWashable.bmp");
            //string codelogo2 = Path.Combine(Application.StartupPath, @"/images/Pastewall.bmp");
            //string codelogo3 = Path.Combine(Application.StartupPath, @"/images/GoodLight.bmp");


            string[] printparams;
            double Xoff = 0;
            double Yoff = 0;

            try
            {

                var print = new PrintThroughVBDll.ClsPrintThroughVB6Class();


                print.SetCurrentDefaultPrinter(GetDefaultPrinter());

                if (!File.Exists(layoutFile))
                {
                    MessageBox.Show("Layout.txt not found", "Driver Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (StreamReader read = new StreamReader(layoutFile))
                {
                    while (!read.EndOfStream)
                    {
                        string line = read.ReadLine();

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        if (!line.StartsWith("~"))
                            continue;

                        switch (line.Trim().ToUpper())
                        {
                            case "~XOFF":
                                Xoff = double.Parse(read.ReadLine());
                                break;

                            case "~YOFF":
                                Yoff = double.Parse(read.ReadLine());
                                break;

                            case "~FIX":
                                printparams = ReadLayout(read, line);
                                print.PrintDataOnPrinter(
                                    double.Parse(printparams[0]) + Xoff,
                                    double.Parse(printparams[1]) + Yoff,
                                    printparams[2],
                                    bool.Parse(printparams[3]),
                                    bool.Parse(printparams[4]),
                                    short.Parse(printparams[5]),
                                    short.Parse(printparams[6]),
                                    printparams.Length > 7 ? printparams[7] : ""
                                    );
                                break;

                            case "~PRODUCTDESC":
                                PrintField(read, print, line, txtProductDesc.Text, Xoff, Yoff);
                                break;

                            case "~DIMENSIONS":
                                PrintField(read, print, line, txtDimensions.Text, Xoff, Yoff);
                                break;

                            case "~SKUCODE":
                                PrintField(read, print, line, txtSKUCode.Text, Xoff, Yoff);
                                break;

                            case "~SKUDESC":
                                PrintField(read, print, line, txtSKUDesc.Text, Xoff, Yoff);
                                break;

                            case "~MRPRS":
                                PrintField(read, print, line, txtMRPRs.Text, Xoff, Yoff);
                                break;

                            case "~MONTHYEARIMPORT":
                                PrintField(read, print, line, txtMonthandYearofImport.Text, Xoff, Yoff);
                                break;

                            case "~BATCHCODE":
                                PrintField(read, print, line, txtBatchCode.Text, Xoff, Yoff);
                                break;

                            case "~COUNTRYOFORIGIN":
                                PrintField(read, print, line, txtCountryofOrigin.Text, Xoff, Yoff);
                                break;

                            case "~PLANTCODE":
                                PrintField(read, print, line, txtPlantCode.Text, Xoff, Yoff);
                                break;

                            case "~QUANTITY":
                                PrintField(read, print, line, txtQuantity.Text, Xoff, Yoff);
                                break;

                            case "~IMPORTEDBY":
                                PrintField(read, print, line, txtImportedBy.Text, Xoff, Yoff);
                                break;

                            case "~SERIALNO":
                                string fullSerial =
    GetDateCode() + SerialNo.ToString("0000");

                                PrintField(read, print, line, fullSerial, Xoff, Yoff);
                                break;


                        }

                    }


                }
                print.SenJobToPrinter();
            }




            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Driver Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private string GetDateCode()
        {
           DateTime now = DateTime.Now;
            string year = (now.Year % 100).ToString("00");
            string month = now.Month.ToString("00");
            string day = now.Day.ToString("00");
            return year + month + day;
        }

        //private int GetYearMonthCode()
        //{
        //    DateTime now = DateTime.Now;
        //    return (now.Year % 100) * 100 + now.Month;
        //}

        private void PrintField(
      StreamReader read,
      PrintThroughVBDll.ClsPrintThroughVB6Class print,
      string tag,
      string value,
      double Xoff,
      double Yoff)
        {
            string[] p = ReadLayout(read, tag);
            print.PrintDataOnPrinter(
                double.Parse(p[0]) + Xoff,
                double.Parse(p[1]) + Yoff,
                p[2],
                bool.Parse(p[3]),
                bool.Parse(p[4]),
                short.Parse(p[5]),
                short.Parse(p[6]),
                value
            );
        }

        private string[] ReadLayout(StreamReader read, string tag)
        {
            string layoutline;

            while (true)
            {
                layoutline = read.ReadLine();

                if (layoutline == null)
                    throw new Exception($"Layout missing after {tag}");

                if (string.IsNullOrWhiteSpace(layoutline))
                    continue;

                if (!layoutline.Contains("#"))
                    throw new Exception($"Invalid Layout after {tag}\nLine : {layoutline}");

                break;
            }

            string[] arr = layoutline.Split('#');

            if (arr.Length < 7)
                throw new Exception(
                    $"Invalid format after {tag}. Expected atleast 7 values but got {arr.Length}\n : {layoutline}"
                );

            return arr;
        }


        private string GetDefaultPrinter()
        {
            string filepath = Path.Combine(
                Application.StartupPath,
                "TextFiles",
                "Driver.txt"
                );

            if (!File.Exists(filepath))
            {
                MessageBox.Show(
                    "Driver.txt not found\n" + filepath,
                    "Driver Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
                return string.Empty;
            }
            return File.ReadAllText(filepath).Trim();
        }
    }
}
