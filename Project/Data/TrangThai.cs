using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class TrangThai
{
    public string MaTrangThai { get; set; } = null!;

    public string TenTrangThai { get; set; } = null!;

    public virtual ICollection<HdBanHang> HdBanHangs { get; set; } = new List<HdBanHang>();
}
