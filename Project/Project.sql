CREATE DATABASE Project

USE Project;
GO

-- Bảng Thương Hiệu
CREATE TABLE ThuongHieu (
    MaThuongHieu NVARCHAR(50) PRIMARY KEY,
    TenThuongHieu NVARCHAR(100)
);



-- Bảng Bộ Nhớ Trong
CREATE TABLE BoNhoTrong (
    MaBoNhoTrong NVARCHAR(50) PRIMARY KEY,
    DungLuong NVARCHAR(50)
);

-- Bảng RAM
CREATE TABLE Ram (
    MaRam NVARCHAR(50) PRIMARY KEY,
    DungLuong NVARCHAR(50)
);

-- Bảng Màu Sắc
CREATE TABLE Mau (
    MaMau NVARCHAR(50) PRIMARY KEY,
    TenMau NVARCHAR(50)
);

-- Bảng Phân quyền
CREATE TABLE PhanQuyen (
    MaQuyen int PRIMARY KEY,
    TenQuyen NVARCHAR(50)
);

-- Bảng Điện Thoại
CREATE TABLE DienThoai (
    MaSP NVARCHAR(50) PRIMARY KEY,
    TenSP NVARCHAR(100),
    MaThuongHieu NVARCHAR(50),
	HeDieuHanh NVARCHAR(100),
	CameraTruoc NVARCHAR(100),
	CameraSau NVARCHAR(100),
    ManHinh NVARCHAR(100),
    MaBoNhoTrong NVARCHAR(50),
    MaRam NVARCHAR(50),
	CPU NVARCHAR(100),
	Pin NVARCHAR(100),
	ChatLieu NVARCHAR(500),
	KTKL NVARCHAR(500),
	TDRM NVARCHAR(100),
    MaMau NVARCHAR(50),
    SL INT CHECK(SL >= 0),  -- Đảm bảo số lượng không âm
    GiaMoi DECIMAL(18, 2),
	GiaCu DECIMAL(18, 2),
	MoTa NVARCHAR(1000),
    HinhAnh NVARCHAR(255),
	AnhThongSo NVARCHAR(255),
    CONSTRAINT FK_DienThoai_ThuongHieu FOREIGN KEY (MaThuongHieu) REFERENCES ThuongHieu(MaThuongHieu),
    CONSTRAINT FK_DienThoai_BoNhoTrong FOREIGN KEY (MaBoNhoTrong) REFERENCES BoNhoTrong(MaBoNhoTrong),
    CONSTRAINT FK_DienThoai_Ram FOREIGN KEY (MaRam) REFERENCES Ram(MaRam),
    CONSTRAINT FK_DienThoai_Mau FOREIGN KEY (MaMau) REFERENCES Mau(MaMau)
);


-- Bảng Tài Khoản
-- Bảng Tài Khoản
CREATE TABLE TaiKhoan (
  MaTaiKhoan NVARCHAR(50) NOT NULL PRIMARY KEY,
  MatKhau NVARCHAR(255) NOT NULL,
  MaQuyen int,
  Ten NVARCHAR(100) NOT NULL,
  SDT NVARCHAR(15) NOT NULL,
  DiaChi NVARCHAR(255) NOT NULL,
  CONSTRAINT FK_TaiKhoan_PhanQuyen FOREIGN KEY (MaQuyen) REFERENCES PhanQuyen(MaQuyen)  
);


-- Bảng Giỏ Hàng (Liên kết với tài khoản và sản phẩm)
CREATE TABLE GioHang (
  MaTaiKhoan NVARCHAR(50) NOT NULL,  -- Mã tài khoản người dùng
  MaSP NVARCHAR(50) NOT NULL,         -- Mã sản phẩm
  SoLuong INT NOT NULL CHECK(SoLuong > 0),  -- Số lượng sản phẩm
  Gia DECIMAL(18, 2) NOT NULL,        -- Giá của sản phẩm
  PRIMARY KEY (MaTaiKhoan, MaSP),     -- Khóa chính là sự kết hợp của MaTaiKhoan và MaSP
  FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan),
  FOREIGN KEY (MaSP) REFERENCES DienThoai(MaSP)
);

-- Bảng Trạng Thái Đơn Hàng
CREATE TABLE TrangThai (
  MaTrangThai NVARCHAR(50) PRIMARY KEY,
  TenTrangThai NVARCHAR(100) NOT NULL
);

-- Bảng Hóa Đơn Bán Hàng
CREATE TABLE HD_BanHang (
  MaHD NVARCHAR(50) PRIMARY KEY,
  MaTaiKhoan NVARCHAR(50) NOT NULL,
  Ten NVARCHAR(100) NOT NULL,
  SDT NVARCHAR(15) NOT NULL,
  DiaChi NVARCHAR(255) NOT NULL,
  NgayBan DATETIME NOT NULL,
  TongTien DECIMAL(18, 2) NOT NULL,  -- Có thể tính tổng tiền bằng cách tính lại từ CT_HD_BanHang
  MaTrangThai NVARCHAR(50) NOT NULL,
  FOREIGN KEY (MaTaiKhoan) REFERENCES TaiKhoan(MaTaiKhoan),
  FOREIGN KEY (MaTrangThai) REFERENCES TrangThai(MaTrangThai)
);

-- Bảng Chi Tiết Hóa Đơn Bán Hàng
CREATE TABLE CT_HD_BanHang (
  MaHD NVARCHAR(50) NOT NULL,
  MaSP NVARCHAR(50) NOT NULL,  
  Gia DECIMAL(18, 2) NOT NULL,
  SoLuong INT NOT NULL CHECK(SoLuong > 0),
  FOREIGN KEY (MaHD) REFERENCES HD_BanHang(MaHD),
  FOREIGN KEY (MaSP) REFERENCES DienThoai(MaSP)
);
ALTER TABLE CT_HD_BanHang
ADD CONSTRAINT PK_CT_HD_BanHang PRIMARY KEY (MaHD, MaSP);

INSERT INTO PhanQuyen (MaQuyen, TenQuyen) VALUES 
(1, N'Quản trị viên'),  -- Admin
(2, N'Khách hàng');      -- Customer


-- Thêm dữ liệu vào bảng TrangThai
INSERT INTO TrangThai(MaTrangThai, TenTrangThai) VALUES
('TT001', N'Đang chờ xử lý '),
('TT002', N'Đang giao'),
('TT003', N'Đã giao');

-- Thêm dữ liệu vào bảng ThuongHieu
INSERT INTO ThuongHieu (MaThuongHieu, TenThuongHieu) VALUES
('TH001', 'Apple'),
('TH002', 'Samsung'),
('TH003', 'Xiaomi');



-- Thêm dữ liệu vào bảng BoNhoTrong
INSERT INTO BoNhoTrong (MaBoNhoTrong, DungLuong) VALUES
('BN001', '128GB'),
('BN002', '256GB'),
('BN003', '512GB'),
('BN004', '64GB');

-- Thêm dữ liệu vào bảng Ram
INSERT INTO Ram (MaRam, DungLuong) VALUES
('RAM002', '6GB'),
('RAM003', '8GB'),
('RAM004', '12GB'),
('RAM005', '16GB');

-- Thêm dữ liệu vào bảng Mau (màu sắc)
INSERT INTO Mau (MaMau, TenMau) VALUES
('MAU001', N'Đen'),
('MAU002', N'Trắng'),
('MAU003', N'Xanh dương nhạt'),
('MAU004', N'Vàng nhạt'),
('MAU005', N'Xanh dương'),
('MAU006', N'Tím nhạt'),
('MAU007', N'Xám'),
('MAU008', N'Xanh lá'),
('MAU009', N'Vàng');

-- Thêm dữ liệu vào bảng DienThoai
INSERT INTO DienThoai (MaSP, TenSP, MaThuongHieu, HeDieuHanh, CameraTruoc, CameraSau, ManHinh, MaBoNhoTrong, MaRam, CPU, Pin, ChatLieu, KTKL, TDRM, MaMau, SL, GiaMoi, GiaCu, MoTa, HinhAnh, AnhThongSo)
VALUES
-- iPhone 16
('SP001', 'iPhone 16', 'TH001', 'iOS 18', '12 MP', '48 MP & 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN002', 'RAM003', N'Apple A18 Bionic 6 nhân', '3561 mAh', N'Khung nhôm & Mặt lưng kính cường lực', '147.6 mm - 71.6 mm - 7.8 mm - 170 g', '09/2024', 'MAU001', 100, 25990000, 26490000, N'iPhone 16 (2024) dự kiến sẽ là phiên bản mới nhất trong dòng iPhone, có thể trang bị chip A18, màn hình OLED tiên tiến, và thiết kế hiện đại với các tùy chọn kích thước khác nhau. Các tính năng nổi bật có thể bao gồm camera cải tiến, thời lượng pin lâu hơn, và tích hợp sâu hơn với trí tuệ nhân tạo (AI). Nó cũng có thể hỗ trợ mạng 5G và các cải tiến về khả năng bảo mật như Face ID hoặc cảm biến vân tay dưới màn hình.', 'iphone16_den.jpg', 'iphone16_thongso.jpg'),
('SP002', 'iPhone 16', 'TH001', 'iOS 18', '12 MP', '48 MP & 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN002', 'RAM003', N'Apple A18 Bionic 6 nhân', '3561 mAh', N'Khung nhôm & Mặt lưng kính cường lực', '147.6 mm - 71.6 mm - 7.8 mm - 170 g', '09/2024', 'MAU002', 100, 25990000, 26490000, N'iPhone 16 (2024) dự kiến sẽ là phiên bản mới nhất trong dòng iPhone, có thể trang bị chip A18, màn hình OLED tiên tiến, và thiết kế hiện đại với các tùy chọn kích thước khác nhau. Các tính năng nổi bật có thể bao gồm camera cải tiến, thời lượng pin lâu hơn, và tích hợp sâu hơn với trí tuệ nhân tạo (AI). Nó cũng có thể hỗ trợ mạng 5G và các cải tiến về khả năng bảo mật như Face ID hoặc cảm biến vân tay dưới màn hình.', 'iphone16_trang.jpg', 'iphone16_thongso.jpg'),
('SP003', 'iPhone 16', 'TH001', 'iOS 18', '12 MP', '48 MP & 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN003', 'RAM003', N'Apple A18 Bionic 6 nhân', '3561 mAh', N'Khung nhôm & Mặt lưng kính cường lực', '147.6 mm - 71.6 mm - 7.8 mm - 170 g', '09/2024', 'MAU001', 100, 25990000, 26490000, N'iPhone 16 (2024) dự kiến sẽ là phiên bản mới nhất trong dòng iPhone, có thể trang bị chip A18, màn hình OLED tiên tiến, và thiết kế hiện đại với các tùy chọn kích thước khác nhau. Các tính năng nổi bật có thể bao gồm camera cải tiến, thời lượng pin lâu hơn, và tích hợp sâu hơn với trí tuệ nhân tạo (AI). Nó cũng có thể hỗ trợ mạng 5G và các cải tiến về khả năng bảo mật như Face ID hoặc cảm biến vân tay dưới màn hình.', 'iphone16_den.jpg', 'iphone16_thongso.jpg'),
('SP004', 'iPhone 16', 'TH001', 'iOS 18', '12 MP', '48 MP & 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN003', 'RAM003', N'Apple A18 Bionic 6 nhân', '3561 mAh', N'Khung nhôm & Mặt lưng kính cường lực', '147.6 mm - 71.6 mm - 7.8 mm - 170 g', '09/2024', 'MAU002', 100, 25990000, 26490000, N'iPhone 16 (2024) dự kiến sẽ là phiên bản mới nhất trong dòng iPhone, có thể trang bị chip A18, màn hình OLED tiên tiến, và thiết kế hiện đại với các tùy chọn kích thước khác nhau. Các tính năng nổi bật có thể bao gồm camera cải tiến, thời lượng pin lâu hơn, và tích hợp sâu hơn với trí tuệ nhân tạo (AI). Nó cũng có thể hỗ trợ mạng 5G và các cải tiến về khả năng bảo mật như Face ID hoặc cảm biến vân tay dưới màn hình.', 'iphone16_trang.jpg', 'iphone16_thongso.jpg'),

-- iPhone 15
('SP005', 'iPhone 15', 'TH001', 'iOS 17', '12 MP', '48 MP & 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN001', 'RAM002', 'Apple A16 Bionic', '3349 mAh', N'Khung nhôm & Mặt lưng kính cường lực', N'Dài 147.6 mm - Ngang 71.6 mm - Dày 7.8 mm - Nặng 171 g', '09/2023', 'MAU003', 100, 19790000, 22990000, N'iPhone 15 (ra mắt năm 2023) là dòng smartphone mới nhất của Apple, với thiết kế tinh tế và nhiều nâng cấp. Điểm nổi bật bao gồm cổng sạc USB-C thay vì Lightning, chip A16 Bionic (đối với iPhone 15 và 15 Plus) và A17 Pro (trên iPhone 15 Pro và Pro Max), cùng với màn hình OLED sắc nét. Các mẫu Pro có viền titan nhẹ, camera cải tiến với cảm biến chính 48MP và khả năng zoom quang học cao hơn. Nó cũng hỗ trợ kết nối 5G, Dynamic Island, và dung lượng pin tốt hơn so với các mẫu trước đó.', 'iphone15_xanhduongnhat.jpg', 'iphone15_thongso.jpg'),
('SP006', 'iPhone 15', 'TH001', 'iOS 17', '12 MP', '48 MP & 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN001', 'RAM002', 'Apple A16 Bionic', '3349 mAh', N'Khung nhôm & Mặt lưng kính cường lực', N'Dài 147.6 mm - Ngang 71.6 mm - Dày 7.8 mm - Nặng 171 g', '09/2023', 'MAU004', 100, 19790000, 22990000, N'iPhone 15 (ra mắt năm 2023) là dòng smartphone mới nhất của Apple, với thiết kế tinh tế và nhiều nâng cấp. Điểm nổi bật bao gồm cổng sạc USB-C thay vì Lightning, chip A16 Bionic (đối với iPhone 15 và 15 Plus) và A17 Pro (trên iPhone 15 Pro và Pro Max), cùng với màn hình OLED sắc nét. Các mẫu Pro có viền titan nhẹ, camera cải tiến với cảm biến chính 48MP và khả năng zoom quang học cao hơn. Nó cũng hỗ trợ kết nối 5G, Dynamic Island, và dung lượng pin tốt hơn so với các mẫu trước đó.', 'iphone15_xanhduongnhat.jpg', 'iphone15_thongso.jpg'),
('SP007', 'iPhone 15', 'TH001', 'iOS 17', '12 MP', '48 MP & 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN002', 'RAM002', 'Apple A16 Bionic', '3349 mAh', N'Khung nhôm & Mặt lưng kính cường lực', N'Dài 147.6 mm - Ngang 71.6 mm - Dày 7.8 mm - Nặng 171 g', '09/2023', 'MAU003', 100, 19790000, 22990000, N'iPhone 15 (ra mắt năm 2023) là dòng smartphone mới nhất của Apple, với thiết kế tinh tế và nhiều nâng cấp. Điểm nổi bật bao gồm cổng sạc USB-C thay vì Lightning, chip A16 Bionic (đối với iPhone 15 và 15 Plus) và A17 Pro (trên iPhone 15 Pro và Pro Max), cùng với màn hình OLED sắc nét. Các mẫu Pro có viền titan nhẹ, camera cải tiến với cảm biến chính 48MP và khả năng zoom quang học cao hơn. Nó cũng hỗ trợ kết nối 5G, Dynamic Island, và dung lượng pin tốt hơn so với các mẫu trước đó.', 'iphone15_xanhduongnhat.jpg', 'iphone15_thongso.jpg'),
('SP008', 'iPhone 15', 'TH001', 'iOS 17', '12 MP', '48 MP & 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN002', 'RAM002', 'Apple A16 Bionic', '3349 mAh', N'Khung nhôm & Mặt lưng kính cường lực', N'Dài 147.6 mm - Ngang 71.6 mm - Dày 7.8 mm - Nặng 171 g', '09/2023', 'MAU004', 100, 19790000, 22990000, N'iPhone 15 (ra mắt năm 2023) là dòng smartphone mới nhất của Apple, với thiết kế tinh tế và nhiều nâng cấp. Điểm nổi bật bao gồm cổng sạc USB-C thay vì Lightning, chip A16 Bionic (đối với iPhone 15 và 15 Plus) và A17 Pro (trên iPhone 15 Pro và Pro Max), cùng với màn hình OLED sắc nét. Các mẫu Pro có viền titan nhẹ, camera cải tiến với cảm biến chính 48MP và khả năng zoom quang học cao hơn. Nó cũng hỗ trợ kết nối 5G, Dynamic Island, và dung lượng pin tốt hơn so với các mẫu trước đó.', 'iphone15_xanhduongnhat.jpg', 'iphone15_thongso.jpg'),

-- iPhone 14
('SP009', 'iPhone 14', 'TH001', 'iOS 17', '12 MP', '2 camera 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN001', 'RAM002', 'Apple A15 Bionic', '3349 mAh', N'Khung nhôm & Mặt lưng kính cường lực', 'Dài 146.7 mm - Ngang 71.5 mm - Dày 7.8 mm - Nặng 172 g', '09/2022', 'MAU005', 100, 17590000, 19090000, N'iPhone 14 (ra mắt năm 2022) là dòng điện thoại thông minh của Apple với nhiều cải tiến về hiệu năng và tính năng so với thế hệ trước. Nó có màn hình OLED Super Retina XDR, chip A15 Bionic (vẫn rất mạnh mẽ) cho phiên bản tiêu chuẩn và A16 Bionic cho iPhone 14 Pro. Camera chính 12MP trên phiên bản thường và 48MP trên mẫu Pro mang lại chất lượng ảnh sắc nét, cùng với các tính năng như chế độ Cinematic và Photonic Engine. Ngoài ra, iPhone 14 còn hỗ trợ kết nối 5G, tính năng phát hiện va chạm (Crash Detection), và thời lượng pin dài hơn so với các phiên bản trước.', 'iphone14_xanhduong.jpg', 'iphone14_thongso.jpg'),
('SP010', 'iPhone 14', 'TH001', 'iOS 17', '12 MP', '2 camera 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN001', 'RAM002', 'Apple A15 Bionic', '3349 mAh', N'Khung nhôm & Mặt lưng kính cường lực', 'Dài 146.7 mm - Ngang 71.5 mm - Dày 7.8 mm - Nặng 172 g', '09/2022', 'MAU005', 100, 17590000, 19090000, N'iPhone 14 (ra mắt năm 2022) là dòng điện thoại thông minh của Apple với nhiều cải tiến về hiệu năng và tính năng so với thế hệ trước. Nó có màn hình OLED Super Retina XDR, chip A15 Bionic (vẫn rất mạnh mẽ) cho phiên bản tiêu chuẩn và A16 Bionic cho iPhone 14 Pro. Camera chính 12MP trên phiên bản thường và 48MP trên mẫu Pro mang lại chất lượng ảnh sắc nét, cùng với các tính năng như chế độ Cinematic và Photonic Engine. Ngoài ra, iPhone 14 còn hỗ trợ kết nối 5G, tính năng phát hiện va chạm (Crash Detection), và thời lượng pin dài hơn so với các phiên bản trước.', 'iphone14_timnhat.jpg', 'iphone14_thongso.jpg'),
('SP011', 'iPhone 14', 'TH001', 'iOS 17', '12 MP', '2 camera 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN002', 'RAM002', 'Apple A15 Bionic', '3349 mAh', N'Khung nhôm & Mặt lưng kính cường lực', 'Dài 146.7 mm - Ngang 71.5 mm - Dày 7.8 mm - Nặng 172 g', '09/2022', 'MAU005', 100, 17590000, 19090000, N'iPhone 14 (ra mắt năm 2022) là dòng điện thoại thông minh của Apple với nhiều cải tiến về hiệu năng và tính năng so với thế hệ trước. Nó có màn hình OLED Super Retina XDR, chip A15 Bionic (vẫn rất mạnh mẽ) cho phiên bản tiêu chuẩn và A16 Bionic cho iPhone 14 Pro. Camera chính 12MP trên phiên bản thường và 48MP trên mẫu Pro mang lại chất lượng ảnh sắc nét, cùng với các tính năng như chế độ Cinematic và Photonic Engine. Ngoài ra, iPhone 14 còn hỗ trợ kết nối 5G, tính năng phát hiện va chạm (Crash Detection), và thời lượng pin dài hơn so với các phiên bản trước.', 'iphone14_xanhduong.jpg', 'iphone14_thongso.jpg'),
('SP012', 'iPhone 14', 'TH001', 'iOS 17', '12 MP', '2 camera 12 MP', N'6.1" - Tần số quét 60 Hz', 'BN002', 'RAM002', 'Apple A15 Bionic', '3349 mAh', N'Khung nhôm & Mặt lưng kính cường lực', 'Dài 146.7 mm - Ngang 71.5 mm - Dày 7.8 mm - Nặng 172 g', '09/2022', 'MAU005', 100, 17590000, 19090000, N'iPhone 14 (ra mắt năm 2022) là dòng điện thoại thông minh của Apple với nhiều cải tiến về hiệu năng và tính năng so với thế hệ trước. Nó có màn hình OLED Super Retina XDR, chip A15 Bionic (vẫn rất mạnh mẽ) cho phiên bản tiêu chuẩn và A16 Bionic cho iPhone 14 Pro. Camera chính 12MP trên phiên bản thường và 48MP trên mẫu Pro mang lại chất lượng ảnh sắc nét, cùng với các tính năng như chế độ Cinematic và Photonic Engine. Ngoài ra, iPhone 14 còn hỗ trợ kết nối 5G, tính năng phát hiện va chạm (Crash Detection), và thời lượng pin dài hơn so với các phiên bản trước.', 'iphone14_timnhat.jpg', 'iphone14_thongso.jpg'),

-- Samsung Galaxy S24 Ultra 5G
('SP013', 'Samsung Galaxy S24 Ultra 5G', 'TH002', N'Android 14, giao diện One UI 6.0', '40 MP', N'4 camera (200 MP chính, 12 MP góc siêu rộng, 10 MP telephoto 3x zoom, 10 MP telephoto 10x zoom)', N'6.8" - Tần số quét 120 Hz, Dynamic AMOLED 2X', 'BN002', 'RAM004', 'Qualcomm Snapdragon 8 Gen 3', '5000 mAh', N'Khung nhôm Armor, mặt lưng kính cường lực Gorilla Glass Victus 2', N'Dài 163.4 mm - Ngang 78.1 mm - Dày 8.9 mm - Nặng 234 g', '02/2024', 'MAU007', 100, 29990000, 33990000, N'Samsung Galaxy S24 Ultra 5G là mẫu điện thoại cao cấp nhất của dòng Galaxy S24 với nhiều nâng cấp mạnh mẽ. Máy được trang bị màn hình Dynamic AMOLED 2X rộng 6.8 inch, hỗ trợ tần số quét 120 Hz giúp cho hình ảnh mượt mà và rõ nét. Bộ vi xử lý Snapdragon 8 Gen 3 cùng với RAM lên tới 16GB mang lại hiệu năng vượt trội cho các tác vụ đa nhiệm và chơi game nặng. Camera chính 200 MP kết hợp với khả năng zoom quang học 10x và nhiều tính năng chụp ảnh tiên tiến, giúp ghi lại hình ảnh sắc nét, chi tiết. Pin dung lượng lớn 5000 mAh hỗ trợ sạc nhanh và sạc không dây. Sản phẩm còn hỗ trợ kết nối 5G, tích hợp nhiều tính năng bảo mật và công nghệ cao như cảm biến vân tay dưới màn hình.', 's24ultra_xam.jpg', 'samsung-galaxy-s24-ultra-thongso.jpg'),
('SP014', 'Samsung Galaxy S24 Ultra 5G', 'TH002', N'Android 14, giao diện One UI 6.0', '40 MP', N'4 camera (200 MP chính, 12 MP góc siêu rộng, 10 MP telephoto 3x zoom, 10 MP telephoto 10x zoom)', N'6.8" - Tần số quét 120 Hz, Dynamic AMOLED 2X', 'BN002', 'RAM004', 'Qualcomm Snapdragon 8 Gen 3', '5000 mAh', N'Khung nhôm Armor, mặt lưng kính cường lực Gorilla Glass Victus 2', N'Dài 163.4 mm - Ngang 78.1 mm - Dày 8.9 mm - Nặng 234 g', '02/2024', 'MAU001', 100, 29990000, 33990000, N'Samsung Galaxy S24 Ultra 5G là mẫu điện thoại cao cấp nhất của dòng Galaxy S24 với nhiều nâng cấp mạnh mẽ. Máy được trang bị màn hình Dynamic AMOLED 2X rộng 6.8 inch, hỗ trợ tần số quét 120 Hz giúp cho hình ảnh mượt mà và rõ nét. Bộ vi xử lý Snapdragon 8 Gen 3 cùng với RAM lên tới 16GB mang lại hiệu năng vượt trội cho các tác vụ đa nhiệm và chơi game nặng. Camera chính 200 MP kết hợp với khả năng zoom quang học 10x và nhiều tính năng chụp ảnh tiên tiến, giúp ghi lại hình ảnh sắc nét, chi tiết. Pin dung lượng lớn 5000 mAh hỗ trợ sạc nhanh và sạc không dây. Sản phẩm còn hỗ trợ kết nối 5G, tích hợp nhiều tính năng bảo mật và công nghệ cao như cảm biến vân tay dưới màn hình.', 's24ultra_den.jpg', 'samsung-galaxy-s24-ultra-thongso.jpg'),
('SP015', 'Samsung Galaxy S24 Ultra 5G', 'TH002', N'Android 14, giao diện One UI 6.0', '40 MP', N'4 camera (200 MP chính, 12 MP góc siêu rộng, 10 MP telephoto 3x zoom, 10 MP telephoto 10x zoom)', N'6.8" - Tần số quét 120 Hz, Dynamic AMOLED 2X', 'BN003', 'RAM004', 'Qualcomm Snapdragon 8 Gen 3', '5000 mAh', N'Khung nhôm Armor, mặt lưng kính cường lực Gorilla Glass Victus 2', N'Dài 163.4 mm - Ngang 78.1 mm - Dày 8.9 mm - Nặng 234 g', '02/2024', 'MAU007', 100, 33490000, 37490000, N'Samsung Galaxy S24 Ultra 5G là mẫu điện thoại cao cấp nhất của dòng Galaxy S24 với nhiều nâng cấp mạnh mẽ. Máy được trang bị màn hình Dynamic AMOLED 2X rộng 6.8 inch, hỗ trợ tần số quét 120 Hz giúp cho hình ảnh mượt mà và rõ nét. Bộ vi xử lý Snapdragon 8 Gen 3 cùng với RAM lên tới 16GB mang lại hiệu năng vượt trội cho các tác vụ đa nhiệm và chơi game nặng. Camera chính 200 MP kết hợp với khả năng zoom quang học 10x và nhiều tính năng chụp ảnh tiên tiến, giúp ghi lại hình ảnh sắc nét, chi tiết. Pin dung lượng lớn 5000 mAh hỗ trợ sạc nhanh và sạc không dây. Sản phẩm còn hỗ trợ kết nối 5G, tích hợp nhiều tính năng bảo mật và công nghệ cao như cảm biến vân tay dưới màn hình.', 's24ultra_xam.jpg', 'samsung-galaxy-s24-ultra-thongso.jpg'),
('SP016', 'Samsung Galaxy S24 Ultra 5G', 'TH002', N'Android 14, giao diện One UI 6.0', '40 MP', N'4 camera (200 MP chính, 12 MP góc siêu rộng, 10 MP telephoto 3x zoom, 10 MP telephoto 10x zoom)', N'6.8" - Tần số quét 120 Hz, Dynamic AMOLED 2X', 'BN003', 'RAM004', 'Qualcomm Snapdragon 8 Gen 3', '5000 mAh', N'Khung nhôm Armor, mặt lưng kính cường lực Gorilla Glass Victus 2', N'Dài 163.4 mm - Ngang 78.1 mm - Dày 8.9 mm - Nặng 234 g', '02/2024', 'MAU001', 100, 33490000, 37490000, N'Samsung Galaxy S24 Ultra 5G là mẫu điện thoại cao cấp nhất của dòng Galaxy S24 với nhiều nâng cấp mạnh mẽ. Máy được trang bị màn hình Dynamic AMOLED 2X rộng 6.8 inch, hỗ trợ tần số quét 120 Hz giúp cho hình ảnh mượt mà và rõ nét. Bộ vi xử lý Snapdragon 8 Gen 3 cùng với RAM lên tới 16GB mang lại hiệu năng vượt trội cho các tác vụ đa nhiệm và chơi game nặng. Camera chính 200 MP kết hợp với khả năng zoom quang học 10x và nhiều tính năng chụp ảnh tiên tiến, giúp ghi lại hình ảnh sắc nét, chi tiết. Pin dung lượng lớn 5000 mAh hỗ trợ sạc nhanh và sạc không dây. Sản phẩm còn hỗ trợ kết nối 5G, tích hợp nhiều tính năng bảo mật và công nghệ cao như cảm biến vân tay dưới màn hình.', 's24ultra_den.jpg', 'samsung-galaxy-s24-ultra-thongso.jpg'),

-- Samsung Galaxy M55 5G
('SP017', 'Samsung Galaxy M55 5G', 'TH002', N'Android 14, giao diện One UI 6.0', '32 MP', N'3 camera (108 MP chính, 8 MP góc siêu rộng, 5 MP macro)', N'6.7" - Tần số quét 120 Hz, Super AMOLED', 'BN002', 'RAM004', 'MediaTek Dimensity 920', '6000 mAh', N'Khung nhựa, mặt lưng nhựa cao cấp', N'Dài 164.2 mm - Ngang 76.5 mm - Dày 8.4 mm - Nặng 215 g', '10/2024', 'MAU001', 100, 9990000, 12690000, N'Samsung Galaxy M55 5G là mẫu điện thoại tầm trung với cấu hình mạnh mẽ và thời lượng pin ấn tượng. Máy được trang bị màn hình Super AMOLED 6.7 inch với tần số quét 120 Hz, mang lại trải nghiệm hình ảnh sắc nét và mượt mà. Camera chính 108 MP kết hợp với ống kính góc siêu rộng và macro, giúp người dùng chụp được nhiều góc độ sáng tạo. Chip xử lý MediaTek Dimensity 920 cùng với RAM lên tới 12GB giúp máy hoạt động mượt mà trong các tác vụ nặng. Dung lượng pin 6000 mAh hỗ trợ sạc nhanh, đáp ứng nhu cầu sử dụng lâu dài. Galaxy M55 5G còn hỗ trợ kết nối 5G, mang lại trải nghiệm mạng tốc độ cao.', 'm55_den.jpg', 'samsung-galaxy-m55-thongso.jpg'),
('SP018', 'Samsung Galaxy M55 5G', 'TH002', N'Android 14, giao diện One UI 6.0', '32 MP', N'3 camera (108 MP chính, 8 MP góc siêu rộng, 5 MP macro)', N'6.7" - Tần số quét 120 Hz, Super AMOLED', 'BN002', 'RAM004', 'MediaTek Dimensity 920', '6000 mAh', N'Khung nhựa, mặt lưng nhựa cao cấp', N'Dài 164.2 mm - Ngang 76.5 mm - Dày 8.4 mm - Nặng 215 g', '10/2024', 'MAU008', 100, 9990000, 12690000, N'Samsung Galaxy M55 5G là mẫu điện thoại tầm trung với cấu hình mạnh mẽ và thời lượng pin ấn tượng. Máy được trang bị màn hình Super AMOLED 6.7 inch với tần số quét 120 Hz, mang lại trải nghiệm hình ảnh sắc nét và mượt mà. Camera chính 108 MP kết hợp với ống kính góc siêu rộng và macro, giúp người dùng chụp được nhiều góc độ sáng tạo. Chip xử lý MediaTek Dimensity 920 cùng với RAM lên tới 12GB giúp máy hoạt động mượt mà trong các tác vụ nặng. Dung lượng pin 6000 mAh hỗ trợ sạc nhanh, đáp ứng nhu cầu sử dụng lâu dài. Galaxy M55 5G còn hỗ trợ kết nối 5G, mang lại trải nghiệm mạng tốc độ cao.','m55_xanhla.jpg', 'samsung-galaxy-m55-thongso.jpg'),

-- Samsung Galaxy A35 5G
('SP019', 'Samsung Galaxy A35 5G 128GB ', 'TH002', N'Android 14, giao diện One UI 6.0', '13 MP', N'3 camera (64 MP chính, 8 MP góc siêu rộng, 5 MP cảm biến độ sâu)', N'6.5" - Tần số quét 90 Hz, Super AMOLED', 'BN001', 'RAM003', 'Qualcomm Snapdragon 695 5G', '5000 mAh', N'Khung nhựa, mặt lưng nhựa bóng', N'Dài 161.6 mm - Ngang 74.1 mm - Dày 8.2 mm - Nặng 190 g', '03/2024', 'MAU001', 100, 7990000, 8290000, N'Samsung Galaxy A35 5G là chiếc điện thoại thuộc phân khúc tầm trung với khả năng kết nối 5G và hiệu năng tốt. Máy sở hữu màn hình Super AMOLED 6.5 inch với tần số quét 90 Hz, mang lại hình ảnh mượt mà và rõ nét. Camera chính 64 MP cùng với các camera phụ giúp người dùng chụp ảnh chi tiết và đa dạng. Chip Snapdragon 695 5G kết hợp với RAM lên đến 8GB đảm bảo máy chạy mượt mà cho các ứng dụng hàng ngày và chơi game nhẹ. Pin 5000 mAh cung cấp thời lượng sử dụng lâu dài, cùng với hỗ trợ sạc nhanh giúp giảm thời gian sạc.', 'a35_den.jpg', 'samsung-galaxy-a35-thongso.jpg'),
('SP020', 'Samsung Galaxy A35 5G 128GB', 'TH002', N'Android 14, giao diện One UI 6.0', '13 MP', N'3 camera (64 MP chính, 8 MP góc siêu rộng, 5 MP cảm biến độ sâu)', N'6.5" - Tần số quét 90 Hz, Super AMOLED', 'BN001', 'RAM003', 'Qualcomm Snapdragon 695 5G', '5000 mAh', N'Khung nhựa, mặt lưng nhựa bóng', N'Dài 161.6 mm - Ngang 74.1 mm - Dày 8.2 mm - Nặng 190 g', '03/2024', 'MAU009', 100, 7990000, 8290000, N'Samsung Galaxy A35 5G là chiếc điện thoại thuộc phân khúc tầm trung với khả năng kết nối 5G và hiệu năng tốt. Máy sở hữu màn hình Super AMOLED 6.5 inch với tần số quét 90 Hz, mang lại hình ảnh mượt mà và rõ nét. Camera chính 64 MP cùng với các camera phụ giúp người dùng chụp ảnh chi tiết và đa dạng. Chip Snapdragon 695 5G kết hợp với RAM lên đến 8GB đảm bảo máy chạy mượt mà cho các ứng dụng hàng ngày và chơi game nhẹ. Pin 5000 mAh cung cấp thời lượng sử dụng lâu dài, cùng với hỗ trợ sạc nhanh giúp giảm thời gian sạc.', 'a35_vang.jpg', 'samsung-galaxy-a35-thongso.jpg'),
('SP021', 'Samsung Galaxy A35 5G 256GB ', 'TH002', N'Android 14, giao diện One UI 6.0', '13 MP', N'3 camera (64 MP chính, 8 MP góc siêu rộng, 5 MP cảm biến độ sâu)', N'6.5" - Tần số quét 90 Hz, Super AMOLED', 'BN002', 'RAM003', 'Qualcomm Snapdragon 695 5G', '5000 mAh', N'Khung nhựa, mặt lưng nhựa bóng', N'Dài 161.6 mm - Ngang 74.1 mm - Dày 8.2 mm - Nặng 190 g', '03/2024', 'MAU001', 100, 8790000, 9290000, N'Samsung Galaxy A35 5G là chiếc điện thoại thuộc phân khúc tầm trung với khả năng kết nối 5G và hiệu năng tốt. Máy sở hữu màn hình Super AMOLED 6.5 inch với tần số quét 90 Hz, mang lại hình ảnh mượt mà và rõ nét. Camera chính 64 MP cùng với các camera phụ giúp người dùng chụp ảnh chi tiết và đa dạng. Chip Snapdragon 695 5G kết hợp với RAM lên đến 8GB đảm bảo máy chạy mượt mà cho các ứng dụng hàng ngày và chơi game nhẹ. Pin 5000 mAh cung cấp thời lượng sử dụng lâu dài, cùng với hỗ trợ sạc nhanh giúp giảm thời gian sạc.', 'a35_den.jpg','samsung-galaxy-a35-thongso.jpg'),
('SP022', 'Samsung Galaxy A35 5G 256GB', 'TH002', N'Android 14, giao diện One UI 6.0', '13 MP', N'3 camera (64 MP chính, 8 MP góc siêu rộng, 5 MP cảm biến độ sâu)', N'6.5" - Tần số quét 90 Hz, Super AMOLED', 'BN002', 'RAM003', 'Qualcomm Snapdragon 695 5G', '5000 mAh', N'Khung nhựa, mặt lưng nhựa bóng', N'Dài 161.6 mm - Ngang 74.1 mm - Dày 8.2 mm - Nặng 190 g', '03/2024', 'MAU009', 100, 8790000, 9290000, N'Samsung Galaxy A35 5G là chiếc điện thoại thuộc phân khúc tầm trung với khả năng kết nối 5G và hiệu năng tốt. Máy sở hữu màn hình Super AMOLED 6.5 inch với tần số quét 90 Hz, mang lại hình ảnh mượt mà và rõ nét. Camera chính 64 MP cùng với các camera phụ giúp người dùng chụp ảnh chi tiết và đa dạng. Chip Snapdragon 695 5G kết hợp với RAM lên đến 8GB đảm bảo máy chạy mượt mà cho các ứng dụng hàng ngày và chơi game nhẹ. Pin 5000 mAh cung cấp thời lượng sử dụng lâu dài, cùng với hỗ trợ sạc nhanh giúp giảm thời gian sạc.', 'a35_vang.jpg', 'samsung-galaxy-a35-thongso.jpg'),

-- Xiaomi Redmi 13
('SP023', 'Xiaomi Redmi 13', 'TH003', 'Android 14, MIUI 15', '16 MP', N'50 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.6" - Tần số quét 90 Hz, IPS LCD', 'BN001', 'RAM002', 'MediaTek Dimensity 810', '5000 mAh', N'Khung nhựa, mặt lưng nhựa', N'Dài 165.5 mm - Ngang 76.2 mm - Dày 8.4 mm - Nặng 195 g', '01/2024', 'MAU009', 100, 3990000, 4990000, N'Xiaomi Redmi 13 là mẫu điện thoại tầm trung với cấu hình ổn định và mức giá hợp lý. Máy sở hữu màn hình IPS LCD rộng 6.6 inch, hỗ trợ tần số quét 90 Hz, cho trải nghiệm hiển thị mượt mà. Hệ thống camera chính 50 MP và các camera phụ giúp chụp ảnh đa dạng, từ góc rộng đến chi tiết cận cảnh. Chip MediaTek Dimensity 810 cùng RAM tối đa 6GB giúp máy xử lý mượt mà các tác vụ hàng ngày. Dung lượng pin 5000 mAh cho phép sử dụng lâu dài, và hỗ trợ sạc nhanh giúp rút ngắn thời gian sạc. Xiaomi Redmi 13 cũng hỗ trợ kết nối 5G, giúp nâng cao trải nghiệm sử dụng mạng tốc độ cao.', 'redmi13_vang.jpg', 'redmi-13-thongso.jpg'),
('SP024', 'Xiaomi Redmi 13', 'TH003', 'Android 14, MIUI 15', '16 MP', N'50 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.6" - Tần số quét 90 Hz, IPS LCD', 'BN001', 'RAM002', 'MediaTek Dimensity 810', '5000 mAh', N'Khung nhựa, mặt lưng nhựa', N'Dài 165.5 mm - Ngang 76.2 mm - Dày 8.4 mm - Nặng 195 g', '01/2024', 'MAU001', 100, 3990000, 4990000, N'Xiaomi Redmi 13 là mẫu điện thoại tầm trung với cấu hình ổn định và mức giá hợp lý. Máy sở hữu màn hình IPS LCD rộng 6.6 inch, hỗ trợ tần số quét 90 Hz, cho trải nghiệm hiển thị mượt mà. Hệ thống camera chính 50 MP và các camera phụ giúp chụp ảnh đa dạng, từ góc rộng đến chi tiết cận cảnh. Chip MediaTek Dimensity 810 cùng RAM tối đa 6GB giúp máy xử lý mượt mà các tác vụ hàng ngày. Dung lượng pin 5000 mAh cho phép sử dụng lâu dài, và hỗ trợ sạc nhanh giúp rút ngắn thời gian sạc. Xiaomi Redmi 13 cũng hỗ trợ kết nối 5G, giúp nâng cao trải nghiệm sử dụng mạng tốc độ cao.', 'redmi13_den.jpg', 'redmi-13-thongso.jpg'),
('SP025', 'Xiaomi Redmi 13', 'TH003', 'Android 14, MIUI 15', '16 MP', N'50 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.6" - Tần số quét 90 Hz, IPS LCD', 'BN001', 'RAM003', 'MediaTek Dimensity 810', '5000 mAh', N'Khung nhựa, mặt lưng nhựa', N'Dài 165.5 mm - Ngang 76.2 mm - Dày 8.4 mm - Nặng 195 g', '01/2024', 'MAU009', 100, 3990000, 4990000, N'Xiaomi Redmi 13 là mẫu điện thoại tầm trung với cấu hình ổn định và mức giá hợp lý. Máy sở hữu màn hình IPS LCD rộng 6.6 inch, hỗ trợ tần số quét 90 Hz, cho trải nghiệm hiển thị mượt mà. Hệ thống camera chính 50 MP và các camera phụ giúp chụp ảnh đa dạng, từ góc rộng đến chi tiết cận cảnh. Chip MediaTek Dimensity 810 cùng RAM tối đa 6GB giúp máy xử lý mượt mà các tác vụ hàng ngày. Dung lượng pin 5000 mAh cho phép sử dụng lâu dài, và hỗ trợ sạc nhanh giúp rút ngắn thời gian sạc. Xiaomi Redmi 13 cũng hỗ trợ kết nối 5G, giúp nâng cao trải nghiệm sử dụng mạng tốc độ cao.', 'redmi13_vang.jpg', 'redmi-13-thongso.jpg'),
('SP026', 'Xiaomi Redmi 13', 'TH003', 'Android 14, MIUI 15', '16 MP', N'50 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.6" - Tần số quét 90 Hz, IPS LCD', 'BN001', 'RAM003', 'MediaTek Dimensity 810', '5000 mAh', N'Khung nhựa, mặt lưng nhựa', N'Dài 165.5 mm - Ngang 76.2 mm - Dày 8.4 mm - Nặng 195 g', '01/2024', 'MAU001', 100, 3990000, 4990000, N'Xiaomi Redmi 13 là mẫu điện thoại tầm trung với cấu hình ổn định và mức giá hợp lý. Máy sở hữu màn hình IPS LCD rộng 6.6 inch, hỗ trợ tần số quét 90 Hz, cho trải nghiệm hiển thị mượt mà. Hệ thống camera chính 50 MP và các camera phụ giúp chụp ảnh đa dạng, từ góc rộng đến chi tiết cận cảnh. Chip MediaTek Dimensity 810 cùng RAM tối đa 6GB giúp máy xử lý mượt mà các tác vụ hàng ngày. Dung lượng pin 5000 mAh cho phép sử dụng lâu dài, và hỗ trợ sạc nhanh giúp rút ngắn thời gian sạc. Xiaomi Redmi 13 cũng hỗ trợ kết nối 5G, giúp nâng cao trải nghiệm sử dụng mạng tốc độ cao.', 'redmi13_den.jpg', 'redmi-13-thongso.jpg'),

-- Xiaomi Redmi Note 13
('SP027', 'Xiaomi Redmi Note 13', 'TH003', 'Android 14, MIUI 15', '16 MP', N'50 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.67" - Tần số quét 120 Hz, AMOLED', 'BN001', 'RAM003', 'MediaTek Dimensity 920', '5000 mAh', N'Khung nhôm, mặt lưng kính', N'Dài 164.2 mm - Ngang 76.1 mm - Dày 8.3 mm - Nặng 199 g', '10/2024', 'MAU001', 100, 5990000, 6490000, N'Xiaomi Redmi Note 13 là mẫu điện thoại thuộc dòng Redmi Note với nhiều nâng cấp đáng giá. Máy được trang bị màn hình AMOLED 6.67 inch hỗ trợ tần số quét 120 Hz, mang lại trải nghiệm hiển thị sống động và mượt mà. Camera chính 50 MP cùng với các camera phụ hỗ trợ chụp ảnh từ góc rộng đến macro, cho phép người dùng thỏa sức sáng tạo trong nhiếp ảnh. Hiệu năng được đảm bảo bởi chip MediaTek Dimensity 920 và RAM lên đến 8GB, đáp ứng tốt các nhu cầu từ làm việc đến giải trí. Pin dung lượng 5000 mAh kèm theo công nghệ sạc nhanh giúp kéo dài thời gian sử dụng, đảm bảo hiệu suất trong suốt ngày dài. Máy cũng hỗ trợ kết nối 5G, cho phép trải nghiệm internet tốc độ cao.','note13_den.jpg', 'redmi-note-13-thongso.jpg'),
('SP028', 'Xiaomi Redmi Note 13', 'TH003', 'Android 14, MIUI 15', '16 MP', N'50 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.67" - Tần số quét 120 Hz, AMOLED', 'BN001', 'RAM003', 'MediaTek Dimensity 920', '5000 mAh', N'Khung nhôm, mặt lưng kính', N'Dài 164.2 mm - Ngang 76.1 mm - Dày 8.3 mm - Nặng 199 g', '10/2024', 'MAU009', 100, 5990000, 6490000, N'Xiaomi Redmi Note 13 là mẫu điện thoại thuộc dòng Redmi Note với nhiều nâng cấp đáng giá. Máy được trang bị màn hình AMOLED 6.67 inch hỗ trợ tần số quét 120 Hz, mang lại trải nghiệm hiển thị sống động và mượt mà. Camera chính 50 MP cùng với các camera phụ hỗ trợ chụp ảnh từ góc rộng đến macro, cho phép người dùng thỏa sức sáng tạo trong nhiếp ảnh. Hiệu năng được đảm bảo bởi chip MediaTek Dimensity 920 và RAM lên đến 8GB, đáp ứng tốt các nhu cầu từ làm việc đến giải trí. Pin dung lượng 5000 mAh kèm theo công nghệ sạc nhanh giúp kéo dài thời gian sử dụng, đảm bảo hiệu suất trong suốt ngày dài. Máy cũng hỗ trợ kết nối 5G, cho phép trải nghiệm internet tốc độ cao.','note13_vang.jpg', 'redmi-note-13-thongso.jpg'),
('SP029', 'Xiaomi Redmi Note 13', 'TH003', 'Android 14, MIUI 15', '16 MP', N'50 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.67" - Tần số quét 120 Hz, AMOLED', 'BN002', 'RAM003', 'MediaTek Dimensity 920', '5000 mAh', N'Khung nhôm, mặt lưng kính', N'Dài 164.2 mm - Ngang 76.1 mm - Dày 8.3 mm - Nặng 199 g', '10/2024', 'MAU001', 100, 5990000, 6490000, N'Xiaomi Redmi Note 13 là mẫu điện thoại thuộc dòng Redmi Note với nhiều nâng cấp đáng giá. Máy được trang bị màn hình AMOLED 6.67 inch hỗ trợ tần số quét 120 Hz, mang lại trải nghiệm hiển thị sống động và mượt mà. Camera chính 50 MP cùng với các camera phụ hỗ trợ chụp ảnh từ góc rộng đến macro, cho phép người dùng thỏa sức sáng tạo trong nhiếp ảnh. Hiệu năng được đảm bảo bởi chip MediaTek Dimensity 920 và RAM lên đến 8GB, đáp ứng tốt các nhu cầu từ làm việc đến giải trí. Pin dung lượng 5000 mAh kèm theo công nghệ sạc nhanh giúp kéo dài thời gian sử dụng, đảm bảo hiệu suất trong suốt ngày dài. Máy cũng hỗ trợ kết nối 5G, cho phép trải nghiệm internet tốc độ cao.','note13_den.jpg', 'redmi-note-13-thongso.jpg'),
('SP030', 'Xiaomi Redmi Note 13', 'TH003', 'Android 14, MIUI 15', '16 MP', N'50 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.67" - Tần số quét 120 Hz, AMOLED', 'BN002', 'RAM003', 'MediaTek Dimensity 920', '5000 mAh', N'Khung nhôm, mặt lưng kính', N'Dài 164.2 mm - Ngang 76.1 mm - Dày 8.3 mm - Nặng 199 g', '10/2024', 'MAU009', 100, 5990000, 6490000, N'Xiaomi Redmi Note 13 là mẫu điện thoại thuộc dòng Redmi Note với nhiều nâng cấp đáng giá. Máy được trang bị màn hình AMOLED 6.67 inch hỗ trợ tần số quét 120 Hz, mang lại trải nghiệm hiển thị sống động và mượt mà. Camera chính 50 MP cùng với các camera phụ hỗ trợ chụp ảnh từ góc rộng đến macro, cho phép người dùng thỏa sức sáng tạo trong nhiếp ảnh. Hiệu năng được đảm bảo bởi chip MediaTek Dimensity 920 và RAM lên đến 8GB, đáp ứng tốt các nhu cầu từ làm việc đến giải trí. Pin dung lượng 5000 mAh kèm theo công nghệ sạc nhanh giúp kéo dài thời gian sử dụng, đảm bảo hiệu suất trong suốt ngày dài. Máy cũng hỗ trợ kết nối 5G, cho phép trải nghiệm internet tốc độ cao.','note13_vang.jpg', 'redmi-note-13-thongso.jpg'),

-- Xiaomi Redmi Note 13 Pro
('SP031', 'Xiaomi Redmi Note 13 Pro', 'TH003', 'Android 14, MIUI 15', '20 MP', N'108 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.67" - Tần số quét 120 Hz, AMOLED', 'BN001', 'RAM003', 'Qualcomm Snapdragon 7 Gen 2', '5160 mAh', N'Khung nhôm, mặt lưng kính cường lực', N'Dài 164.3 mm - Ngang 76.7 mm - Dày 8.4 mm - Nặng 204 g', '10/2024', 'MAU001', 100, 7290000, 7790000, N'Xiaomi Redmi Note 13 Pro là phiên bản nâng cấp mạnh mẽ trong dòng Redmi Note với nhiều tính năng tiên tiến. Máy sở hữu màn hình AMOLED 6.67 inch, tần số quét 120 Hz cho trải nghiệm hình ảnh mượt mà và chất lượng hiển thị sắc nét. Hệ thống camera 108 MP mang đến khả năng chụp ảnh chi tiết cao, kết hợp với camera góc siêu rộng 8 MP và camera macro 2 MP, giúp người dùng linh hoạt trong việc chụp ảnh từ nhiều góc độ khác nhau. Vi xử lý Snapdragon 7 Gen 2 mạnh mẽ đảm bảo hiệu năng vượt trội cho các tác vụ nặng và chơi game. Dung lượng pin 5160 mAh lớn kèm theo sạc nhanh, hỗ trợ thời gian sử dụng dài và giảm thiểu thời gian sạc. Redmi Note 13 Pro cũng hỗ trợ kết nối 5G, cho phép người dùng truy cập internet tốc độ cao.','note13pro_den.jpg', 'xiaomi-redmi-note-13-pro-thongso.jpg'),
('SP032', 'Xiaomi Redmi Note 13 Pro', 'TH003', 'Android 14, MIUI 15', '20 MP', N'108 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.67" - Tần số quét 120 Hz, AMOLED', 'BN001', 'RAM003', 'Qualcomm Snapdragon 7 Gen 2', '5160 mAh', N'Khung nhôm, mặt lưng kính cường lực', N'Dài 164.3 mm - Ngang 76.7 mm - Dày 8.4 mm - Nặng 204 g', '10/2024', 'MAU008', 100, 7290000, 7790000, N'Xiaomi Redmi Note 13 Pro là phiên bản nâng cấp mạnh mẽ trong dòng Redmi Note với nhiều tính năng tiên tiến. Máy sở hữu màn hình AMOLED 6.67 inch, tần số quét 120 Hz cho trải nghiệm hình ảnh mượt mà và chất lượng hiển thị sắc nét. Hệ thống camera 108 MP mang đến khả năng chụp ảnh chi tiết cao, kết hợp với camera góc siêu rộng 8 MP và camera macro 2 MP, giúp người dùng linh hoạt trong việc chụp ảnh từ nhiều góc độ khác nhau. Vi xử lý Snapdragon 7 Gen 2 mạnh mẽ đảm bảo hiệu năng vượt trội cho các tác vụ nặng và chơi game. Dung lượng pin 5160 mAh lớn kèm theo sạc nhanh, hỗ trợ thời gian sử dụng dài và giảm thiểu thời gian sạc. Redmi Note 13 Pro cũng hỗ trợ kết nối 5G, cho phép người dùng truy cập internet tốc độ cao.', 'note13pro_xanhla.jpg', 'xiaomi-redmi-note-13-pro-thongso.jpg'),
('SP033', 'Xiaomi Redmi Note 13 Pro', 'TH003', 'Android 14, MIUI 15', '20 MP', N'108 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.67" - Tần số quét 120 Hz, AMOLED', 'BN002', 'RAM003', 'Qualcomm Snapdragon 7 Gen 2', '5160 mAh', N'Khung nhôm, mặt lưng kính cường lực', N'Dài 164.3 mm - Ngang 76.7 mm - Dày 8.4 mm - Nặng 204 g', '10/2024', 'MAU001', 100, 7990000, 8490000, N'Xiaomi Redmi Note 13 Pro là phiên bản nâng cấp mạnh mẽ trong dòng Redmi Note với nhiều tính năng tiên tiến. Máy sở hữu màn hình AMOLED 6.67 inch, tần số quét 120 Hz cho trải nghiệm hình ảnh mượt mà và chất lượng hiển thị sắc nét. Hệ thống camera 108 MP mang đến khả năng chụp ảnh chi tiết cao, kết hợp với camera góc siêu rộng 8 MP và camera macro 2 MP, giúp người dùng linh hoạt trong việc chụp ảnh từ nhiều góc độ khác nhau. Vi xử lý Snapdragon 7 Gen 2 mạnh mẽ đảm bảo hiệu năng vượt trội cho các tác vụ nặng và chơi game. Dung lượng pin 5160 mAh lớn kèm theo sạc nhanh, hỗ trợ thời gian sử dụng dài và giảm thiểu thời gian sạc. Redmi Note 13 Pro cũng hỗ trợ kết nối 5G, cho phép người dùng truy cập internet tốc độ cao.', 'note13pro_den.jpg', 'xiaomi-redmi-note-13-pro-thongso.jpg'),
('SP034', 'Xiaomi Redmi Note 13 Pro', 'TH003', 'Android 14, MIUI 15', '20 MP', N'108 MP chính, 8 MP góc siêu rộng, 2 MP macro', N'6.67" - Tần số quét 120 Hz, AMOLED', 'BN002', 'RAM003', 'Qualcomm Snapdragon 7 Gen 2', '5160 mAh', N'Khung nhôm, mặt lưng kính cường lực', N'Dài 164.3 mm - Ngang 76.7 mm - Dày 8.4 mm - Nặng 204 g', '10/2024', 'MAU008', 100, 7990000, 8490000, N'Xiaomi Redmi Note 13 Pro là phiên bản nâng cấp mạnh mẽ trong dòng Redmi Note với nhiều tính năng tiên tiến. Máy sở hữu màn hình AMOLED 6.67 inch, tần số quét 120 Hz cho trải nghiệm hình ảnh mượt mà và chất lượng hiển thị sắc nét. Hệ thống camera 108 MP mang đến khả năng chụp ảnh chi tiết cao, kết hợp với camera góc siêu rộng 8 MP và camera macro 2 MP, giúp người dùng linh hoạt trong việc chụp ảnh từ nhiều góc độ khác nhau. Vi xử lý Snapdragon 7 Gen 2 mạnh mẽ đảm bảo hiệu năng vượt trội cho các tác vụ nặng và chơi game. Dung lượng pin 5160 mAh lớn kèm theo sạc nhanh, hỗ trợ thời gian sử dụng dài và giảm thiểu thời gian sạc. Redmi Note 13 Pro cũng hỗ trợ kết nối 5G, cho phép người dùng truy cập internet tốc độ cao.', 'note13pro_xanhla.jpg', 'xiaomi-redmi-note-13-pro-thongso.jpg');



--Thêm tài khoản
INSERT INTO TaiKhoan (MaTaiKhoan, MatKhau, Ten, SDT, DiaChi, MaQuyen) VALUES
('chien123','123456',N'Hoàng Hữu Chiến','0923872167',N'Hà Nội',2),
('dai123','123456',N'Hoàng Văn Đại','0923872162',N'Hà Nội',2);

INSERT INTO HD_BanHang (MaHD, MaTaiKhoan, Ten, SDT, DiaChi, NgayBan, TongTien, MaTrangThai) VALUES
('HD001', 'chien123', N'Hoàng Hữu Chiến','0923872167',N'Hà Nội', '2024-10-01', 0, 'TT003'),  -- Tổng tiền sẽ được tính lại từ CT_HD_BanHang
('HD002', 'chien123', N'Hoàng Hữu Chiến','0923872167',N'Hà Nội', '2024-10-02', 0, 'TT003'),
('HD003', 'dai123',N'Hoàng Văn Đại','0923872162',N'Hà Nội', '2024-10-03', 0, 'TT002'),
('HD004', 'dai123',N'Hoàng Văn Đại','0923872162',N'Hà Nội', '2024-10-04', 0, 'TT001'),
('HD005', 'chien123', N'Hoàng Hữu Chiến','0923872167',N'Hà Nội', '2024-10-05', 0, 'TT002'),
('HD006', 'dai123',N'Hoàng Văn Đại','0923872162',N'Hà Nội', '2024-10-06', 0, 'TT001');


INSERT INTO CT_HD_BanHang (MaHD, MaSP, Gia, SoLuong) VALUES
('HD001', 'SP002', 25990000, 1),
('HD001', 'SP005', 19790000, 2),
('HD002', 'SP010', 17590000, 1),
('HD003', 'SP015', 29990000, 1),
('HD004', 'SP020', 7990000, 3),
('HD004', 'SP025', 3990000, 1),
('HD005', 'SP030', 5990000, 2),
('HD006', 'SP031', 7290000, 1),
('HD006', 'SP032', 7290000, 1);

UPDATE HD_BanHang
SET TongTien = (
    SELECT SUM(CT_HD_BanHang.Gia * CT_HD_BanHang.SoLuong)
    FROM CT_HD_BanHang
    WHERE CT_HD_BanHang.MaHD = HD_BanHang.MaHD
)
WHERE MaHD IN (SELECT DISTINCT MaHD FROM CT_HD_BanHang);
