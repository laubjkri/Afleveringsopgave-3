using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Afleveringsopgave_3
{
    public class CsvReaderRow
    {
        private List<string> _columns;
        private CsvReaderHeaders? _headers;
        private int _rowIndex;        
        public int RowIndex => _rowIndex;

        public CsvReaderRow(string row, char delimiter, int rowIndex, CsvReaderHeaders? headers)            
        {
            _columns = SplitRowToStringList(row, delimiter);
            _headers = headers;
            _rowIndex = rowIndex;
        }

        private static List<string> SplitRowToStringList(string row, char delimiter)
        {
            string[] entriesArray = row.Split(delimiter);
            return entriesArray.ToList();
        }

        private int GetColumnIndex(string header)
        {
            if (_headers == null)
            {
                throw new InvalidOperationException("Headers have not been defined.");
            }

            return _headers.GetColumnIndex(header);
        }

        public string GetColumnString(int columnIndex)
        {
            return _columns[columnIndex];
        }

        public string GetColumnString(string header)
        {
            return _columns[GetColumnIndex(header)];          
        }

        public double GetColumnDouble(int columnIndex)
        {            
            string columnString = GetColumnString(columnIndex);

            if (double.TryParse(columnString, out double columnValue))
            {
                return columnValue;
            }
            else 
            {
                if (string.IsNullOrWhiteSpace(columnString))
                {
                    columnString = "(empty)";
                }

                throw new Exception($"Could not get double value from column {columnIndex} row {_rowIndex}. Column content: {columnString}");
            }

        }

        public double GetColumnDouble(string header) 
        { 
            int columnIndex = GetColumnIndex(header);
            return GetColumnDouble(columnIndex);
        }

    }
}
