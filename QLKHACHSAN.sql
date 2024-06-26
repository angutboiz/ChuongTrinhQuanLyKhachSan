﻿CREATE DATABASE QLKHACHSAN
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
	cusdate DATE,       
    cusaddress NVARCHAR(255),
	isRemove bit default 0 -- mặc định là hiển thị, khi xóa sẽ ko hiển thị
);

-- Tạo bảng Phòng
CREATE TABLE Room (
    roomid INT IDENTITY(1,1) PRIMARY KEY,
    roomnumber NVARCHAR(20) UNIQUE NOT NULL,
    roomtype NVARCHAR(50) NOT NULL, -- kiểu phòng: phòng vip, phòng thường, phòng quạt, phòng lạnh, phòng đôi,...
    roomrate DECIMAL(10, 0), -- giá tiền theo giờ
	fulldaynight DECIMAL(10, 0),
	fullnight DECIMAL(10, 0),
    roomstatus NVARCHAR(50), -- 3 giá trị: đang sử dụng - phòng trống - bảo trì (dọn phòng...)
);

-- tạo bảng Dịch Vụ
CREATE TABLE Service (
	serid INT IDENTITY(1,1) PRIMARY KEY,
	sername NVARCHAR(100) UNIQUE NOT NULL,
	serprice DECIMAL(10, 0) NOT NULL, -- thu tiền dịch vụ như: nước, bánh, trái cây,....
	sertype NVARCHAR(50) NOT NULL, -- loại dịch vụ: ĐỒ MẶN, ĐỒ ĂN VẶT
)

CREATE TABLE ServiceOrder(
	serdetailid INT IDENTITY(1,1) PRIMARY KEY,
	serid int,
	roomid int,
	serquantity INT, -- số lượng khách mua
	sertotal DECIMAL(10, 0), -- tổng tiền
	FOREIGN KEY (serid) REFERENCES Service (serid),
	FOREIGN KEY (roomid) REFERENCES Room (roomid)
)


-- Tạo bảng Đặt phòng
CREATE TABLE Booking (
    bookid INT IDENTITY(1,1) PRIMARY KEY,
	staffid INT,
    cusid INT,
    roomid INT,
	serdetailid INT,
    booktype NVARCHAR(50), -- loại book: theo ngay theo tháng hay qua đêm
    checkin SMALLDATETIME, -- lưu lại ngày giờ lúc khách đặt phòng
    checkout SMALLDATETIME, -- lưu lại ngày giờ lúc khách trả phòng
    totalhours INT, -- lưu lại tổng giờ thuê 
    bookstatus NVARCHAR(50), -- 2 giá trị: đang thuê phòng hoặc trả phòng
	payamount DECIMAL(10, 0), -- tổng tiền thanh toán
    FOREIGN KEY (staffid) REFERENCES Staff(staffid),
    FOREIGN KEY (cusid) REFERENCES Customer(cusid),
    FOREIGN KEY (roomid) REFERENCES Room(roomid),
	FOREIGN KEY (serdetailid) REFERENCES ServiceOrder (serdetailid)
);

CREATE TABLE History (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	roomname nvarchar(50),
	roomtype nvarchar(50),
	payamount DECIMAL(10, 0),
	staffname nvarchar(50),
	cusname nvarchar(50),
	cusphone nvarchar(50),
	checkin SMALLDATETIME,
	checkout SMALLDATETIME,
	totalhours int,
	totalamount DECIMAL(10, 0),
)

CREATE TABLE HistoryService (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    historyID INT,
    sername NVARCHAR(100),
    serquantity INT,
    sertotal DECIMAL(10, 0),
    seramount DECIMAL(10, 0),
    FOREIGN KEY (historyID) REFERENCES History(ID)
);

create table RememberLogin (
	 id int  PRIMARY KEY,
	 Username NVARCHAR(50),
	 Password NVARCHAR(50),
	 isCheck bit default 0
)

INSERT INTO RememberLogin (id)
values (1)

INSERT INTO Staff (staffname, staffsex, staffphone, staffdate, staffaddress, Username, Password, Role)
VALUES ('admin', 'Nam', '093764812', '01-01-2003', '123 Main Street', 'admin', 'admin', 'admin')

INSERT INTO Customer (cusname, cusemail, cusphone, cusaddress)
VALUES (N'Văn Thanh', 'alice@example.com', '5551234567', N'Biên Hòa'),
       (N'Long Phạm', 'bob@example.com', '5559876543', 'Bình Dương')

INSERT INTO Room (roomnumber, roomtype, roomrate, roomstatus)
VALUES ('101', 'Superior', 200000, N'Phòng trống'),
       ('102', 'Deluxe Twin Bed', 250000, N'Phòng trống'),
       ('872', 'VIP', 300000, N'Bảo trì'),
       ('382', 'Deluxe King Bed', 300000, N'Phòng trống')

INSERT INTO Service (sername, serprice, sertype)
VALUES	(N'Gà hấp lá chanh', 250000, N'Đồ ăn mặn'),
		(N'Ghẹ hấp', 150000, N'Đồ ăn mặn'),
		(N'Hàu nướng', 10000, N'Đồ ăn mặn'),
		(N'Bim bim', 10000, N'Đồ ăn nhanh'),
		(N'Khoai tây chiên', 22000, N'Đồ ăn nhanh'),
		(N'Bỏng ngô', 25000, N'Đồ ăn nhanh'),
		(N'Hoa quả', 100000, N'Đồ ăn nhanh'),
		(N'Rượu', 300000, N'Nước'),
		(N'Bia', 20000, N'Nước'),
		(N'Cafe', 50000, N'Nước')

	
	select DISTINCT h.* from History as h
	inner join HistoryService as hs on hs.historyID = h.ID
	select * from Booking where roomid = 5