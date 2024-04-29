using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
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

            }

            if (tcName.SelectedTab == tpPhong)
            {

            }
            if (tcName.SelectedTab == tpQLPhong
                )
            {

            }

            if (role != "admin" && tcName.SelectedTab == tpNV)
            {
                tcName.SelectedTab = tpPhong;
                MessageBox.Show("Bạn không có quyền vào trang nhân viên", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                         where r.isRemove == false
                         select new
                         {
                             r.roomid,
                             r.roomnumber,
                             r.roomtype,
                             r.roomrate,
                             r.roomstatus
                         };

            dgvRoom.DataSource = result.ToList();
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
            if (txbRName.Text == "" )
            {
                MessageBox.Show("Vui lòng nhập tên phòng","Lưu ý",MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    roomrate = decimal.Parse(txbRPrice.Text),
                    roomtype = cbRType.Text,
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

            try
            {
                int id = Convert.ToInt32(txbRID.Text);

                Room room = db.Room.Find(id);
                room.roomnumber = txbRName.Text;
                room.roomtype = cbRType.Text;
                room.roomrate = decimal.Parse(txbRPrice.Text);
                room.roomstatus = cbRStatus.Text;

                db.SaveChanges();
                LoadDataRoom();
                MessageBox.Show("Sửa phòng [" + txbRName.Text + "] thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFieldRoom();
                LoadRoom();
            }
            catch (Exception es)
            {
                MessageBox.Show("Tên phòng này đã có, vui lòng đặt tên khác" +es);
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

            foreach (var item in db.Room)
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
                        detailRoom.ShowDialog();
                    }
                };

                Label label1 = new Label();
                label1.Text = "Loại phòng: " + item.roomtype;
                label1.ForeColor = Color.Black;
                label1.Location = new Point(0, newY + 30);
                clonedGroupBox.Controls.Add(label1);

                Label label2 = new Label();
                label2.Text = "Giá tiền: " + item.roomrate;
                label2.ForeColor = Color.Black;
                label2.Location = new Point(0, newY + 50);
                clonedGroupBox.Controls.Add(label2);


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
            Funct.FormatNumberWithCommas(txbRPrice);
        }

        private void rbNhom_CheckedChanged(object sender, EventArgs e)
        {
            /*if (rbNhom.Checked)
            {

            }*/
        }

        private void btnRefesh_Click(object sender, EventArgs e)
        {
            LoadDataRoom();
            LoadRoom();
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
            if (txbNVSDT.Text.Trim().Length > 10 && txbNVSDT.Text.Trim().Length < 10)
            {
                MessageBox.Show("Vui lòng nhập số điện thoại có 10 chữ số ", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txbNVSDT.Focus();
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

        private void btnNVThem_Click(object sender, EventArgs e)
        {

            if (CheckValidationNhanVien())
            {
                int sdt = int.Parse(txbNVSDT.Text.Trim());
                bool sdtExists = db.Staff.Any(r => r.staffphone == sdt);

                if (sdtExists)
                {
                    MessageBox.Show("Số điện thoại này đã được đăng kí", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txbRName.Focus();
                    return;
                } else
                {
                    db.Staff.Add(new Staff()
                    {
                        staffname = txbNVName.Text,
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
                    LoadRoom();
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
                    staff.staffname = txbNVName.Text;
                    staff.staffsex = cbNVSex.Text;
                    staff.staffphone = int.Parse(txbNVSDT.Text);
                    staff.staffaddress = txbNVAddress.Text;
                    staff.Username = txbUser.Text;
                    staff.Password = txbNVPass.Text;
                    staff.Role = cbRole.Text;

                    db.SaveChanges();
                    LoadDataNhanVien();
                    MessageBox.Show("Sửa nhân viên [" + staff.staffname + "] thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFieldNV();
                    LoadRoom();
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
            DialogResult res = MessageBox.Show("Bạn có chắc muốn xóa nhân viên "+txbNVName.Text, "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                int id = Convert.ToInt32(txbNVID.Text);

                Staff staff = db.Staff.Find(id);
                staff.isRemove = true;
                db.SaveChanges();
                ClearFieldNV();
                LoadRoom();
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
    }
}
