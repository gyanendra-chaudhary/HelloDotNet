using System.Text;
Console.Clear();
Console.WriteLine("Encoding");

string original = "The quick brown fox jumps over a lazy dog";

// ASCII
ASCIIEncoding ascii = new ASCIIEncoding();
Byte[] asciiencodingBytes = ascii.GetBytes(original);

Console.WriteLine("ASCII encoding");

foreach(Byte b in asciiencodingBytes)
    Console.Write($"{b},");


Console.WriteLine();

// Unicode

UnicodeEncoding unicode = new UnicodeEncoding();
Byte[] unicodeencodedBytes = unicode.GetBytes(original);
foreach(Byte b in unicodeencodedBytes)
    Console.Write($"{b},");


// Utf32

UTF32Encoding utf32 = new UTF32Encoding();
Byte[] utf32encodedBytes = utf32.GetBytes(original);
foreach(Byte b in utf32encodedBytes)
    Console.Write($"{b},");


// Utf8
UTF8Encoding utf8 = new UTF8Encoding();
Byte[] utf8encodingBytes = utf8.GetBytes(original);
Console.WriteLine("UTF8 encoding: ");
foreach(Byte b in utf8encodingBytes)
    Console.Write("${b},");

Console.WriteLine();

// Decoding (Any)

Char[] chars;
Decoder utf8decoder = Encoding.UTF8.GetDecoder();


// First we get how many chars compose the string array
int chatCount = utf8decoder.GetCharCount(utf8encodingBytes, 0, utf32encodedBytes.Length);
chars = new char[chatCount];

// Now we decode our byte array
int charsDecodedCount = utf8decoder.GetChars(utf8encodingBytes, 8, utf8encodingBytes.Length, chars, 0);

