#include <iostream>
#include <locale>
#include <fstream>
#include <codecvt>
#include <sstream>

using namespace std;

const std::codecvt_mode kBom =
static_cast<std::codecvt_mode>(std::generate_header |
                               std::consume_header);

typedef std::codecvt_utf8<wchar_t, 0x10ffff, kBom> WideConvUtf8Bom;

std::wostream& WinEndLine(std::wostream& out)
{
    return out.put(L'\r').put(L'\n').flush();
}

int main(int argc, const char * argv[])
{
    const wchar_t* writeBuffer = L"ABCあいうえお亞伊羽絵尾abc";

    // UTF8でファイル出力
    {
        std::wfstream file;
        WideConvUtf8Bom cvt(1);

        std::locale loc(file.getloc(), &cvt);
        auto oldLocale = file.imbue(loc);
        file.open("utf8bom.txt", std::ios::out | std::ios::binary);

        file << writeBuffer << WinEndLine;
        file.close();
        file.imbue(oldLocale);
    }

    // UTF8でファイル入力
    {
        std::wifstream file;
        WideConvUtf8Bom cvt(1);

        std::locale loc(file.getloc(), &cvt);
        auto oldLocale = file.imbue(loc);
        file.open("utf8bom.txt", std::ios::in | std::ios::binary);

        std::wstringstream wss;

        wss << file.rdbuf();
        std::wstring ws = wss.str();

        std::wstring removeStr = L"\r";
        auto pos(ws.find(removeStr));

        while (pos != std::wstring::npos) {
            ws.replace(pos, removeStr.length(), L"");// CR 部分の除去
            pos = ws.find(removeStr, pos);
        }

        file.close();
        file.imbue(oldLocale);
    }
    
    return 0;
}