using System.Drawing.Text;
using System.Reflection;
using System.Runtime.InteropServices;

// 
public static class FontLoader
{
    public static PrivateFontCollection LoadEmbeddedFont(string resourceName)
    {

        var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        foreach (var name in names)
            Console.WriteLine(name);

        PrivateFontCollection fontCollection = new PrivateFontCollection();

        using (Stream fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
        {
            if (fontStream == null)
                throw new Exception("no fonts found: reversi.fonts.TempleOS.ttf");

            byte[] fontData = new byte[fontStream.Length];
            fontStream.Read(fontData, 0, (int)fontStream.Length);

            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);

            fontCollection.AddMemoryFont(fontPtr, fontData.Length);
            Marshal.FreeCoTaskMem(fontPtr);
        }

        return fontCollection;
    }
}
