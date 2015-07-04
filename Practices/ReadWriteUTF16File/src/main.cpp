#include <iostream>
#include <locale>
#include <fstream>
#include <codecvt>

using namespace std;

const std::codecvt_mode kLEBom =
static_cast<std::codecvt_mode>(std::little_endian |
                               std::generate_header |
                               std::consume_header);
const std::codecvt_mode kBEBom =
static_cast<std::codecvt_mode>(std::generate_header |
                               std::consume_header);

typedef std::codecvt_utf16<wchar_t, 0x10ffff, kLEBom> WideConvUtf16LEBom;
typedef std::codecvt_utf16<wchar_t, 0x10ffff, kBEBom> WideConvUtf16BEBom;

std::wostream& WinEndLine(std::wostream& out)
{
    return out.put(L'\r').put(L'\n').flush();
}

int main(int argc, const char * argv[])
{
    const wchar_t* writeBuffer = L"ABCあいうえお亞伊羽絵尾abc";

    // UTF16 リトルエンディアンでファイル出力
    {
        std::wfstream file;
        WideConvUtf16LEBom cvt(1);

        std::locale loc(file.getloc(), &cvt);
        auto oldLocale = file.imbue(loc);
        file.open("utf16le.txt", std::ios::out | std::ios::binary);

        file << writeBuffer << WinEndLine;
        file.close();
        file.imbue(oldLocale);
    }

    // UTF16 リトルエンディアンでファイル入力
    {
        std::wifstream file;
        WideConvUtf16LEBom cvt(1);

        std::locale loc(file.getloc(), &cvt);
        auto oldLocale = file.imbue(loc);
        file.open("utf16le.txt", std::ios::in | std::ios::binary);

        std::wstring ws;

        for(wchar_t c; file.get(c);) {
            if (c == L'\r') { // CR 部分の除去
                continue;
            }

            ws.push_back(c);
        }

        file.close();
        file.imbue(oldLocale);
    }

    // UTF16 ビッグエンディアンでファイル出力
    {
        std::wfstream file;
        WideConvUtf16BEBom cvt(1);

        std::locale loc(file.getloc(), &cvt);
        auto oldLocale = file.imbue(loc);
        file.open("utf16be.txt", std::ios::out | std::ios::binary);

        file << writeBuffer << WinEndLine;
        file.close();
        file.imbue(oldLocale);
    }

    // UTF16 ビッグエンディアンでファイル入力
    {
        std::wifstream file;
        WideConvUtf16BEBom cvt(1);

        std::locale loc(file.getloc(), &cvt);
        auto oldLocale = file.imbue(loc);
        file.open("utf16be.txt", std::ios::in | std::ios::binary);

        std::wstring ws;

        for(wchar_t c; file.get(c);) {
            if (c == L'\r') { // CR 部分の除去
                continue;
            }

            ws.push_back(c);
        }

        file.close();
        file.imbue(oldLocale);
    }

    return 0;
}
