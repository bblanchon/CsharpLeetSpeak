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
            var dispenser = new IMetaDataDispenser();
           
            var metaDataImportGuid = typeof(IMetaDataImport).GUID;

            object rawScope;
            var hr = dispenser.OpenScope(location, 0, ref metaDataImportGuid, out rawScope);

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
            uint length;
            metaDataImport.GetUserString(id, null, 0, out length);

            var buffer = new char[length];
            metaDataImport.GetUserString(id, buffer, length, out length);

            return new string(buffer);
        }

        IEnumerable<uint> EnumerateUserStringIds()
        {
            var hEnum = IntPtr.Zero;
            var buffer = new uint[16];
            uint count;

            metaDataImport.EnumUserStrings(ref hEnum, buffer, (uint)buffer.Length, out count);
            
            try
            {
                while (count > 0)
                {
                    for (var i = 0; i < count; i++)
                        yield return buffer[i];

                    metaDataImport.EnumUserStrings(ref hEnum, buffer, (uint)buffer.Length, out count);
                }
            }
            finally
            {
                metaDataImport.CloseEnum(hEnum);
            }
        }
    }
}
