using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigEditor
{
    namespace Exceptions
    {
        class InvalidInputException : System.Exception { }
        class ValueDoesntExistException : System.Exception { }
        class KeyDoesntExistException : System.Exception { }
    }

    public class ConfigEditor
    {
        Hashtable Keys;
        Hashtable Values;

        public ConfigEditor()
        {
            Keys = new Hashtable();
            Values = new Hashtable();
        }

        public ConfigEditor(TextReader reader)
        {
            Keys = new Hashtable();
            Values = new Hashtable();
            ConfigParser(reader);
        }

        public string GetValue(string Key, string ValueName)
        {
            Hashtable KeyTable = (Hashtable)Keys[Key];

            if (KeyTable != null)
            {
                if (KeyTable.ContainsKey(ValueName))
                {
                    return (string)KeyTable[ValueName];
                }
                else
                {
                    throw new Exceptions.ValueDoesntExistException();
                }
            }
            else
            {
                throw new Exceptions.KeyDoesntExistException();
            }
        }

        public string GetValue(string ValueName)
        {
            if (Values.ContainsKey(ValueName))
            {
                return (string)Values[ValueName];
            }
            else
            {
                throw new Exceptions.ValueDoesntExistException();
            }
        }

        public void RemoveKey(string KeyName)
        {
            if (Keys.ContainsKey(KeyName))
            {
                Keys.Remove(KeyName);
            }
        }

        public bool KeyExists(string KeyName)
        {
            return Keys.Contains(KeyName);
        }

        public bool ValueExists(string KeyName, string ValueName)
        {
            Hashtable KeyTable = (Hashtable)Keys[KeyName];

            if (KeyTable != null)
            {
                return KeyTable.Contains(ValueName);
            }
            else
            {
                throw new Exceptions.KeyDoesntExistException();
            }
        }

        public bool ValueExists(string ValueName)
        {
            return Values.Contains(ValueName);
        }

        public void SetValue(string Key, string ValueName, string Value)
        {
            Hashtable KeyTable = (Hashtable)Keys[Key];

            if (KeyTable != null)
            {
                KeyTable[ValueName] = Value;
            }
            else
            {
                throw new Exceptions.KeyDoesntExistException();
            }
        }

        public void SetValue(string ValueName, string Value)
        {
            Values[ValueName] = Value;
        }

        public void AddKey(string NewKey)
        {
            Hashtable New = new Hashtable();
            Keys[NewKey] = New;
        }

        public IList<string> GetKeyList()
        {
            IDictionaryEnumerator Enumerator = Keys.GetEnumerator();
            IList<string> KeyList = new List<string>();

            while (Enumerator.MoveNext())
            {
                KeyList.Add((string)Enumerator.Key);
            }

            return KeyList;
        }

        public void SaveConfig(TextWriter writer)
        {
            IDictionaryEnumerator Enumerator = Values.GetEnumerator();

            writer.WriteLine("#############################################################");
            writer.WriteLine("# This config file is for the MDM application. Do NOT edit! #");
            writer.WriteLine("#############################################################");
            writer.WriteLine("\n\r\n\r");

            writer.WriteLine("#Keyless Entries");
            writer.WriteLine("\n\r\n\r");

            while (Enumerator.MoveNext())
            {
                writer.WriteLine("{0} = {1}", Enumerator.Key, Enumerator.Value);
            }

            writer.WriteLine("#Keyed Entries");
            writer.WriteLine("\n\r\n\r");

            Enumerator = Keys.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                IDictionaryEnumerator SecondEnumerator = ((Hashtable)Enumerator.Value).GetEnumerator();

                writer.WriteLine("[{0}]", Enumerator.Key);
                while (SecondEnumerator.MoveNext())
                {
                    writer.WriteLine("{0} = {1}", SecondEnumerator.Key, SecondEnumerator.Value);
                }
            }
        }

        private void ConfigParser(System.IO.TextReader reader)
        {
            System.Collections.Hashtable CurrentKey = null;
            string Line, ValueName, Value;
            while (null != (Line = reader.ReadLine()))
            {
                int j, i = 0;
                while (Line.Length > i && Char.IsWhiteSpace(Line, i)) i++;
                if (Line.Length <= i)
                    continue;
                if (Line[i] == '#')
                    continue;
                if (Line[i] == '[')
                {
                    string KeyName;
                    j = Line.IndexOf(']', i);
                    if (j == -1)
                        throw new Exceptions.InvalidInputException();

                    KeyName = Line.Substring(i + 1, j - i - 1).Trim();

                    if (!Keys.ContainsKey(KeyName))
                    {
                        this.AddKey(KeyName);
                    }
                    CurrentKey = (System.Collections.Hashtable)Keys[KeyName];
                    while (Line.Length > ++j && Char.IsWhiteSpace(Line, j));
                    if (Line.Length > j)
                    {
                        if (Line[j] != '#')
                            throw new Exceptions.InvalidInputException();
                    }
                    continue;
                }
                
                j = Line.IndexOf('=', i);
                if (j == -1)
                    throw new Exceptions.InvalidInputException();
                ValueName = Line.Substring(i, j - i).Trim();
                if ((i = Line.IndexOf('#', j + 1)) != -1)
                    Value = Line.Substring(j + 1, i - (j + 1)).Trim();
                else
                    Value = Line.Substring(j + 1).Trim();
                if (CurrentKey != null)
                {
                    CurrentKey[ValueName] = Value;
                }
                else
                {
                    Values[ValueName] = Value;
                }
            }
        }
    }
}
