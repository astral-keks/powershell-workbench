using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation.Provider;
using System.Text;
using System.Threading.Tasks;

namespace AstralKeks.Workbench.PowerShell.Provider
{
    public class CmdletProviderWriter : IContentWriter
    {
        private readonly CmdletProviderItem _item;
        private readonly Stream _content;
        private readonly StreamWriter _writer;

        public CmdletProviderWriter(CmdletProviderItem item)
        {
            _item = item;
            _content = _item.Write();
            _writer = new StreamWriter(_content);
        }

        ~CmdletProviderWriter()
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
            _writer.Close();
            _content.Close();
        }

        public IList Write(IList content)
        {
            foreach (var line in content)
                _writer.WriteLine(line);
            return content;
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }
    }
}
