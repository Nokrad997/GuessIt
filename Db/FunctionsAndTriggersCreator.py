from sqlalchemy import create_engine

DATABASE_URL = "postgresql://postgres:root@localhost:5432/Guess_It" #change postgres to root in order to work within container
engine = create_engine(DATABASE_URL)

def execute_sql_file(sql_file_path):
     with open(sql_file_path, 'r') as file:
        sql_script = file.read()

        connection = engine.raw_connection()
        try:
            cursor = connection.cursor()
            cursor.execute(sql_script)
            connection.commit()
            print(f"Skrypt SQL z {sql_file_path} został wykonany pomyślnie.")
        finally:
            cursor.close()
            connection.close()

execute_sql_file("Functions.sql")
execute_sql_file("Triggers.sql")