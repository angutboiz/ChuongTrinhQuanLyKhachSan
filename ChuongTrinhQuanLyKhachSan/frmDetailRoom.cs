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
    public partial class frmDetailRoom : Form
    {
        private Room room;
        private Color roomColor;
        public int idphong;
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public frmDetailRoom(Room room, Color roomColor)
        {
            InitializeComponent();
            this.room = room;
            this.roomColor = roomColor;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDetailRoom_Load(object sender, EventArgs e)
        {
            grpRoom.Text = "Phòng "+ room.roomnumber;
            grpRoom.BorderColor = roomColor;
            grpRoom.ForeColor = Color.White;
            grpRoom.CustomBorderColor = roomColor;

            txbStart.Text = DateTime.Now.ToString("HH:mm:ss");
            btnThanhToan.Enabled = false;

            var service = from s in db.Service select s.sername;
            cbService.DataSource = service.ToList();

            var staff = from nv in db.Staff
                        where nv.isRemove == false && nv.Role != "admin"
                        select nv.staffname;

            cbNV.DataSource = staff.ToList();

            var customer = from cus in db.Customer
                        where cus.isRemove == false
                        select cus.cusname;

            cbKH.DataSource = customer.ToList();
        }

        private DateTime startTime;
        private TimeSpan elapsedTime;
        private void timer1_Tick(object sender, EventArgs e)
        {
            txbEnd.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            if (chkTuChinh.Checked)
            {
                startTime = DateTime.Parse(txbStart.Text);
                txbStart.Text = startTime.ToString("HH:mm:ss");
                timer1.Start();

            }
            else
            {
                startTime = DateTime.Now;
                txbStart.Text = startTime.ToString("HH:mm:ss");
                timer1.Start();

            }
            btnThanhToan.Enabled = true;
            btnDatPhong.Enabled = false;
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            DateTime endTime = DateTime.Now;
            txbEnd.Text = endTime.ToString("HH:mm:ss");

            // Tính toán tổng thời gian giữa startTime và endTime
            elapsedTime = endTime - startTime;
            // Hiển thị tổng thời gian trong định dạng phù hợp
            txbTongTime.Text = elapsedTime.ToString(@"hh\:mm\:ss");

            btnThanhToan.Enabled = false;
            btnDatPhong.Enabled = true;
        }

        void LoadServicesComboBox()
        {

        }

        private void btnFrmService_Click(object sender, EventArgs e)
        {
            int soluong = int.Parse(txbServiceSoluong.Text);
            dgvService.Rows.Add(soluong);
            /// todo ở đây
        }
        int soluong = 0;
        private void btnGiam_Click(object sender, EventArgs e)
        {
            if (soluong == 0)
            {
                soluong = 0;
                return;
            }
            else
            {
                soluong--;
                txbServiceSoluong.Text = soluong.ToString();

            }
        }

        private void btnTang_Click(object sender, EventArgs e)
        {
            soluong++;
            txbServiceSoluong.Text = soluong.ToString();
        }

        private void grpRoom_Click(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox4_Click(object sender, EventArgs e)
        {

        }

        private void cbNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tennv = cbNV.Text;
            var staff = db.Staff.FirstOrDefault(nv => nv.staffname == tennv);

            if (staff != null)
            {
                txbNVID.Text = staff.staffid.ToString();
                txbNVSDT.Text = staff.staffphone.ToString();
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên có tên " + tennv, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tenkh = cbKH.Text;
            var cus = db.Customer.FirstOrDefault(c => c.cusname == tenkh);

            
            if (cus != null)
            {
                txbKHID.Text = cus.cusid.ToString();
                txbKHPhone.Text = cus.cusphone;
            }
            else
            {
                MessageBox.Show("Không tìm thấy khách hàng có tên " + cus, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbService_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
