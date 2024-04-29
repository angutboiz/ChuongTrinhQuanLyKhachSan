using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChuongTrinhQuanLyKhachSan
{
    public class Funct
    {
        public static string RemoveDot(string number)
        {
            return number.Replace(",", "");
        }


        public static void FormatNumberWithCommas(Guna2TextBox txb)
        {
            // Lưu vị trí con trỏ hiện tại
            int currentCursorPosition = txb.SelectionStart;

            // Xóa dấu phẩy và chuyển về chuỗi không có dấu phẩy
            string originalText = txb.Text.Replace(",", "");

            // Kiểm tra và thêm dấu phẩy sau mỗi 3 số
            int length = originalText.Length;
            if (length > 3)
            {
                int count = 0;
                StringBuilder formattedText = new StringBuilder();

                for (int i = length - 1; i >= 0; i--)
                {
                    formattedText.Insert(0, originalText[i]);
                    count++;

                    if (count == 3 && i > 0)
                    {
                        formattedText.Insert(0, ",");
                        count = 0; // Đặt lại count sau khi thêm dấu phẩy
                    }
                }

                txb.Text = formattedText.ToString();
            }

            // Đặt lại vị trí con trỏ
            txb.SelectionStart = currentCursorPosition + (txb.Text.Length - originalText.Length);
        }
    }
}
