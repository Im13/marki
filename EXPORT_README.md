# 🚀 HƯỚNG DẪN EXPORT DATABASE

## Quick Start - Chạy Ngay

```bash
cd /Users/bachnguyenluong/Documents/working/marki
python3 EXPORT_DATABASE.py
```

## Script Sẽ Làm Gì?

✅ Tự động backup seed data hiện tại  
✅ Export tất cả tables từ `marki.db`  
✅ Export tất cả tables từ `identity.db`  
✅ Lưu vào folder: `API/Infrastructure/Data/SeedData/`  
✅ Hiển thị báo cáo chi tiết với màu sắc  

## Output

```
API/Infrastructure/Data/SeedData/
├── backup_20251108_143000/     ← Backup tự động
│   ├── types.json
│   └── ...
├── types.json                   ← Từ marki.db
├── products.json
├── customers.json
├── orders.json
├── province.json
├── district.json
├── ward.json
└── Identity/                    ← Từ identity.db
    └── aspnetusers.json
```

## Test Seed Process

```bash
cd API
dotnet ef database drop
dotnet ef database update
dotnet run
```

## ⚠️ QUAN TRỌNG

1. **PHẢI test seed process trước khi drop production database**
2. **Keep backup ở nhiều nơi** (cloud, external drive)
3. **Verify dữ liệu** sau khi seed

## Kiểm Tra Dữ Liệu

```bash
# Xem số lượng records
cd API/Infrastructure/Data/SeedData
for f in *.json; do echo "$f: $(python3 -c "import json; print(len(json.load(open('$f'))))")"; done

# Preview một file
cat products.json | python3 -m json.tool | head -20
```

## Troubleshooting

**Lỗi: Database not found**
```bash
ls -la API/API/*.db
```

**Lỗi: Permission denied**
```bash
chmod +x EXPORT_DATABASE.py
```

## Success Checklist

- [ ] Script chạy không lỗi
- [ ] Tất cả tables exported
- [ ] Backup folder created
- [ ] JSON files có dữ liệu
- [ ] Test seed trên database mới
- [ ] App chạy OK với dữ liệu mới
- [ ] Backup lưu nhiều nơi

---

**Made for Marki Project** 🎯

*Always backup before dropping database!* 🔒
