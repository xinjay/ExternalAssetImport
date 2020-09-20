//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using UnityEngine;
//using UnityEditor;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using UnityEngine.Profiling;
//using Debug = UnityEngine.Debug;

//public class Test : MonoBehaviour
//{
//    private const string TexturePath = "Assets/Texture";
//    [MenuItem("Test/GeneratTexture")]
//    static void GeneratTexture()
//    {
//        var path = EditorUtility.SaveFolderPanel("存储路径", Application.dataPath, "");
//        if (!string.IsNullOrEmpty(path))
//        {
//            //生成500张图片
//            var count = 500;
//            for (var index = 0; index < count; index++)
//            {
//                EditorUtility.DisplayProgressBar("生成图片", $"[{index + 1}/{count}]", (index + 1f) / count);
//              ////  var random = AssetImportUtils.GetRandomNameByTimeStamp();
//              //  var filename = $"{path}/{random}.png";
//              //  var texture = new Texture2D(128, 128);
//              //  var bytes = texture.EncodeToPNG();
//              //  File.WriteAllBytes(filename, bytes);
//            }
//            EditorUtility.ClearProgressBar();
//        }
//    }

//    /// <summary>
//    ///批量导入贴图
//    /// </summary>
//    [MenuItem("Test/ImportTexture")]
//    static void ImportTexture()
//    {
//        var folder = EditorUtility.OpenFolderPanel("打开图片路径", Application.dataPath, "");
//        if (!string.IsNullOrEmpty(folder))
//        {
//            var stopWatch = new System.Diagnostics.Stopwatch();
//            stopWatch.Reset();
//            stopWatch.Start();
//            var files = Directory.GetFiles(folder)
//                .Where(file => file.EndsWith("png") || file.EndsWith("jpg") || file.EndsWith("exr"));
//            foreach (var file in files)
//            {
//                var fileName = Path.GetFileName(file);
//                var targetPath = $"{TexturePath}/{fileName}";
//                File.Copy(file, targetPath, true);
//                AssetDatabase.ImportAsset(targetPath);
//            }
//            stopWatch.Stop();
//            var time = stopWatch.ElapsedMilliseconds;
//            Debug.Log($"耗时:{time}ms");
//        }
//    }

//    /// <summary>
//    ///批量导入贴图（合批）
//    /// </summary>
//    [MenuItem("Test/ImportTextureInBatchMode")]
//    static void ImportTextureInBatchMode()
//    {
//        var folder = EditorUtility.OpenFolderPanel("打开图片路径", Application.dataPath, "");
//        if (!string.IsNullOrEmpty(folder))
//        {
//            var stopWatch = new System.Diagnostics.Stopwatch();
//            stopWatch.Reset();
//            stopWatch.Start();
//            try
//            {
//                AssetImportUtils.StartAssetEditing();
//                var files = Directory.GetFiles(folder)
//                    .Where(file => file.EndsWith("png") || file.EndsWith("jpg") || file.EndsWith("exr"));
//                foreach (var file in files)
//                {
//                    var fileName = Path.GetFileName(file);
//                    var targetPath = $"{TexturePath}/{fileName}";
//                    File.Copy(file, targetPath, true);
//                    AssetDatabase.ImportAsset(targetPath);
//                }
//            }
//            finally
//            {
//                AssetImportUtils.StopAssetEditing();
//            }
//            stopWatch.Stop();
//            var time = stopWatch.ElapsedMilliseconds;
//            Debug.Log($"耗时:{time}ms");
//        }
//    }

//    /// <summary>
//    ///批量设置贴图
//    /// </summary>
//    [MenuItem("Test/TextureSetting")]
//    static void TextureSetting()
//    {
//        var stopWatch = new System.Diagnostics.Stopwatch();
//        stopWatch.Reset();
//        stopWatch.Start();
//        var files = Directory.GetFiles(TexturePath)
//            .Where(file => file.EndsWith("png") || file.EndsWith("jpg") || file.EndsWith("exr"));
//        foreach (var file in files)
//        {
//            var importer = AssetImporter.GetAtPath(file) as TextureImporter;
//            importer.isReadable = true;
//            importer.SaveAndReimport();
//            AssetDatabase.ImportAsset(file);
//        }
//        stopWatch.Stop();
//        var time = stopWatch.ElapsedMilliseconds;
//        Debug.Log($"耗时:{time}ms");
//    }

//    /// <summary>
//    ///批量设置贴图（合批）
//    /// </summary>
//    [MenuItem("Test/TextureSettingInBatchMode")]
//    static void TextureSettingInBatchMode()
//    {
//        var stopWatch = new System.Diagnostics.Stopwatch();
//        stopWatch.Reset();
//        stopWatch.Start();
//        try
//        {
//            var files = Directory.GetFiles(TexturePath)
//                .Where(file => file.EndsWith("png") || file.EndsWith("jpg") || file.EndsWith("exr"));
//            foreach (var file in files)
//            {
//                var importer = AssetImporter.GetAtPath(file) as TextureImporter;
//                importer.isReadable = true;
//                importer.SaveAndReimport();
//                AssetDatabase.ImportAsset(file);
//            }
//        }
//        finally
//        {
//            AssetImportUtils.StopAssetEditing();
//        }
//        stopWatch.Stop();
//        var time = stopWatch.ElapsedMilliseconds;
//        Debug.Log($"耗时:{time}ms");
//    }


//    [MenuItem("Test/MoreStartAssetEditing(界面无响应)")]
//    static void MoreStartAssetEditing()
//    {
//        try
//        {
//            AssetDatabase.StartAssetEditing();
//            AssetDatabase.StartAssetEditing();
//        }
//        finally
//        {
//            AssetDatabase.StopAssetEditing();
//        }
//    }
//    [MenuItem("Test/MoreStopAssetEditing(报错)")]
//    static void MoreStoptAssetEditing()
//    {
//        try
//        {
//            AssetDatabase.StartAssetEditing();
//            AssetDatabase.StopAssetEditing();
//        }
//        finally
//        {
//            AssetDatabase.StopAssetEditing();
//        }
//    }

//    [MenuItem("Test/InvalidImportInBatchMode")]
//    static void InvalidImportInBatchMode()
//    {
//        var texture = new Texture2D(128, 128);
//        var bytes = texture.EncodeToPNG();
//        var path = "Assets/Test.png";
//        if (File.Exists(path))//避免遗留数据影响
//            AssetDatabase.DeleteAsset(path);
//        try
//        {
//            AssetImportUtils.StartAssetEditing();
//            File.WriteAllBytes(path, bytes);
//            AssetDatabase.ImportAsset(path); //此时Import无效
//            var importer = (TextureImporter)AssetImporter.GetAtPath(path); //importer=null;
//            importer.isReadable = true; //抛出异常
//        }
//        catch (Exception e)
//        {
//            Debug.LogError(e.Message);
//        }
//        finally
//        {
//            AssetImportUtils.StopAssetEditing();
//        }
//    }

//    [MenuItem("Value/ValueTypeArray")]
//    static void ValueTypeTest()
//    {
//        // var arr = new ValueType[] { new ValueType() { a = 1 }, new ValueType() { a = 2 }, new ValueType() { a = 3 }, };
//        var src = new int[] { 7, 8, 9, 10, 11, 12 };
//        var arr = new Vector3Int[] { new Vector3Int(1, 2, 3), new Vector3Int(4, 5, 6) };
//        unsafe
//        {
//            //fixed (ValueType* p = &arr[0])
//            //fixed (Vector3Int* pp = &arr[0])
//            //// fixed (int* p = &arr[0].x)
//            //// fixed (int* p = &arr[0].a)
//            //{
//            //    int* p = (int*)pp;
//            //    for (var index = 0; index < arr.Length; index++)
//            //    {
//            //        var value = *(p - index);
//            //        var addr = (int)(p - index);
//            //        Debug.Log($"*(p + {index})={value},addr=0x{addr.ToString("X")}");
//            //    }
//            //    // Memory
//            //}
//            foreach (var item in arr)
//            {
//                Debug.Log(item);
//            }
//            fixed (int* p1 = &src[0])
//            {
//                fixed (Vector3Int* p2 = &arr[0])
//                {
//                    Buffer.MemoryCopy(p1, p2, 24, 24);
//                }
//            }

//            foreach (var item in arr)
//            {
//                Debug.Log(item);
//            }
//        }
//    }


//    [MenuItem("Value/StackOverflow")]
//    static void OverFlow()
//    {
//        try
//        {
//            var inde = 10240;
//            while (inde < 1024 * 1024)
//            {
//                upbound = inde++;
//                time = 0;
//                TestOverFlow();
//            }
//        }
//        catch (Exception e)
//        {
//            Console.WriteLine(e);
//            Debug.Log(time);
//            // throw;
//        }
//        // TestOverFlow();
//        Debug.Log(time);
//    }

//    private static int upbound = 1024 * 1024;
//    private static int time = 0;
//    static void TestOverFlow()
//    {
//        if (time < upbound)
//        {
//            time++;
//            TestOverFlow();
//        }
//    }

//    [MenuItem("Value/Hack")]
//    static void Hack()
//    {
//        var length = 90000;
//        var arr = new Vector3[length];
//        var arr2 = new Vector3[length];
//        var src = new float[length * 3];
//        var bytesLength = length * 12;
//        for (var index = 0; index < length * 3; index++)
//        {
//            src[index] = index;
//        }
//        var stopWatch = new System.Diagnostics.Stopwatch();

//        unsafe
//        {
//            fixed (float* p1 = &src[0])
//            {
//                float* f = stackalloc float[length * 3];
//                for (var index = 0; index < length * 3; index++)
//                {
//                    *(f + index) = index;
//                }
//                fixed (Vector3* p2 = &arr[0])
//                {
//                    stopWatch.Reset();
//                    stopWatch.Start();
//                    float* pp = (float*)p2;
//                    Buffer.MemoryCopy(f, p2, bytesLength, bytesLength);
//                    //for (var index = 0; index < length * 3; index++)
//                    //{
//                    //    *(pp + index) = *(p1 + index);
//                    //}
//                    // Array.Copy();
//                    // Buffer.co
//                    //var pp1 = new IntPtr(p1);
//                    //var pp2 = new IntPtr(p2);
//                    //FastMemCopy.FastMemoryCopy(pp1, pp2, bytesLength);
//                    stopWatch.Stop();
//                    Debug.Log($"内存拷贝耗时：{stopWatch.ElapsedMilliseconds}ms");
//                }
//            }
//        }


//        stopWatch.Reset();
//        stopWatch.Start();
//        for (var index = 0; index < length; index++)
//        {
//            // arr2[index] = new Vector3(src[index * 3], src[index * 3 + 1], src[index * 3 + 2]);
//            arr2[index].x = src[index * 3];
//            arr2[index].y = src[index * 3 + 1];
//            arr2[index].z = src[index * 3 + 2];
//        }
//        stopWatch.Stop();
//        Debug.Log($"直接复制耗时：{stopWatch.ElapsedMilliseconds}ms");
//        for (var index = 0; index < length; index++)
//        {
//            if (arr[index] != arr2[index])
//            {
//                Debug.LogError("错误");
//                break;
//            }
//        }
//    }
//}


//public class ValueType
//{
//    public int a;

//}

//internal static class FastMemCopy
//{
//    [Flags]
//    private enum AllocationTypes : uint
//    {
//        Commit = 0x1000,
//        Reserve = 0x2000,
//        Reset = 0x80000,
//        LargePages = 0x20000000,
//        Physical = 0x400000,
//        TopDown = 0x100000,
//        WriteWatch = 0x200000
//    }

//    [Flags]
//    private enum MemoryProtections : uint
//    {
//        Execute = 0x10,
//        ExecuteRead = 0x20,
//        ExecuteReadWrite = 0x40,
//        ExecuteWriteCopy = 0x80,
//        NoAccess = 0x01,
//        ReadOnly = 0x02,
//        ReadWrite = 0x04,
//        WriteCopy = 0x08,
//        GuartModifierflag = 0x100,
//        NoCacheModifierflag = 0x200,
//        WriteCombineModifierflag = 0x400
//    }

//    [Flags]
//    private enum FreeTypes : uint
//    {
//        Decommit = 0x4000,
//        Release = 0x8000
//    }

//    [UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)]
//    private unsafe delegate void FastMemCopyDelegate();

//    private static class NativeMethods
//    {
//        [DllImport("kernel32.dll", SetLastError = true)]
//        internal static extern IntPtr VirtualAlloc(
//            IntPtr lpAddress,
//            UIntPtr dwSize,
//            AllocationTypes flAllocationType,
//            MemoryProtections flProtect);

//        [DllImport("kernel32")]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        internal static extern bool VirtualFree(
//            IntPtr lpAddress,
//            uint dwSize,
//            FreeTypes flFreeType);
//    }
//    public static unsafe void FastMemoryCopy(IntPtr src, IntPtr dst, int nBytes)
//    {
//        if (IntPtr.Size == 4)
//        {
//            //we are in 32 bit mode

//            //allocate memory for our asm method
//            IntPtr p = NativeMethods.VirtualAlloc(
//                IntPtr.Zero,
//                new UIntPtr((uint)x86_FastMemCopy_New.Length),
//                AllocationTypes.Commit | AllocationTypes.Reserve,
//                MemoryProtections.ExecuteReadWrite);

//            try
//            {
//                //copy our method bytes to allocated memory
//                Marshal.Copy(x86_FastMemCopy_New, 0, p, x86_FastMemCopy_New.Length);

//                //make a delegate to our method
//                FastMemCopyDelegate _fastmemcopy =
//  (FastMemCopyDelegate)Marshal.GetDelegateForFunctionPointer(p,
//    typeof(FastMemCopyDelegate));

//                //offset to the end of our method block
//                p += x86_FastMemCopy_New.Length;

//                //store length param
//                p -= 8;
//                Marshal.Copy(BitConverter.GetBytes((long)nBytes), 0, p, 4);

//                //store destination address param
//                p -= 8;
//                Marshal.Copy(BitConverter.GetBytes((long)dst), 0, p, 4);

//                //store source address param
//                p -= 8;
//                Marshal.Copy(BitConverter.GetBytes((long)src), 0, p, 4);

//                //Start stopwatch
//                Stopwatch sw = new Stopwatch();
//                sw.Start();

//                //copy-past all data 10 times
//                for (int i = 0; i < 10; i++)
//                    _fastmemcopy();

//                //stop stopwatch
//                sw.Stop();

//                //get message with measured time
//                Debug.LogError(sw.ElapsedTicks.ToString());
//                //System.Windows.Forms.MessageBox.Show(sw.ElapsedTicks.ToString());
//            }
//            catch (Exception ex)
//            {
//                //if any exception
//                Debug.LogError(ex.Message);
//                //System.Windows.Forms.MessageBox.Show(ex.Message);
//            }
//            finally
//            {
//                //free allocated memory
//                NativeMethods.VirtualFree(p, (uint)(x86_FastMemCopy_New.Length),
//  FreeTypes.Release);
//                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
//            }
//        }
//        else if (IntPtr.Size == 8)
//        {
//            throw new ApplicationException("x64 is not supported yet!");
//        }
//    }
//    private static byte[] x86_FastMemCopy_New = new byte[]
//{
//  0x90, //nop do nothing
//  0x60, //pushad store flag register on stack
//  0x95, //xchg ebp, eax eax contains memory address of our method
//  0x8B, 0xB5, 0x5A, 0x01, 0x00, 0x00, //mov esi,[ebp][00000015A] get source buffer address
//  0x89, 0xF0, //mov eax,esi
//  0x83, 0xE0, 0x0F, //and eax,00F will check if it is 16 byte aligned
//  0x8B, 0xBD, 0x62, 0x01, 0x00, 0x00, //mov edi,[ebp][000000162] get destination address
//  0x89, 0xFB, //mov ebx,edi
//  0x83, 0xE3, 0x0F, //and ebx,00F will check if it is 16 byte aligned
//  0x8B, 0x8D, 0x6A, 0x01, 0x00, 0x00, //mov ecx,[ebp][00000016A] get number of bytes to copy
//  0xC1, 0xE9, 0x07, //shr ecx,7 divide length by 128
//  0x85, 0xC9, //test ecx,ecx check if zero
//  0x0F, 0x84, 0x1C, 0x01, 0x00, 0x00, //jz 000000146 &darr; copy the rest
//  0x0F, 0x18, 0x06, //prefetchnta [esi] pre-fetch non-temporal source data for reading
//  0x85, 0xC0, //test eax,eax check if source address is 16 byte aligned
//  0x0F, 0x84, 0x8B, 0x00, 0x00, 0x00, //jz 0000000C0 &darr; go to copy if aligned
//  0x0F, 0x18, 0x86, 0x80, 0x02, 0x00, 0x00, //prefetchnta [esi][000000280] pre-fetch more source data
//  0x0F, 0x10, 0x06, //movups xmm0,[esi] copy 16 bytes of source data
//  0x0F, 0x10, 0x4E, 0x10, //movups xmm1,[esi][010] copy more 16 bytes
//  0x0F, 0x10, 0x56, 0x20, //movups xmm2,[esi][020] copy more
//  0x0F, 0x18, 0x86, 0xC0, 0x02, 0x00, 0x00, //prefetchnta [esi][0000002C0] pre-fetch more
//  0x0F, 0x10, 0x5E, 0x30, //movups xmm3,[esi][030]
//  0x0F, 0x10, 0x66, 0x40, //movups xmm4,[esi][040]
//  0x0F, 0x10, 0x6E, 0x50, //movups xmm5,[esi][050]
//  0x0F, 0x10, 0x76, 0x60, //movups xmm6,[esi][060]
//  0x0F, 0x10, 0x7E, 0x70, //movups xmm7,[esi][070] we&apos;ve copied 128 bytes of source data
//  0x85, 0xDB, //test ebx,ebx check if destination address is 16 byte aligned
//  0x74, 0x21, //jz 000000087 &darr; go to past if aligned
//  0x0F, 0x11, 0x07, //movups [edi],xmm0 past first 16 bytes to non-aligned destination address
//  0x0F, 0x11, 0x4F, 0x10, //movups [edi][010],xmm1 past more
//  0x0F, 0x11, 0x57, 0x20, //movups [edi][020],xmm2
//  0x0F, 0x11, 0x5F, 0x30, //movups [edi][030],xmm3
//  0x0F, 0x11, 0x67, 0x40, //movups [edi][040],xmm4
//  0x0F, 0x11, 0x6F, 0x50, //movups [edi][050],xmm5
//  0x0F, 0x11, 0x77, 0x60, //movups [edi][060],xmm6
//  0x0F, 0x11, 0x7F, 0x70, //movups [edi][070],xmm7 we&apos;ve pasted 128 bytes of source data
//  0xEB, 0x1F, //jmps 0000000A6 &darr; continue
//  0x0F, 0x2B, 0x07, //movntps [edi],xmm0 past first 16 bytes to aligned destination address
//  0x0F, 0x2B, 0x4F, 0x10, //movntps [edi][010],xmm1 past more
//  0x0F, 0x2B, 0x57, 0x20, //movntps [edi][020],xmm2
//  0x0F, 0x2B, 0x5F, 0x30, //movntps [edi][030],xmm3
//  0x0F, 0x2B, 0x67, 0x40, //movntps [edi][040],xmm4
//  0x0F, 0x2B, 0x6F, 0x50, //movntps [edi][050],xmm5
//  0x0F, 0x2B, 0x77, 0x60, //movntps [edi][060],xmm6
//  0x0F, 0x2B, 0x7F, 0x70, //movntps [edi][070],xmm7 we&apos;ve pasted 128 bytes of source data
//  0x81, 0xC6, 0x80, 0x00, 0x00, 0x00, //add esi,000000080 increment source address by 128
//  0x81, 0xC7, 0x80, 0x00, 0x00, 0x00, //add edi,000000080 increment destination address by 128
//  0x83, 0xE9, 0x01, //sub ecx,1 decrement counter
//  0x0F, 0x85, 0x7A, 0xFF, 0xFF, 0xFF, //jnz 000000035 &uarr; continue if not zero
//  0xE9, 0x86, 0x00, 0x00, 0x00, //jmp 000000146 &darr; go to copy the rest of data

//  0x0F, 0x18, 0x86, 0x80, 0x02, 0x00, 0x00, //prefetchnta [esi][000000280] pre-fetch source data
//  0x0F, 0x28, 0x06, //movaps xmm0,[esi] copy 128 bytes from aligned source address
//  0x0F, 0x28, 0x4E, 0x10, //movaps xmm1,[esi][010] copy more
//  0x0F, 0x28, 0x56, 0x20, //movaps xmm2,[esi][020]
//  0x0F, 0x18, 0x86, 0xC0, 0x02, 0x00, 0x00, //prefetchnta [esi][0000002C0] pre-fetch more data
//  0x0F, 0x28, 0x5E, 0x30, //movaps xmm3,[esi][030]
//  0x0F, 0x28, 0x66, 0x40, //movaps xmm4,[esi][040]
//  0x0F, 0x28, 0x6E, 0x50, //movaps xmm5,[esi][050]
//  0x0F, 0x28, 0x76, 0x60, //movaps xmm6,[esi][060]
//  0x0F, 0x28, 0x7E, 0x70, //movaps xmm7,[esi][070] we&apos;ve copied 128 bytes of source data
//  0x85, 0xDB, //test ebx,ebx check if destination address is 16 byte aligned
//  0x74, 0x21, //jz 000000112 &darr; go to past if aligned
//  0x0F, 0x11, 0x07, //movups [edi],xmm0 past 16 bytes to non-aligned destination address
//  0x0F, 0x11, 0x4F, 0x10, //movups [edi][010],xmm1 past more
//  0x0F, 0x11, 0x57, 0x20, //movups [edi][020],xmm2
//  0x0F, 0x11, 0x5F, 0x30, //movups [edi][030],xmm3
//  0x0F, 0x11, 0x67, 0x40, //movups [edi][040],xmm4
//  0x0F, 0x11, 0x6F, 0x50, //movups [edi][050],xmm5
//  0x0F, 0x11, 0x77, 0x60, //movups [edi][060],xmm6
//  0x0F, 0x11, 0x7F, 0x70, //movups [edi][070],xmm7 we&apos;ve pasted 128 bytes of data
//  0xEB, 0x1F, //jmps 000000131 &darr; continue copy-past
//  0x0F, 0x2B, 0x07, //movntps [edi],xmm0 past 16 bytes to aligned destination address
//  0x0F, 0x2B, 0x4F, 0x10, //movntps [edi][010],xmm1 past more
//  0x0F, 0x2B, 0x57, 0x20, //movntps [edi][020],xmm2
//  0x0F, 0x2B, 0x5F, 0x30, //movntps [edi][030],xmm3
//  0x0F, 0x2B, 0x67, 0x40, //movntps [edi][040],xmm4
//  0x0F, 0x2B, 0x6F, 0x50, //movntps [edi][050],xmm5
//  0x0F, 0x2B, 0x77, 0x60, //movntps [edi][060],xmm6
//  0x0F, 0x2B, 0x7F, 0x70, //movntps [edi][070],xmm7 we&apos;ve pasted 128 bytes of data
//  0x81, 0xC6, 0x80, 0x00, 0x00, 0x00, //add esi,000000080 increment source address by 128
//  0x81, 0xC7, 0x80, 0x00, 0x00, 0x00, //add edi,000000080 increment destination address by 128
//  0x83, 0xE9, 0x01, //sub ecx,1 decrement counter
//  0x0F, 0x85, 0x7A, 0xFF, 0xFF, 0xFF, //jnz 0000000C0 &uarr; continue copy-past if non-zero
//  0x8B, 0x8D, 0x6A, 0x01, 0x00, 0x00, //mov ecx,[ebp][00000016A] get number of bytes to copy
//  0x83, 0xE1, 0x7F, //and ecx,07F get rest number of bytes
//  0x85, 0xC9, //test ecx,ecx check if there are bytes
//  0x74, 0x02, //jz 000000155 &darr; exit if there are no more bytes
//  0xF3, 0xA4, //rep movsb copy rest of bytes
//  0x0F, 0xAE, 0xF8, //sfence performs a serializing operation on all store-to-memory instructions
//  0x61, //popad restore flag register
//  0xC3, //retn return from our method to C#
  
//  0x00, 0x00, 0x00, 0x00, //source buffer address
//  0x00, 0x00, 0x00, 0x00,

//  0x00, 0x00, 0x00, 0x00, //destination buffer address
//  0x00, 0x00, 0x00, 0x00,

//  0x00, 0x00, 0x00, 0x00, //number of bytes to copy-past
//  0x00, 0x00, 0x00, 0x00
//};
//}