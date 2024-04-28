using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public frmHome()
        {
            InitializeComponent();
        }

        private void frmHome_Load(object sender, EventArgs e)
        {
            timer1.Start();

            LoadData();
            LoadRoom();
            AddBindingRoom();
        }

        void LoadData()
        {
            // TODO: This line of code loads data into the 'qLKHACHSANDataSet3.Room' table. You can move, or remove it, as needed.
            this.roomTableAdapter.Fill(this.qLKHACHSANDataSet3.Room);
            // TODO: This line of code loads data into the 'qLKHACHSANDataSet1.Customer' table. You can move, or remove it, as needed.
            this.customerTableAdapter.Fill(this.qLKHACHSANDataSet1.Customer);
            // TODO: This line of code loads data into the 'qLKHACHSANDataSet.Staff' table. You can move, or remove it, as needed.
            this.staffTableAdapter.Fill(this.qLKHACHSANDataSet.Staff);

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbDatetimeNow.Text = DateTime.Now.ToString("HH:mm:ss") + "   |   Ngày: " + DateTime.Now.ToString("dd-MM-yyyy");
        }

        // sử lí room

        void AddBindingRoom()
        {
            txbRID.DataBindings.Add(new Binding("Text", dgvRoom.DataSource, "roomid"));
            txbRName.DataBindings.Add(new Binding("Text", dgvRoom.DataSource, "roomnumber"));
            txbRPrice.DataBindings.Add(new Binding("Text", dgvRoom.DataSource, "roomrate"));
            cbRType.DataBindings.Add(new Binding("Text", dgvRoom.DataSource, "roomtype"));
            cbRStatus.DataBindings.Add(new Binding("Text", dgvRoom.DataSource, "roomstatus"));
        }

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
                LoadData();
                MessageBox.Show("Thêm phòng [" + txbRName.Text + "] thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFieldRoom();

            }

        }

        private void btnRClear_Click(object sender, EventArgs e)
        {
            ClearFieldRoom();
        }

        private void btnREdit_Click(object sender, EventArgs e)
        {
            string roomNumber = txbRName.Text.Trim();
            bool roomNumberExists = db.Room.Any(r => r.roomnumber == roomNumber);

            if (roomNumberExists)
            {
                MessageBox.Show("Số phòng đã tồn tại. Vui lòng chọn số phòng khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbRName.Focus();
                return;
            }
            else
            {

                int id = Convert.ToInt32(txbRID.Text);

                Room room = db.Room.Find(id);
                room.roomnumber = txbRName.Text;
                room.roomtype = cbRType.Text;
                room.roomrate = decimal.Parse(txbRPrice.Text);
                room.roomstatus = cbRStatus.Text;

                db.SaveChanges();
                LoadData();
                MessageBox.Show("Sửa phòng [" + txbRName.Text + "] thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFieldRoom();
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
                this.tabPage4.Controls.Add(clonedGroupBox);

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
    }
}
