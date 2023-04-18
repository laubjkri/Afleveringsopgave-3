using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afleveringsopgave_3
{
    public class CsvReaderHeaders
    {
        private Dictionary<string, int> _headers;
        public CsvReaderHeaders(string line, char delimiter)
        {
            _headers = new Dictionary<string, int>();

            string[] headersArray = line.Split(delimiter);

            for (int i = 0; i < headersArray.Length; i++)
            {
                // Using TryAdd to ignore headers that exist multiple times
                _headers.TryAdd(headersArray[i], i);                
            }
        }

        public int GetColumnIndex(string header)
        {
            if (_headers.TryGetValue(header, out int columnIndex))
            {
                return columnIndex;
            }
            else
            {
                throw new KeyNotFoundException($"Header \"{header}\" was not found.");
            }            
        }
    }
}
