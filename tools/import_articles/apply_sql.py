"""Apply a batch of INSERT statements to the chronicle SQLite database.

Usage: python apply_sql.py <db_path> <sql_file>

Note: sqlite3's executescript() does not roll back earlier statements in the
same file if a later one fails, so if this reports an error, check how many
rows were actually inserted (printed below) before re-running a fixed batch
for the remainder.
"""
import sqlite3
import sys
from pathlib import Path


def main():
    if len(sys.argv) != 3:
        print("Usage: python apply_sql.py <db_path> <sql_file>")
        sys.exit(1)

    db_path, sql_path = sys.argv[1], sys.argv[2]
    sql = Path(sql_path).read_text(encoding="utf-8")

    con = sqlite3.connect(db_path)
    before = con.total_changes
    try:
        con.executescript(sql)
        con.commit()
    except sqlite3.Error as e:
        print(f"ГРЕШКА при изпълнение: {e}")
        print(f"Редове, приложени преди грешката: {con.total_changes - before}")
        con.close()
        sys.exit(1)

    applied = con.total_changes - before
    con.close()
    print(f"Готово. Приложени редове: {applied}")


if __name__ == "__main__":
    main()
