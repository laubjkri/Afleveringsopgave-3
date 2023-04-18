using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KL_Utils
{

    /// <summary>
    /// This class was made so i could specify the newline character(s).
    /// The standard streamreader class has a fixed line change of either \r,\n or \r\n. 
    /// This can be a problem if reading an excel edited CSV file where line changes inside a cell are made with \n.
    /// </summary>
    public class StreamReader : System.IO.StreamReader
    {
        private Queue<int> _buffer;

        public string NewLine { get; set; } = "\r\n"; // Default to Windows line break
        
        public StreamReader(string filePath) : base(filePath) 
        {
            _buffer = new();            
        }

        private new int Read()
        {
            if(_buffer.Count > 0)
            {
                return _buffer.Dequeue();
            }
            else
            {
                return base.Read();
            }
        }

        private void PutBack(int ch)
        {
            _buffer.Enqueue(ch);
        }

        public override string? ReadLine()
        {
            StringBuilder sb = new StringBuilder();

            // Continue reading until line break or EOF
            while (true)
            {
                int readReturn = Read();

                // Check for EOF
                if (readReturn == -1) 
                { 
                    break;
                }


                char c = (char)readReturn;

                // Check for new line
                if (c == NewLine[0])
                {
                    PutBack(c);
                    if (GetNewLine())
                    {
                        break;
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }


            return sb.ToString();
        }



        /// <summary>
        /// Checks for new line character set. 
        /// If the complete set is found, the newline character(s) will be consumed.
        /// If not, the characters will be putback into the buffer for further processing.
        /// </summary>
        /// <returns>True if newline is found.</returns>
        private bool GetNewLine()
        {
            Queue<int> queue = new();

            for (int i = 0; i < NewLine.Length; i++)
            {
                int readReturn = Read();
                queue.Enqueue(readReturn);

                // Check for EOF
                if (readReturn == -1) {

                    while (queue.Count > 0)
                    {
                        PutBack(queue.Dequeue());
                    }

                    return false;
                }

                // Check for newline chars
                char c = (char)readReturn;
                if (c != NewLine[i])
                {
                    while (queue.Count > 0)
                    {
                        PutBack(queue.Dequeue());
                    }

                    return false;
                }
            }

            // New line chars found and consumed
            return true;
        }


    }
}
