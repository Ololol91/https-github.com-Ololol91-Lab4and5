using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    class Generate
    {
        private static string connStr = "server=localhost;user=root;database=mysql;password=root;";
        MySqlConnection connection = new MySqlConnection(connStr);

        string begin_dir = Environment.CurrentDirectory +  @"\Sample";
        string end_dir = Environment.CurrentDirectory + @"\Test";

        int count = 1;
        string db = "";
        List<Table> tables;
        public Generate(List<Table> tbl)
        {
            tables = tbl;           
        }
        public void Test()
        {
            while (CheckDB() != true) { }
            CreateDir();
            CopyDir(begin_dir, end_dir);
            //CreateModels();
            //CreateControllers();
            //CreateSystemTableController();
            CreateModelsT4();
            CreateControllersT4();
            CreateDB();
            //CreateTablesInDB();
            CreateTablesInDBT4();
            CreateSystemTableControllerT4();

            MessageBox.Show("УСПЕШНО!"); 
        }
        private void CreateModelsT4()
        {
            foreach (Table t in tables)
            {
                Directory.CreateDirectory(end_dir + @"\Sample\Sample\Model\");
                string writePath = end_dir + @"\Sample\Sample\Model\" + t.Label.Text + ".cs";

                try
                {
                    using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                    {
                        var messageTmpl = new ModelGenerate();
                        messageTmpl.table = t;
                        string message = messageTmpl.TransformText();
                        sw.WriteLine(message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        private void CreateControllersT4()
        {
            foreach (Table t in tables)
            {
                Directory.CreateDirectory(end_dir + @"\Sample\Sample\Controller\");
                string writePath = end_dir + @"\Sample\Sample\Controller\" + t.Label.Text + "Controller.cs";
                try
                {
                    using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                    {
                        var messageTmpl = new ClassController();
                        messageTmpl.table = t;
                        string message = messageTmpl.TransformText();
                        sw.WriteLine(message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        private void CreateSystemTableControllerT4()
        {
            Directory.CreateDirectory(end_dir + @"\Sample\Sample\Controller\");
            string writePath = end_dir + @"\Sample\Sample\Controller\SystemTableController.cs";
            var messageTmpl = new TableController();
            messageTmpl.dataBase = db;
            messageTmpl.listTable = tables;
            string message = messageTmpl.TransformText();
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
      
        private void CreateDir()
        {
            bool flag = false;
            while (flag != true)
            {
                if (Directory.Exists(end_dir + count) != true)
                {
                    end_dir = end_dir + count;
                    flag = true;
                }
                else
                    count++;
            }
        }
        private bool CheckDB()
        {

            string str = "SHOW DATABASES LIKE 'Test" + count + "'";
            string temp = "";
            try
            {
                connection.Open();
                MySqlCommand myCommand = new MySqlCommand(str, connection);
                MySqlDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    temp = reader[0].ToString();
                }

                if (temp != "Test" + count)
                    return true;           
                else       
                    count++;
                return false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        private void CreateDB()
        {
            string str = "CREATE DATABASE Test" + count;
            MySqlCommand myCommand = new MySqlCommand(str, connection);
            try
            {
                connection.Open();
                myCommand.ExecuteNonQuery();
                connStr = "server=localhost;user=root;database=Test"+ count +";password=;";
                db = "Test" + count;
                connection = new MySqlConnection(connStr);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
        private void CreateTablesInDBT4()
        {
            string writePath = end_dir + @"\db.txt";
            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                string file = "";
                sw.WriteLine(file);
            }
            foreach (Table t in tables)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                    {
                        var messageTmpl = new CreateTablesDB();
                        messageTmpl.table = t;
                        string message = messageTmpl.TransformText();
                        sw.WriteLine(message);
                        string zapr = "set foreign_key_checks = 0";
                        SQLQuery(zapr);
                        SQLQuery(message);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        private void CreateTablesInDB()
        {
            string writePath = end_dir + @"\db.txt";
            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                string file = "";
                sw.WriteLine(file);
            }
            foreach (Table t in tables)
            {                
                try
                {                    
                    using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                    {
                        string file = "";
                        file += "CREATE TABLE " + t.Label.Text + "\n(\n\t";
                        foreach (Field f in t.Field)
                        {
                            file += f.Label.Text + " ";
                            if (f.ComboBox.Text == "string")
                            {
                                file += "VARCHAR(40) NOT NULL,\n\t";
                            }
                            else if (f.ComboBox.Text == "int" && f.Label.Text == "Id")
                            {
                                file += "INT NOT NULL AUTO_INCREMENT,\n\t";
                            }
                            else if (f.ComboBox.Text == "int")
                            {
                                file += "INT NOT NULL,\n\t";
                            }
                        }

                        file += "PRIMARY KEY(Id)";

                        foreach (Field f in t.Field)
                        {
                            if (f.Label.Text.Contains("Id") && f.Label.Text.Length > 2)
                            {
                                file += ",\n\tFOREIGN KEY (" + f.Label.Text + ") REFERENCES " + f.Label.Text.Replace("Id", "") + "(Id)";
                            }
                        }

                        file += "\n);\n\n";
                        sw.WriteLine(file);
                        string zapr = "set foreign_key_checks = 0";
                        SQLQuery(zapr);
                        SQLQuery(file);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void CopyDir(string FromDir, string ToDir)
        {
            Directory.CreateDirectory(ToDir);
            foreach (string s1 in Directory.GetFiles(FromDir))
            {
                string s2 = ToDir + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(FromDir))
            {
                CopyDir(s, ToDir + "\\" + Path.GetFileName(s));
            }
        }
        private bool SQLQuery(string sql)
        {
            try
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.Connection = connection;
                command.CommandText = sql;
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        private void CreateModels()
        {

            foreach (Table t in tables)
            {
                Directory.CreateDirectory(end_dir + @"\Sample\Sample\Model\");
                string writePath = end_dir + @"\Sample\Sample\Model\" + t.Label.Text + ".cs";
                try
                {
                    using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                    {
                        string file = "";
                        file += "using System;\nusing System.Collections.Generic;\nusing System.Text;\n\n" +
                            "namespace Sample.Model\n{\n\tclass " + t.Label.Text + "\n\t{\n";
                        foreach (Field f in t.Field)
                            file += "\t\tpublic " + f.ComboBox.Text + " " + f.Label.Text + " { get; set; }\n";
                        file += "\t}\n}";
                        //MessageBox.Show(file);
                        sw.WriteLine(file);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        private void CreateControllers()
        {
            foreach (Table t in tables)
            {
                Directory.CreateDirectory(end_dir + @"\Sample\Sample\Controller\");
                string writePath = end_dir + @"\Sample\Sample\Controller\" + t.Label.Text + "Controller.cs";
                try
                {
                    using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                    {

                        ////////////////
                        string file = "";
                        file += "using MySql.Data.MySqlClient;\nusing Sample.Model;\nusing System;\nusing System.Collections.Generic;" +
                            "\nusing System.Linq;\nusing System.Text;\nusing System.Threading.Tasks;\nusing System.Windows.Forms;\n\n" +
                            "namespace Sample\n{\n\tclass " + t.Label.Text + "Controller\n\t{";
                        ////////////////
                        file += "\n\t\tpublic static string showStr = \"SELECT ";
                        for (int i = 0; i < t.Field.Count; i++)
                        {
                            if (i == t.Field.Count - 1)
                                file += t.Field[i].Label.Text;
                            else
                                file += t.Field[i].Label.Text + ", ";
                        }
                        file += " FROM " + t.Label.Text + "\";";
                        /////////////////
                        /////////////////
                        file += "\n\t\tpublic static void Insert" + t.Label.Text + "(List<string> listField)\n\t\t{";
                        file += "\n\t\t\ttry" + "\n\t\t\t{";

                        file += "\n\t\t\t\tQuery.connection.Open();";
                        file += "\n\t\t\t\tstring sqlExpression = \"INSERT INTO " + t.Label.Text + " (";
                        for (int i = 0; i < t.Field.Count; i++)
                        {
                            if (t.Field[i].Label.Text != "Id")
                            {
                                if (i == t.Field.Count - 1)
                                    file += t.Field[i].Label.Text;
                                else
                                    file += t.Field[i].Label.Text + ", ";
                            }
                        }
                        file += ") VALUES (";
                        for (int i = 0; i < t.Field.Count; i++)
                        {
                            if (t.Field[i].Label.Text != "Id")
                            {
                                if (i == t.Field.Count - 1)
                                    file += "@" + t.Field[i].Label.Text;
                                else
                                    file += "@" + t.Field[i].Label.Text + ", ";
                            }
                        }
                        file += ")\";";
                        file += "\n\t\t\t\tMySqlCommand command = new MySqlCommand(sqlExpression, Query.connection);";
                        for (int i = 0; i < t.Field.Count; i++)
                        {
                            if (t.Field[i].Label.Text != "Id")
                            {
                                file += "\n\t\t\t\tMySqlParameter " + t.Field[i].Label.Text + "Param = new MySqlParameter(\"@" + t.Field[i].Label.Text + "\", listField[" + (i - 1) + "]);";
                                file += "\n\t\t\t\tcommand.Parameters.Add(" + t.Field[i].Label.Text + "Param);";
                            }
                        }
                        file += "\n\t\t\t\tcommand.ExecuteNonQuery();";
                        file += "\n\t\t\t\tMessageBox.Show(\" Готово! \");";

                        file += "\n\t\t\t}";
                        file += "\n\t\t\tcatch(Exception ex)" + " {  MessageBox.Show(ex.ToString()); }";
                        file += "\n\t\t\tfinally" + " {  Query.connection.Close(); }";
                        file += "\n\t\t}";
                        //////////////////////////////////////////
                        //////////////////////////////////////////
                        file += "\n\t\tpublic static void Update" + t.Label.Text + "(List<string> listField, int id)\n\t\t{";
                        file += "\n\t\t\ttry" + "\n\t\t\t{";

                        file += "\n\t\t\t\tQuery.connection.Open();";
                        file += "\n\t\t\t\tstring sqlExpression = \"UPDATE " + t.Label.Text + " SET ";
                        for (int i = 0; i < t.Field.Count; i++)
                        {
                            if (t.Field[i].Label.Text != "Id")
                            {
                                if (i == t.Field.Count - 1)
                                    file += t.Field[i].Label.Text + " = @" + t.Field[i].Label.Text;
                                else
                                    file += t.Field[i].Label.Text + " = @" + t.Field[i].Label.Text + ", ";
                            }
                        }
                        file += " WHERE ID = @Id\";";

                        file += "\n\t\t\t\tMySqlCommand command = new MySqlCommand(sqlExpression, Query.connection);";
                        for (int i = 0; i < t.Field.Count; i++)
                        {
                            if (t.Field[i].Label.Text != "Id")
                            {
                                file += "\n\t\t\t\tMySqlParameter " + t.Field[i].Label.Text + "Param = new MySqlParameter(\"@" + t.Field[i].Label.Text + "\", listField[" + (i - 1) + "]);";
                                file += "\n\t\t\t\tcommand.Parameters.Add(" + t.Field[i].Label.Text + "Param);";
                            }
                            else
                            {
                                file += "\n\t\t\t\tMySqlParameter " + t.Field[i].Label.Text + "Param = new MySqlParameter(\"@" + t.Field[i].Label.Text + "\", id);";
                                file += "\n\t\t\t\tcommand.Parameters.Add(" + t.Field[i].Label.Text + "Param);";
                            }
                        }
                        file += "\n\t\t\t\tcommand.ExecuteNonQuery();";
                        file += "\n\t\t\t\tMessageBox.Show(\" Готово! \");";

                        file += "\n\t\t\t}";
                        file += "\n\t\t\tcatch(Exception ex)" + " {  MessageBox.Show(ex.ToString()); }";
                        file += "\n\t\t\tfinally" + " {  Query.connection.Close(); }";
                        file += "\n\t\t}";
                        ////////////////////////////////////
                        ////////////////////////////////////
                        file += "\n\t\tpublic static List<string> Combo" + t.Label.Text + "()\n\t\t{";
                        file += "\n\t\t\ttry" + "\n\t\t\t{";

                        file += "\n\t\t\t\tList<string> result = new List<string>();";
                        file += "\n\t\t\t\tQuery.connection.Open();";
                        file += "\n\t\t\t\tstring sqlExpression = \"SELECT Id From " + t.Label.Text + "\";";
                        file += "\n\t\t\t\tMySqlCommand command = new MySqlCommand(sqlExpression, Query.connection);";
                        file += "\n\t\t\t\tMySqlDataReader reader = command.ExecuteReader();";
                        file += "\n\t\t\t\twhile (reader.Read())\n\t\t\t\t{";
                        file += "\n\t\t\t\t\tresult.Add(reader.GetString(0));\n\t\t\t\t}";
                        file += "\n\t\t\t\treader.Close();";
                        file += "\n\t\t\t\treturn result;";

                        file += "\n\t\t\t}";
                        file += "\n\t\t\tcatch(Exception ex)" + " {  MessageBox.Show(ex.ToString()); return null;}";
                        file += "\n\t\t\tfinally" + " {  Query.connection.Close(); }";
                        file += "\n\t\t}";

                        file += "\n\t}\n}";
                        //MessageBox.Show(file);

                        sw.WriteLine(file);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        private void CreateSystemTableController()
        {

            Directory.CreateDirectory(end_dir + @"\Sample\Sample\Controller\");
            string writePath = end_dir + @"\Sample\Sample\Controller\SystemTableController.cs";
            string file = "";
            //////////////////////////////
            file += "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Text;\nusing System.Threading.Tasks;\n\nnamespace Sample\n{";
            file += "\n\tclass SystemTableController\n\t{";
            //////////////////////////////
            file += "\n\t\tpublic static string connStr = \"server = localhost; port = 3306; database = " + db + "; uid = root; password =; Persist Security Info = True; SslMode = none; Allow Zero Datetime = true\";";
            //////////////////////////////
            file += "\n\t\tpublic static string DefinitionShowTable(string table)\n\t\t{";
            foreach (Table t in tables)
            {
                file += "\n\t\t\tif (table == \"" + t.Label.Text + "\")";
                file += "\n\t\t\t\treturn " + t.Label.Text + "Controller.showStr;";
            }
            file += "\n\t\t\treturn null;";
            file += "\n\t\t}";
            //////////////////////////////
            file += "\n\t\tpublic static void DefinitionInsertTable(string table, List<string> listFields)\n\t\t{";
            foreach (Table t in tables)
            {
                file += "\n\t\t\tif (table == \"" + t.Label.Text + "\")";
                file += "\n\t\t\t\t" + t.Label.Text + "Controller.Insert" + t.Label.Text + "(listFields);";
            }
            file += "\n\t\t}";
            //////////////////////////////
            file += "\n\t\tpublic static void DefinitionUpdateTable(string table, List<string> listFields, int id)\n\t\t{";
            foreach (Table t in tables)
            {
                file += "\n\t\t\tif (table == \"" + t.Label.Text + "\")";
                file += "\n\t\t\t\t" + t.Label.Text + "Controller.Update" + t.Label.Text + "(listFields, id);";
            }
            file += "\n\t\t}";
            //////////////////////////////
            file += "\n\t\tpublic static List<string> DefinitionComboBox(string table)\n\t\t{";
            foreach (Table t in tables)
            {
                file += "\n\t\t\tif (table == \"" + t.Label.Text + "\")";
                file += "\n\t\t\t\treturn " + t.Label.Text + "Controller.Combo" + t.Label.Text + "();";
            }
            file += "\n\t\t\treturn null;";
            file += "\n\t\t}";
            file += "\n\t}\n}";
            try
            {
                using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(file);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
