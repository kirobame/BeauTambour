using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Flux
{
    public class RuntimeSheet
    {
        public Sheet Source { get; private set; }
        
        //--------------------------------------------------------------------------------------------------------------
        
        public IReadOnlyDictionary<string, int> Columns => columns;
        public IReadOnlyDictionary<string, int> Rows => rows;
        
        public IEnumerable<string> ArrayKeys => values.Keys;
        public IReadOnlyDictionary<string, List<string>> ColumnKeys => columnKeys;
        public IReadOnlyDictionary<string, List<string>> RowKeys => rowKeys;

        public IEnumerable<string[,]> Arrays => values.Select(keyValuePair => keyValuePair.Value);
        public IEnumerable<string> Values
        {
            get
            {
                var list = new List<string>();
                foreach (var array in Arrays)
                {
                    for (var x = 0; x < array.GetLength(0); x++)
                    {
                        for (var y = 0; y < array.GetLength(1); y++) list.Add(array[x,y]);
                    }
                }

                return list;
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        private Dictionary<string, int> columns = new Dictionary<string, int>();
        private Dictionary<string, int> rows = new Dictionary<string, int>();
        
        private Dictionary<string, string[,]> values = new Dictionary<string, string[,]>();
        
        private Dictionary<string, List<string>> columnKeys = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> rowKeys = new Dictionary<string, List<string>>();

        //--------------------------------------------------------------------------------------------------------------
        
        public string[,] this[string arrayKey] => values[arrayKey];
        public string this[string arrayKey, string columnKey, string rowKey]
        {
            get
            {
                return values[arrayKey][columns[columnKey], rows[rowKey]];
            }
        }

        public bool TryGet(string arrayKey, string columnKey, string rowKey , out string result)
        {
            var isArrayKeyValid = values.ContainsKey(arrayKey);
            var isColumnKeyValid = columns.ContainsKey(columnKey);
            var isRowKeyValid = rows.ContainsKey(rowKey);

            if (isArrayKeyValid && isColumnKeyValid && isRowKeyValid)
            {
                result = this[arrayKey, columnKey, rowKey];
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        public void Process(Sheet sheet)
        {
            Source = sheet;
            
            columns.Clear();
            rows.Clear();
            
            values.Clear();
            
            columnKeys.Clear();
            rowKeys.Clear();

            for (var x = 0; x < sheet.Size.x; x++)
            {
                for (var y = 0; y < sheet.Size.y; y++)
                {
                    var item = sheet[x,y];
                    if (item == string.Empty || item[0] != 'ⓘ') continue;

                    var id = item.Replace("ⓘ-", string.Empty);

                    columnKeys.Add(id, new List<string>());
                    rowKeys.Add(id, new List<string>());
                    
                    var size = Vector2Int.zero;

                    for (var i = x + 1; i < sheet.Size.x; i++)
                    {
                        var subItem = sheet[i,y];
                        if (!subItem.Contains('Ⓒ')) break;

                        var key = subItem.Replace("Ⓒ-", string.Empty);
                        
                        columns.Add(key, size.x);
                        columnKeys[id].Add(key);

                        size.x++;
                    }
                    
                    for (var i = y + 1; i < sheet.Size.y; i++)
                    {
                        var subItem = sheet[x,i];
                        if (!subItem.Contains('Ⓡ')) break;

                        var key = subItem.Replace("Ⓡ-", string.Empty);
                        
                        rows.Add(key, size.y);
                        rowKeys[id].Add(key);

                        size.y++;
                    }

                    var array = new string[size.x, size.y];
                    for (var subX = 0; subX < size.x; subX++)
                    {
                        for (var subY = 0; subY < size.y; subY++)
                        {
                            var value = sheet[x + 1 + subX, y + 1 + subY];
                            array[subX, subY] = value;
                        }
                    }
                    
                    values.Add(id, array);
                }
            }
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        private bool IsValueValid(string value)
        {
            if (value == string.Empty) return false;
            
            var isColumnId = value.Contains('Ⓒ');
            var isRowId = value.Contains('Ⓡ');
            var isArrayId = value.Contains('ⓘ');

            return !isColumnId && !isRowId && !isArrayId;
        }
        
        //--------------------------------------------------------------------------------------------------------------

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var arrayKey in values.Keys)
            {
                builder.AppendLine($"Array | {arrayKey} |---------------------------------");

                var columns = columnKeys[arrayKey];
                var rows = rowKeys[arrayKey];

                for (var x = 0; x < columns.Count; x++)
                {
                    for (var y = 0; y < rows.Count; y++)
                    {
                        builder.AppendLine($"---- : R.{columns[x]} / C.{rows[y]} = {this[arrayKey, columns[x], rows[y]]}");
                    }
                }
            }

            return builder.ToString();
        }
    }
}