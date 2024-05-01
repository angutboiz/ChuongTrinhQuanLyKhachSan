CREATE DATABASE QLKHACHSAN
DROP DATABASE QLKHACHSAN

-- Tạo bảng Nhân Viên
CREATE TABLE Staff (
    staffid INT PRIMARY KEY IDENTITY(1,1),
    staffname NVARCHAR(50) NOT NULL,
	staffsex NVARCHAR(20),
    staffphone NVARCHAR(10) UNIQUE NOT NULL,
	staffdate DATE,                  
    staffaddress NVARCHAR(100),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
	isRemove bit default 0 -- mặc định là hiển thị, khi xóa sẽ ko hiển thị
);


-- Tạo bảng Khách hàng
CREATE TABLE Customer (
    cusid INT IDENTITY(1,1) PRIMARY KEY,
    cusname NVARCHAR(100) NOT NULL,
    cusemail NVARCHAR(100),
    cusphone NVARCHAR(10) UNIQUE NOT NULL,
	cusdate SMALLDATETIME,       
    cusaddress NVARCHAR(255),
	isRemove bit default 0 -- mặc định là hiển thị, khi xóa sẽ ko hiển thị
);

-- Tạo bảng Phòng
CREATE TABLE Room (
    roomid INT IDENTITY(1,1) PRIMARY KEY,
    roomnumber NVARCHAR(20) UNIQUE NOT NULL,
    roomtype NVARCHAR(50) NOT NULL, -- kiểu phòng: phòng vip, phòng thường, phòng quạt, phòng lạnh, phòng đôi,...
    roomrate DECIMAL(10, 0), -- giá tiền
    roomstatus NVARCHAR(50), -- 3 giá trị: đang sử dụng - phòng trống - bảo trì (dọn phòng...)
	isRemove bit default 0 -- mặc định là hiển thị, khi xóa sẽ ko hiển thị
);

-- tạo bảng Dịch Vụ
CREATE TABLE Service (
	serid INT IDENTITY(1,1) PRIMARY KEY,
	sername NVARCHAR(100),
	serquantity INT, -- số lượng khách hàng sử dụng
	sertax DECIMAL(10, 0), -- thu phí thêm các dịch vụ như: nước, bánh, trái cây,....
)

-- Tạo bảng Đặt phòng
CREATE TABLE Booking (
    bookid INT IDENTITY(1,1) PRIMARY KEY,
	staffid INT,
    cusid INT,
    roomid INT,
	serid INT,
    checkin SMALLDATETIME, -- lưu lại ngày giờ lúc khách đặt phòng
    checkout SMALLDATETIME, -- lưu lại ngày giờ lúc khách trả phòng
    bookstatus NVARCHAR(50), -- 2 giá trị: đang thuê phòng hoặc trả phòng
	payamount DECIMAL(10, 0), -- tổng tiền thanh toán
    paymethod NVARCHAR(50), -- hình thức thanh toán: chuyển khoản - tiền mặt?
    FOREIGN KEY (staffid) REFERENCES Staff(staffid),
    FOREIGN KEY (cusid) REFERENCES Customer(cusid),
    FOREIGN KEY (roomid) REFERENCES Room(roomid),
	FOREIGN KEY (serid) REFERENCES Service (serid)
);



-- Dữ liệu giả định cho bảng Nhân viên
INSERT INTO Staff (staffname, staffsex, staffphone, staffdate, staffaddress, Username, Password, Role)
VALUES ('admin', 'Nam', '093764812', '01-01-2003', '123 Main Street', 'admin', 'admin', 'admin'

-- Dữ liệu giả định cho bảng Khách hàng
INSERT INTO Customer (cusname, cusemail, cusphone, cusaddress)
VALUES ('Alice Johnson', 'alice@example.com', '5551234567', '789 Oak Avenue'),
       ('Bob Williams', 'bob@example.com', '5559876543', '321 Pine Street');

-- Dữ liệu giả định cho bảng Phòng
INSERT INTO Room (roomnumber, roomtype, roomrate, roomstatus)
VALUES ('101', 'Standard', 100, N'Đang sử dụng'),
       ('102', 'Deluxe', 150, N'Bảo trì'),
       ('103', 'Suite', 200, N'Phòng trống'); 

-- Dữ liệu giả định cho bảng Đặt phòng
INSERT INTO Booking (staffid, cusid, roomid, checkin, checkout, bookstatus)
VALUES (1, 1, 1, '2024-04-01', '2024-04-03', 'Checked In'), -- Nhân viên John Doe đặt phòng cho khách hàng Alice Johnson
       (2, 2, 2, '2024-04-05', '2024-04-08', 'Checked In'); -- Nhân viên Jane Smith đặt phòng cho khách hàng Bob Williams

-- Dữ liệu giả định cho bảng Dịch vụ
INSERT INTO Service (sername, serquantity, sertax)
VALUES ('Breakfast', 1, 10),
       ('Laundry', 2, 20);

