namespace LeetSpeak
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using LeetSpeak.Interop;

    static class MetaDataHelpers
    {
        public static IEnumerable<string> EnumerateUserStrings(string location)
        {
            var mdImport = GetMetaDataImport(location);
                      
            foreach(var id in EnumerateUserStringIds(mdImport))
            {
                var buffer = new char[256];
                uint length;
                mdImport.GetUserString(id, buffer, (uint)(buffer.Length - 1), out length);

                yield return new string(buffer, 0, (int)length);
            }
        }

        static IEnumerable<uint> EnumerateUserStringIds(IMetaDataImport mdImport)
        {
            var hEnum = IntPtr.Zero;
            var ids = new uint[100];
            uint count;

            mdImport.EnumUserStrings(ref hEnum, ids, 100, out count);

            for (int i = 0; i < count; i++)
            {
                yield return ids[i];
            }

            mdImport.CloseEnum(hEnum);
        }

        static IMetaDataImport GetMetaDataImport(string location)
        {
            var dispenser = new CorMetaDataDispenser();
            var dispenserEx = (IMetaDataDispenserEx)dispenser;

            var metaDataImportGuid = typeof(IMetaDataImport).GUID;

            object rawScope;
            var hr = dispenserEx.OpenScope(location, 0, ref metaDataImportGuid, out rawScope);

            if (hr != 0)
            {
                Marshal.ThrowExceptionForHR((int)hr);
            }

            return (IMetaDataImport)rawScope;
        }
    }
}
