# AppQLNV_DL

## Overview
AppQLNV_DL is a cross-platform mobile application built with **.NET MAUI**, designed to manage employees and job assignments efficiently.  
The application follows the **MVVM architecture** and communicates with a **RESTful API** to handle business logic and data processing.

AppQLNV_DL là một ứng dụng di động đa nền tảng được xây dựng bằng **.NET MAUI**, nhằm hỗ trợ quản lý nhân viên và phân công công việc một cách hiệu quả. 
Ứng dụng áp dụng kiến trúc **MVVM** và giao tiếp với **RESTful API** để xử lý logic nghiệp vụ và dữ liệu.

The system supports both **employees** and **customers**, allowing managers to assign tasks to employees and enabling customers to place service requests directly through the app.

Hệ thống hỗ trợ cả **nhân viên** và **khách hàng**, cho phép người quản lý giao việc cho nhân viên và khách hàng có thể đặt dịch vụ trực tiếp trên ứng dụng.

---

## Frontend (Mobile App)

The frontend is developed using **.NET MAUI** with the **MVVM pattern**, responsible for:
- User interface rendering
- User interaction handling
- Communication with backend APIs

Frontend được phát triển bằng **.NET MAUI** theo mô hình **MVVM**, chịu trách nhiệm:
- Hiển thị giao diện người dùng  
- Xử lý tương tác người dùng  
- Giao tiếp với Backend thông qua API  

---

## Features

### Authentication
- Login / Logout
- Role-based access (Admin / Employee / Customer)
- Persistent login using local storage

Xác thực người dùng:
- Đăng nhập / Đăng xuất  
- Phân quyền theo vai trò (Admin / Nhân viên / Khách hàng)  
- Lưu trạng thái đăng nhập cục bộ  

---

### Employee Management
- View employee list
- Search and filter employees
- Add / edit / delete employee information

Quản lý nhân viên:
- Xem danh sách nhân viên  
- Tìm kiếm và lọc nhân viên  
- Thêm / chỉnh sửa / xoá thông tin nhân viên  

---

### Job Assignment
- Create and manage job tasks
- Assign tasks to employees
- Track task status (Pending / In Progress / Completed)

Quản lý và phân công công việc:
- Tạo và quản lý các công việc  
- Phân công công việc cho nhân viên  
- Theo dõi trạng thái công việc (Chờ xử lý / Đang thực hiện / Hoàn thành)  

---

### Customer Features
- Customer account registration and login
- View available services
- Place job requests directly through the app
- Track assigned jobs and job status

Chức năng dành cho khách hàng:
- Đăng ký và đăng nhập tài khoản khách hàng  
- Xem danh sách dịch vụ  
- Đặt công việc/dịch vụ trực tiếp trên ứng dụng  
- Theo dõi công việc và trạng thái xử lý  

---

## Technologies
- **.NET MAUI**
- **XAML**
- **MVVM Pattern**
- **CommunityToolkit.Mvvm**
- **HttpClient**
- **RESTful API**

Công nghệ sử dụng:
- .NET MAUI  
- XAML  
- Kiến trúc MVVM  
- CommunityToolkit.Mvvm  
- Giao tiếp HTTP với RESTful API  

---
  
## Folder Structure

- `Views/`  
  Chứa các màn hình giao diện (XAML)

- `ViewModels/`  
  Chứa logic xử lý và data binding theo mô hình MVVM

- `Models/`  
  Chứa các lớp mô hình dữ liệu (Entities, DTOs)

- `Services/`  
  Chứa các lớp gọi API và xử lý dữ liệu từ backend
