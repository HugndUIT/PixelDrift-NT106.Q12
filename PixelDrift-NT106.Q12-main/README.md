# PixelDrift-NT106.Q12
Đồ án lập trình mạng căn bản  

24520602 - Nguyễn Duy Hưng - HugndUIT  
24520596 - Lê Tiến Hưng - huwngzxje  
24520587 - Hoàng Phi Hùng - phihung-207   
24520549 - Ngô Xuân Minh Hoàng - Mhoang1302  
24520254 - Nguyễn Hoàng Khánh Đăng - Deep-sea-whale  

Đây là bài tập nhóm xây dựng ứng dụng đăng ký – đăng nhập người dùng bằng C# WinForms và SQL Server.  
Ứng dụng cho phép người dùng thực hiện:  
-	Đăng ký tài khoản mới với các kiểm tra hợp lệ:  
+ Định dạng email hoặc số điện thoại.  
+ Kiểm tra độ mạnh mật khẩu.  
+ Xác nhận mật khẩu trùng khớp.  
+ Mã hóa mật khẩu (SHA256) trước khi lưu vào cơ sở dữ liệu.  
-	Đăng nhập bằng tài khoản đã đăng ký.  
-	Hiển thị thông tin người dùng sau khi đăng nhập thành công.  
Hệ thống sử dụng cơ sở dữ liệu QlyNguoiDung với bảng Users (gồm: Id, Username, Password, Email).  
Mục tiêu của bài tập là giúp sinh viên rèn luyện kỹ năng làm việc nhóm, sử dụng Git/GitHub, và lập trình ứng dụng có kết nối CSDL thực tế.  
Cài đặt và sử dụng:
-	Ở Form mở đầu, ta sẽ tạo 3 nút đăng kí, đăng nhập và thoát game. Sau đó người dùng sẽ ấn vào nút có chức năng tương ứng với cầu, nếu người dùng chưa có tài khoản ta sẽ gợi ý người dùng sử dụng chức năng đăng kí trước, còn nếu có rồi thì sử dụng chức năng đăng nhập . Cuối cùng nếu muốn tắt ứng dụng thì sử dụng nút thoát game.  
-	Ở Form đăng kí, ta sẽ tạo các trường gồm: email hoặc số điện thoại , tên đăng nhập, mật khẩu và xác nhận mật khẩu . Người dùng khi đăng kí phải điền hết các thông tin yêu cầu trên. Sau khi nhập hết dữ liệu xong , người dùng phải dùng nút xác nhận để đăng kí tài khoản. Sau đó, chương trình sẽ kiểm tra email, tên đăng nhập đã có trong cơ sở dữ liệu chưa , mật khẩu có đạt độ mạnh hay không và nếu thõa hết các điều kiện trên thì sẽ đăng kí thành công và chuyển tiếp tới Form đăng nhập  
-	Ở Form đăng nhập , ta tạo các trường gồm các trường email hoặc số điện thoại và mật khẩu. Người dùng sẽ nhập thông tin đã đăng kí và dùng nút đăng nhập. Sau đó, chương trình sẽ lấy thông tin từ cơ sở dữ liệu và đối chứng xem có trùng khớp với thông tin người dùng nhập không. Nếu có thì đăng nhập thành công, còn không thì báo lỗi  
-	Ở Form hiển thị thông tin, sẽ hiển thị 3 trường dữ liệu gồm: ID, Username, Email/Sđt của người dùng.  

Giao diện ứng dụng:  
- Form đăng kí: bao gồm Email hoặc số điện thoại, người dùng có thể chọn 1 trong 2 để nhập, Tên đăng nhập, người dùng nhập tên để hiện thị trong game sau này, Mật khẩu và xác nhận mật khẩu, nơi người dùng nhập mât khẩu và xác nhận mật khẩu của mình.  
- Form đăng nhập: bao gồm Email/số điện thoại dể người dùng nhập vào để đăng nhập vô game, sau đó nhập vào ô mật khẩu.  
- Form hiển thị thông tin: hiển thị thông tin như id, tên đăng nhập, email/số điện thoại.  
- Form mở đầu: mở đầu game, cho phép người dùng đăng nhập hoặc đăng kí nếu chưa có tài khoản.  
