# Hướng dẫn Deploy Marki lên VPS với SQLite

Hướng dẫn này sẽ giúp bạn deploy ứng dụng Marki lên VPS sử dụng SQLite thay vì SQL Server.

## 📋 Yêu cầu hệ thống

- VPS với Ubuntu 20.04+ hoặc Debian 11+
- Tối thiểu 2GB RAM, 2 CPU cores
- 20GB+ dung lượng ổ cứng
- Domain name (tùy chọn, có thể dùng IP)

## 🔧 Bước 1: Chuẩn bị VPS

### 1.1. Cập nhật hệ thống
```bash
sudo apt update && sudo apt upgrade -y
```

### 1.2. Cài đặt .NET 9.0 SDK
```bash
# Thêm Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Cài đặt .NET SDK
sudo apt-get update
sudo apt-get install -y dotnet-sdk-9.0
```

### 1.3. Cài đặt Node.js và npm
```bash
# Cài đặt Node.js 20.x
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt-get install -y nodejs

# Cài đặt Angular CLI
sudo npm install -g @angular/cli
```

### 1.4. Cài đặt Nginx
```bash
sudo apt-get install -y nginx
```

### 1.5. Cài đặt Redis (cho giỏ hàng)
```bash
sudo apt-get install -y redis-server
sudo systemctl enable redis-server
sudo systemctl start redis-server
```

## 📦 Bước 2: Build và Deploy ứng dụng

### 2.1. Upload code lên VPS
```bash
# Trên máy local, tạo file tar
cd /path/to/marki
tar -czf marki.tar.gz --exclude='node_modules' --exclude='bin' --exclude='obj' .

# Upload lên VPS (thay YOUR_VPS_IP và USERNAME)
scp marki.tar.gz username@YOUR_VPS_IP:/home/username/
```

### 2.2. Trên VPS, giải nén và build
```bash
# Tạo thư mục cho ứng dụng
sudo mkdir -p /var/www/marki
sudo chown $USER:$USER /var/www/marki

# Giải nén
cd /var/www/marki
tar -xzf ~/marki.tar.gz

# Build Admin Angular app
cd Admin
npm install
ng build --configuration production --output-path=../build-output/admin

# Build ClientUI Angular app
cd ../ClientUI
npm install
ng build --configuration production --output-path=../build-output/client-ui

# Build .NET API
cd ../API/API
dotnet restore
dotnet publish -c Release -o /var/www/marki/publish
```

### 2.3. Copy Angular builds vào thư mục publish
```bash
# Copy Angular builds vào wwwroot
cp -r /var/www/marki/build-output/admin/* /var/www/marki/publish/wwwroot/admin/
cp -r /var/www/marki/build-output/client-ui/* /var/www/marki/publish/wwwroot/
```

## ⚙️ Bước 3: Cấu hình ứng dụng

### 3.1. Tạo thư mục cho database
```bash
sudo mkdir -p /var/www/marki/data
sudo chown $USER:$USER /var/www/marki/data
```

### 3.2. Cập nhật appsettings.json
```bash
cd /var/www/marki/publish
nano appsettings.json
```

Cập nhật nội dung:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=/var/www/marki/data/marki.db",
    "Redis": "localhost:6379"
  },
  "Token": {
    "Key": "YOUR_SUPER_SECRET_KEY_MIN_32_CHARACTERS_LONG_CHANGE_THIS_IN_PRODUCTION",
    "Issuer": "https://yourdomain.com"
  },
  "ApiUrl": "https://yourdomain.com/",
  "Cors": {
    "AllowedOrigins": [
      "https://yourdomain.com",
      "https://www.yourdomain.com"
    ]
  },
  "CloudinarySettings": {
    "CloudName": "your_cloud_name",
    "ApiKey": "your_api_key",
    "ApiSecret": "your_api_secret"
  },
  "FacebookSettings": {
    "AccessToken": "your_facebook_token",
    "AdAccountId": "your_ad_account_id"
  },
  "RoleSettings": {
    "SuperAdmin": {
      "Name": "SuperAdmin",
      "DisplayName": "Super Administrator",
      "Description": "Full system access",
      "Permissions": ["*"]
    },
    "Admin": {
      "Name": "Admin", 
      "DisplayName": "Administrator",
      "Description": "Administrative access",
      "Permissions": ["ManageOrders", "ViewReports", "ManageUsers"]
    },
    "Employee": {
      "Name": "Employee",
      "DisplayName": "Employee", 
      "Description": "Limited access",
      "Permissions": ["ViewOrders", "UpdateOrderStatus"]
    },
    "Customer": {
      "Name": "Customer",
      "DisplayName": "Customer",
      "Description": "Customer access", 
      "Permissions": ["PlaceOrder", "ViewOwnOrders"]
    },
    "OrderNotificationRoles": ["SuperAdmin", "Admin"],
    "OrderManagementRoles": ["SuperAdmin", "Admin"]
  }
}
```

**Lưu ý quan trọng:**
- Thay `YOUR_SUPER_SECRET_KEY_MIN_32_CHARACTERS_LONG_CHANGE_THIS_IN_PRODUCTION` bằng một key bí mật mạnh (ít nhất 32 ký tự)
- Thay `yourdomain.com` bằng domain của bạn hoặc IP VPS
- Cập nhật Cloudinary và Facebook settings nếu cần

### 3.3. Chạy migrations để tạo database
```bash
cd /var/www/marki/publish
export ASPNETCORE_ENVIRONMENT=Production
export ASPNETCORE_URLS="http://localhost:5000"
dotnet API.dll --migrate
```

Hoặc nếu cần chạy migration thủ công:
```bash
cd /var/www/marki/API/API
export ASPNETCORE_ENVIRONMENT=Production
dotnet ef database update --project ../Infrastructure/Infrastructure.csproj --startup-project .
```

## 🔄 Bước 4: Tạo Systemd Service

### 4.1. Tạo service file
```bash
sudo nano /etc/systemd/system/marki.service
```

Thêm nội dung:
```ini
[Unit]
Description=Marki API Service
After=network.target redis-server.service

[Service]
Type=notify
User=www-data
WorkingDirectory=/var/www/marki/publish
ExecStart=/usr/bin/dotnet /var/www/marki/publish/API.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=marki-api
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000

[Install]
WantedBy=multi-user.target
```

### 4.2. Khởi động service
```bash
sudo systemctl daemon-reload
sudo systemctl enable marki.service
sudo systemctl start marki.service
sudo systemctl status marki.service
```

## 🌐 Bước 5: Cấu hình Nginx

### 5.1. Tạo Nginx config
```bash
sudo nano /etc/nginx/sites-available/marki
```

Thêm nội dung (thay `yourdomain.com` bằng domain của bạn):
```nginx
server {
    listen 80;
    server_name yourdomain.com www.yourdomain.com;

    # Redirect HTTP to HTTPS (nếu có SSL)
    # return 301 https://$server_name$request_uri;

    # Hoặc nếu chưa có SSL, dùng config này:
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }

    # Static files
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2|ttf|eot)$ {
        proxy_pass http://localhost:5000;
        expires 1y;
        add_header Cache-Control "public, immutable";
    }
}
```

### 5.2. Kích hoạt site
```bash
sudo ln -s /etc/nginx/sites-available/marki /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

## 🔒 Bước 6: Cài đặt SSL với Let's Encrypt (Tùy chọn nhưng khuyến nghị)

```bash
# Cài đặt Certbot
sudo apt-get install -y certbot python3-certbot-nginx

# Lấy certificate (thay yourdomain.com)
sudo certbot --nginx -d yourdomain.com -d www.yourdomain.com

# Certbot sẽ tự động cập nhật Nginx config
```

## 🔍 Bước 7: Kiểm tra và Monitoring

### 7.1. Kiểm tra logs
```bash
# API logs
sudo journalctl -u marki.service -f

# Nginx logs
sudo tail -f /var/log/nginx/error.log
sudo tail -f /var/log/nginx/access.log
```

### 7.2. Kiểm tra database files
```bash
ls -lh /var/www/marki/data/
# Sẽ thấy marki.db (chứa cả store data và identity data)
```

### 7.3. Backup database
Tạo script backup:
```bash
sudo nano /usr/local/bin/backup-marki.sh
```

Thêm nội dung:
```bash
#!/bin/bash
BACKUP_DIR="/var/backups/marki"
DATE=$(date +%Y%m%d_%H%M%S)
mkdir -p $BACKUP_DIR

# Backup SQLite database (chứa cả store data và identity data)
cp /var/www/marki/data/marki.db $BACKUP_DIR/marki_$DATE.db

# Xóa backups cũ hơn 30 ngày
find $BACKUP_DIR -name "*.db" -mtime +30 -delete

echo "Backup completed: $DATE"
```

Cấp quyền và thêm vào crontab:
```bash
sudo chmod +x /usr/local/bin/backup-marki.sh
sudo crontab -e
# Thêm dòng: 0 2 * * * /usr/local/bin/backup-marki.sh
```

## 🚀 Bước 8: Cập nhật ứng dụng

Khi cần cập nhật code:

```bash
# 1. Upload code mới lên VPS
# 2. Build lại
cd /var/www/marki/API/API
dotnet publish -c Release -o /var/www/marki/publish

# 3. Copy Angular builds
cp -r /var/www/marki/build-output/admin/* /var/www/marki/publish/wwwroot/admin/
cp -r /var/www/marki/build-output/client-ui/* /var/www/marki/publish/wwwroot/

# 4. Chạy migrations nếu có
cd /var/www/marki/publish
dotnet ef database update --project ../Infrastructure/Infrastructure.csproj --startup-project .

# 5. Restart service
sudo systemctl restart marki.service
```

## 📝 Ghi chú quan trọng

1. **Bảo mật:**
   - Đổi JWT secret key trong production
   - Cấu hình firewall (UFW) để chỉ mở port 80, 443
   - Không commit file appsettings.json có thông tin nhạy cảm

2. **Performance:**
   - SQLite phù hợp cho ứng dụng nhỏ đến trung bình
   - Nếu traffic cao, cân nhắc chuyển sang PostgreSQL hoặc MySQL
   - Redis giúp cải thiện performance cho giỏ hàng

3. **Monitoring:**
   - Cài đặt monitoring tools như Prometheus + Grafana (tùy chọn)
   - Theo dõi disk space cho SQLite databases
   - Setup log rotation

4. **Troubleshooting:**
   - Nếu service không start: `sudo journalctl -u marki.service -n 50`
   - Nếu database lock: Kiểm tra quyền truy cập file
   - Nếu Redis connection fail: `sudo systemctl status redis-server`

## ✅ Checklist hoàn thành

- [ ] .NET 9.0 SDK đã cài đặt
- [ ] Node.js và Angular CLI đã cài đặt
- [ ] Nginx đã cài đặt và cấu hình
- [ ] Redis đã cài đặt và chạy
- [ ] Ứng dụng đã build và deploy
- [ ] Database đã được tạo và migrate
- [ ] Systemd service đã tạo và chạy
- [ ] Nginx đã cấu hình reverse proxy
- [ ] SSL certificate đã cài đặt (nếu có domain)
- [ ] Backup script đã setup
- [ ] Firewall đã cấu hình

Chúc bạn deploy thành công! 🎉