#!/usr/bin/env python3
"""
🔍 Verify Exported Seed Data
Kiểm tra tính toàn vẹn của dữ liệu đã export
"""

import json
import sqlite3
from pathlib import Path

# Colors
G = '\033[92m'; R = '\033[91m'; Y = '\033[93m'; C = '\033[96m'; E = '\033[0m'

def verify_json_file(json_path):
    """Verify JSON file is valid"""
    try:
        with open(json_path, 'r', encoding='utf-8') as f:
            data = json.load(f)
        return True, len(data) if isinstance(data, list) else 1
    except json.JSONDecodeError as e:
        return False, f"Invalid JSON: {e}"
    except Exception as e:
        return False, str(e)

def count_db_records(db_path, table_name):
    """Count records in database table"""
    try:
        conn = sqlite3.connect(str(db_path))
        cursor = conn.cursor()
        cursor.execute(f"SELECT COUNT(*) FROM [{table_name}]")
        count = cursor.fetchone()[0]
        conn.close()
        return count
    except Exception as e:
        return f"Error: {e}"

def main():
    print(f"\n{C}{'='*70}{E}")
    print(f"{C}🔍 Verifying Exported Seed Data{E}")
    print(f"{C}{'='*70}{E}\n")
    
    base = Path("/Users/bachnguyenluong/Documents/working/marki/API")
    marki_db = base / "API/marki.db"
    seed_dir = base / "Infrastructure/Data/SeedData"
    
    # Tables to verify from marki.db
    tables_to_verify = [
        ('Products', 'products.json'),
        ('ProductTypes', 'types.json'),
        ('Customers', 'customers.json'),
        ('OfflineOrders', 'orders.json'),
        ('Provinces', 'province.json'),
        ('Districts', 'district.json'),
        ('Wards', 'ward.json'),
        ('DeliveryMethods', 'delivery.json'),
        ('OfflineOrderStatus', 'orderStatuses.json'),
    ]
    
    print(f"{C}Verifying marki.db exports:{E}\n")
    
    all_good = True
    for table_name, json_file in tables_to_verify:
        json_path = seed_dir / json_file
        
        # Check JSON file exists and is valid
        if not json_path.exists():
            print(f"{R}✗{E} {json_file:.<35} Missing")
            all_good = False
            continue
        
        valid, result = verify_json_file(json_path)
        if not valid:
            print(f"{R}✗{E} {json_file:.<35} {result}")
            all_good = False
            continue
        
        json_count = result
        
        # Compare with database
        if marki_db.exists():
            db_count = count_db_records(marki_db, table_name)
            
            if isinstance(db_count, str):
                print(f"{Y}⚠{E} {json_file:.<35} {json_count:>5} rows (DB: {db_count})")
            elif db_count == json_count:
                print(f"{G}✓{E} {json_file:.<35} {json_count:>5} rows {G}(Match!){E}")
            else:
                print(f"{R}✗{E} {json_file:.<35} {json_count:>5} rows (DB has {db_count})")
                all_good = False
        else:
            print(f"{G}✓{E} {json_file:.<35} {json_count:>5} rows")
    
    # Check for extra JSON files
    print(f"\n{C}Checking for extra files:{E}\n")
    
    expected_files = {f for _, f in tables_to_verify}
    actual_files = {f.name for f in seed_dir.glob("*.json")}
    extra_files = actual_files - expected_files
    
    if extra_files:
        print(f"{Y}⚠ Found extra JSON files not in verification list:{E}")
        for f in sorted(extra_files):
            json_path = seed_dir / f
            valid, result = verify_json_file(json_path)
            if valid:
                print(f"  - {f}: {result} rows")
            else:
                print(f"  - {f}: {R}Invalid{E}")
    else:
        print(f"{G}✓ No extra files{E}")
    
    # Summary
    print(f"\n{C}{'='*70}{E}")
    if all_good:
        print(f"{G}✅ All verifications passed!{E}")
        print(f"{G}Your seed data is ready to use.{E}")
    else:
        print(f"{R}⚠ Some issues found. Please review above.{E}")
    print(f"{C}{'='*70}{E}\n")

if __name__ == "__main__":
    main()
