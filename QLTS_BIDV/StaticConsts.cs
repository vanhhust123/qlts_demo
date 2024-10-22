using QLTS_BIDV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QLTS_BIDV
{
    public class StaticConsts
    {
        static public string SyncfusionToken { set; get; } = "NDg4Mzc2QDMxMzkyZTMyMmUzMEpCWDdjd3llZVI1T1RjRmNxbEZ3b29Tdm5DMkJLNHRwNTVaU3ptYU5NTHM9";

        static public List<Asset> Assets { get; set; } = new List<Asset>()
        {
            new Asset(){
                MaTs = "230824TM-007-42", TenTs = "Hộp chữ nhật INOX",
                NamSx = "2017",
                PhongBanTs = "Phòng kỹ thuật",
                IsActive = true,
                IsCheckStatus = true, LoaiTs = "TSC",
                NguyenGiaTs = 300000, VitriTs = "Số 2/2 Nguyễn Hoàng, Mỹ Đình 1, Hà Nội",
                SerialTs = "24DK-007", MoTaTs = "TSC Description", AssetInventoryStatus = AssetInventoryStatus.ChuaKiemKe
            }, new Asset(){
                MaTs = "230124TM-004-42", TenTs = "Màn hình",
                NamSx = "2019",
                PhongBanTs = "Phòng kỹ thuật",
                IsActive = true,
                IsCheckStatus = false, LoaiTs = "TSCD",
                NguyenGiaTs = 3000000, VitriTs = "Số 2/2 Nguyễn Hoàng, Mỹ Đình 1, Hà Nội",
                SerialTs = "24VM-007", MoTaTs = "TSCD Description",
                AssetInventoryStatus = AssetInventoryStatus.ChuaKiemKe
            }, new Asset(){
                MaTs = "120831GM-007-42", TenTs = "Ổ cứng",
                NamSx = "2020",
                PhongBanTs = "Phòng kỹ thuật",
                IsActive = false,
                IsCheckStatus = false, LoaiTs = "TSC",
                NguyenGiaTs = 300000, VitriTs = "Số 2/2 Nguyễn Hoàng, Mỹ Đình 1, Hà Nội",
                SerialTs = "26TM-007", MoTaTs = "Ổ cứng 500GB",
                AssetInventoryStatus = AssetInventoryStatus.ChuaKiemKe
            },new Asset(){
                MaTs = "240824HN-007-42", TenTs = "Máy chiếu",
                NamSx = "2022",
                PhongBanTs = "Phòng kế toán",
                IsActive = true,
                IsCheckStatus = false, LoaiTs = "TSC",
                NguyenGiaTs = 5000000, VitriTs = "Số 2/2 Nguyễn Hoàng, Mỹ Đình 1, Hà Nội",
                SerialTs = "24AP-007", MoTaTs = "Máy chiếu treo tường",
                AssetInventoryStatus = AssetInventoryStatus.ChuaKiemKe
            }, new Asset(){
                MaTs = "230824AB-019-47", TenTs = "Bàn tròn",
                NamSx = "2017",
                PhongBanTs = "Phòng kế toán",
                IsActive = true,
                IsCheckStatus = true, LoaiTs = "TSC",
                NguyenGiaTs = 300000, VitriTs = "Số 2/2 Nguyễn Hoàng, Mỹ Đình 1, Hà Nội",
                SerialTs = "24XM-007", MoTaTs = "TSC Description",
                AssetInventoryStatus = AssetInventoryStatus.ChuaKiemKe
            }
        };

        private static List<Inventory> Inventory { set; get; } = new List<Inventory>()
        {
                new Inventory() {
                    Id = "1",
                    Code = "KK23-000219", Name = "Kiểm kê 2023 – Đợt 1", Period = "01-2023", Date = "31-01-2023",
                    IsCheck = false, Status = InventoryStatus.ChuaThucHien
                },
                new Inventory() {
                    Id = "2",
                    Code = "KK23-000210", Name = "Kiểm kê 2023 – Đợt 2", Period = "04-2023", Date = "30-04-2023",
                    IsCheck = false, Status = InventoryStatus.ChuaThucHien
                }, new Inventory() {
                    Id = "3",
                    Code = "KK23-000119", Name = "Kiểm kê 2023 – Đợt 3", Period = "09-2023", Date = "30-09-2023",
                    IsCheck = false, Status = InventoryStatus.ChuaThucHien
                },new Inventory() {
                    Id = "4",
                    Code = "KK23-000120", Name = "Kiểm kê 2023 – Đợt 4", Period = "12-2023", Date = "31-12-2023",
                    IsCheck = false, Status = InventoryStatus.ChuaThucHien
                }
        };


        static public List<Inventory> GetInventories()
        {
            return StaticConsts.Inventory.OrderByDescending(x => x.Id).ToList();
        }


        // For mock data 
        static public List<AssetInventory> GlobalMockData { set; get; } = new List<AssetInventory>();

        public static void AddInventoryAsset(string inventoryCode, Asset asset)
        {
            // Change trạng thái
            Asset assetFound = StaticConsts.Assets.FirstOrDefault(x => x.MaTs == asset.MaTs);
            assetFound.AssetInventoryStatus = AssetInventoryStatus.DaKiemKe;

            Inventory inventoryFound = StaticConsts.Inventory.FirstOrDefault(x => x.Code == inventoryCode);
            if (inventoryFound.Status == InventoryStatus.ChuaThucHien) inventoryFound.Status = InventoryStatus.DangThucHien;

            AssetInventory assetInventoryCheck = StaticConsts.GlobalMockData.FirstOrDefault(x => x.Inventory.Code == inventoryCode);
            // Không có thì thêm mới. 
            var newAssetInventory = new AssetInventory()
            {
                Inventory = inventoryFound,
                Asset = new List<Asset>() { assetFound }
            };
            if (assetInventoryCheck is null) StaticConsts.GlobalMockData.Add(newAssetInventory);
            else // Có rồi thì tiến hành cập nhật
            {
                Asset assetCheck = assetInventoryCheck.Asset.FirstOrDefault(x => x.MaTs == assetFound.MaTs);
                if (assetCheck is null)
                    assetInventoryCheck.Asset.Add(assetFound);
            }
        }

        public static List<Asset> GetAssetOfPeriod(string inventoryCode)
        {
            AssetInventory assetInventoryCheck = StaticConsts.GlobalMockData.FirstOrDefault(x => x.Inventory.Code == inventoryCode);
            if (assetInventoryCheck is null) return new List<Asset>();
            return assetInventoryCheck.Asset; 
        }

    }
}
