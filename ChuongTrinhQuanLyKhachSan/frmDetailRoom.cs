using System;
using System.Data;
using System.Drawing;
using System.Linq;

using System.Windows.Forms;

namespace ChuongTrinhQuanLyKhachSan
{
    public partial class frmDetailRoom : Form
    {
        Room room;
        private Color roomColor;
        public int idphong;
        string booktype = "";

        int tongtien = 0;
        int soluong = 1;
        QLKHACHSANEntities db = new QLKHACHSANEntities();

        Color green = ColorTranslator.FromHtml("#15803d");
        Color red = ColorTranslator.FromHtml("#b91c1c");
        Color gray = ColorTranslator.FromHtml("#374151");

        Color zinc = ColorTranslator.FromHtml("#4b5563");
        Color orange = ColorTranslator.FromHtml("#dc2626");
        Color emeral = ColorTranslator.FromHtml("#16a34a");

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
            DefaultField();

            //kiếm xem id room có trong booking chưa, nếu có thì hiển thị data staff và customer lên load form
            var booking = db.Booking.FirstOrDefault(b => b.roomid == room.roomid );
            if (booking != null && booking.bookstatus == "Đang thuê phòng")
            {
                var staffid = db.Staff.Find(booking.staffid);
                var custom = db.Customer.Find(booking.cusid);

                if (staffid != null && custom != null)
                {
                    cbNV.Text = staffid.staffname.ToString();
                    cbKH.Text = custom.cusname.ToString();
                    txbStart.Text = booking.checkin.ToString();
                    btnServiceAdd.Enabled = true;
                    timer1.Start();
                    TinhThoiGian();
                    btnServiceAdd.FillColor = green;
                    btnDatPhong.FillColor = green;

                    grbNV.CustomBorderColor = emeral;
                    grbKH.CustomBorderColor = emeral;
                    grbService.CustomBorderColor = emeral;
                    grbDetail.CustomBorderColor = emeral;
                    grbService.Visible = true;

                    var totalall = db.ServiceOrder.Where(o => o.roomid == room.roomid).Sum(o => o.sertotal);

                    lbTongTatCaTien.Text = string.Format("Tổng tất cả tiền: : {0:#,##0}đ", tongtien + totalall ?? 0);

                  
                }

            }
            else
            {
                ClearData();
                btnDatPhong.Enabled = true;
                btnThanhToan.Enabled = false;
            }
        }

        void DefaultField()
        {
            soluong = 1;
            grpRoom.Text = "Phòng " + room.roomnumber;
            grpRoom.BorderColor = roomColor;
            grbService.Visible = false;
            btnClearData.Enabled = false;
            grbNV.CustomBorderColor = zinc;
            grbKH.CustomBorderColor = zinc;
            grbService.CustomBorderColor = zinc;
            grbDetail.CustomBorderColor = zinc;

            grpRoom.ForeColor = Color.White;
            grpRoom.CustomBorderColor = roomColor;
            btnDatPhong.FillColor = gray;
            btnClose.FillColor = gray;

            LoadDataService();
            btnServiceAdd.Enabled = false;

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

                btnThanhToan.FillColor = green;
                btnClose.FillColor = green;
                grbNV.CustomBorderColor = emeral;
                grbKH.CustomBorderColor = emeral;
                grbService.CustomBorderColor = emeral;
                grbDetail.CustomBorderColor = emeral;


            }
            else if (room.roomstatus == "Bảo trì")
            {
                btnDatPhong.Enabled = false;
                btnThanhToan.Enabled = false;
                btnClearData.Enabled = true;

                btnClearData.FillColor = red;
                btnClose.FillColor = red;

                grbNV.CustomBorderColor = orange;
                grbKH.CustomBorderColor = orange;
                grbService.CustomBorderColor = orange;
                grbDetail.CustomBorderColor = orange;

            }

            if (room.roomtype == "Superior")
            {
                txbGiaTheoGio.Text = "200 000";
                lbGia.Text = "Thuê theo 2 giờ";

            }
            else if (room.roomtype == "Deluxe Twin Bed")
            {
                txbGiaTheoGio.Text = "250 000";
                lbGia.Text = "Thuê theo 2 giờ";
            }
            else if (room.roomtype == "Deluxe King Bed")
            {
                txbGiaTheoGio.Text = "300 000";
                lbGia.Text = "Thuê theo 2 giờ";
            }
            else if (room.roomtype == "VIP")
            {
                txbGiaTheoGio.Text = "300 000";
                lbGia.Text = "Thuê theo 2 giờ";
            }
            booktype = "Thuê theo giờ";
        }

        private DateTime startTime;
        private void timer1_Tick(object sender, EventArgs e)
        {
            txbEnd.Text = DateTime.Now.ToString();
        }

        private void btnDatPhong_Click(object sender, EventArgs e)
        {
            if (cbKH.SelectedIndex == -1 || cbNV.SelectedIndex == -1)
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
                    checkin = startTime,
                    bookstatus = "Đang thuê phòng",
                    booktype = booktype
                });

                db.SaveChanges();

                ClearData();
                ClearData();

                btnThanhToan.Enabled = true;
                btnDatPhong.Enabled = false;
                btnServiceAdd.Enabled = true;
                grpRoom.BorderColor = green;
                grpRoom.CustomBorderColor = green;

               Application.Restart();
            }




        }

        
        private void btnFrmService_Click(object sender, EventArgs e)
        {
            int quantity = int.Parse(txbServiceSoluong.Text);
            string name = cbService.Text;
            var ser = db.Service.SingleOrDefault(s => s.sername == name);
            decimal total = quantity * ser.serprice;


            db.ServiceOrder.Add(new ServiceOrder()
            {
                serid = ser.serid,
                roomid = room.roomid,
                serquantity = quantity,
                sertotal = total
            });
            db.SaveChanges();
            LoadDataService();

            txbServiceSoluong.Text = "1";
            soluong = 1;
            TinhThoiGian();
            var totalall = db.ServiceOrder.Where(o => o.roomid == room.roomid).Sum(o => o.sertotal);
            decimal totall = tongtien + totalall ?? 0;
            lbTongTatCaTien.Text = string.Format("Tổng tất cả tiền: : {0:#,##0}đ", totall);
        }

        private void btnGiam_Click(object sender, EventArgs e)
        {
            if (soluong == 1)
            {
                soluong = 1;
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


        void TinhThoiGian()
        {
            DateTime endTime = DateTime.Now;
            txbEnd.Text = endTime.ToString();
            DateTime startTime = DateTime.Parse(txbStart.Text);

            // Tính toán tổng thời gian giữa startTime và endTime
            TimeSpan elapsedTime = endTime - startTime;
            // Hiển thị tổng thời gian trong định dạng phù hợp
            double totalHours = elapsedTime.TotalHours;

            txbTongTime.Text = Math.Round(totalHours, 0).ToString();

            int giatheogio = int.Parse(txbGiaTheoGio.Text.Replace(" ",""));

            //2 tiếng đàu không tính nên 2
            int tongthoigian = int.Parse(txbTongTime.Text) - 2;
            int phi = int.Parse(txbPhi.Text.Replace(" ", ""));

            if (tongthoigian < 0)
            {
                tongtien = giatheogio;

            } else
            {
                tongtien = giatheogio + (tongthoigian * phi);
            }


            lbTongTien.Text = string.Format("Tổng tiền: {0:#,##0}đ", tongtien);

            var totalall = db.ServiceOrder.Where(o => o.roomid == room.roomid).Sum(o => o.sertotal);
            decimal totall = tongtien + totalall ?? 0;
            lbTongTatCaTien.Text = string.Format("Tổng tất cả tiền: : {0:#,##0}đ", totall);

        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            TinhThoiGian();

            var bk = db.Booking.FirstOrDefault(b => b.roomid == room.roomid);

            lbTongTien.Text = string.Format("Tổng tiền: {0:#,##0}đ", tongtien);
            var endTime = DateTime.Parse(txbEnd.Text);
            bk.checkout = endTime;

            var totalall = db.ServiceOrder.Where(o => o.roomid == room.roomid).Sum(o => o.sertotal);
            decimal totall = 0;
            if (totalall != null)
            {
                totall = tongtien + totalall ?? 0;
            }
            else
            {
                totall = tongtien;

            }
            lbTongTatCaTien.Text = string.Format("Tổng tất cả tiền: : {0:#,##0}đ", totall);

            if (totall > 0)
            {
                bk.payamount = totall;
                bk.totalhours = int.Parse(txbTongTime.Text);

                Room r = db.Room.Find(room.roomid);
                r.roomstatus = "Bảo trì";

                int staffid = int.Parse(txbNVID.Text);
                var staff = db.Staff.SingleOrDefault(s => s.staffid == staffid);

                int cusid = int.Parse(txbKHID.Text);
                var cus = db.Customer.SingleOrDefault(c => c.cusid == cusid);

                db.History.Add(new History()
                {
                    roomname = room.roomnumber,
                    roomtype = room.roomtype,
                    payamount = tongtien,
                    staffname = staff.staffname,
                    cusname = cus.cusname,
                    cusphone = cus.cusphone,
                    checkin = bk.checkin,
                    checkout = endTime,
                    totalhours = bk.totalhours,
                    totalamount = totall
                });
                db.SaveChanges();


                var book = db.Booking.SingleOrDefault(b => b.roomid == room.roomid);
                var history = db.History.SingleOrDefault(h => h.roomname == room.roomnumber && h.checkin == book.checkin);
                var serviceOrders = db.ServiceOrder.Where(so => so.roomid == room.roomid).ToList();
                if (serviceOrders.ToList().Count() > 0)
                {
                    var checkout = history.checkin;
                    foreach (var item in serviceOrders)
                    {
                        var name = db.Service.SingleOrDefault(n => n.serid == item.serid);
                        db.HistoryService.Add(new HistoryService()
                        {
                            historyID = history.ID,
                            sername = name.sername,
                            serquantity = item.serquantity,
                            sertotal = item.sertotal,
                            seramount = totalall
                        });
                    }
                }
                else
                {


                    db.HistoryService.Add(new HistoryService()
                    {
                        historyID = history.ID,
                        sername = null,
                        serquantity = null,
                        sertotal = null,
                        seramount = 0
                    });
                }


                MessageBox.Show("Thanh toán thành công phòng: " + room.roomnumber + "\nSố tiền: " + string.Format("{0:#,##0}đ", totall), "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);



                if (serviceOrders.Any())
                {
                    db.ServiceOrder.RemoveRange(serviceOrders);
                }

                var handle = db.Booking.SingleOrDefault(s => s.roomid == room.roomid);
                db.Booking.Remove(handle);

                db.SaveChanges();
                Application.Restart();
            }
            else
            {
                MessageBox.Show("Chưa tính được tổng tiền, vui lòng thử lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        void LoadDataService()
        {
            var service = from so in db.ServiceOrder
                          where so.roomid == room.roomid
                           join s in db.Service on so.serid equals s.serid
                           select new
                           {
                               so.serdetailid,
                               s.sername,
                               so.serquantity,
                               so.sertotal
                           };



            dgvService.DataSource = service.ToList();
            var totalall = db.ServiceOrder.Where(o=> o.roomid == room.roomid).Sum(o => o.sertotal );
            lbTongService.Text = string.Format("{0:#,##0}đ", totalall ?? 0) ;
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
            Application.Restart();
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
            if ( chkTuChinh.Checked)
            {
                txbStart.Text = startDate.ToString();
                txbStart.Enabled = true;
                txbEnd.Enabled = true;
                txbEnd.Text = endDate.ToString();
            }
            else
            {
                txbStart.Enabled = false;
                txbEnd.Enabled = false;

                txbStart.Text = "";
                txbEnd.Text = "";
            }
        }

        private void rbMan_CheckedChanged(object sender, EventArgs e)
        {
            var ser = from Service in db.Service
                           where Service.sertype == "Đồ ăn mặn"
                           select Service.sername;

            if (rbMan.Checked)
            {
                cbService.DataSource = ser.ToList();
            }
        }

        private void rbSnack_CheckedChanged(object sender, EventArgs e)
        {
            var ser = from Service in db.Service
                      where Service.sertype == "Đồ ăn nhanh"
                      select Service.sername;

            if (rbSnack.Checked)
            {
                cbService.DataSource = ser.ToList();
            }
        }

        private void rbNuoc_CheckedChanged(object sender, EventArgs e)
        {
            var ser = from Service in db.Service
                      where Service.sertype == "Nước"
                      select Service.sername;

            if (rbNuoc.Checked)
            {
                cbService.DataSource = ser.ToList();
            }
        }

        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now;

        private void dgvService_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DialogResult res = MessageBox.Show("Bạn có chắc muốn xóa dịch vụ này không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                int id = Convert.ToInt32(lbSerID.Text);
                var query = db.ServiceOrder.Find(id);
                db.ServiceOrder.Remove(query);

                db.SaveChanges();
                LoadDataService();
            }
        }

        private void dgvService_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            lbSerID.Text = dgvService.SelectedRows[0].Cells[0].Value.ToString();


        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Courier New", 10);
            float fontHeight = font.GetHeight();
            int startX = 50;
            int startY = 55;
            int Offset = 40;
            graphics.DrawString("Welcome to MSST", new Font("Courier New", 14),
                                new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("Ticket No:" ,
                     new Font("Courier New", 14),
                     new SolidBrush(Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
            graphics.DrawString("Ticket Date :" ,
                     new Font("Courier New", 12),
                     new SolidBrush(Color.Black), startX, startY + Offset);
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void txbStart_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
