﻿-- Create database
CREATE DATABASE QuanCafe;
GO

USE QuanCafe;
GO

-- Bảng Danh mục sản phẩm
CREATE TABLE DanhMucSanPham (
    id_danh_muc INT IDENTITY PRIMARY KEY,
    ten_danh_muc NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
    mo_ta NVARCHAR(MAX)
);

-- Bảng Bàn
CREATE TABLE Ban (
    id_ban INT IDENTITY PRIMARY KEY,
    ten_ban NVARCHAR(100) NOT NULL DEFAULT N'Bàn chưa có tên',
    trang_thai NVARCHAR(100) NOT NULL DEFAULT N'Trống' -- Trống || Có người
);

-- Bảng Nhân viên
CREATE TABLE NhanVien (
    id_nhan_vien INT IDENTITY PRIMARY KEY,
    ten_nhan_vien NVARCHAR(100),
    chuc_vu NVARCHAR(50),
    so_dien_thoai VARCHAR(20),
    email NVARCHAR(100),
	password NVARCHAR(255)
);

-- Bảng Khách hàng
CREATE TABLE KhachHang (
    id_khach_hang INT IDENTITY PRIMARY KEY,
    ten_khach_hang NVARCHAR(100),
    so_dien_thoai VARCHAR(20),
    email NVARCHAR(100),
    ghi_chu NVARCHAR(MAX)
);

-- Bảng Sản phẩm
CREATE TABLE SanPham (
    id_san_pham INT IDENTITY PRIMARY KEY,
    ten_san_pham NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
    gia FLOAT NOT NULL DEFAULT 0,
    id_danh_muc INT NOT NULL,
    mo_ta NVARCHAR(MAX),

    CONSTRAINT FK_SanPham_DanhMuc FOREIGN KEY (id_danh_muc) REFERENCES DanhMucSanPham(id_danh_muc)
);



-- Bảng Hóa đơn
CREATE TABLE HoaDon (
    id_hoa_don INT IDENTITY PRIMARY KEY,
    id_ban INT NOT NULL,
    id_nhan_vien INT NOT NULL,
    id_khach_hang INT,
    thoi_gian DATETIME NOT NULL DEFAULT GETDATE(),
    trang_thai INT NOT NULL DEFAULT 0, -- 1: đã thanh toán || 0: chưa thanh toán
    tong_tien DECIMAL(18,2) DEFAULT 0,

    CONSTRAINT FK_HoaDon_Ban FOREIGN KEY (id_ban) REFERENCES Ban(id_ban),
    CONSTRAINT FK_HoaDon_NhanVien FOREIGN KEY (id_nhan_vien) REFERENCES NhanVien(id_nhan_vien),
    CONSTRAINT FK_HoaDon_KhachHang FOREIGN KEY (id_khach_hang) REFERENCES KhachHang(id_khach_hang)
);

-- Bảng Chi tiết hóa đơn
CREATE TABLE ChiTietHoaDon (
    id_chi_tiet_hoa_don INT IDENTITY PRIMARY KEY,
    id_hoa_don INT NOT NULL,
    id_san_pham INT NOT NULL,
    so_luong INT NOT NULL DEFAULT 0,
    don_gia DECIMAL(18,2) NOT NULL DEFAULT 0,

    CONSTRAINT FK_CTHD_HoaDon FOREIGN KEY (id_hoa_don) REFERENCES HoaDon(id_hoa_don),
    CONSTRAINT FK_CTHD_SanPham FOREIGN KEY (id_san_pham) REFERENCES SanPham(id_san_pham)
);

INSERT INTO DanhMucSanPham (ten_danh_muc, mo_ta) VALUES 
(N'Cà phê', N'Các loại cà phê nóng và đá'),
(N'Trà sữa', N'Trà sữa và topping'),
(N'Nước ép', N'Nước trái cây tươi'),
(N'Bánh ngọt', N'Bánh tráng miệng');


INSERT INTO Ban (ten_ban, trang_thai) VALUES 
(N'Bàn 1', N'Trống'),
(N'Bàn 2', N'Có người'),
(N'Bàn 3', N'Trống'),
(N'Bàn 4', N'Trống');

INSERT INTO NhanVien (ten_nhan_vien, chuc_vu, so_dien_thoai, email) VALUES 
(N'Nguyễn Văn A', N'Quản lý', '0912345678', 'a@quancafe.vn'),
(N'Lê Thị B', N'Nhân viên', '0987654321', 'b@quancafe.vn');

UPDATE NhanVien
SET password = N'123456'
WHERE ten_nhan_vien = N'Nguyễn Văn A';

UPDATE NhanVien
SET password = N'123456'
WHERE ten_nhan_vien = N'Lê Thị B';


INSERT INTO KhachHang (ten_khach_hang, so_dien_thoai, email, ghi_chu) VALUES 
(N'Trần Minh', '0909123456', 'minh@gmail.com', N'Khách quen'),
(N'Hoàng Yến', '0938123456', 'yen@yahoo.com', N'Không thêm đường');

INSERT INTO SanPham (ten_san_pham, gia, id_danh_muc, mo_ta) VALUES 
(N'Cà phê đen', 20000, 1, N'Cà phê phin nguyên chất'),
(N'Cà phê sữa', 25000, 1, N'Cà phê phin + sữa đặc'),
(N'Trà sữa trân châu', 30000, 2, N'Trà sữa truyền thống với trân châu đen'),
(N'Nước ép cam', 35000, 3, N'Nước cam nguyên chất'),
(N'Bánh mousse dâu', 40000, 4, N'Bánh mềm vị dâu');

-- Hóa đơn
INSERT INTO HoaDon (id_ban, id_nhan_vien, id_khach_hang, trang_thai, tong_tien)
VALUES (1, 1, 1, 0, 75000);

-- Chi tiết hóa đơn
INSERT INTO ChiTietHoaDon (id_hoa_don, id_san_pham, so_luong, don_gia) VALUES 
(2, 1, 1, 20000), -- Cà phê đen
(2, 3, 1, 30000), -- Trà sữa
(2, 5, 1, 25000); -- Bánh mousse dâu


SELECT * FROM DanhMucSanPham;
SELECT * FROM Ban;
SELECT * FROM NhanVien;
SELECT * FROM KhachHang;
SELECT * FROM SanPham;
SELECT * FROM DanhMucSanPham;

SELECT * FROM HoaDon;
SELECT * FROM ChiTietHoaDon;

delete from NhanVien where id_nhan_vien = 4;
//----------------------------------------------------------------------------------------------
CREATE PROCEDURE sp_DoanhThuTheoNgay
    @Ngay DATE
AS
BEGIN
    SELECT 
        h.id_hoa_don,
        h.thoi_gian,
        h.tong_tien,
        nv.ten_nhan_vien,
        kh.ten_khach_hang
    FROM HoaDon h
    LEFT JOIN NhanVien nv ON h.id_nhan_vien = nv.id_nhan_vien
    LEFT JOIN KhachHang kh ON h.id_khach_hang = kh.id_khach_hang
    WHERE CONVERT(DATE, h.thoi_gian) = @Ngay AND h.trang_thai = 1;
END

EXEC sp_DoanhThuTheoNgay '2025-04-13';

CREATE VIEW DoanhThuTheoNgay AS
SELECT 
    CONVERT(DATE, thoi_gian) AS Ngay,
    SUM(tong_tien) AS TongDoanhThu,
    COUNT(*) AS SoHoaDon
FROM HoaDon
WHERE trang_thai = 1 -- chỉ tính những hóa đơn đã thanh toán
GROUP BY CONVERT(DATE, thoi_gian);

SELECT * FROM DoanhThuTheoNgay;  
DROP VIEW IF EXISTS DoanhThuTheoNgay;
DROP VIEW IF EXISTS DoanhThuTheoThang;
DROP PROCEDURE IF EXISTS sp_DoanhThuTheoNgay;


SELECT * 
FROM HoaDon
WHERE trang_thai = 1 
  AND thoi_gian BETWEEN '2025-04-13' AND '2025-04-15';

UPDATE HoaDon
SET trang_thai = 1
WHERE id_hoa_don IN (1, 2);


INSERT INTO DanhMucSanPham (ten_danh_muc, mo_ta) VALUES 
(N'Cà phê', N'Các loại cà phê như đen, sữa, espresso'),
(N'Trà sữa', N'Trà sữa các vị, thêm topping trân châu, pudding'),
(N'Đá xay', N'Sinh tố đá xay mát lạnh'),
(N'Nước ép', N'Nước ép trái cây nguyên chất, không đường'),
(N'Sinh tố', N'Sinh tố xoài, dâu, bơ, chuối...'),
(N'Bánh ngọt', N'Bánh bông lan, mousse, tiramisu...'),
(N'Tráng miệng', N'Rau câu, flan, pudding'),
(N'Nước suối', N'Lavie, Aquafina, Dasani...'),
(N'Trà nóng', N'Trà gừng, trà đào, trà sen'),
(N'Sữa chua', N'Sữa chua dẻo, mít, nếp cẩm');
