using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanCafe.Data;
using System.Windows.Forms;
using QuanCafe.Models;

namespace QuanCafe.Services
{
    public class SanPhamService
    {
        private readonly ConnectDB _db;

        public SanPhamService()
        {
            _db = new ConnectDB();
        }

        public DataTable GetAllSanPham()
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                // Đảm bảo JOIN đúng bảng DanhMucSanPham
                string query = @"
                    SELECT 
                        SanPham.id_san_pham, 
                        SanPham.ten_san_pham, 
                        SanPham.gia, 
                        DanhMucSanPham.ten_danh_muc, 
                        SanPham.mo_ta 
                    FROM SanPham 
                    INNER JOIN DanhMucSanPham 
                        ON SanPham.id_danh_muc = DanhMucSanPham.id_danh_muc";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Thêm sản phẩm
        public bool ThemSanPham(string tenSanPham, decimal gia, int danhMucId, string moTa)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"
                    INSERT INTO SanPham (ten_san_pham, gia, id_danh_muc, mo_ta)
                    VALUES (@ten, @gia, @danhMucId, @moTa)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ten", tenSanPham);
                cmd.Parameters.AddWithValue("@gia", gia);
                cmd.Parameters.AddWithValue("@danhMucId", danhMucId);
                cmd.Parameters.AddWithValue("@moTa", moTa ?? (object)DBNull.Value);

                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex.Message);
                    return false;
                }
            }
        }

        // Sửa sản phẩm
        public bool SuaSanPham(int id, string tenSanPham, decimal gia, int danhMucId, string moTa)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"
                    UPDATE SanPham 
                    SET ten_san_pham = @ten, 
                        gia = @gia, 
                        id_danh_muc = @danhMucId, 
                        mo_ta = @moTa 
                    WHERE id_san_pham = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ten", tenSanPham);
                cmd.Parameters.AddWithValue("@gia", gia);
                cmd.Parameters.AddWithValue("@danhMucId", danhMucId);
                cmd.Parameters.AddWithValue("@moTa", moTa ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi sửa sản phẩm: " + ex.Message);
                    return false;
                }
            }
        }

        // Xóa sản phẩm
        public bool XoaSanPham(int id)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "DELETE FROM SanPham WHERE id_san_pham = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    return result > 0; // Trả về true nếu xóa thành công ít nhất 1 dòng
                }
                catch (SqlException ex)
                {
                    // Hiển thị thông báo lỗi cụ thể từ CSDL
                    MessageBox.Show("Lỗi CSDL: " + ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                    return false;
                }
            }
        }

        public DataTable TimKiemSanPham(string keyword, decimal? giaTu = null, decimal? giaDen = null, int? danhMucId = null)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                StringBuilder query = new StringBuilder(@"
                    SELECT 
                        SanPham.id_san_pham, 
                        SanPham.ten_san_pham, 
                        SanPham.gia, 
                        DanhMucSanPham.ten_danh_muc, 
                        SanPham.mo_ta 
                    FROM SanPham 
                    INNER JOIN DanhMucSanPham 
                        ON SanPham.id_danh_muc = DanhMucSanPham.id_danh_muc 
                    WHERE 1=1");

                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(keyword))
                {
                    query.Append(" AND SanPham.ten_san_pham LIKE @keyword"); // Đã sửa lỗi chính tả
                    parameters.Add(new SqlParameter("@keyword", $"%{keyword}%"));
                }

                if (giaTu.HasValue)
                {
                    query.Append(" AND SanPham.gia >= @giaTu");
                    parameters.Add(new SqlParameter("@giaTu", giaTu.Value));
                }

                if (giaDen.HasValue)
                {
                    query.Append(" AND SanPham.gia <= @giaDen");
                    parameters.Add(new SqlParameter("@giaDen", giaDen.Value));
                }

                if (danhMucId.HasValue && danhMucId.Value > 0)
                {
                    query.Append(" AND SanPham.id_danh_muc = @danhMucId");
                    parameters.Add(new SqlParameter("@danhMucId", danhMucId.Value));
                }

                SqlDataAdapter adapter = new SqlDataAdapter(query.ToString(), conn);
                adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        // Lấy sản phẩm theo ID
        public SanPham GetSanPhamById(int idSanPham)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"
            SELECT 
                *
            FROM SanPham 
            WHERE SanPham.id_san_pham = @idSanPham";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idSanPham", idSanPham);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Lấy dữ liệu và tạo đối tượng SanPham
                        return new SanPham
                        {
                            IdSanPham = (int)reader["id_san_pham"],
                            TenSanPham = reader["ten_san_pham"].ToString(),
                            Gia = (double)reader["gia"],
                            IdDanhMuc = (int)reader["id_danh_muc"],
                            MoTa = reader["mo_ta"] as string
                        };
                    }
                    else
                    {
                        return null; // Nếu không tìm thấy sản phẩm
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy sản phẩm: " + ex.Message);
                    return null;
                }
            }
        }





        public DataTable GetSanPhamByDanhMuc(int danhMucId)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"
                    SELECT 
                        sp.id_san_pham, 
                        sp.ten_san_pham, 
                        sp.gia, 
                        dm.ten_danh_muc, 
                        sp.mo_ta 
                    FROM SanPham sp 
                    JOIN DanhMucSanPham dm ON sp.id_danh_muc = dm.id_danh_muc 
                    WHERE sp.id_danh_muc = @DanhMucId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DanhMucId", danhMucId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public DataTable GetAllSanPhamPaged(int pageNumber, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"
                    DECLARE @TotalRows INT;
            
                    SELECT @TotalRows = COUNT(*) 
                    FROM SanPham 
                    INNER JOIN DanhMucSanPham 
                        ON SanPham.id_danh_muc = DanhMucSanPham.id_danh_muc;

                    SELECT 
                        SanPham.id_san_pham, 
                        SanPham.ten_san_pham, 
                        SanPham.gia, 
                        DanhMucSanPham.ten_danh_muc, 
                        SanPham.mo_ta 
                    FROM SanPham 
                    INNER JOIN DanhMucSanPham 
                        ON SanPham.id_danh_muc = DanhMucSanPham.id_danh_muc
                    ORDER BY SanPham.id_san_pham
                    OFFSET (@PageNumber - 1) * @PageSize ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT @TotalRows AS TotalRows;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count >= 2)
                {
                    totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRows"]);
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }

        public DataTable TimKiemSanPhamPaged(string keyword, decimal? giaTu, decimal? giaDen, int? danhMucId, int pageNumber, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            using (SqlConnection conn = _db.GetConnection())
            {
                StringBuilder query = new StringBuilder(@"
                    DECLARE @TotalRows INT;
        
                    -- Phần đếm tổng số bản ghi
                    SELECT @TotalRows = COUNT(*) 
                    FROM SanPham 
                    INNER JOIN DanhMucSanPham 
                        ON SanPham.id_danh_muc = DanhMucSanPham.id_danh_muc 
                    WHERE 1=1");

                List<SqlParameter> parameters = new List<SqlParameter>();

                // Thêm điều kiện tìm kiếm
                if (!string.IsNullOrEmpty(keyword))
                {
                    query.Append(" AND SanPham.ten_san_pham LIKE @keyword");
                    parameters.Add(new SqlParameter("@keyword", $"%{keyword}%"));
                }

                if (giaTu.HasValue)
                {
                    query.Append(" AND SanPham.gia >= @giaTu");
                    parameters.Add(new SqlParameter("@giaTu", giaTu.Value));
                }

                if (giaDen.HasValue)
                {
                    query.Append(" AND SanPham.gia <= @giaDen");
                    parameters.Add(new SqlParameter("@giaDen", giaDen.Value));
                }

                if (danhMucId.HasValue && danhMucId.Value > 0)
                {
                    query.Append(" AND SanPham.id_danh_muc = @danhMucId");
                    parameters.Add(new SqlParameter("@danhMucId", danhMucId.Value));
                }

                // Phần lấy dữ liệu phân trang
                query.Append(@";
                    -- Phần lấy dữ liệu
                    SELECT 
                        SanPham.id_san_pham, 
                        SanPham.ten_san_pham, 
                        SanPham.gia, 
                        DanhMucSanPham.ten_danh_muc, 
                        SanPham.mo_ta 
                    FROM SanPham 
                    INNER JOIN DanhMucSanPham 
                        ON SanPham.id_danh_muc = DanhMucSanPham.id_danh_muc 
                    WHERE 1=1");

                // Thêm LẠI các điều kiện (cần dùng lại cùng parameters)
                if (!string.IsNullOrEmpty(keyword))
                    query.Append(" AND SanPham.ten_san_pham LIKE @keyword");

                if (giaTu.HasValue)
                    query.Append(" AND SanPham.gia >= @giaTu");

                if (giaDen.HasValue)
                    query.Append(" AND SanPham.gia <= @giaDen");

                if (danhMucId.HasValue && danhMucId.Value > 0)
                    query.Append(" AND SanPham.id_danh_muc = @danhMucId");

                query.Append(@"
                    ORDER BY SanPham.id_san_pham
                    OFFSET (@PageNumber - 1) * @PageSize ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT @TotalRows AS TotalRows;");

                // Thêm parameters phân trang
                parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                parameters.Add(new SqlParameter("@PageSize", pageSize));

                SqlCommand cmd = new SqlCommand(query.ToString(), conn);
                cmd.Parameters.AddRange(parameters.ToArray());

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count > 0)
                {
                    totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRows"]);
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }

        public DataTable GetSanPhamByDanhMucPaged(int danhMucId, int pageNumber, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"
                    DECLARE @TotalRows INT;
            
                    SELECT @TotalRows = COUNT(*) 
                    FROM SanPham 
                    WHERE id_danh_muc = @DanhMucId;

                    SELECT 
                        SanPham.id_san_pham, 
                        SanPham.ten_san_pham, 
                        SanPham.gia, 
                        DanhMucSanPham.ten_danh_muc, 
                        SanPham.mo_ta 
                    FROM SanPham 
                    INNER JOIN DanhMucSanPham 
                        ON SanPham.id_danh_muc = DanhMucSanPham.id_danh_muc
                    WHERE SanPham.id_danh_muc = @DanhMucId
                    ORDER BY SanPham.id_san_pham
                    OFFSET (@PageNumber - 1) * @PageSize ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    SELECT @TotalRows AS TotalRows;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DanhMucId", danhMucId);
                cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                if (ds.Tables.Count >= 2)
                {
                    totalRecords = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRows"]);
                    return ds.Tables[0];
                }
                return new DataTable();
            }
        }
    }
}
