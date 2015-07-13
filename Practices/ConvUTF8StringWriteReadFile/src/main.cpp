#include <iostream>
#include <locale>
#include <codecvt>
#include <fstream>
#include <sstream>

using namespace std;

const std::codecvt_mode kBom =
static_cast<std::codecvt_mode>(std::generate_header |
                               std::consume_header);

typedef std::codecvt_utf8<wchar_t, 0x10ffff, kBom> WideConvUtf8Bom;

std::ostream& WinEndLine(std::ostream& out)
{
    return out.put('\r').put('\n').flush();
}

int main(int argc, const char * argv[])
{
    const wchar_t* writeBuffer = L"ABCあいうえお亞伊羽絵尾abc";

    // wchar を UTF8に変換後にファイル出力
    {
        std::wstring_convert<WideConvUtf8Bom, wchar_t> convertWideToUTF8;
        std::string utf8String = convertWideToUTF8.to_bytes(writeBuffer);

        std::fstream file;
        WideConvUtf8Bom cvt(1);

        std::locale loc(file.getloc(), &cvt);
        auto oldLocale = file.imbue(loc);
        file.open("utf8bom.txt", std::ios::out | std::ios::binary);

        file << utf8String << WinEndLine;
        file.close();
        file.imbue(oldLocale);
    }

    // UTF8でファイル入力後、UTF8文字列をワイド文字列に変換
    {
        std::ifstream file;
        WideConvUtf8Bom cvt(1);

        std::locale loc(file.getloc(), &cvt);
        auto oldLocale = file.imbue(loc);
        file.open("utf8bom.txt", std::ios::in | std::ios::binary);

        std::stringstream ss;

        ss << file.rdbuf();
        std::string utf8String = ss.str();

        std::string removeStr = "\r";
        auto pos(utf8String.find(removeStr));

        while (pos != std::wstring::npos) {
            utf8String.replace(pos, removeStr.length(), "");// CR 部分の除去
            pos = utf8String.find(removeStr, pos);
        }

        file.close();
        file.imbue(oldLocale);

        std::wstring_convert<WideConvUtf8Bom, wchar_t> convertWideToUTF8;
        std::wstring wideString = convertWideToUTF8.from_bytes(utf8String);

        std::wcout << wideString << std::endl;
    }

    return 0;
}
