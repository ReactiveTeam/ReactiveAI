using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveAI
{
    public static class ReaderWriterLockSlimExtensions
    {
        public static IDisposable Read(this ReaderWriterLockSlim @this)
        {
            return new ReadLockToken(@this);
        }

        public static IDisposable Write(this ReaderWriterLockSlim @this)
        {
            return new WriteLockToken(@this);
        }

        sealed class ReadLockToken : IDisposable
        {
            ReaderWriterLockSlim _sync;

            public void Dispose()
            {
                if (_sync != null)
                {
                    _sync.ExitReadLock();
                    _sync = null;
                }
            }

            public ReadLockToken(ReaderWriterLockSlim sync)
            {
                _sync = sync;
                sync.EnterReadLock();
            }
        }

        sealed class WriteLockToken : IDisposable
        {
            ReaderWriterLockSlim _sync;

            public void Dispose()
            {
                if (_sync != null)
                {
                    _sync.ExitWriteLock();
                    _sync = null;
                }
            }

            public WriteLockToken(ReaderWriterLockSlim sync)
            {
                _sync = sync;
                sync.EnterWriteLock();
            }
        }
    }
}
