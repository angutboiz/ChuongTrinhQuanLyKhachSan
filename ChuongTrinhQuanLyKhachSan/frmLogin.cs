using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChuongTrinhQuanLyKhachSan
{
    
    public partial class frmLogin : Form
    {
        QLKHACHSANEntities db = new QLKHACHSANEntities();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmHome frmHome = new frmHome();
            string username = txbUser.Text.Trim();
            string password = txbPass.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.");
                return;
            }
            else
            {
                try
                {
                    var user = db.Staff.FirstOrDefault(u => u.Username == username);

                    if (user != null)
                    {
                        frmHome.name = username;
                        frmHome.role = user.Role;
                        frmHome.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Sai mật khẩu hoặc tên đăng nhập", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
