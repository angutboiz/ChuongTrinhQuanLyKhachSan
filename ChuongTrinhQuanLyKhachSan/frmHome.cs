using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChuongTrinhQuanLyKhachSan
{
    public partial class frmHome : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public string name;
        public string role;

        public frmHome()
        {
            InitializeComponent();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            timer1.Start();
            tcName.SelectedTab = tpPhong;
            tabPage2.Text = name;

            LoadDataNhanVien();
            LoadDataKhachHang();
            LoadDataRoom();
            LoadRoom();
            LoadDataService();
            LoadDataHistory();
            LoadDataDateMonthYearToComboBox();
        }

        private void tcName_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tcName.SelectedTab == tabPage1) tcName.SelectedTab = tpPhong;
            if (tcName.SelectedTab == tabPage2) tcName.SelectedTab = tpPhong;

            if (tcName.SelectedTab == tpNV)
            {
                LoadDataNhanVien();
            }

            if (tcName.SelectedTab == tpKH)
            {
                LoadDataKhachHang();
            }

            if (tcName.SelectedTab == tpPhong)
            {
                LoadRoom();
                LoadDataRoom();
            }
            if (tcName.SelectedTab == tpQLPhong)
            {
                LoadDataRoom();
            }

            if (tcName.SelectedTab == tpQLService)
            {
                LoadDataService();
            }

            if (role != "admin" && (tcName.SelectedTab == tpNV || tcName.SelectedTab == tpQLPhong || tcName.SelectedTab == tpQLService))
            {
                tcName.SelectedTab = tpPhong;
                MessageBox.Show("Bạn không có quyền vào trang này", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        void LoadDataNhanVien()
        {
            var result = from c in db.Staff
                         where c.isRemove == false
                         select new
                         {
                             c.staffid,
                             c.staffname,
                             c.staffsex,
                             c.staffphone,
                             c.staffdate,
                             c.staffaddress,
                             c.Username,
                             c.Password,
                             c.Role
                         };
            dgvNV.DataSource = result.ToList();
        }

        void LoadDataKhachHang()
        {
            var result = from c in db.Customer
                         where c.isRemove == false
                         select new
                         {
                             c.cusid,
                             c.cusname,
                             c.cusemail,
                             c.cusphone,
                             c.cusdate,
                             c.cusaddress
                         };
            dgvKH.DataSource = result.ToList();
        }

        void LoadDataRoom()
        {
            var result = from r in db.Room
                         select new
                         {
                             r.roomid,
                             r.roomnumber,
                             r.roomtype,
                             r.roomrate,
                             r.fullnight,
                             r.fulldaynight,
                             r.roomstatus
                         };

            dgvRoom.DataSource = result.ToList();
        }

        void LoadDataService()
        {
            var result = from r in db.Service
                         select new
                         {
                             r.serid,
                             r.sername,
                             r.serprice,
                             r.sertype,
                         };

            dgvService.DataSource = result.ToList();
        }

        void LoadDataHistory()
        {
            var result = from r in db.History
                         select r;

            dgvHistory.DataSource = result.ToList();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            lbDatetimeNow.Text = DateTime.Now.ToString("HH:mm:ss") + "   |   Ngày: " + DateTime.Now.ToString("dd-MM-yyyy");
        }

        // sử lí room

        void ClearFieldRoom()
        {
            txbRID.Text = "";
            txbRName.Text = "";
            txbRPrice.Text = "";
            cbRStatus.SelectedIndex = 0;
            cbRType.SelectedIndex = 0;
        }

        bool CheckValidationRoom()
        {
            if (txbRName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên phòng", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbRName.Focus();
                return false;
            }
            if (txbRPrice.Text == "")
            {
                MessageBox.Show("Vui lòng nhập số tiền", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbRPrice.Focus();
                return false;
            }
            if (cbRType.Text == "")
            {
                MessageBox.Show("Vui lòng chọn kiểu phòng", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cbRStatus.Text == "")
            {
                MessageBox.Show("Vui lòng chọn trạng thái phòng", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnRAdd_Click(object sender, EventArgs e)
        {
            string roomNumber = txbRName.Text.Trim();

            decimal rate = decimal.Parse(txbRPrice.Text.Replace(" ", ""));
            decimal fullngaydem = decimal.Parse(txbTheoNgay.Text.Replace(" ", ""));
            decimal fulldem = decimal.Parse(txbQuaDem.Text.Replace(" ", ""));

            bool roomNumberExists = db.Room.Any(r => r.roomnumber == roomNumber);

            if (roomNumberExists)
            {
                MessageBox.Show("Số phòng đã tồn tại. Vui lòng chọn số phòng khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbRName.Focus();
                return;
            }

            if (CheckValidationRoom())
            {

                db.Room.Add(new Room()
                {
                    roomnumber = txbRName.Text,
                    roomrate = rate,
                    roomtype = cbRType.Text,
                    fulldaynight = fullngaydem,
                    fullnight = fulldem,
                    roomstatus = cbRStatus.Text,

                });

                db.SaveChanges();
                LoadDataRoom();
                MessageBox.Show("Thêm phòng [" + txbRName.Text + "] thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFieldRoom();
                LoadRoom();
            }

        }

        private void btnRClear_Click(object sender, EventArgs e)
        {
            ClearFieldRoom();
        }

        private void btnREdit_Click(object sender, EventArgs e)
        {
            decimal rate = decimal.Parse(txbRPrice.Text.Replace(" ", ""));
            decimal fullngaydem = decimal.Parse(txbTheoNgay.Text.Replace(" ", ""));
            decimal fulldem = decimal.Parse(txbQuaDem.Text.Replace(" ", ""));

            try
            {
                int id = Convert.ToInt32(txbRID.Text);

                Room room = db.Room.Find(id);
                room.roomnumber = txbRName.Text;
                room.roomtype = cbRType.Text;
                room.roomrate = rate;
                room.fulldaynight = fullngaydem;
                room.fullnight = fulldem;
                room.roomstatus = cbRStatus.Text;

                db.SaveChanges();
                LoadDataRoom();
                MessageBox.Show("Sửa phòng [" + txbRName.Text + "] thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFieldRoom();
                LoadRoom();
            }
            catch (Exception es)
            {
                MessageBox.Show("Tên phòng này đã có, vui lòng đặt tên khác" + es);
            }

        }

        private void LoadRoom()
        {
            int newY = 55; // Set initial Y position
            int i = 0;

            Color green = ColorTranslator.FromHtml("#15803d");
            Color red = ColorTranslator.FromHtml("#b91c1c");
            Color gray = ColorTranslator.FromHtml("#374151");

            panelUse.BackColor = green;
            panelBaoTri.BackColor = red;
            panelTrong.BackColor = gray;

            var room = db.Room;

            foreach (var item in room)
            {

                Color selectedRoomColor = ColorTranslator.FromHtml("#374151");
                Guna2GroupBox clonedGroupBox = new Guna2GroupBox();

                clonedGroupBox.Name = item.roomid.ToString();
                clonedGroupBox.Text = "Phòng " + item.roomnumber;
                clonedGroupBox.TextAlign = HorizontalAlignment.Right;
                clonedGroupBox.Size = new Size(175, 175);
                clonedGroupBox.ForeColor = Color.White;

                if (item.roomstatus == "Phòng trống")
                {
                    clonedGroupBox.CustomBorderColor = gray;
                    clonedGroupBox.BorderColor = gray;
                    selectedRoomColor = gray;
                }
                if (item.roomstatus == "Đang sử dụng")
                {
                    clonedGroupBox.CustomBorderColor = green;
                    clonedGroupBox.BorderColor = green;
                    selectedRoomColor = green;
                }
                if (item.roomstatus == "Bảo trì")
                {
                    clonedGroupBox.CustomBorderColor = red;
                    clonedGroupBox.BorderColor = red;
                    selectedRoomColor = red;
                }
                // Cộng thêm giá trị location vào để nó cách các phần tử ra
                int newX = 0;

                if (i % 5 == 0 && i != 0) // Nếu đã thêm đủ 4 phần tử trên một hàng
                {
                    newY += 200; // Tăng vị trí y để bắt đầu một hàng mới
                }

                newX = 35 + (i % 5) * 200; // Tính toán vị trí x dựa trên vị trí trong hàng

                clonedGroupBox.Location = new Point(newX, newY);

                this.tpPhong.Controls.Add(clonedGroupBox);

                clonedGroupBox.Click += (sender, e) =>
                {
                    // Lấy id của phòng từ Name của Guna2GroupBox
                    int roomId = int.Parse(((Guna2GroupBox)sender).Name);

                    // Tìm phòng tương ứng với roomId trong danh sách phòng và truyền nó vào form chi tiết
                    Room selectedRoom = db.Room.FirstOrDefault(r => r.roomid == roomId);
                    if (selectedRoom != null)
                    {
                        frmDetailRoom detailRoom = new frmDetailRoom(selectedRoom, selectedRoomColor);
                        detailRoom.idphong = roomId;
                        detailRoom.ShowDialog();
                    }
                };

                Label label1 = new Label();
                label1.Text = "Loại phòng: " + item.roomtype;
                label1.AutoSize = true;
                label1.ForeColor = Color.Black;
                label1.Location = new Point(0, 50);
                clonedGroupBox.Controls.Add(label1);

                Label label2 = new Label();
                label2.Text = string.Format("Giá tiền: {0:#,##0}đ", item.roomrate);
                label2.ForeColor = Color.Black;
                label2.Location = new Point(0, 70);
                label2.AutoSize = true;
                clonedGroupBox.Controls.Add(label2);

                var booking = db.Booking.FirstOrDefault(b => b.roomid == item.roomid);

                Label label3 = new Label();
                label3.ForeColor = Color.Black;
                label3.Location = new Point(0, 90);
                label3.AutoSize = true;
                if (item.roomstatus == "Đang sử dụng") label3.Text = "Thời gian đặt:\n" + (booking?.checkin.ToString() ?? " Chưa đặt");
                clonedGroupBox.Controls.Add(label3);


                Label label4 = new Label();
                label4.ForeColor = Color.Black;
                label4.Location = new Point(0, 130);
                label4.AutoSize = true;
                label4.Text = "Kiểu thuê: " + (booking?.booktype ?? "Chưa thuê");
                clonedGroupBox.Controls.Add(label4);





                i++;
            }
        }

        private void txbRName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void txbRPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void txbRPrice_TextChanged(object sender, EventArgs e)
        {
            //Funct.FormatNumberWithCommas(txbRPrice);
        }

        private void rbNhom_CheckedChanged(object sender, EventArgs e)
        {
            /*if (rbNhom.Checked)
            {

            }*/
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        // CRUD nhân viên

        bool CheckValidationNhanVien()
        {
            if (txbNVName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbNVName.Focus();
                return false;
            }
            if (txbNVSDT.Text == "")
            {
                MessageBox.Show("Vui lòng nhập số điện thoại", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbNVSDT.Focus();
                return false;
            }
            if (txbUser.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbUser.Focus();
                return false;
            }
            if (txbNVPass.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbNVPass.Focus();
                return false;
            }


            return true;
        }

        void ClearFieldNV()
        {
            txbNVID.Text = "";
            txbNVName.Text = "";
            cbNVSex.Text = "";
            txbNVSDT.Text = "";
            dtpNVDate.Text = "";
            txbNVAddress.Text = "";
            txbUser.Text = "";
            txbNVPass.Text = "";
            cbRole.Text = "";
        }

        void ClearFieldKH()
        {
            txbKHID.Text = "";
            txbKHName.Text = "";
            txbKHSDT.Text = "";
            txbKHEmail.Text = "";
            txbKHAddress.Text = "";
            txbKHSearch.Text = "";
        }

        private void btnNVThem_Click(object sender, EventArgs e)
        {

            if (CheckValidationNhanVien())
            {
                string sdt = txbNVSDT.Text.Trim();
                string username = txbUser.Text.Trim();
                bool sdtExists = db.Staff.Any(r => r.staffphone == sdt);
                bool usenameExists = db.Staff.Any(r => r.Username == username);

                if (sdt.Trim().Length != 10)
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại có 10 chữ số ", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txbNVSDT.Focus();
                }
                else
                {

                    if (sdtExists)
                    {
                        MessageBox.Show("Số điện thoại này đã được đăng kí", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txbRName.Focus();
                        return;
                    }
                    else if (usenameExists)
                    {
                        MessageBox.Show("Tên đăng nhập này đã tồn tại, vui lòng nhập tên khác", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txbUser.Focus();
                        return;
                    }
                    else
                    {
                        db.Staff.Add(new Staff()
                        {
                            staffname = Funct.VietHoaCacKyTu(txbNVName.Text),
                            staffsex = cbNVSex.Text,
                            staffphone = sdt,
                            staffdate = dtpNVDate.Value,
                            staffaddress = txbNVAddress.Text,
                            Username = txbUser.Text,
                            Password = txbNVPass.Text,
                            Role = cbRole.Text,
                            isRemove = false

                        });

                        db.SaveChanges();
                        LoadDataNhanVien();
                        MessageBox.Show("Thêm nhân viên [" + txbNVName.Text + "] thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFieldNV();
                    }
                }
            }

        }

        private void btnNVEdit_Click(object sender, EventArgs e)
        {
            if (CheckValidationNhanVien())
            {
                try
                {
                    int id = Convert.ToInt32(txbNVID.Text);

                    Staff staff = db.Staff.Find(id);
                    staff.staffname = Funct.VietHoaCacKyTu(txbNVName.Text);
                    staff.staffsex = cbNVSex.Text;
                    staff.staffphone = txbNVSDT.Text;
                    staff.staffaddress = txbNVAddress.Text;
                    staff.Username = txbUser.Text;
                    staff.Password = txbNVPass.Text;
                    staff.Role = cbRole.Text;

                    db.SaveChanges();
                    LoadDataNhanVien();
                    MessageBox.Show("Sửa nhân viên [" + staff.staffname + "] thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFieldNV();
                }
                catch (Exception es)
                {
                    MessageBox.Show(es.ToString());
                }

            }
        }

        private void btnNVClear_Click(object sender, EventArgs e)
        {
            ClearFieldNV();
        }

        private void txbNVSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void dgvNV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DialogResult res = MessageBox.Show("Bạn có chắc muốn xóa nhân viên " + txbNVName.Text, "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                int id = Convert.ToInt32(txbNVID.Text);

                Staff staff = db.Staff.Find(id);
                staff.isRemove = true;
                db.SaveChanges();
                ClearFieldNV();
                LoadDataNhanVien();
            }
        }

        private void dgvNV_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txbNVID.Text = dgvNV.SelectedRows[0].Cells[0].Value.ToString();
            txbNVName.Text = dgvNV.SelectedRows[0].Cells[1].Value.ToString();
            cbNVSex.Text = dgvNV.SelectedRows[0].Cells[2].Value.ToString();
            txbNVSDT.Text = dgvNV.SelectedRows[0].Cells[3].Value.ToString();
            dtpNVDate.Text = dgvNV.SelectedRows[0].Cells[4].Value.ToString();
            txbNVAddress.Text = dgvNV.SelectedRows[0].Cells[5].Value.ToString();
            txbUser.Text = dgvNV.SelectedRows[0].Cells[6].Value.ToString();
            txbNVPass.Text = dgvNV.SelectedRows[0].Cells[7].Value.ToString();
            cbRole.Text = dgvNV.SelectedRows[0].Cells[8].Value.ToString();
        }

        bool CheckValidationKhachHang()
        {
            if (txbKHName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbKHName.Focus();
                return false;
            }
            if (txbKHSDT.Text == "")
            {
                MessageBox.Show("Vui lòng nhập số điện thoại", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbKHSDT.Focus();
                return false;
            }

            return true;
        }

        private void btnKHAdd_Click(object sender, EventArgs e)
        {
            if (CheckValidationKhachHang())
            {
                string sdt = txbKHSDT.Text;
                bool sdtExists = db.Staff.Any(r => r.staffphone == sdt);

                if (sdtExists)
                {
                    MessageBox.Show("Số điện thoại này đã được đăng kí", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txbKHSDT.Focus();
                    return;
                }
                db.Customer.Add(new Customer()
                {
                    cusname = Funct.VietHoaCacKyTu(txbKHName.Text),
                    cusemail = txbKHEmail.Text,
                    cusphone = txbKHSDT.Text,
                    cusdate = dtpKHDate.Value,
                    cusaddress = txbKHAddress.Text,
                    isRemove = false
                });

                db.SaveChanges();
                LoadDataKhachHang();
                MessageBox.Show("Thêm nhân viên [" + txbKHName.Text + "] thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFieldKH();
            }
        }

        private void btnKHEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(txbKHID.Text);

                Customer cus = db.Customer.Find(id);
                cus.cusname = Funct.VietHoaCacKyTu(txbKHName.Text);
                cus.cusemail = txbKHEmail.Text;
                cus.cusphone = txbKHSDT.Text;
                cus.cusaddress = txbKHAddress.Text;
                cus.cusdate = dtpKHDate.Value;

                db.SaveChanges();
                LoadDataKhachHang();
                MessageBox.Show("Sửa khách hàng [" + txbKHName.Text + "] thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFieldKH();
            }
            catch (Exception es)
            {
                MessageBox.Show("Lỗi\n" + es);
            }
        }

        private void btnKHClear_Click(object sender, EventArgs e)
        {
            ClearFieldKH();
        }

        private void txbKHSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvKH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void dgvKH_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvKH_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txbKHID.Text = dgvKH.SelectedRows[0].Cells[0].Value.ToString();
            txbKHName.Text = dgvKH.SelectedRows[0].Cells[1].Value.ToString();
            txbKHSDT.Text = dgvKH.SelectedRows[0].Cells[2].Value.ToString();
            txbKHEmail.Text = dgvKH.SelectedRows[0].Cells[3].Value.ToString();
            dtpKHDate.Text = dgvKH.SelectedRows[0].Cells[4].Value?.ToString();
            txbKHAddress.Text = dgvKH.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void dgvKH_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DialogResult res = MessageBox.Show("Bạn có chắc muốn xóa khách hàng " + txbKHName.Text, "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                int id = Convert.ToInt32(txbKHID.Text);

                Customer cus = db.Customer.Find(id);
                cus.isRemove = true;
                db.SaveChanges();
                ClearFieldKH();
                LoadDataKhachHang();
            }
        }

        private void dgvRoom_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txbRID.Text = dgvRoom.SelectedRows[0].Cells[0].Value.ToString();
            txbRName.Text = dgvRoom.SelectedRows[0].Cells[1].Value.ToString();
            cbRType.Text = dgvRoom.SelectedRows[0].Cells[2].Value.ToString();
            txbRPrice.Text = dgvRoom.SelectedRows[0].Cells[3].Value.ToString();
            cbRStatus.Text = dgvRoom.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void dgvRoom_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DialogResult res = MessageBox.Show("Bạn có chắc muốn xóa phòng " + txbRName.Text, "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                int id = Convert.ToInt32(txbRID.Text);
                var query = db.Room.Find(id);
                db.Room.Remove(query);

                db.SaveChanges();
                ClearFieldRoom();
                LoadDataRoom();
            }
        }

        private void cbRStatus_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void cbRType_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* VIP
 Quạt
 Thường
 Máy lạnh*/
            if (cbRType.SelectedIndex == 0)
            {
                txbRPrice.Text = "200 000";
                txbQuaDem.Text = "480 000";
                txbTheoNgay.Text = "650 000";

            }
            if (cbRType.SelectedIndex == 1)
            {
                txbRPrice.Text = "250 000";
                txbQuaDem.Text = "600 000";
                txbTheoNgay.Text = "700 000";
            }
            if (cbRType.SelectedIndex == 2)
            {
                txbRPrice.Text = "300 000";
                txbQuaDem.Text = "600 000";
                txbTheoNgay.Text = "800 000";
            }
            if (cbRType.SelectedIndex == 3)
            {
                txbRPrice.Text = "300 000";
                txbQuaDem.Text = "700 000";
                txbTheoNgay.Text = "900 000";
            }
        }

        private void txbSerPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)8))
                e.Handled = true;
        }

        bool CheckValidationService()
        {
            if (txbSerName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên dịch vụ", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbSerName.Focus();
                return false;
            }
            if (txbSerPrice.Text == "")
            {
                MessageBox.Show("Vui lòng nhập giá tiền", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbSerPrice.Focus();
                return false;
            }
            if (cbSerType.Text == "")
            {
                MessageBox.Show("Vui lòng chọn loại dịch vụ", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbSerType.Focus();
                return false;
            }
            return true;

        }

        void ClearFieldService()
        {
            txbSerID.Text = "";
            txbSerName.Text = "";
            txbSerPrice.Text = "";
            cbSerType.Text = "";
        }

        private void btnSerThem_Click(object sender, EventArgs e)
        {
            if (CheckValidationService())
            {
                db.Service.Add(new Service()
                {
                    sername = Funct.VietHoaMotKyTu(txbSerName.Text),
                    serprice = int.Parse(txbSerPrice.Text),
                    sertype = cbSerType.Text,
                });

                db.SaveChanges();
                LoadDataService();
            }
        }

        private void btnSerEdit_Click(object sender, EventArgs e)
        {
            if (CheckValidationService())
            {
                int id = Convert.ToInt32(txbSerID.Text);

                Service ser = db.Service.Find(id);
                ser.sername = Funct.VietHoaMotKyTu(txbSerName.Text);
                ser.serprice = int.Parse(txbSerPrice.Text);
                ser.sertype = cbSerType.Text;

                db.SaveChanges();
                MessageBox.Show("Sửa dịch vụ [" + txbSerName.Text + "] thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataService();
            }
        }

        private void dgvService_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            txbSerID.Text = dgvService.SelectedRows[0].Cells[0].Value.ToString();
            txbSerName.Text = dgvService.SelectedRows[0].Cells[1].Value.ToString();
            txbSerPrice.Text = dgvService.SelectedRows[0].Cells[2].Value.ToString();
            cbSerType.Text = dgvService.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void dgvService_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DialogResult res = MessageBox.Show("Bạn có chắc muốn xóa dịch vụ " + txbSerName.Text, "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                int id = Convert.ToInt32(txbSerID.Text);
                Service serviceToDelete = db.Service.FirstOrDefault(s => s.serid == id);
                if (serviceToDelete != null)
                {


                    db.Service.Remove(serviceToDelete);

                    db.SaveChanges();
                    ClearFieldService();
                    LoadDataService();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy dịch vụ cần xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Excel Workbook|*.xlsx";
                saveFileDialog1.Title = "Save an Excel File";
                saveFileDialog1.FileName = "Báo cáo doanh thu tháng "+DateTime.Now.Month+ " năm "+DateTime.Now.Year+".xlsx"; 
                saveFileDialog1.ShowDialog();

                if (saveFileDialog1.FileName != "")
                {
                    Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                    // creating new WorkBook within Excel application  
                    Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                    // creating new Excelsheet in workbook  
                    Microsoft.Office.Interop.Excel.Worksheet worksheet = null; // Change _Worksheet to Worksheet
                                                                               // see the excel sheet behind the program  
                    app.Visible = true;
                    // get the reference of first sheet. By default its name is Sheet1.  
                    // store its reference to worksheet  
                    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.ActiveSheet;
                    // changing the name of active sheet  
                    worksheet.Name = "Exported from gridview";
                    // storing header part in Excel  
                    for (int i = 1; i < dgvHistory.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i] = dgvHistory.Columns[i - 1].HeaderText;
                    }
                    // storing Each row and column value to excel sheet  
                    for (int i = 0; i < dgvHistory.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvHistory.Columns.Count; j++)
                        {
                            worksheet.Cells[i + 2, j + 1] = dgvHistory.Rows[i].Cells[j].Value.ToString();
                        }
                    }
                    workbook.SaveAs(saveFileDialog1.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    app.Quit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gặp lỗi trong quá trình xuất, Mã lỗi:\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private void cbDDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void cbDMonth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void cbDYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == (char)8))
                e.Handled = true;
        }

        private void rbDDay_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDDay.Checked)
            {
                cbDDay.Text = DateTime.Now.Day.ToString();
            }
            else
            {
                cbDDay.Text = "";
            }
        }

        private void rbDMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDMonth.Checked)
            {
                cbDMonth.Text = DateTime.Now.Month.ToString();
            }
            else
            {
                cbDMonth.Text = "";
            }
        }

        private void rbDYear_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDYear.Checked)
            {
                cbDYear.Text = DateTime.Now.Year.ToString();
            }
            else
            {
                cbDYear.Text = "";
            }
        }

        void LoadDataDateMonthYearToComboBox()
        {
            cbDDay.Items.Clear();
            cbDMonth.Items.Clear();
            cbDYear.Items.Clear();

            // Load data for days
            for (int i = 1; i <= 31; i++)
            {
                cbDDay.Items.Add(i);
            }

            // Load data for months
            for (int i = 1; i <= 12; i++)
            {
                cbDMonth.Items.Add(i);
            }

            // Load data for years (you can adjust the range as needed)
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 100; i <= currentYear; i++)
            {
                cbDYear.Items.Add(i);
            }

            // Optionally, you can set default values
            // For example, if you want to set today's date as default:
            cbDDay.SelectedItem = DateTime.Now.Day;
            cbDMonth.SelectedItem = DateTime.Now.Month;
            cbDYear.SelectedItem = DateTime.Now.Year;

            var mostFrequentRoom = db.History
                                .GroupBy(h => h.roomname)
                                .OrderByDescending(g => g.Count())
                                .Select(g => new
                                {
                                    RoomName = g.Key,
                                    Count = g.Count()
                                })
                                .FirstOrDefault();
            if (mostFrequentRoom != null)
            {
                lbPhongNhieuNhat.Text = "Phòng được đặt nhiều nhất: " + mostFrequentRoom.RoomName.ToString() ?? "Chưa có dữ liệu";
                lbSoLanDatPhong.Text = "Số lần đặt phòng: " + mostFrequentRoom.Count.ToString() ?? "Chưa có dữ liệu";
            }

            var mostFrequentKH = db.History
                                .GroupBy(h => h.cusname)
                                .OrderByDescending(g => g.Count())
                                .Select(g => new
                                {
                                    Cusname = g.Key,
                                    Count = g.Count()
                                })
                                .FirstOrDefault();

            if (mostFrequentRoom != null)
            {
                lbTenKH.Text = "Tên khách hàng ủng hộ nhiều nhất: " + mostFrequentKH.Cusname.ToString() ?? "Chưa có dữ liệu";
                lbSoLanKH.Text = "Số lần khách đặt phòng: " + mostFrequentKH.Count.ToString() ?? "Chưa có dữ liệu";
            }
        }

        void QueryDashboard()
        {
            if (rbDDay.Checked)
            {
                int day = int.Parse(cbDDay.Text);


                DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
                var result = from h in db.History
                             where DbFunctions.TruncateTime(h.checkin) == startDate
                             select h;

                dgvHistory.DataSource = result.ToList();

                var totalRevenue = db.History.Where(h => DbFunctions.TruncateTime(h.checkin) == startDate).Sum(h => h.payamount);
                cbDTotal.Text = string.Format("{0:#,##0}đ", totalRevenue ?? 0);

            }

            if (rbDMonth.Checked)
            {
                int month = int.Parse(cbDMonth.Text);
                int year = DateTime.Now.Year;

                DateTime startDate = new DateTime(year, month, 1); // Ngày đầu tiên của tháng
                DateTime endDate = startDate.AddMonths(1).AddDays(-1); // Ngày cuối cùng của tháng

                var result = from h in db.History
                             where h.checkin >= startDate && h.checkin <= endDate
                             select h;

                dgvHistory.DataSource = result.ToList();

                var totalRevenue = result.Sum(h => h.payamount);
                cbDTotal.Text = string.Format("{0:#,##0}đ", totalRevenue ?? 0);
            }

            if (rbDYear.Checked)
            {
                int year = int.Parse(cbDYear.Text);

                DateTime startDate = new DateTime(year, 1, 1); // Ngày đầu tiên của năm
                DateTime endDate = startDate.AddYears(1).AddDays(-1); // Ngày cuối cùng của năm

                var result = from h in db.History
                             where h.checkin >= startDate && h.checkin <= endDate
                             select h;

                dgvHistory.DataSource = result.ToList();

                var totalRevenue = result.Sum(h => h.payamount);
                cbDTotal.Text = string.Format("{0:#,##0}đ", totalRevenue ?? 0);

            }

            if (rbDDashboard.Checked)
            {
                var totalRevenue = db.History.Sum(h => h.payamount);
                cbDTotal.Text = string.Format("{0:#,##0}đ", totalRevenue ?? 0);
            }
        }

        private void cbDDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryDashboard();
        }

        private void cbDMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryDashboard();
        }

        private void cbDYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryDashboard();
        }

        private void rbDDashboard_CheckedChanged(object sender, EventArgs e)
        {
            QueryDashboard();
        }
    }
}
