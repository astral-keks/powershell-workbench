using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation.Provider;

namespace AstralKeks.Workbench.PowerShell.Provider
{
    public class CmdletProviderReader : IContentReader
    {
        private readonly CmdletProviderItem _item;
        private readonly Stream _content;
        private readonly StreamReader _reader;

        public CmdletProviderReader(CmdletProviderItem item)
        {
            _item = item;
            _content = _item.Read();
            _reader = new StreamReader(_content);
        }

        ~CmdletProviderReader()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Dispose();
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            _reader.Close();
            _content.Close();
        }

        public IList Read(long readCount)
        {
            var lines = new List<string>();
            while (readCount-- > 0 && !_reader.EndOfStream)
            {
                var line = _reader.ReadLine();
                lines.Add(line);
            }

            return lines;
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }
    }
}
