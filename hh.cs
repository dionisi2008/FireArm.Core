using System.IO;
public class s
{

using System.IO;

public int Function485631()
{
    // 0x485631
    int v1 = unknown_ef394521(); // 0x48563b
    byte[] buffer = new byte[1];
    using (FileStream fs = new FileStream(v1.ToString(), FileMode.Open, FileAccess.ReadWrite))
    {
        fs.Seek(-16, SeekOrigin.End);
        fs.Read(buffer, 0, 1);
        fs.Seek(-16, SeekOrigin.End);
        fs.WriteByte((byte)(buffer[0] + v1));
    }
    int v3 = 0;
    int v4 = v3; // 0x48565f
    _40_LStrClr();
    _40_LStrAsg();
    _40_LStrAsg();
    int v5 = v4; // 0x4856b2
    if ((char)*(int*)(v1 + 772) != 0)
    {
        // 0x4856b8
        function_485000();
        TOpenDialog_GetFileName();
        ExtractFileExt();
        TOpenDialog_GetFileName();
        string v6 = "chp"; // bp-60, 0x48571e
        string v7 = Path.Combine(v6, v1.ToString()); // 0x48571e
        ExtractFilePath();
        _40_LStrAsg();
        v5 = v7;
        if (File.Exists(v7))
        {
            // 0x485758
            TControl_SetText();
            TTreeNodes_Clear();
            TActionBars_GetActionBar();
            THeaderSection_SetText();
            g136[60] = v1;
            g136[56] = 0x484668;
            g136[76] = v1;
            g136[72] = 0x484b04;
            function_4752d0();
            TPageControl_GetPage();
            TPageControl_SetActivePage();
            v5 = v7;
        }
    }
    // 0x485815
    *(int*)(v5 + 8) = 0x485854;
    _40_LStrArrayClr();
    _40_LStrArrayClr();
    _40_LStrClr();
    return _40_LStrClr();
}

}
