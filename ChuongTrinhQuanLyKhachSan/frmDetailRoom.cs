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

namespace ChuongTrinhQuanLyKhachSan
{
    public partial class frmDetailRoom : Form
    {
        Room room;
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
            grpRoom.Text = "Phòng " + room.roomnumber;
            grpRoom.BorderColor = roomColor;
            grpRoom.ForeColor = Color.White;
            grpRoom.CustomBorderColor = roomColor;


            txbGiaTheoGio.Text = room.roomrate.ToString();
            txbLoaiPhong.Text = room.roomtype;

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

            if (room.roomstatus == "Đang sử dụng")
            {
                btnDatPhong.Enabled = false;
                btnThanhToan.Enabled = true;
                btnClearData.Enabled = false;
               
            }
            else if (room.roomstatus == "Bảo trì")
            {
                btnDatPhong.Enabled = false;
                btnThanhToan.Enabled = false;
                btnClearData.Enabled = true;
            }



            //kiếm xem id room có trong booking chưa, nếu có thì hiển thị data staff và customer lên load form
            var booking = db.Booking.FirstOrDefault(b => b.roomid == room.roomid);
            
            if (booking != null && booking.bookstatus == "Đang thuê phòng")
            {
                var staffid = db.Staff.Find(booking.staffid);
                var custom = db.Customer.Find(booking.cusid);

                if (staffid != null && custom != null)
                {
                    cbNV.Text = staffid.staffname.ToString();
                    cbKH.Text = custom.cusname.ToString();
                    txbStart.Text = booking.checkin.ToString();
                }
                
            }
            else
            {
                ClearData();
                btnDatPhong.Enabled = true;
                btnThanhToan.Enabled = false;
                btnClearData.Enabled = false;
            }
        }

        private DateTime startTime;
        private void timer1_Tick(object sender, EventArgs e)
        {
            txbEnd.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            if (cbKH.SelectedIndex == -1 && cbNV.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn chưa chọn tên nhân viên hoặc tên khách hàng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (chkTuChinh.Checked)
                {
                    startTime = DateTime.Parse(txbStart.Text);
                    txbStart.Text = startTime.ToString();
                    timer1.Start();

                }
                else
                {
                    startTime = DateTime.Now;
                    txbStart.Text = startTime.ToString();
                    timer1.Start();

                }

                

                Room r = db.Room.Find(room.roomid);
                r.roomstatus = "Đang sử dụng";

                db.Booking.Add(new Booking()
                {
                    staffid = int.Parse(txbNVID.Text),
                    cusid = int.Parse(txbKHID.Text),
                    roomid = room.roomid,
                    checkin = DateTime.Now,
                    bookstatus = "Đang thuê"
                });

                db.SaveChanges();

                ClearData();
                ClearData();

                btnThanhToan.Enabled = true;
                btnDatPhong.Enabled = false;

            }


            
          
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            DateTime endTime = DateTime.Now;
            txbEnd.Text = endTime.ToString();
            DateTime startTime = DateTime.Parse(txbStart.Text);

            // Tính toán tổng thời gian giữa startTime và endTime
            TimeSpan elapsedTime = endTime - startTime;
            // Hiển thị tổng thời gian trong định dạng phù hợp
            double totalHours = elapsedTime.TotalHours;

            txbTongTime.Text = Math.Round(totalHours,0).ToString();

            int giatheogio = int.Parse(txbGiaTheoGio.Text);
            int tongthoigian = int.Parse(txbTongTime.Text);
            int phi = int.Parse(txbPhiTheoGio.Text.Replace(" ", ""));

            int tongtien = (tongthoigian * phi) + giatheogio;

            lbTongTien.Text = string.Format("Tổng tiền: {0:#,##0}đ", tongtien);

            var bk = db.Booking.SingleOrDefault(b => b.roomid == room.roomid);
            bk.checkout = DateTime.Parse(txbEnd.Text);
            bk.bookstatus = "Trả phòng";


            Room r = db.Room.Find(room.roomid);
            r.roomstatus = "Bảo trì";
            db.SaveChanges();



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
          
        }

        private void cbService_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnClearData_Click(object sender, EventArgs e)
        {

            var check = db.Room.SingleOrDefault(b => b.roomid == room.roomid);
            check.roomstatus = "Phòng trống";
            db.SaveChanges();
            ClearData();
            ClearData();
        }

        void ClearData()
        {
            cbService.SelectedIndex = -1;
            cbKH.SelectedIndex = -1;
            cbNV.SelectedIndex = -1;

            txbNVID.Text = "";
            txbNVSDT.Text = "";
            txbKHID.Text = "";
            txbKHPhone.Text = "";
        }

        private void chkTuChinh_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTuChinh.Checked)
            {
                txbStart.Enabled = true;
                txbStart.Text = "20/04/2024 12:30";
            } else
            {
                txbStart.Enabled = false;
                txbStart.Text = "";
            }
        }
    }
}
