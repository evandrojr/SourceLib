﻿    /*
  
		   GNU LESSER GENERAL PUBLIC LICENSE
                       Version 3, 29 June 2007

 Copyright (C) 2007 Free Software Foundation, Inc. <http://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.


  This version of the GNU Lesser General Public License incorporates
the terms and conditions of version 3 of the GNU General Public
License, supplemented by the additional permissions listed below.

  0. Additional Definitions. 

  As used herein, "this License" refers to version 3 of the GNU Lesser
General Public License, and the "GNU GPL" refers to version 3 of the GNU
General Public License.

  "The Library" refers to a covered work governed by this License,
other than an Application or a Combined Work as defined below.

  An "Application" is any work that makes use of an interface provided
by the Library, but which is not otherwise based on the Library.
Defining a subclass of a class defined by the Library is deemed a mode
of using an interface provided by the Library.

  A "Combined Work" is a work produced by combining or linking an
Application with the Library.  The particular version of the Library
with which the Combined Work was made is also called the "Linked
Version".

  The "Minimal Corresponding Source" for a Combined Work means the
Corresponding Source for the Combined Work, excluding any source code
for portions of the Combined Work that, considered in isolation, are
based on the Application, and not on the Linked Version.

  The "Corresponding Application Code" for a Combined Work means the
object code and/or source code for the Application, including any data
and utility programs needed for reproducing the Combined Work from the
Application, but excluding the System Libraries of the Combined Work.

  1. Exception to Section 3 of the GNU GPL.

  You may convey a covered work under sections 3 and 4 of this License
without being bound by section 3 of the GNU GPL.

  2. Conveying Modified Versions.

  If you modify a copy of the Library, and, in your modifications, a
facility refers to a function or data to be supplied by an Application
that uses the facility (other than as an argument passed when the
facility is invoked), then you may convey a copy of the modified
version:

   a) under this License, provided that you make a good faith effort to
   ensure that, in the event an Application does not supply the
   function or data, the facility still operates, and performs
   whatever part of its purpose remains meaningful, or

   b) under the GNU GPL, with none of the additional permissions of
   this License applicable to that copy.

  3. Object Code Incorporating Material from Library Header Files.

  The object code form of an Application may incorporate material from
a header file that is part of the Library.  You may convey such object
code under terms of your choice, provided that, if the incorporated
material is not limited to numerical parameters, data structure
layouts and accessors, or small macros, inline functions and templates
(ten or fewer lines in length), you do both of the following:

   a) Give prominent notice with each copy of the object code that the
   Library is used in it and that the Library and its use are
   covered by this License.

   b) Accompany the object code with a copy of the GNU GPL and this license
   document.

  4. Combined Works.

  You may convey a Combined Work under terms of your choice that,
taken together, effectively do not restrict modification of the
portions of the Library contained in the Combined Work and reverse
engineering for debugging such modifications, if you also do each of
the following:

   a) Give prominent notice with each copy of the Combined Work that
   the Library is used in it and that the Library and its use are
   covered by this License.

   b) Accompany the Combined Work with a copy of the GNU GPL and this license
   document.

   c) For a Combined Work that displays copyright notices during
   execution, include the copyright notice for the Library among
   these notices, as well as a reference directing the user to the
   copies of the GNU GPL and this license document.

   d) Do one of the following:

       0) Convey the Minimal Corresponding Source under the terms of this
       License, and the Corresponding Application Code in a form
       suitable for, and under terms that permit, the user to
       recombine or relink the Application with a modified version of
       the Linked Version to produce a modified Combined Work, in the
       manner specified by section 6 of the GNU GPL for conveying
       Corresponding Source.

       1) Use a suitable shared library mechanism for linking with the
       Library.  A suitable mechanism is one that (a) uses at run time
       a copy of the Library already present on the user's computer
       system, and (b) will operate properly with a modified version
       of the Library that is interface-compatible with the Linked
       Version. 

   e) Provide Installation Information, but only if you would otherwise
   be required to provide such information under section 6 of the
   GNU GPL, and only to the extent that such information is
   necessary to install and execute a modified version of the
   Combined Work produced by recombining or relinking the
   Application with a modified version of the Linked Version. (If
   you use option 4d0, the Installation Information must accompany
   the Minimal Corresponding Source and Corresponding Application
   Code. If you use option 4d1, you must provide the Installation
   Information in the manner specified by section 6 of the GNU GPL
   for conveying Corresponding Source.)

  5. Combined Libraries.

  You may place library facilities that are a work based on the
Library side by side in a single library together with other library
facilities that are not Applications and are not covered by this
License, and convey such a combined library under terms of your
choice, if you do both of the following:

   a) Accompany the combined library with a copy of the same work based
   on the Library, uncombined with any other library facilities,
   conveyed under the terms of this License.

   b) Give prominent notice with the combined library that part of it
   is a work based on the Library, and explaining where to find the
   accompanying uncombined form of the same work.

  6. Revised Versions of the GNU Lesser General Public License.

  The Free Software Foundation may publish revised and/or new versions
of the GNU Lesser General Public License from time to time. Such new
versions will be similar in spirit to the present version, but may
differ in detail to address new problems or concerns.

  Each version is given a distinguishing version number. If the
Library as you received it specifies that a certain numbered version
of the GNU Lesser General Public License "or any later version"
applies to it, you have the option of following the terms and
conditions either of that published version or of any later version
published by the Free Software Foundation. If the Library as you
received it does not specify a version number of the GNU Lesser
General Public License, you may choose any version of the GNU Lesser
General Public License ever published by the Free Software Foundation.

  If the Library as you received it specifies that a proxy can decide
whether future versions of the GNU Lesser General Public License shall
apply, that proxy's public statement of acceptance of any version is
permanent authorization for you to choose that version for the
Library.
  
 
*/

// TODO: Documentar esta classe


using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.IO;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Xml;

namespace ShowLib
{

    public class Csv
    {
        public char DelimitadorCaractere = ';';
        public string ArquivoNome;
        private TextReader tr;
        public CsvLinha Linha;
        public System.Globalization.CultureInfo CultureInfoBRA = new System.Globalization.CultureInfo("pt-br");
        public System.Globalization.CultureInfo CultureInfoEUA = new System.Globalization.CultureInfo("en-us");

        public Csv()
        {
        }

        public Csv(string arquivoNome)
        {
            ArquivoNome = arquivoNome;           
        }

        public void ColumnsRead()
        {
            List<string> lstColunaNome = new List<string>();
            tr = new StreamReader(ArquivoNome, System.Text.Encoding.Default);
            lstColunaNome = LerLinhaTexto();
            Linha = new CsvLinha();

            foreach (string s in lstColunaNome)
            {
                CsvColuna col = new CsvColuna(s);
                Linha.LstCsvColuna.Add(col);
            }
        }

        public void Fechar()
        {
            tr.Close();
        }

        public void EscreveCsv(DataTable dt, string ArquivoNome)
        {
            StringBuilder sb = new StringBuilder();
            string valor = "";
            TextWriter tw = new StreamWriter(ArquivoNome, false, System.Text.Encoding.Default);
            decimal dc;

            for (int x = 0; x < dt.Columns.Count; ++x)
            {
                sb.Append(dt.Columns[x].ColumnName);
                if (x < dt.Columns.Count - 1)
                    sb.Append(DelimitadorCaractere);
            }
            tw.WriteLine(sb.ToString());

            for (int y = 0; y < dt.Rows.Count; ++y)
            {
                sb = new StringBuilder();
                for (int x = 0; x < dt.Columns.Count; ++x)
                {
                    valor = dt.Rows[y][x].ToString().Trim();
                    if (Decimal.TryParse(valor, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfoEUA, out dc))
                    {
                        valor = valor.Replace(",", "");
                        valor = valor.Replace('.', ',');
                    }
                    else
                        valor = Convert.ToString(dt.Rows[y][x], CultureInfoBRA).Trim();
                    sb.Append("\"" + valor.Replace("\"", "\"\"") + "\"");
                    if (x < dt.Columns.Count - 1)
                        sb.Append(DelimitadorCaractere);
                }
                tw.WriteLine(sb.ToString());
            }
            tw.Close();
        }

        public bool LerLinha()
        {
            string linha, valor = "";
            linha = tr.ReadLine();
            if (linha == null)
                return false;
            int linhaLength = linha.Length;
            int delimitadorPosicao = 0, delimitadorUltimaPosicao = -1;
            bool ultimoDelimitadorEraFalso = false;
            List<int> lstAspaPosA = new List<int>();
            List<int> lstAspaPosB = new List<int>();
            int aspasPosA = 0, aspasPosB = 0, delimitadorUltimaPosicaoFalsa = 0;
            int index = 0;
            int aspasCnt = 0;
            bool encontrouAspasA, encontrouAspasB;

            //Guarda a posição das aspas duplas

            do
            {
                //if (aspasCnt == 28)
                //    aspasCnt = aspasCnt;
                encontrouAspasA = encontrouAspasB = false;
                aspasPosA = linha.IndexOf('"', aspasPosA);
                if (aspasPosA == -1)
                    break;
                //Verifica se acabou 
                if (aspasPosA + 2 > linhaLength)
                {
                    //Achou A mas não achou B, esta inconsistente!
                    break;
                }
                else
                {
                    //possivel ter outro registro
                    if (linha.Length > aspasPosA + 2)
                    {

                        //Detecta aspas falsa
                        if (linha[aspasPosA + 1] == '"' && linha[aspasPosA + 2] != ';')
                        {

                            //É falsa, tente a próxima aspas
                            ++aspasPosA;
                            continue;
                        }
                        else
                        {
                            encontrouAspasA = true;
                        }
                    }
                    else
                        encontrouAspasA = true;
                }

                if (aspasPosA + 2 > linhaLength)
                {
                    throw new Exception("Faltou fecha aspas em " + linha);
                }

                aspasPosB = aspasPosA;
                do
                {
                    if (aspasPosB >= linhaLength)
                    {
                        throw new Exception("Faltou fecha aspas em " + linha);
                    }
                    else
                    {
                        aspasPosB = linha.IndexOf('"', aspasPosB + 1);
                        //Toda as aspas são verdadeiras até que se prove o contrário!
                        encontrouAspasB = true;
                        //Detecta aspas falsa
                        if (aspasPosB + 2 <= linhaLength)
                            if (linha[aspasPosB + 1] == '"')
                            {
                                //É falsa, tente a próxima aspas
                                ++aspasPosB;
                                continue;
                            }
                            else
                            {
                                encontrouAspasB = true;
                                break;
                            }
                    }
                } while (linha.IndexOf('"', aspasPosB + 1) != -1);
                if (encontrouAspasA && !encontrouAspasB)
                    throw new Exception("Faltou fecha aspas em " + linha);
                lstAspaPosA.Add(aspasPosA);
                lstAspaPosB.Add(aspasPosB);
                aspasPosA = aspasPosB + 1;
                aspasCnt++;

            } while (aspasPosA != -1);

            if (lstAspaPosA.Count != lstAspaPosB.Count)
            {
                throw new Exception("Arquivo CSV inconsistente!");
            }

            //Lê os valores
            delimitadorPosicao = -1;
            do
            {
                //Entra aqui na primeira vez pois na primeira nem existia delimitador para ser falso
                if (!ultimoDelimitadorEraFalso)
                {
                    delimitadorUltimaPosicao = delimitadorPosicao;
                }

                if (!ultimoDelimitadorEraFalso)
                    delimitadorPosicao = linha.IndexOf(DelimitadorCaractere, delimitadorUltimaPosicao + 1);
                else
                    delimitadorPosicao = linha.IndexOf(DelimitadorCaractere, delimitadorUltimaPosicaoFalsa + 1);

                // Testa para ver se o delimitador encontra era dentro de um campo,
                // não uma delimitador separador de campos;
                //ultimoDelimitadorEraFalso = false;
                ultimoDelimitadorEraFalso = false;
                for (int i = 0; i < lstAspaPosA.Count; ++i)
                {
                    if (lstAspaPosA[i] < delimitadorPosicao && delimitadorPosicao < lstAspaPosB[i])
                    {
                        ultimoDelimitadorEraFalso = true;
                        delimitadorUltimaPosicaoFalsa = lstAspaPosB[i];
                        continue;
                    }
                }
                if (ultimoDelimitadorEraFalso)
                    continue;
                delimitadorUltimaPosicaoFalsa = 0;
                //Não chegou na última virgula ainda
                if (!(delimitadorPosicao == -1))
                    valor = linha.Substring(delimitadorUltimaPosicao + 1, delimitadorPosicao - delimitadorUltimaPosicao - 1);
                else
                    valor = linha.Substring(delimitadorUltimaPosicao + 1, linhaLength - delimitadorUltimaPosicao - 1);
                //Remove aspas duplas quando houver
                if (valor.Length > 0)
                {
                    if (valor[0] == '"' && valor[valor.Length - 1] == '"')
                        valor = valor.Substring(1, valor.Length - 2);
                    valor = valor.Replace("\"\"", "\"");
                }
                Linha.LstCsvColuna[index].Valor = valor;
                ++index;
            } while (delimitadorPosicao != -1);

            return true;
        }


        private List<string> LerLinhaTexto()
        {
            string linha, valor = "";
            linha = tr.ReadLine();
            if (linha == null)
            {
                List<string> vazia = null;
                return vazia;
            }
            int linhaLength = linha.Length;
            int delimitadorPosicao = 0, delimitadorUltimaPosicao = -1;
            bool ultimoDelimitadorEraFalso = false;
            List<string> lstValor = new List<string>();
            List<int> lstAspaPosA = new List<int>();
            List<int> lstAspaPosB = new List<int>();
            int aspasPosA = 0, aspasPosB = 0, delimitadorUltimaPosicaoFalsa = 0;
            int index = 0;
            bool encontrouAspasA, encontrouAspasB;


            //Guarda a posição das aspas duplas

            do
            {
                encontrouAspasA = encontrouAspasB = false;
                aspasPosA = linha.IndexOf('"', aspasPosA);
                if (aspasPosA == -1)
                    break;
                //Verifica se acabou 
                if (aspasPosA + 2 > linhaLength)
                {
                    //Achou A mas não achou B, esta inconsistente!
                    break;
                }
                else
                {
                    //Detecta aspas falsa
                    if (linha[aspasPosA + 1] == '"')
                    {
                        //É falsa, tente a próxima aspas
                        ++aspasPosA;
                        continue;
                    }
                    else
                    {
                        encontrouAspasA = true;
                    }
                }

                if (aspasPosA + 2 > linhaLength)
                {
                    throw new Exception("Faltou fecha aspas em " + linha);
                }

                aspasPosB = aspasPosA;
                do
                {
                    if (aspasPosB + 1 > linhaLength)
                    {
                        throw new Exception("Faltou fecha aspas em " + linha);
                    }
                    else
                    {
                        aspasPosB = linha.IndexOf('"', aspasPosB + 1);
                        //Detecta aspas falsa
                        if (aspasPosB + 2 <= linhaLength)
                            if (linha[aspasPosB + 1] == '"')
                            {
                                //É falsa, tente a próxima aspas
                                ++aspasPosB;
                                continue;
                            }
                            else
                            {
                                encontrouAspasB = true;
                                break;
                            }
                    }
                } while (aspasPosB != -1);
                if (encontrouAspasA && !encontrouAspasB)
                    throw new Exception("Faltou fecha aspas em " + linha);
                lstAspaPosA.Add(aspasPosA);
                lstAspaPosB.Add(aspasPosB);
                aspasPosA = aspasPosB + 1;

            } while (aspasPosA != -1);

            if (lstAspaPosA.Count != lstAspaPosB.Count)
            {
                throw new Exception("Arquivo CSV inconsistente!");
            }

            //Lê os valores
            delimitadorPosicao = -1;
            do
            {
                //Entra aqui na primeira vez pois na primeira nem existia delimitador para ser falso
                if (!ultimoDelimitadorEraFalso)
                {
                    delimitadorUltimaPosicao = delimitadorPosicao;
                }

                if (!ultimoDelimitadorEraFalso)
                    delimitadorPosicao = linha.IndexOf(DelimitadorCaractere, delimitadorUltimaPosicao + 1);
                else
                    delimitadorPosicao = linha.IndexOf(DelimitadorCaractere, delimitadorUltimaPosicaoFalsa + 1);

                // Quer dizer que a delimitador encontra era dentro de um campo,
                // não uma delimitador separadora de campos;
                //ultimoDelimitadorEraFalso = false;
                ultimoDelimitadorEraFalso = false;
                for (int i = 0; i < lstAspaPosA.Count; ++i)
                {
                    if (lstAspaPosA[i] < delimitadorPosicao && delimitadorPosicao < lstAspaPosB[i])
                    {
                        ultimoDelimitadorEraFalso = true;
                        delimitadorUltimaPosicaoFalsa = lstAspaPosB[i];
                        continue;
                    }
                }
                if (ultimoDelimitadorEraFalso)
                    continue;
                delimitadorUltimaPosicaoFalsa = 0;
                //Não chegou na última virgula ainda
                if (!(delimitadorPosicao == -1))
                    valor = linha.Substring(delimitadorUltimaPosicao + 1, delimitadorPosicao - delimitadorUltimaPosicao - 1);
                else
                    valor = linha.Substring(delimitadorUltimaPosicao + 1, linhaLength - delimitadorUltimaPosicao - 1);
                //Remove aspas duplas quando houver
                if (valor[0] == '"' && valor[valor.Length - 1] == '"')
                    valor = valor.Substring(1, valor.Length - 2);
                valor = valor.Replace("\"\"", "\"");
                lstValor.Add(valor);
                ++index;

            } while (delimitadorPosicao != -1);

            return lstValor;

        }

    }

    public class CsvLinha
    {
        public List<CsvColuna> LstCsvColuna;

        public CsvLinha()
        {
            LstCsvColuna = new List<CsvColuna>();
        }
    }

    public class CsvColuna
    {
        public CsvColuna(string nome)
        {
            Nome = nome;
        }

        public string Valor;
        public string Nome;
    }
}
