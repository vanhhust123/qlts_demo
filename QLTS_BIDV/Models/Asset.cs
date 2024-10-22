using Newtonsoft.Json;
using QLTS_BIDV.Helper.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QLTS_BIDV.Models
{
    public class Asset
    {
        public string MaTs { set; get; }
        public string TenTs { set; get; }
        public string NamSx { set; get; }
        public string SerialTs { set; get; }
        public string LoaiTs { set; get; }
        public int NguyenGiaTs { set; get; }
        public string VitriTs { set; get; }
        public string MoTaTs { set; get; }
        public string PhongBanTs { set; get; }
        public bool IsActive { set; get; }
        public bool IsCheckStatus { set; get; }
        public AssetInventoryStatus AssetInventoryStatus { set; get; }

        [JsonIgnore]
        public string AssetInventoryStatusText
        {
            get => this.AssetInventoryStatus.GetAttribute<DisplayAttribute>().Name;
            set { }
        }

    }

    public enum AssetInventoryStatus
    {
        [Display(Name = "Chưa kiểm kê")]
        ChuaKiemKe = 0,
        [Display(Name = "Đã kiểm kê")]
        DaKiemKe = 1
    }

    public class AssetInventory
    {
        public List<Asset> Asset { set; get; } = new List<Asset>() { };
        public Inventory Inventory { set; get; } = new Inventory() { };
    }
}
