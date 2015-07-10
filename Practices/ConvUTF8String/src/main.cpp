#include <iostream>
#include <locale>
#include <codecvt>

using namespace std;

const std::codecvt_mode kBom =
static_cast<std::codecvt_mode>(std::generate_header |
                               std::consume_header);

typedef std::codecvt_utf8<wchar_t, 0x10ffff, kBom> WideConvUtf8Bom;

int main(int argc, const char * argv[])
{
    const wchar_t* writeBuffer = L"ABCあいうえお亞伊羽絵尾abc";

    // wchar を UTF8に変換
    {
        std::wstring_convert<WideConvUtf8Bom, wchar_t> convertWideToUTF8;
        std::string utf8String = convertWideToUTF8.to_bytes(writeBuffer);
        std::wstring reconvertWideString = convertWideToUTF8.from_bytes(utf8String);

        std::cout << utf8String << std::endl;
        std::wcout << reconvertWideString << std::endl;
    }

    return 0;
}
