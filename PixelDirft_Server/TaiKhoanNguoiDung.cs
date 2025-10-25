using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelDirft_Server
{
    [Table("taikhoannguoidung")]
    public class TaiKhoanNguoiDung: BaseModel
    {
        [PrimaryKey("UserID")]
        public long UserID { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Birthday")]
        public DateTime Birthday { get; set; }
    }
}
