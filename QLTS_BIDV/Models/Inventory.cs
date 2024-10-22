using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using QLTS_BIDV.Helper.Extensions; 

namespace QLTS_BIDV.Models
{
    public class Inventory
    {
        public string Id { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Period { set; get; }
        public string Date { set; get; }
        public bool IsCheck { set; get; }
        public string StatusText { get => this.Status.GetAttribute<DisplayAttribute>().Name; set { } }
        public InventoryStatus Status { set; get; }
    }

    public class InventoryDetail
    {
        public string Id { set; get; }
        public string InventoryId { set; get; }
        public string AssetNumber { set; get; }
        public string Description { set; get; }
        public string Quantity { set; get; }
        public string RealQuantity { set; get; }
        public bool IsCheck { set; get; }
    }

    public enum InventoryStatus
    {
        [Display(Name = "Chưa thực hiện")]
        ChuaThucHien = 0,
        [Display(Name = "Đang thực hiện")]
        DangThucHien = 1,
        [Display(Name = "Đã thực hiện")]
        DaThucHien = 2
    }
}
