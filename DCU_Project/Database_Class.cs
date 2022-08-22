using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace DCU_Project
{
    public class Reincidencias
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Caso { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public List<object> Values { get; set; }
        public List<object> Keys { get; set; }

        public string CreateQuery()
        {
            string query = "";
            int k = 0;
            foreach (var key in Keys)
            {
                if (k == 0)
                {
                    query += String.Format("{0} {1},", key, "INTEGER PRIMARY KEY");
                }
                else if (k + 1 == Keys.Count())
                {
                    query += String.Format("{0} {1}", key, "INT");
                }
                else
                {
                    query += String.Format("{0} {1},", key, "TEXT");
                }
                k += 1;
            }
            return query;
        }

        public Reincidencias(int id, string nombre, string apellido, string caso, string descripcion, int cantidad)
        {
            ID = id;
            Nombre = nombre;
            Apellido = apellido;
            Caso = caso;
            Descripcion = descripcion;
            Cantidad = cantidad;
            Values = new List<object>();
            Values.Add(id.ToString());
            Values.Add(nombre);
            Values.Add(apellido);
            Values.Add(caso);
            Values.Add(descripcion);
            Values.Add(cantidad.ToString());
            Keys = new List<object>();
            Keys.Add("ID");
            Keys.Add("Nombre");
            Keys.Add("Apellido");
            Keys.Add("Caso");
            Keys.Add("Descripcion");
            Keys.Add("Cantidad");
        }

        public string ToString()
        {
            string str = "(";
            foreach (var item in Values)
            {
                str += String.Format("{0},", item);
            }
            str = str.Substring(0, str.Length - 1) + ")";
            return str;
        }
    }

    public class db_schema
    {
        public string tbl_name = "Reincidencias";
        public string dbname = "Database.db";
        public Dictionary<string, string> columns = new Dictionary<string, string>();
        public SQLiteConnection sqlite_conn;
        public SQLiteCommand sqlite_command = null;
        public Dictionary<int, Reincidencias> reincidencias;
        public void Run()
        {
            reincidencias = new Dictionary<int, Reincidencias>();
            sqlite_conn = new SQLiteConnection(String.Format("Data Source={0}; Version = 3; New = True; Compress = True;", dbname));
            sqlite_conn.Open();
            sqlite_command = sqlite_conn.CreateCommand();
        }

        public void DropTable()
        {
            sqlite_command.CommandText = "DROP TABLE IF EXISTS " + tbl_name;
            sqlite_command.ExecuteNonQuery();
        }
        public void CreateTable()
        {
            Reincidencias r = new Reincidencias(1, "", "", "", "", 1);
            string Createsql = String.Format("CREATE TABLE IF NOT EXISTS {0}({1});", tbl_name, r.CreateQuery());
            sqlite_command.CommandText = Createsql;
            sqlite_command.ExecuteNonQuery();
        }

        public int UpdateData(string id, Reincidencias new_reincidencia)
        {
            string str_update = String.Format("UPDATE {0} SET\n", tbl_name);
            int v = 0;
            foreach (string value in new_reincidencia.Values)
            {
                if (v!=0)
                {
                    str_update += String.Format("{0} = '{1}',\n", new_reincidencia.Keys[v], value);
                }
                v++;
            }
            str_update = str_update.Substring(0, str_update.Length - 2);
            str_update += String.Format(" WHERE ID = {0}", id);
            sqlite_command.CommandText = str_update;
            sqlite_conn.Open();
            System.Windows.Forms.MessageBox.Show(str_update);
            return sqlite_command.ExecuteNonQuery();
        }

        public int InsertData(Reincidencias reincidencia)
        {
            string values_str = "(";
            int v = 0;
            foreach (var value in reincidencia.Values)
            {
                values_str += String.Format("'{0}'", value);
                v++;
                if (v != reincidencia.Values.Count())
                {
                    values_str += ", ";
                }
            }
            values_str += ")";
            sqlite_command.CommandText = String.Format("INSERT INTO {0} VALUES{1};", tbl_name, values_str);
            sqlite_conn.Open();
            return sqlite_command.ExecuteNonQuery();
        }

        public void ReadData(string id = "")
        {
            reincidencias.Clear();
            SQLiteDataReader sqlite_datareader;
            sqlite_command.CommandText = "SELECT * FROM " + tbl_name;
            if (id != null & id != "")
            {
                sqlite_command.CommandText += String.Format(" Where ID='{0}'", id);
            }
            if (sqlite_conn.State!=System.Data.ConnectionState.Open)
            {
                sqlite_conn.Open();
            }
            sqlite_datareader = sqlite_command.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                reincidencias.Add(int.Parse(sqlite_datareader.GetValues().Get(0)), new Reincidencias(
                int.Parse(sqlite_datareader.GetValues().Get(0)), sqlite_datareader.GetValues().Get(1),
                sqlite_datareader.GetValues().Get(2), sqlite_datareader.GetValues().Get(3),
                sqlite_datareader.GetValues().Get(4), int.Parse(sqlite_datareader.GetValues().Get(5))));

            }
            sqlite_datareader.Close();
            sqlite_conn.Close();
        }

        public int DeleteData(string id = "")
        {
            SQLiteCommand sqlite_command = sqlite_conn.CreateCommand();
            sqlite_command.CommandText = String.Format("Delete From {0}", tbl_name);
            if (id != null & id != "")
            {
                sqlite_command.CommandText += String.Format(" Where ID='{0}'", id);
            }
            sqlite_conn.Open();
            return sqlite_command.ExecuteNonQuery();
        }
    }
}
