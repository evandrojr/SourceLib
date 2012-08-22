using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


    /// <summary>
    /// Class that manages .ini files
    /// </summary>
    public class IniInterpreter
    {
        string filename;
        private Dictionary<string, string> dataDic = new Dictionary<string, string>();

        public IniInterpreter(string filename)
        {
            this.filename = filename;
        }

        /// <summary>
        /// Used to load the default values
        /// </summary>
        /// <param name="kvp"></param>
        public void Include(KeyValuePair<string, string> kvp)
        {
            Write(kvp.Key, kvp.Value);
        }

        public void Load()
        {
             StreamReader sr;
             string s;
             string val, key;
             int equalPos;
             sr=File.OpenText(filename);
             s=sr.ReadLine();
             while(s!=null)
             {
                 if (!s.Contains("="))
                 {
                     s = sr.ReadLine();
                     continue;
                 }
                 equalPos=s.IndexOf('=');
                 key = s.Substring(0, equalPos).Trim();
                 val = s.Substring(equalPos + 1, s.Length - equalPos -1).Trim();
                 dataDic.Add(key.ToLower(),val.ToLower());
                 s=sr.ReadLine();
             }
             sr.Close();
       }

        public void Save()
        {
            StreamWriter sw;
            sw = File.CreateText(filename);

            foreach (KeyValuePair<string, string> data in dataDic)
            {
                sw.WriteLine(data.Key + "=" + data.Value);
            }
            sw.Close();
        }

        public void SaveEncrypted()
        {
            StreamWriter sw;
            sw = File.CreateText(filename);

            foreach (KeyValuePair<string, string> data in dataDic)
            {
                sw.WriteLine(Crypt.Transform(data.Key + "=" + data.Value.ToLower()));
            }
            sw.Close();
        }

        public void LoadEncrypted()
        {
            StreamReader sr;
            string s;
            string val, key;
            int equalPos;
            sr = File.OpenText(filename);
            s = sr.ReadLine();
            while (s != null)
            {
                s = Crypt.Transform(s);
                if (!s.Contains("="))
                {
                    s = sr.ReadLine();
                    continue;
                }
                equalPos = s.IndexOf('=');
                key = s.Substring(0, equalPos).Trim();
                val = s.Substring(equalPos + 1, s.Length - equalPos - 1).Trim();
                dataDic.Add(key, val);
                s = sr.ReadLine();
            }
            sr.Close();
        }


        public string Read(string key)
        {
            key = key.ToLower();
            return dataDic[key].ToLower();
        }

        public string ReadTry(string key)
        {
            try
            {
                key = key.ToLower();
                return dataDic[key].ToLower();
            }
            catch {
                return "";
            }
        }

        public void Write(string key, string val)
        {
            key = key.ToLower();
            if (dataDic.ContainsKey(key))
                dataDic[key] = val;
            else
                dataDic.Add(key, val);
        }

        public void CreateIfNotPresent(KeyValuePair<string, string> kvp)
        {
            if (!dataDic.ContainsKey(kvp.Key.ToLower()))
                dataDic.Add(kvp.Key.ToLower(), kvp.Value);
        }

        public void Erase(string key)
        {
            key = key.ToLower();
            if (dataDic.ContainsKey(key))
                dataDic.Remove(key);
        }
    }


    public static class Crypt
    {
        public static Int32 key = 129;

        public static String Transform(String textToEncrypt)
        {
            if (String.IsNullOrEmpty(textToEncrypt))
                return String.Empty;

            StringBuilder inSb = new StringBuilder(textToEncrypt);
            StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
            char c;
            for (int i = 0; i < textToEncrypt.Length; i++)
            {
                c = inSb[i];
                c = (char)(c ^ key);
                outSb.Append(c);
            }
            return outSb.ToString();
        }
    }