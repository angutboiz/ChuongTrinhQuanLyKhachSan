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
        }
    }
}
