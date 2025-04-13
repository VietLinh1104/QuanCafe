using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using QuanCafe.Models;
using System.Security.Cryptography;
using System.Text;
using QuanCafe.Data;
using System.Data;
using System.Windows.Forms;
using QuanCafe.Helpers;

namespace QuanCafe.Services
{
    public class NhanVienService
    {
        private readonly ConnectDB db = new ConnectDB();

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public bool Register(NhanVien nv)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                string query = @"INSERT INTO NhanVien (ten_nhan_vien, chuc_vu, so_dien_thoai, email, password)
                                 VALUES (@Ten, @ChucVu, @SDT, @Email, @Password)";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Ten", nv.TenNhanVien);
                cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
                cmd.Parameters.AddWithValue("@SDT", nv.SoDienThoai);
                cmd.Parameters.AddWithValue("@Email", nv.Email);
                cmd.Parameters.AddWithValue("@Password", HashPassword(nv.Password)); // Mã hóa mật khẩu

                conn.Open();
                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Lỗi đăng ký: " + ex.Message);
                    return false;
                }
            }
        }

        public string Login(string soDienThoai, string password)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                string query = @"SELECT * FROM NhanVien WHERE so_dien_thoai = @SoDienThoai AND Password = @Password";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                cmd.Parameters.AddWithValue("@Password", HashPassword(password)); // Mã hóa mật khẩu

                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string ten = reader["so_dien_thoai"].ToString();
                    string role = reader["chuc_vu"].ToString();
                    return Helpers.JwtHelper.GenerateToken(ten, role); // Tạo token
                }
                else
                {
                    return null; // Đăng nhập thất bại
                }
            }
        }

        public bool UpdateWithoutPassword(NhanVien nv)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                string query = @"UPDATE NhanVien 
                         SET ten_nhan_vien = @Ten,
                             chuc_vu = @ChucVu,
                             so_dien_thoai = @SoDienThoai,
                             email = @Email
                         WHERE id_nhan_vien = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Ten", nv.TenNhanVien);
                    cmd.Parameters.AddWithValue("@ChucVu", nv.ChucVu);
                    cmd.Parameters.AddWithValue("@SoDienThoai", nv.SoDienThoai);
                    cmd.Parameters.AddWithValue("@Email", nv.Email);
                    cmd.Parameters.AddWithValue("@Id", nv.IdNhanVien);

                    conn.Open();
                    try
                    {
                        return cmd.ExecuteNonQuery() > 0;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Lỗi cập nhật: " + ex.Message);
                        return false;
                    }
                }
            }
        }


        public NhanVien GetBySoDienThoai(string soDienThoai)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                string query = "SELECT * FROM NhanVien WHERE so_dien_thoai = @SoDienThoai";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);

                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new NhanVien
                    {
                        IdNhanVien = (int)reader["IdNhanVien"],
                        TenNhanVien = reader["TenNhanVien"].ToString(),
                        ChucVu = reader["ChucVu"].ToString(),
                        SoDienThoai = reader["SoDienThoai"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString() // Lấy mật khẩu từ DB
                    };
                }
                return null;
            }
        }

        public NhanVien GetByEmail(string email)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                string query = "SELECT * FROM NhanVien WHERE Email = @Email";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new NhanVien
                    {
                        IdNhanVien = (int)reader["IdNhanVien"],
                        TenNhanVien = reader["TenNhanVien"].ToString(),
                        ChucVu = reader["ChucVu"].ToString(),
                        SoDienThoai = reader["SoDienThoai"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString() // Lấy mật khẩu từ DB
                    };
                }
                return null;
            }
        }

        public DataTable GetAll()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                string query = "SELECT * FROM NhanVien";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader); // Đổ dữ liệu từ reader vào DataTable
                    }
                }
            }
            return dt;
        }

        public bool IsEmailExists(string email)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM NhanVien WHERE email = @Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool IsPhoneExists(string soDienThoai)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM NhanVien WHERE so_dien_thoai = @SoDienThoai";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public DataTable SearchNhanVien(string keyword)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = db.GetConnection())
            {
                // Query tìm kiếm theo tên, số điện thoại hoặc email
                string query = @"
            SELECT * FROM NhanVien
            WHERE ten_nhan_vien LIKE @Keyword
            OR so_dien_thoai LIKE @Keyword
            OR email LIKE @Keyword";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");  // Sử dụng ký tự % để tìm kiếm theo phần của chuỗi

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader); // Đổ dữ liệu từ reader vào DataTable
                    }
                }
            }
            return dt;
        }


        public (bool emailExists, bool phoneExists) CheckEmailAndPhone(string email, string soDienThoai)
        {
            bool emailExists = false;
            bool phoneExists = false;

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                // Kiểm tra email
                string emailQuery = "SELECT COUNT(*) FROM NhanVien WHERE email = @Email";
                using (SqlCommand cmd = new SqlCommand(emailQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    emailExists = (int)cmd.ExecuteScalar() > 0;
                }

                // Kiểm tra số điện thoại
                string phoneQuery = "SELECT COUNT(*) FROM NhanVien WHERE so_dien_thoai = @SoDienThoai";
                using (SqlCommand cmd = new SqlCommand(phoneQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@SoDienThoai", soDienThoai);
                    phoneExists = (int)cmd.ExecuteScalar() > 0;
                }
            }

            return (emailExists, phoneExists);
        }




        public bool DeleteById(int id)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                // Kiểm tra xem có hóa đơn nào liên quan đến nhân viên này không
                string checkQuery = "SELECT COUNT(*) FROM HoaDon WHERE id_nhan_vien = @Id";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                int count = (int)checkCmd.ExecuteScalar();  // Số lượng hóa đơn liên quan

                if (count > 0)
                {
                    MessageBox.Show("Không thể xóa nhân viên này vì có hóa đơn liên quan.");
                    return false;  // Không xóa được nếu có hóa đơn
                }

                // Nếu không có hóa đơn, tiếp tục xóa nhân viên
                string deleteQuery = "DELETE FROM NhanVien WHERE id_nhan_vien = @Id";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn);
                deleteCmd.Parameters.AddWithValue("@Id", id);

                return deleteCmd.ExecuteNonQuery() > 0;  // Thực hiện xóa
            }
        }



    }
}
