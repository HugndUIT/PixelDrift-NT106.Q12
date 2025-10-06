--Tao Database
CREATE DATABASE QlyNguoiDung

--Tao bang Users
CREATE TABLE Users
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL,
    Password NVARCHAR(64) NOT NULL,
    Email NVARCHAR(100) NOT NULL
)
