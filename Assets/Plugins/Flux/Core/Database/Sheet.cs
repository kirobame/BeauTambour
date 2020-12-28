using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Flux
{
    [Serializable]
    public class Sheet
    {
        public bool IsInitialized => size != Vector2Int.zero;

        public string Name => name;
        public string Version => version;
    
        public Vector2Int Size => size;

        [SerializeField] private string name;
        [SerializeField] private string version;
    
        [SerializeField] private string[] array;
        [SerializeField] private Vector2Int size;

        public string this[Vector2Int index] => this[index.x, index.y];
        public string this[int x, int y]
        {
            get
            {
                var index = x + y * size.x;
                return array[index];
            }
        }

        public bool Process(string data)
        {
            var lines = new List<List<string>>();
            lines.Add(new List<string>());

            var y = 0;
            var advancement = 0;
            var occurence = data.IndexOf(',', advancement);

            while (occurence != -1)
            {
                var substring = data.Substring(advancement, occurence - advancement);
                if (substring.Contains('"'))
                {
                    occurence = data.IndexOf('"', advancement + 1) + 1;
                    substring = data.Substring(advancement + 1, occurence - advancement - 2);
                }
                else
                {
                    var split =  Regex.Split(substring, "\r\n|\r|\n");
                    if (split.Length == 2)
                    {
                        lines[y].Add(split[0]);

                        lines.Add(new List<string>());
                        y++;
                        
                        lines[y].Add(split[1]);

                        advancement = occurence + 1;
                        occurence = data.IndexOf(',', advancement);

                        continue;
                    }
                }
                
                lines[y].Add(substring);

                advancement = occurence + 1;
                if (advancement >= data.Length)
                {
                    occurence = -1;
                    continue;
                }

                occurence = data.IndexOf(',', advancement);
                if (occurence == -1)
                {
                    var entry = data.Substring(advancement);
                    entry = entry.Replace(new string(new char[] { (char)34 }), string.Empty);
                    
                    lines[y].Add(entry);
                }
            }

            size = new Vector2Int(lines.First().Count - 1, lines.Count - 1);
            array = new string[size.x * size.y];

            var header = lines[0][0].Split('=');
            name = header[0];
            version = header[1];
            
            var index = 0;
            for (y = 1; y < lines.Count; y++)
            {
                for (var x = 1; x < size.x + 1; x++)
                {
                    //Debug.Log($"For CURRENT : ({x - 1}, {y - 1}) / TOTAL : ({size.x}, {size.y}) || LENGTH : ({lines[y].Count})");
                    //Debug.Log($"Entry is : {lines[y][x]} --- {index}/{array.Length}");
                    
                    array[index] = lines[y][x];
                    index++;
                }
            }

            return true;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var y = 0; y < size.y; y++)
            {
                for (var x = 0; x < size.x; x++)
                {
                    var item = this[x, y];

                    var value = item == string.Empty ? "Empty" : item;
                    var suffix = x == size.x - 1 ? ";" : ",";

                    builder.Append($" {value}{suffix}");
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}