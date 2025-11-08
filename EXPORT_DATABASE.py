#!/usr/bin/env python3
"""
🚀 Marki Database Exporter - Quick Run
Save this file and run: python3 EXPORT_DATABASE.py
"""

import sqlite3
import json
import os
from datetime import datetime
from pathlib import Path

# ANSI Colors for pretty output
class C:
    P = '\033[95m'; B = '\033[94m'; C = '\033[96m'
    G = '\033[92m'; Y = '\033[93m'; R = '\033[91m'
    E = '\033[0m'; BOLD = '\033[1m'

def export_table(conn, table_name):
    """Export a single table to list of dicts"""
    cursor = conn.cursor()
    try:
        cursor.execute(f"SELECT * FROM [{table_name}]")
        rows = cursor.fetchall()
        if not rows:
            return [], 0
        columns = [d[0] for d in cursor.description]
        data = [dict(zip(columns, row)) for row in rows]
        return data, len(rows)
    except Exception as e:
        print(f"   {C.R}✗{C.E} Error: {e}")
        return None, 0

def export_database(db_path, output_dir, db_name):
    """Export all tables from a database"""
    print(f"\n{C.B}{'═'*70}{C.E}")
    print(f"{C.B}  📦 Exporting {db_name}.db{C.E}")
    print(f"{C.B}{'═'*70}{C.E}\n")
    
    # Create output dir
    output_dir.mkdir(parents=True, exist_ok=True)
    
    # Connect to database
    conn = sqlite3.connect(str(db_path))
    cursor = conn.cursor()
    
    # Get all tables
    cursor.execute("""
        SELECT name FROM sqlite_master 
        WHERE type='table' 
        AND name NOT LIKE 'sqlite_%'
        AND name NOT LIKE '__EFMigrations%'
        ORDER BY name
    """)
    tables = [row[0] for row in cursor.fetchall()]
    
    if not tables:
        print(f"{C.Y}⚠{C.E}  No tables found")
        conn.close()
        return {}
    
    print(f"{C.C}ℹ{C.E}  Found {len(tables)} tables\n")
    
    stats = {'success': 0, 'empty': 0, 'error': 0, 'rows': 0}
    exported = {}
    
    for table in tables:
        print(f"📊 {table:.<40} ", end='', flush=True)
        
        data, count = export_table(conn, table)
        
        if data is None:
            stats['error'] += 1
            continue
        
        if count == 0:
            stats['empty'] += 1
            print(f"{C.Y}⚪ Empty{C.E}")
            continue
        
        # Save to JSON
        output_file = output_dir / f"{table.lower()}.json"
        with open(output_file, 'w', encoding='utf-8') as f:
            json.dump(data, f, indent=2, ensure_ascii=False, default=str)
        
        stats['success'] += 1
        stats['rows'] += count
        exported[table] = data
        print(f"{C.G}✅ {count:>5} rows{C.E}")
    
    conn.close()
    
    # Print summary
    print(f"\n{C.B}{'─'*70}{C.E}")
    print(f"✅ Success: {C.G}{stats['success']:>3}{C.E}")
    print(f"⚪ Empty:   {C.Y}{stats['empty']:>3}{C.E}")
    print(f"❌ Errors:  {C.R}{stats['error']:>3}{C.E}")
    print(f"📊 Rows:    {C.C}{stats['rows']:>6}{C.E}")
    print(f"{C.B}{'─'*70}{C.E}\n")
    
    return exported

def main():
    """Main function"""
    print(f"\n{C.P}{'='*70}{C.E}")
    print(f"{C.P}{C.BOLD}🚀 Marki Database Exporter{C.E}")
    print(f"{C.P}{'='*70}{C.E}")
    
    # Paths
    base = Path("/Users/bachnguyenluong/Documents/working/marki/API")
    marki_db = base / "API/marki.db"
    identity_db = base / "API/identity.db"
    seed_dir = base / "Infrastructure/Data/SeedData"
    
    # Check databases exist
    print(f"\n{C.B}{'═'*70}{C.E}")
    print(f"{C.B}  📋 Checking Files{C.E}")
    print(f"{C.B}{'═'*70}{C.E}\n")
    
    if not marki_db.exists():
        print(f"{C.R}✗{C.E} marki.db not found: {marki_db}")
        return
    print(f"{C.G}✓{C.E} marki.db found ({marki_db.stat().st_size / 1024:.1f} KB)")
    
    if not identity_db.exists():
        print(f"{C.R}✗{C.E} identity.db not found: {identity_db}")
        return
    print(f"{C.G}✓{C.E} identity.db found ({identity_db.stat().st_size / 1024:.1f} KB)")
    
    # Create backup
    print(f"\n{C.B}{'═'*70}{C.E}")
    print(f"{C.B}  💾 Creating Backup{C.E}")
    print(f"{C.B}{'═'*70}{C.E}\n")
    
    timestamp = datetime.now().strftime("%Y%m%d_%H%M%S")
    backup_dir = seed_dir / f"backup_{timestamp}"
    
    json_files = list(seed_dir.glob("*.json"))
    if json_files:
        backup_dir.mkdir(parents=True, exist_ok=True)
        for file in json_files:
            import shutil
            shutil.copy2(file, backup_dir / file.name)
        print(f"{C.G}✓{C.E} Backup created: {backup_dir.name}")
    else:
        print(f"{C.C}ℹ{C.E}  No existing files to backup")
    
    # Export databases
    marki_data = export_database(marki_db, seed_dir, "marki")
    identity_data = export_database(identity_db, seed_dir / "Identity", "identity")
    
    # Final summary
    print(f"{C.B}{'═'*70}{C.E}")
    print(f"{C.B}  ✅ Export Complete!{C.E}")
    print(f"{C.B}{'═'*70}{C.E}\n")
    
    print(f"{C.C}📁 Output:{C.E} {seed_dir}\n")
    print(f"{C.C}📊 Summary:{C.E}")
    print(f"   marki.db:    {len(marki_data)} tables")
    print(f"   identity.db: {len(identity_data)} tables")
    
    if backup_dir.exists():
        print(f"\n{C.C}💾 Backup:{C.E} {backup_dir}")
    
    print(f"\n{C.Y}{C.BOLD}⚠  NEXT STEPS:{C.E}")
    print(f"   {C.Y}1.{C.E} Review JSON files in {seed_dir}")
    print(f"   {C.Y}2.{C.E} Test with: dotnet ef database drop && dotnet ef database update")
    print(f"   {C.Y}3.{C.E} Verify data loaded correctly")
    print(f"   {C.Y}4.{C.E} Keep backup safe!")
    
    print(f"\n{C.G}✨ Export successful!{C.E}\n")
    print(f"{C.P}{'='*70}{C.E}\n")

if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        print(f"\n\n{C.Y}⚠ Cancelled{C.E}\n")
    except Exception as e:
        print(f"\n{C.R}❌ Error: {e}{C.E}\n")
        import traceback
        traceback.print_exc()
