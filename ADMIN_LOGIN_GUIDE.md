# Admin Login Credentials & Setup Guide

## 🔑 Default Login Credentials

### Super Admin Account
- **Username/Email**: `marki` hoặc `admin@marki.vn`
- **Password**: `Pa$$w0rd`

### Customer Account (for testing)
- **Email**: `bob@test.com`
- **Password**: `Pa$$w0rd`

## 🚀 What Was Fixed

### Problem 1: API Routes Returning HTML Instead of JSON
**Error**: 
```
SyntaxError: Unexpected token '<', "<!doctype "... is not valid JSON
```

**Root Cause**:
- Admin Angular app baseHref: `/admin/`
- API endpoint: `/admin/api/account/login`
- ASP.NET Fallback controller was catching ALL `/admin/*` routes
- API routes were being served as HTML (Angular app) instead of JSON

**Solution**:
1. ✅ Created dedicated `Admin/AccountController.cs` with route `[Route("admin/api/[controller]")]`
2. ✅ Updated `Program.cs` to prioritize API routes over SPA fallback
3. ✅ Added check to exclude `/admin/api/*` from SPA fallback

### Problem 2: Unknown Passwords for Seeded Users
**Problem**: JSON seed file had password hashes but no way to know original passwords

**Solution**: 
- Seed file in `AppIdentityDbContextSeed.cs` creates users with known password: `Pa$$w0rd`
- Admin account username: `marki` (can also login with email: `admin@marki.vn`)

## 📝 How to Login to Admin Panel

### Step 1: Access Admin Panel
```
https://sampleecom-btadfdbsasgcfabf.southeastasia-01.azurewebsites.net/admin
```
or locally:
```
https://localhost:5001/admin
```

### Step 2: Enter Credentials
- **Username**: `marki` (hoặc `admin@marki.vn`)
- **Password**: `Pa$$w0rd`

### Step 3: API Endpoint Should Work Now
The login API call:
```
POST https://sampleecom-btadfdbsasgcfabf.southeastasia-01.azurewebsites.net/admin/api/account/login
```
Should return JSON:
```json
{
  "email": "admin@marki.vn",
  "displayName": "Marki",
  "token": "eyJhbGc..."
}
```

## 🔧 Files Changed

1. **API/API/Controllers/Admin/AccountController.cs** (NEW)
   - Dedicated admin login endpoint
   - Route: `admin/api/account/login`
   - Checks user has admin/employee role

2. **API/API/Program.cs**
   - Reordered middleware to prioritize API routes
   - Added check to exclude `/admin/api/*` from SPA fallback
   - Fixed routing conflict

## 👤 User Management

### To Create New Admin User (via API)
Only SuperAdmin can create new admin users:

```http
POST /admin/api/account/register
Authorization: Bearer {superadmin_token}
Content-Type: application/json

{
  "email": "newadmin@marki.vn",
  "displayName": "New Admin",
  "password": "YourPassword123!"
}
```

### User Roles
- **SuperAdmin**: Full access, can create other admins
- **Admin**: Can manage products, orders, customers
- **Employee**: Limited access
- **Customer**: No admin access

### To Check Users in Database
```sql
SELECT * FROM AspNetUsers;
SELECT * FROM AspNetUserRoles;
SELECT * FROM AspNetRoles;
```

## 🧪 Testing After Deploy

1. **Test API directly**:
```bash
curl -X POST https://sampleecom-btadfdbsasgcfabf.southeastasia-01.azurewebsites.net/admin/api/account/login \
  -H "Content-Type: application/json" \
  -d '{"email":"marki","password":"Pa$$w0rd"}'
```

Expected response:
```json
{
  "email": "admin@marki.vn",
  "displayName": "Marki",
  "token": "eyJhbGc..."
}
```

2. **Test Admin Panel**:
- Navigate to `/admin`
- Should show login page
- Enter credentials
- Should successfully login and redirect to dashboard

## 🔒 Security Notes

### For Production:
1. **Change Default Passwords**:
   ```csharp
   // In AppIdentityDbContextSeed.cs
   await userManager.CreateAsync(admin, "YourStrongPassword123!");
   ```

2. **Remove Test Users**:
   - Remove bob@test.com before production
   - Only keep necessary admin accounts

3. **Add Password Requirements**:
   ```csharp
   // In IdentityServiceExtensions.cs
   services.Configure<IdentityOptions>(options =>
   {
       options.Password.RequireDigit = true;
       options.Password.RequiredLength = 12;
       options.Password.RequireNonAlphanumeric = true;
       options.Password.RequireUppercase = true;
       options.Password.RequireLowercase = true;
   });
   ```

4. **Enable Account Lockout**:
   ```csharp
   options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
   options.Lockout.MaxFailedAccessAttempts = 5;
   ```

## 🐛 Troubleshooting

### Issue: Still Getting HTML Instead of JSON
**Solution**: Clear browser cache and redeploy
```bash
dotnet publish ./API -c Release -o ./publish --no-build
```

### Issue: "You don't have permission to access admin panel"
**Solution**: Check user roles in database:
```sql
SELECT u.Email, r.Name 
FROM AspNetUsers u
JOIN AspNetUserRoles ur ON u.Id = ur.UserId
JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email = 'admin@marki.vn';
```

### Issue: Can't Login After Changing Password
**Solution**: Password must meet requirements:
- At least 6 characters
- Contains uppercase and lowercase
- Contains numbers
- Contains special characters ($, !, etc.)

## 📦 Next Steps

1. ✅ Deploy these changes
2. ✅ Test login at `/admin`
3. ✅ Verify API returns JSON
4. ⚠️ Change default passwords before production
5. ⚠️ Add more admin users as needed
