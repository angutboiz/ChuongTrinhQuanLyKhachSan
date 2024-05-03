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

        public static string VietHoaCacKyTu(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string[] words = input.Split(' '); // Tách chuỗi thành mảng các từ dựa trên dấu cách

                for (int i = 0; i < words.Length; i++)
                {
                    // Chuyển đổi chữ cái đầu tiên của từ thành chữ in hoa
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
                }

                // Ghép lại các từ thành chuỗi mới
                return string.Join(" ", words);
            }
            else
            {
                // Trả về chuỗi rỗng nếu đầu vào là null hoặc rỗng
                return string.Empty;
            }
        }
        public static string VietHoaMotKyTu(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return char.ToUpper(input[0]) + input.Substring(1);
            } else
            {
                return string.Empty;
            }
        }


    }
}
