using KL_Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace Afleveringsopgave_3
{
    public class CsvReader
    {
        public event Action<string>? LogEvent;
        public event Action<string>? PopupEvent;
        public event Action? FileReadDoneEvent;

        public string FilePath { get; set; } = string.Empty;
        public char Delimiter = ';';
        public bool FirstRowIsHeader { get; set; } = true;
        public ReadOnlyCollection<CsvReaderRow> Rows => _rows.AsReadOnly();

        private CsvReaderHeaders? _headers;
        private List<CsvReaderRow> _rows = new();
        private Task _exclusiveReadTask = Task.CompletedTask;
        private int _lineNumber;


        /// <summary>
        /// Start a line by line file read.
        /// The file path must be assigned to the owning object.
        /// Only allows one search to be started at a time.
        /// </summary>
        /// <returns>True if new read is successfully started.</returns>
        public async Task<bool> ReadFileLinesExclusive()
        {
            if (_exclusiveReadTask.IsCompleted)
            {
                _exclusiveReadTask = ReadFile();
                await _exclusiveReadTask;
                return true;
            }
            else
            {
                InvokePopupEvent("A read is already active.");
                await Task.CompletedTask;
                return false;
            }
        }

        public Task ReadFile()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                Error("File path is missing.");
            }

            Timer timer = new Timer();
            timer.Start();

            // Using a stream reader to avoid loading the whole file into memory at once
            // as would be the case with File.ReadAllLines()
            using (KL_Utils.StreamReader streamReader = new(FilePath))
            {
                InvokeLogEvent("File read started...");
                streamReader.NewLine = "\r\n";

                if (FirstRowIsHeader)
                {
                    string? firstRow = streamReader.ReadLine();
                    if(firstRow != null)
                    {
                        _headers = new CsvReaderHeaders(firstRow, Delimiter);
                        _lineNumber++;
                    }
                    else
                    {
                        Error("First line of file could not be read.");
                    }
                }                

                // Get the rest of the rows with values
                string? line = streamReader.ReadLine();
                _rows.Clear();
                int entriesFound = 0;

                while (!string.IsNullOrEmpty(line))
                {
                    _rows.Add(new CsvReaderRow(line, Delimiter, _lineNumber, _headers));
                    entriesFound++;
                    _lineNumber++;

                    line = streamReader.ReadLine();
                }

                timer.Stop();

                if (entriesFound == 0)
                {
                    InvokeLogEvent($"File read done in {timer.ElapsedTimeMs} ms. No entries found.");
                }
                else if (entriesFound == 1)
                {
                    InvokeLogEvent($"File read done in {timer.ElapsedTimeMs} ms. {entriesFound} entry found.");
                }
                else
                {
                    InvokeLogEvent($"File read done in {timer.ElapsedTimeMs} ms. {entriesFound} entries found.");
                }

                InvokeFileReadDoneEvent();
            }            

            return Task.CompletedTask;
        }

        public int GetColumnIndexFromHeader(string header)
        {
            try
            {
                if (!FirstRowIsHeader) throw new Exception("The data does not have headers.");
                if (_headers == null) throw new Exception("No headers have been read.");

                return _headers.GetColumnIndex(header);
            }
            catch (Exception e)
            {
                Error(e.Message);
                return 0;
            }            
        }

        private static void Error(string errorMessage, Exception? ex = null)
        {
            string classMessage = "Error CsvReader class: ";
            if (ex != null)
            {
                throw new Exception(classMessage + errorMessage, ex);
            }
            else
            {
                throw new Exception(classMessage + errorMessage);
            }            
        }

        private void InvokeLogEvent(string message) { LogEvent?.Invoke(message); }
        private void InvokePopupEvent(string message) { PopupEvent?.Invoke(message); }
        private void InvokeFileReadDoneEvent() { FileReadDoneEvent?.Invoke(); }



    }
}
