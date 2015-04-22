//Copyright (C) Microsoft Corporation.  All rights reserved.

namespace Ap.Common.Dumper
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ObjectDumper
    {
        public static void Write(object element)
        {
            Write(element, 0);
        }

        public static void Write(object element, int depth)
        {
            Write(element, depth, new DebugWriter());
        }

        public static void Write(object element, int depth, IWriter log)
        {
            var dumper = new ObjectDumper(depth) {_writer = log};
            dumper.WriteObject(null, element);
        }

        private IWriter _writer;
        private int _pos;
        private int _level;
        private readonly int _depth;

        private ObjectDumper(int depth)
        {
            _depth = depth;
        }

        private void Write(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            _writer.Write(message);
            _pos += message.Length;
        }

        private void WriteIndent()
        {
            for (var i = 0; i < _level; i++)
            {
                _writer.Write("  ");
            }
        }

        private void WriteLine()
        {
            _writer.WriteLine();
            _pos = 0;
        }

        private void WriteTab()
        {
            Write("  ");
            while (_pos%8 != 0) Write(" ");
        }

        private void WriteObject(string prefix, object element)
        {
            var processed = ProcessValueType(prefix, element);


            if (processed) return;

            // if it is not processed then try processing it through IEnumerable
            processed = ProcessIterativeType(prefix, element);

            if (!processed)
            {
                // if it is still not processed then it is definitely a ref type.
                ProcessRefType(prefix, element);
            }
        }

        private void ProcessRefType(string prefix, object element)
        {
            var members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
            WriteIndent();
            Write(prefix);
            var propWritten = false;
            foreach (var m in members)
            {
                var f = m as FieldInfo;
                var p = m as PropertyInfo;
                if (f != null || p != null)
                {
                    if (propWritten)
                    {
                        WriteTab();
                    }
                    else
                    {
                        propWritten = true;
                    }
                    Write(m.Name);
                    Write("=");
                    var t = f != null ? f.FieldType : p.PropertyType;
                    if (t.IsValueType || t == typeof (string))
                    {
                        WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
                    }
                    else
                    {
                        if (typeof (IEnumerable).IsAssignableFrom(t))
                        {
                            Write("...");
                        }
                        else
                        {
                            Write("{ }");
                        }
                    }
                }
            }
            if (propWritten) WriteLine();
            if (_level < _depth)
            {
                foreach (var m in members)
                {
                    var f = m as FieldInfo;
                    var p = m as PropertyInfo;
                    if (f != null || p != null)
                    {
                        var t = f != null ? f.FieldType : p.PropertyType;
                        if (!(t.IsValueType || t == typeof (string)))
                        {
                            var value = f != null ? f.GetValue(element) : p.GetValue(element, null);
                            if (value != null)
                            {
                                _level++;
                                WriteObject(m.Name + ": ", value);
                                _level--;
                            }
                        }
                    }
                }
            }
        }

        private bool ProcessIterativeType(string prefix, object element)
        {
            var enumerableElement = element as IEnumerable;
            if (enumerableElement != null)
            {
                foreach (var item in enumerableElement)
                {
                    if (item is IEnumerable && !(item is string))
                    {
                        WriteIndent();
                        Write(prefix);
                        Write("...");
                        WriteLine();
                        if (_level < _depth)
                        {
                            _level++;
                            WriteObject(prefix, item);
                            _level--;
                        }
                    }
                    else
                    {
                        WriteObject(prefix, item);
                    }
                }

                return true;
            }

            return false;
        }

        private bool ProcessValueType(string prefix, object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                WriteIndent();
                Write(prefix);
                WriteValue(element);
                WriteLine();

                return true;
            }

            return false;
        }

        private void WriteValue(object o)
        {
            if (o == null)
            {
                Write("null");
            }
            else if (o is DateTime)
            {
                Write(((DateTime) o).ToShortDateString());
            }
            else if (o is ValueType || o is string)
            {
                Write(o.ToString());
            }
            else if (o is IEnumerable)
            {
                Write("...");
            }
            else
            {
                Write("{ }");
            }
        }
    }
}