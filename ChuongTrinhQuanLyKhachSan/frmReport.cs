using Microsoft.Reporting.WinForms;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChuongTrinhQuanLyKhachSan
{
    public partial class frmReport : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public int id;
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            var historyQuery = db.History.SingleOrDefault(hs => hs.ID == id);
            var historyServiceQuery = db.HistoryService.Where(hs => hs.historyID == id);

            DataTable dtHS = new DataTable("HistoryService");

            dtHS.Columns.Add("sername");
            dtHS.Columns.Add("serquantity");
            dtHS.Columns.Add("sertotal");
            dtHS.Columns.Add("seramount");

            foreach (var item in historyServiceQuery)
            {
                dtHS.Rows.Add(
                    item.sername,
                    item.serquantity,
                    string.Format("{0:#,##0}đ", item.sertotal),
                    string.Format("{0:#,##0}đ", item.seramount)
                );
            }

            DataTable dtH = new DataTable("History");

            dtH.Columns.Add("roomname");
            dtH.Columns.Add("roomtype");
            dtH.Columns.Add("payamount");
            dtH.Columns.Add("staffname");
            dtH.Columns.Add("cusname");
            dtH.Columns.Add("cusphone");
            dtH.Columns.Add("checkin");
            dtH.Columns.Add("checkout");
            dtH.Columns.Add("totalhours");
            dtH.Columns.Add("totalamount");

           

            dtH.Rows.Add(
                historyQuery.roomname,
                historyQuery.roomtype,
                string.Format("{0:#,##0}đ", historyQuery.payamount),
                historyQuery.staffname,
                historyQuery.cusname,
                historyQuery.cusphone,
                historyQuery.checkin,
                historyQuery.checkout,
                historyQuery.totalhours,
                string.Format("{0:#,##0}đ", historyQuery.totalamount)

            );


            ReportDataSource dsHS = new ReportDataSource(dtHS.TableName, dtHS);
            ReportDataSource dsH = new ReportDataSource(dtH.TableName, dtH);
            this.reportViewer1.LocalReport.ReportPath = "Report1.rdlc";
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(dsHS);
            this.reportViewer1.LocalReport.DataSources.Add(dsH);
            this.reportViewer1.RefreshReport();

        }
    }
}
