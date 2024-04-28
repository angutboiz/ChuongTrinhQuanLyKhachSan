using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ChuongTrinhQuanLyKhachSan.All_User_Control
{
    public partial class UC_CustomerRes : UserControl
    {
        function fn = new function();
        String query;
        public UC_CustomerRes()
        {
            InitializeComponent();
        }

        public void setComboBox(String query, ComboBox combo)
        {
            SqlDataReader sdr = fn.getForCombo(query);
            while (sdr.Read())
            {
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    combo.Items.Add(sdr.GetString(i));
                }
            }
            sdr.Close();
        }

        private void txtRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRoomNo.Items.Clear();
            query = "select roomNo from rooms where bed = '" + txtBed.Text + "' and roomType = '" + txtRoom.Text + "' and booked = 'NO'";
            setComboBox(query, txtRoomNo);
        }

        private void txtBed_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRoom.SelectedIndex = -1;
            txtRoomNo.Items.Clear();
            txtPrice.Clear();
        }

        int rid;
        private void txtRoomNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            query = "select price, roomid from rooms where roomNo = '" + txtRoomNo.Text + "'";
            DataSet ds = fn.getData(query);
            txtPrice.Text = ds.Tables[0].Rows[0][0].ToString();
            rid = int.Parse(ds.Tables[0].Rows[0][1].ToString());
        }

        private void btnAllotCustomer_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtContact.Text != "" && txtNationality.Text != "" && txtGender.Text != "" && txtDob.Text != "" && txtIDProof.Text != "" && txtAddress.Text != "" && txtCheckin.Text != "" && txtPrice.Text != "")
            {
                String name = txtName.Text;
                Int64 mobile = Int64.Parse(txtContact.Text);
                String national = txtNationality.Text;
                String gender = txtGender.Text;
                String dob = txtDob.Text;
                String idproof = txtIDProof.Text;
                String address = txtAddress.Text;
                String checkin = txtCheckin.Text;

                query = "insert into customer (cname, mobile, nationality, gender, dob, idproof, address, checkin, roomid) values ('" + name + "'," + mobile + ", '" + national + "','" + gender + "','" + dob + "','" + idproof  + "','" + address + "','" + checkin + "'," + rid + ") update rooms set booked = 'YES' where roomNo = '" + txtRoomNo.Text + "'";
                fn.setData(query, " Số Phòng " + txtRoomNo.Text + "Đăng ký khách hàng thành công.");
                clearAll();
            }  else
            {
                MessageBox.Show("Xin vui lòng nhập đầy đủ thông tin.", "Thông Tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void clearAll()
        {
            txtName.Clear();
            txtContact.Clear();
            txtNationality.Clear();
            txtGender.SelectedIndex = -1;
            txtDob.ResetText();
            txtIDProof.Clear();
            txtAddress.Clear();
            txtCheckin.ResetText();
            txtBed.SelectedIndex = -1;
            txtRoom.SelectedIndex = -1;
            txtRoomNo.Items.Clear();
            txtPrice.Clear();
        }
        private void UC_CustomerRes_Load(object sender, EventArgs e)
        {

        }

        private void UC_CustomerRes_Leave(object sender, EventArgs e)
        {
            clearAll();
        }
    }
}
