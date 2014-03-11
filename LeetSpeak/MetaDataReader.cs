namespace LeetSpeak
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using LeetSpeak.Interop;

    class MetaDataReader
    {
        readonly IMetaDataImport metaDataImport;

        public MetaDataReader(string location)
        {
            var dispenser = new CorMetaDataDispenser();
            var dispenserEx = (IMetaDataDispenser)dispenser;

            var metaDataImportGuid = typeof(IMetaDataImport).GUID;

            object rawScope;
            var hr = dispenserEx.OpenScope(location, 0, ref metaDataImportGuid, out rawScope);

            if (hr != 0)
            {
                Marshal.ThrowExceptionForHR((int)hr);
            }

            metaDataImport = (IMetaDataImport)rawScope;
        }

        public IEnumerable<string> EnumerateUserStrings()
        {
            return EnumerateUserStringIds().Select(GetUserString);
        }

        string GetUserString(uint id)
        {
            var buffer = new char[256];
            uint length;
            metaDataImport.GetUserString(id, buffer, (uint)(buffer.Length - 1), out length);

            return new string(buffer, 0, (int)length);
        }

        IEnumerable<uint> EnumerateUserStringIds()
        {
            var hEnum = IntPtr.Zero;
            var ids = new uint[100];
            uint count;

            metaDataImport.EnumUserStrings(ref hEnum, ids, 100, out count);

            try
            {
                for (int i = 0; i < count; i++)
                {
                    yield return ids[i];
                }
            }
            finally
            {
                metaDataImport.CloseEnum(hEnum);
            }
        }
    }
}
