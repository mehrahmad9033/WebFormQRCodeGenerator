using Microsoft.Reporting.WebForms;
using QRCode.RDLC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZXing;    

namespace QRCode
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //===
            } 
        }     
        private void GenerateCode(string name)
        {
            var writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            var result = writer.Write(name);
            string path = Server.MapPath("~/images/QRImage.jpg");
            var barcodeBitmap = new Bitmap(result); 
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            } 
            System.Drawing.Image img = System.Drawing.Image.FromFile(path);
            LoadReport(ImageToByteArray(img));

        }
        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        void LoadReport(byte[] image)
        {
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            ds ds1 = new ds();
            DataRow dr = ds1.Tables[0].NewRow();
            dr[0] = image;
            ds1.Tables[0].Rows.Add(dr);
           
            ReportDataSource datasource = new ReportDataSource("DataSet1", ds1.Tables[0]);
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateCode("ITEM: "+ItemName.Text+"\n"+ "Price: " + Price.Text + "\n" + "Quantity: " + QTY.Text + "\n" + "Batch: " + Batch.Text + "\n" );
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
    }
}