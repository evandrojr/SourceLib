
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using FirebirdSql.Data.FirebirdClient;//provider do SGBD FireBird
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using Core;
using ShowLib;

namespace ShowLib
{   
    public class Fb
    {        
        private FbConnection con = null;
        public FbConnection Con
        {
            get { return con; }
        }

        public string User;
        public string Password;
        public string Server;
        public string File;
        private string conStr = "";

        //Só pode rodar depois que o usuário logar e for criar uma versão
        public void SetServerConStr(string user, string password, string server, DateTime creationDT)
        {
             User=user;
             Password=password;
             Server = server;
             File = D.DatabaseLocateForManager(creationDT);
             conStr = @"User=" + User + ";Password=" + Password + ";Dialect=3;SERVER=" +  
                 Server + ";DataBase=" + File;
        }

        public string Text(string sql)
        {
            using (FbConnection Con = new FbConnection(conStr))
            {
                Con.Open();
                FbCommand cmd = new FbCommand(sql, Con);
                return Convert.ToString(cmd.ExecuteScalar());
            }
        }

        public DateTime DateTime(string sql)
        {
            using (FbConnection Con = new FbConnection(conStr))
            {
                Con.Open();
                FbCommand cmd = new FbCommand(sql, Con);
                return Convert.ToDateTime(cmd.ExecuteScalar());
            }
        }


        public double Double(string sql)
        {
            using (FbConnection Con = new FbConnection(conStr))
            {
                Con.Open();
                FbCommand cmd = new FbCommand(sql, Con);
                return Convert.ToDouble(cmd.ExecuteScalar());
            }
        }

        public List<String> getConnectedUsers(string sql)
        {
            List<String> lista = new List<string>();
            DataTable dt = this.DataTablePreenche(sql);
            foreach (DataRow linha in dt.Rows)
            {
                lista.Add(linha[0].ToString());
            }
            return lista;
        }

        /// <summary>
        /// Sets a parameter creating it in case it does not exists
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <param name="dataType"></param>
        public static void ParameterSet(string name, string val, string dataType){
            name = name.ToUpper().Trim();
            val = val.ToUpper().Trim();
            dataType = dataType.ToUpper().Trim();

            if(name == "" || dataType =="")
                throw new Exception("Todas as variáveis devem ter valor");
            if(
                !(
                  dataType == "B" 
                  || 
                  dataType == "C" 
                  || 
                  dataType == "F" 
                  || 
                  dataType == "I" 
                  || 
                  dataType=="T"
                  )
                )
                throw new Exception("Tipo de dados inválido!");
            string sql = @"Delete from parametro where NOME='" + name + "' ;" + 
            @"Insert into parametro 
                (nome, tipo, valor, importa_transferencia) values 
                ('" + name + "','" + dataType + "','" + val + "','1') ;";

            D.Fb.SqlScriptExecutive(sql, true);
            return;
        }

        public int Int(string sql)
        {
            using (FbConnection Con = new FbConnection(conStr))
            {
                Con.Open();
                FbCommand cmd = new FbCommand(sql, Con);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public string ConStr
        {
            get { return conStr; }
            set { conStr = value; }
        }

        /// <summary>
        /// Removes Firebird´s comments
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public string CommentRemove(string sql)
        {
            Regex r = new Regex(@"(//.*\n|--.*\n|/\*(.|\n)*?\*/)");
            return  r.Replace(sql, Environment.NewLine);
        }

        //Disconecta do firebird para poder acessar o arquivo da base no sistema de arquivos
        public void ServerDisconnetAllUsers(string user, string pass, string server, string file)
        {
            BaseStop(user, pass, server, file);
        }

        public string LineBreakRemove(string sql)
        {
            return sql.Replace(Environment.NewLine, Environment.NewLine);
        }

        /// <summary>
        /// Double quote a string
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string Q(string sql)
        {
            return '"' + sql + '"';
        }

        public int ExecuteNonQuery(string sql, FbConnection con, FbTransaction trans)
        {
            try
            {
                FbCommand cmd = null;
                int rowsAffected = -1;
                cmd = new FbCommand(sql, con, trans);
                rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro executando ExecuteNonQuery " + sql, ex);
            }
        }

        // isql -user ABDMBA2001MXQ -password aqbxdmm1b0a02 -nod -i script.sql "D:\base\ONLINE.FDB" 


        //public string ScriptExecutive(string sql, bool commit)
        //{
        //    string scriptSqlFile = D.ApplicationDirectory + Guid.NewGuid() + ".sql";
        //    string resultado;

        //    if (commit == false)
        //        sql = "set transaction READ WRITE WAIT SNAPSHOT;" + sql + " rollback         work;";
        //    else
        //        sql = "set transaction READ WRITE WAIT SNAPSHOT;" + sql + " commit         work;";

        //    System.IO.File.WriteAllText(scriptSqlFile, sql, Encoding.Default);


        //    string argumentos = "-user " + User + " -password " + Password + " " + Server + ":\"" + File + "\" -nod -q -i \"" + scriptSqlFile + "\"";
        //    resultado = NeoFcn.CommandLineExecute("isql.exe", argumentos, D.ApplicationDirectory);
        //    System.IO.File.Delete(scriptSqlFile);
        //    if (resultado.Length > 0)
        //        if (resultado[0] == '#')
        //            throw new Exception(resultado);
        //    return resultado;
        //}
        
        public string ScriptExecutiveTransactionAnyCommand(string sql, bool commit)
        {
            string fileBase = D.TempDir + Guid.NewGuid();
            string scriptSqlFile = fileBase + ".sql";
//            string outputFile = fileBase + ".out";
            string tmpFile = fileBase + ".tmp";
            string resultado;
            string argumentos = "";
            string dir = D.GenericBaseDir + @"TMP\";
            string file = Guid.NewGuid().ToString();

            try
            {
                Directory.CreateDirectory(dir);
            }
            catch
            {
            }

            if (commit == false)
            {
                //Parar base
                BaseStop(User, Password, Server, File);
                System.IO.File.Copy(File, dir + file);
                BaseStart(User, Password, Server, File);
                BaseStart(User, Password, Server, dir + file);
                argumentos = 
                    "-user " + User + " -password " + Password + " " + 
                    Server + ":\"" + dir + file + "\" -nod -q -i \"" + 
                    scriptSqlFile + "\" -o \"" + tmpFile + "\"";
            }
            else
            {
                argumentos = 
                    "-user " + User + " -password " + Password + " " + 
                    Server + ":\"" + File + "\" -nod -q -i \"" + scriptSqlFile + 
                    "\" -o \"" + tmpFile + "\"";
            }

            System.IO.File.WriteAllText(scriptSqlFile, sql, Encoding.Default);
            resultado = Fcn.CommandLineExecute("isql.exe", argumentos, D.NeoUpdateDir);

            if(!commit)
                System.IO.File.Delete(scriptSqlFile);
            if (resultado.Length > 0)
                if (resultado[0] == '#')
                    throw new Exception(resultado);
            return resultado;
        }

        //public string ScriptExecutiveTransactionAnyCommandFromFile(string scriptSqlFile, string previousSql, bool commit)
        //{

        //    string resultado;
        //    string argumentos = "";
        //    string dir = D.GenericBaseDir + @"TMP\";
        //    string file = Guid.NewGuid().ToString();

        //    try
        //    {
        //        Directory.CreateDirectory(dir);
        //    }
        //    catch
        //    {
        //    }

        //    if (commit == false)
        //    {
        //        //Parar base
        //        ServerStop(User, Password, Server, File);
        //        System.IO.File.Copy(File, dir + file);
        //        ServerStart(User, Password, Server, File);
        //        ServerStart(User, Password, Server, dir + file);
        //        argumentos = "-user " + User + " -password " + Password + " " + Server + ":\"" + dir + file + "\" -nod -q -i \"" + scriptSqlFile + "\"";
        //    }
        //    else
        //    {
        //        argumentos = "-user " + User + " -password " + Password + " " + Server + ":\"" + File + "\" -nod -q -i \"" + scriptSqlFile + "\"";
        //    }

        //    resultado = NeoFcn.CommandLineExecute("isql.exe", argumentos, D.ApplicationDirectory);

        //    if (!commit)
        //        System.IO.File.Delete(scriptSqlFile);
        //    if (resultado.Length > 0)
        //        if (resultado[0] == '#')
        //            throw new Exception(resultado);
        //    return resultado;
        //}


        /// <summary>
        /// Exucuta um script de atualização do banco
        /// </summary>
        /// <param name="sql">Script de atualização do banco</param>
        /// <param name="commit">True = faz comit ao fim do processo | False: faz rollback ao fim do processo</param>
        /// <param name="outStandarErrorMessage">out - Mensagem de erro</param>
        /// <returns>Código do erro, caso seja 0 não houve erro</returns>
        public void IsqlScriptExecutive(string p_sql, bool commit, out string outStandarErrorMessage)
        {
            string sql = p_sql;
            string stdOut="";
            string baseFile = D.TempDir + Guid.NewGuid();
            string scriptSqlFile = baseFile + ".sql";
            string tmpFile = baseFile + ".tmp";
            outStandarErrorMessage = String.Empty;
            string standardErrorMessage;

            if (commit == false)
                sql = "set transaction READ WRITE WAIT SNAPSHOT;" + sql + " rollback         work;";
            else
                sql = "set transaction READ WRITE WAIT SNAPSHOT;" + sql + " commit         work;";

            System.IO.File.WriteAllText(scriptSqlFile, sql, Encoding.Default);
            
            string argumentos = 
                "-user " + User + " -password " + Password + " " + Server + ":\"" + File + 
                "\" -nod -q -i \"" + scriptSqlFile + "\" -o \"" + tmpFile + "\"";
            
            int errorCode = 
                Fcn.CommandLineExecute(
                    "isql.exe", 
                    argumentos, 
                    out stdOut, 
                    out standardErrorMessage, 
                    D.ApplicationDirectory
                    );
            
            System.IO.File.Delete(scriptSqlFile);
            System.IO.File.Delete(tmpFile);
            
            if (errorCode != 0)
            {
                outStandarErrorMessage = ("####### Erro na atualização #######" + Environment.NewLine);
                if (String.IsNullOrEmpty(standardErrorMessage))
                {
                    outStandarErrorMessage += ("Erro desconhecido ao executar comando no firebird. O servidor pode estar travando" + Environment.NewLine);
                }
                else
                {
                    outStandarErrorMessage += (standardErrorMessage + Environment.NewLine);
                }
                throw new Exception("Erro no ensaio dos script de atualização. " + p_sql);
            }
        }

        public void SqlScriptExecutive(string sql,  bool commit)
        {
 
            sql = CommentRemove(sql);
            sql = LineBreakRemove(sql);
            string[] cmds = sql.Split(';');
            using (FbConnection con = new FbConnection(conStr))
            {
                con.Open();
                FbTransaction trans = con.BeginTransaction();
                foreach (string s in cmds)
                {
                    if (s != "")
                        D.Fb.ExecuteNonQuery(s, con, trans);
                }
                if (commit)
                    trans.Commit();
                else
                    trans.Rollback();
                con.Close();
            }
        }


        public void ExecuteTransaction(List<string> sqlList)
        {

            using (FbConnection con = new FbConnection(conStr))
            {
                string sqlAtual = "";
                try
                {
                    con.Open();
                    FbTransaction trans = con.BeginTransaction();
                    foreach (string s in sqlList)
                    {
                        sqlAtual = s;
                        if (s != "")
                            D.Fb.ExecuteNonQuery(s, con, trans);
                    }
                    trans.Commit();
                    con.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("ExecuteTransaction(List<string> sqlList) " + sqlAtual, ex);
                }
                finally
                {
                    con.Close();
                }
            }
        }
    

        public int ExecuteNonQuery(string sql)
        {
           using (FbConnection con = new FbConnection(conStr))
           {
                try
                {
                    con.Open();
                    FbCommand cmd = null;
                    int rowsAffected = -1;
                    cmd = new FbCommand(sql, con);
                    rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro executando ExecuteNonQuery " + sql, ex);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        /// <summary>
        /// Cria e preenche um DataTable com o resultado do sql,
        /// pode usar com mais de uma tabela
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dt"></param>
        public DataTable DataTablePreenche(string sql)
        {
            using (FbConnection Con = new FbConnection(conStr))
            {
                Con.Open();
                FbCommand cmd = new FbCommand(sql, Con);
                FbDataReader reader = null;
                DataTable dt = new DataTable();
                reader = cmd.ExecuteReader();
                DataSet ds = ConvertDataReaderToDataSet(reader, String.Empty);
                reader.Close();
                return ds.Tables[0];
            }
        }

        ///    <summary>
        /// Converts a NpgsqlDataReader to a DataSet
        ///    <param name='reader'>
        /// NpgsqlDataReader to convert.</param>
        ///    <returns>
        /// DataSet filled with the contents of the reader.</returns>
        ///    </summary>
        public static DataSet ConvertDataReaderToDataSet(FbDataReader reader, string tabela)
        {
            DataSet dataSet = new DataSet();
            do
            {
                // Create new data table

                DataTable schemaTable = reader.GetSchemaTable();
                DataTable dataTable = new DataTable(tabela);

                if (schemaTable != null)
                {
                    // A query returning records was executed

                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        // Create a column name that is unique in the data table
                        string columnName = (string)dataRow["ColumnName"]; //+ "<C" + i + "/>";
                        // Add the column definition to the data table
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }

                    dataSet.Tables.Add(dataTable);

                    // Fill the data table we just created

                    while (reader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < reader.FieldCount; i++)
                            dataRow[i] = reader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    // No records were returned

                    DataColumn column = new DataColumn("RowsAffected");
                    dataTable.Columns.Add(column);
                    dataSet.Tables.Add(dataTable);
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = reader.RecordsAffected;
                    dataTable.Rows.Add(dataRow);
                }
            }
            while (reader.NextResult());
            return dataSet;
        }

        /// <summary>
        /// requires only the fields that are not null, like in Delphi
        /// vp = value, parameter
        /// </summary>
        /// <param name="SPname"></param>
        /// <param name="pv"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public Dictionary<string, string> StoredProcedureExecute(
            string spName, 
            Dictionary<string, string> pv, 
            FbTransaction trans
            )
        {
            FbCommand cmd;
            Dictionary<string, string> rpv = new Dictionary<string, string>();
            DateTime parametroData;

            //StringBuilder para debug
            StringBuilder sbD = new StringBuilder("Execute procedure ");
            sbD.Append(spName).Append("(");
            string sD = null;

            cmd = new FbCommand(spName, Con, trans);
            cmd.CommandType = CommandType.StoredProcedure;

            //Very very important!!!
            FbCommandBuilder.DeriveParameters(cmd);

            foreach (string name in pv.Keys)
            {
                if (!cmd.Parameters.Contains(name))
                {
                    throw new Exception(
                        "Não foi possível encontrar o parametro " + 
                        name + " na stored procedure " + spName
                        );
                }
            }


            int count = 0;
            // Populate the Input Parameters With Values Provided        
            foreach (FbParameter parameter in cmd.Parameters)
            {
                if (parameter.Direction == ParameterDirection.Input)
                {
                    //if (parameter.DbType == DbType.Date)
                    //    count = count;
                    if (pv.ContainsKey(parameter.ParameterName))
                    {
                        if (pv[parameter.ParameterName].ToUpper() == "NULL")
                        {
                            parameter.Value = DBNull.Value;
                            sbD.Append("NULL");
                        }
                        else
                        {
                            if (parameter.DbType == DbType.Int32)
                            {
                                parameter.Value = Convert.ToInt32(pv[parameter.ParameterName]);
                                sbD.Append(parameter.Value.ToString());
                            }
                            else
                                if (
                                    parameter.DbType == DbType.DateTime 
                                    || 
                                    parameter.DbType == DbType.Date
                                    )
                                {
                                    if (pv[parameter.ParameterName] == "")
                                    {
                                        parameter.Value = DBNull.Value;
                                        sbD.Append("NULL");
                                    }
                                    else
                                    {
                                        sbD.Append("'");
                                        parameter.Value = 
                                            Convert.ToDateTime(
                                               pv[parameter.ParameterName], 
                                               D.CultureInfoBRA
                                               );
                                        parametroData = 
                                            Convert.ToDateTime(
                                            pv[parameter.ParameterName], 
                                            D.CultureInfoBRA
                                            );
                                        sbD.Append(
                                            parametroData.ToString(
                                                "yyyy-M-d HH:mm:ss", 
                                                D.CultureInfoBRA
                                                )
                                              );
                                        sbD.Append("'");
                                    }
                                }
                                else
                                    if (parameter.DbType == DbType.String)
                                    {
                                        parameter.Value = 
                                            pv[parameter.ParameterName].Replace("'", "`");
                                        sbD.Append("'");
                                        sbD.Append(parameter.Value.ToString());
                                        sbD.Append("'");
                                    }
                                    else
                                        if (parameter.DbType == DbType.Decimal)
                                        {
                                            parameter.Value = 
                                                Convert.ToDecimal(
                                                pv[parameter.ParameterName].Replace(",", "."), 
                                                D.CultureInfoEUA
                                                );
                                            sbD.Append(
                                                parameter.Value.ToString().Replace(",", ".")
                                                );
                                        }
                                        else
                                            if (parameter.DbType == DbType.Double)
                                            {
                                                parameter.Value = 
                                                    Convert.ToDouble(
                                                    pv[parameter.ParameterName].Replace(",", "."), 
                                                    D.CultureInfoEUA
                                                    );
                                                sbD.Append(
                                                    parameter.Value.ToString().Replace(",", ".")
                                                    );
                                            }
                                            else
                                                if (parameter.DbType == DbType.Int16)
                                                {
                                                    parameter.Value = 
                                                        Convert.ToInt16(
                                                        pv[parameter.ParameterName]
                                                        );
                                                    sbD.Append(parameter.Value.ToString());
                                                }
                                                else
                                                {
                                                    throw new Exception(
                                                        "Tipo de dado não cadastrado"
                                                        );
                                                }
                        }
                    }
                    else
                    {
                        parameter.Value = DBNull.Value;
                        sbD.Append("NULL");
                    }
                    sbD.Append(",");
                    sD = sbD.ToString();
                    //Remove ',' extra
                    sD = sD.Substring(0, sD.Length - 1);
                    sD += ");";
                    ++count;
                }
            }

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro grave: " + ex.Message + " SQL " + sD);
            }
            //            FbCommand cmd2 = new FbCommand(sD,con);
            //            cmd2.ExecuteNonQuery();

            foreach (FbParameter parameter in cmd.Parameters)
            {
                if (
                    parameter.Direction == ParameterDirection.ReturnValue 
                    || 
                    parameter.Direction == ParameterDirection.Output
                    )
                {
                    rpv.Add(parameter.ParameterName, parameter.Value.ToString());
                }
            }
            return rpv;
        }

        /// <summary>
        /// Lança exceção se a base padrão não estiver rodando
        /// </summary>
        public void DatabaseDefaultOnlineCheck()
        {
            DateTime test = DateTime("select CURRENT_DATE from rdb$DATABASE");
        }

        /// <summary>
        /// Lança exceção se a base que foi passada não estiver rodando
        /// </summary>
        public void DatabaseOnlineCheck(string user, string pass, string server, string file)
        {
            DateTime test = DateTime("select CURRENT_DATE from rdb$DATABASE");
            string cs = @"User=" + user + ";Password=" + pass + ";Dialect=3;SERVER=" +
                       server + ";DataBase=" + file;
            using (FbConnection con = new FbConnection(cs))
            {
                con.Open();
                FbCommand cmd = new FbCommand("select CURRENT_DATE from rdb$DATABASE", con);
                cmd.ExecuteScalar();
            }
        }


        /// <summary>
        /// Ativa um banco específico caso já não esteja ativado
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="server"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool BaseStart(string user, string pass, string server, string file)
        {
            int error;
            string standardOutputMessage, standardErrorMessage;

            //TODO: Texto comentado, pois a checagem deve ser feita na base que foi passado os parametros, não na default

            try
            {
                DatabaseOnlineCheck(user, pass, server, file);
            }
            catch
            {
                string argumentos =
                    "-user " + user + " -password " + pass + " " + server + ":\"" + file +
                    "\" -online normal";
                error = Fcn.CommandLineExecute("gfix.exe", argumentos, out standardOutputMessage, out standardErrorMessage,
                    D.ApplicationDirectory);
                if (error == 0)
                    return true;
                else
                {
                    throw new Exception("Erro iniciando a base " + server + ":" + file + standardErrorMessage + " " + standardOutputMessage);
                }
            }
            return true;
        }

        /// <summary>
        /// Para uma base de dados específica caso já não esteja parada
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="server"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool BaseStop(string user, string pass, string server, string file)
        {
            
            int error;
            string standardOutputMessage, standardErrorMessage;

            try
            {
                DatabaseOnlineCheck(user, pass, server, file);
                string argumentos = "-user " + user + " -password " + pass + " " + server + ":\"" + file +
                    "\" -shut full -force 0";
                
                error = Fcn.CommandLineExecute("gfix.exe", argumentos, out standardOutputMessage, out standardErrorMessage, D.ApplicationDirectory);
                if (error == 0)
                    return true;
                else
                {
                    throw new Exception("Erro iniciando a base " + server + ":" + file + standardErrorMessage + " " + standardOutputMessage);
                }
            }
            catch
            {
                return true;
            }
            
        }

        public int ImportaLinha(string sql, FbTransaction trans)
        {
            FbCommand cmd = null;
            int rowsAffected = -1;
            cmd = new FbCommand(sql, Con, trans);
            rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected;
        }


        /// <summary>
        /// Updates a table 
        /// </summary>
        /// <param name="table">Table to be updated</param>
        /// <param name="WhereCondition">The condition for the where (don´t write the where clause)</param>
        /// <param name="keyVal"></param>
        public void Update(string table, string WhereCondition, Dictionary<string, object> keyVal)
        {
            using (FbConnection Con = new FbConnection(conStr))
            {
                Con.Open();
                string upSql = "Update \"" + table + "\" set ";
                foreach (KeyValuePair<string, object> kv in keyVal)
                {
                    upSql += kv.Key + " = @" + kv.Key + ", ";
                }
                //Corta a última virgula e o espaço que entraram extra
                upSql = upSql.Substring(0, upSql.Length - 2);
                upSql += " Where " + WhereCondition;
                FbCommand cmd = new FbCommand(upSql, Con);
                foreach (string s in keyVal.Keys)
                {
                    cmd.Parameters.Add("@" + s, keyVal[s]);
                    //Era usado para SQL
                    //cmd.Parameters.AddWithValue("@" + s, keyVal[s]);
                }
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string campos = "";
                    foreach (KeyValuePair<string, object> kv in keyVal)
                    {
                        campos += kv.Key + " = " + kv.Value + Environment.NewLine;
                    }
                    throw new Exception(
                        "Erro ao executar update " + campos + " " + upSql + ex.Message
                        );
                }
            }
        }


        public void Insert(string table, Dictionary<string, object> keyVal)
        {

            using (FbConnection Con = new FbConnection(D.PgConStr))
            {
                Con.Open();
                string ins1 = "Insert INTO \"" + table + "\" (";
                string ins2 = "(";

                foreach (string s in keyVal.Keys)
                {
                    ins1 += "\"" + s + "\"" + ",";
                    ins2 += "@" + s + ",";
                }
                //Corta a última virgula que entrou extra
                ins1 = ins1.Substring(0, ins1.Length - 1);
                ins2 = ins2.Substring(0, ins2.Length - 1);
                ins1 += ")";
                ins2 += ")";
                string insSql = ins1 + " values " + ins2;

                FbCommand cmd = new FbCommand(insSql, Con);


                foreach (string s in keyVal.Keys)
                {
                    cmd.Parameters.Add("@" + s, keyVal[s]);
                    //Era usado para Sql Server
                    //cmd.Parameters.AddWithValue("@" + s, keyVal[s]);
                }
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    string campos = "";
                    foreach (KeyValuePair<string, object> kv in keyVal)
                    {
                        campos += kv.Key + " = " + kv.Value + Environment.NewLine;
                    }
                    throw new Exception(
                        "Erro ao executar insert " + campos + " " + insSql + ex.Message
                        );
                }
            }
        }

        /// <summary>
        /// Insert com transação
        /// </summary>
        /// <param name="table"></param>
        /// <param name="keyVal"></param>
        /// <param name="con"></param>
        /// <param name="dbTrans"></param>
        public void Insert(
            string table, 
            Dictionary<string, object> keyVal, 
            FbConnection con, 
            FbTransaction dbTrans
            )
        {

            string ins1 = "Insert INTO \"" + table + "\" (";
            string ins2 = "(";

            foreach (string s in keyVal.Keys)
            {
                ins1 += "\"" + s + "\"" + ",";
                ins2 += "@" + s + ",";
            }
            //Corta a última virgula que entrou extra
            ins1 = ins1.Substring(0, ins1.Length - 1);
            ins2 = ins2.Substring(0, ins2.Length - 1);
            ins1 += ")";
            ins2 += ")";
            string insSql = ins1 + " values " + ins2;
            FbCommand cmd = new FbCommand(insSql, con, dbTrans);

            foreach (string s in keyVal.Keys)
            {
                cmd.Parameters.Add("@" + s, keyVal[s]);
                //Era usado para Sql Server
                //cmd.Parameters.AddWithValue("@" + s, keyVal[s]);
            }
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string campos = "";
                foreach (KeyValuePair<string, object> kv in keyVal)
                {
                    campos += kv.Key + " = " + kv.Value + Environment.NewLine;
                }
                throw new Exception(
                    "Erro ao executar insert " + campos + " " + insSql + ex.Message);
            }
        }



        /// <summary>
        /// Updates a table 
        /// </summary>
        /// <param name="table">Table to be updated used with transactions</param>
        /// <param name="WhereCondition">The condition for the where (don´t write the where clause)</param>
        /// <param name="keyVal"></param>
        public void Update(string table, string WhereCondition, Dictionary<string, object> keyVal, FbConnection con, FbTransaction dbTrans)
        {
            string upSql = "Update \"" + table + "\" set ";
            foreach (KeyValuePair<string, object> kv in keyVal)
            {
                upSql += kv.Key + " = @" + kv.Key + ", ";
            }
            //Corta a última virgula e o espaço que entraram extra
            upSql = upSql.Substring(0, upSql.Length - 2);
            if (WhereCondition != String.Empty)
                upSql += " Where " + WhereCondition;
            FbCommand cmd = new FbCommand(upSql, con, dbTrans);
            foreach (string s in keyVal.Keys)
            {
                cmd.Parameters.Add("@" + s, keyVal[s]);
                //Era usado para SQL
                //cmd.Parameters.AddWithValue("@" + s, keyVal[s]);
            }
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string campos = "";
                foreach (KeyValuePair<string, object> kv in keyVal)
                {
                    campos += kv.Key + " = " + kv.Value + Environment.NewLine;
                }
                throw new Exception(
                    "Erro ao executar update " + campos + " " + upSql + ex.Message);
            }
        }

    }
}