using System;
using System.Collections.Generic;
using System.Linq;

namespace LiberPrimusAnalysisTool.Utility.Character
{
    /// <summary>
    /// CharacterRepo
    /// </summary>
    /// <seealso cref="LiberPrimusAnalysisTool.Utility.Character.ICharacterRepo" />
    public class CharacterRepo : ICharacterRepo
    {
        /// <summary>
        /// The ASCII ANSI items
        /// </summary>
        private readonly HashSet<Tuple<string, string, int, string>> _asciiAnsiItems;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterRepo"/> class.
        /// </summary>
        public CharacterRepo()
        {
            #region ANSI

            _asciiAnsiItems = new HashSet<Tuple<string, string, int, string>>();
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<NUL>", 0, "0000000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<SOH>", 1, "0000001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<STX>", 2, "0000010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<ETX>", 3, "0000011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<EOT>", 4, "0000100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<ENQ>", 5, "0000101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<ACK>", 6, "0000110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<BEL>", 7, "0000111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<BS>", 8, "0001000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<HT>", 9, "0001001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "\n", 10, "0001010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<VT>", 11, "0001011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<FF>", 12, "0001100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "\r", 13, "0001101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<SO>", 14, "0001110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<SI>", 15, "0001111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<DLE>", 16, "0010000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<DC1>", 17, "0010001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<DC2>", 18, "0010010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<DC3>", 19, "0010011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<DC4>", 20, "0010100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<NAK>", 21, "0010101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<SYN>", 22, "0010110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<ETB>", 23, "0010111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<CAN>", 24, "0011000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<EM>", 25, "0011001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<SUB>", 26, "0011010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<ESC>", 27, "0011011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<FS>", 28, "0011100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<GS>", 29, "0011101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<RS>", 30, "0011110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<US>", 31, "0011111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", " ", 32, "0100000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "!", 33, "0100001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "\"", 34, "0100010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "#", 35, "0100011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "$", 36, "0100100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "%", 37, "0100101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "&", 38, "0100110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "", 39, "0100111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "(", 40, "0101000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", ")", 41, "0101001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "*", 42, "0101010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "+", 43, "0101011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", ",", 44, "0101100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "-", 45, "0101101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", ".", 46, "0101110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "/", 47, "0101111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "0", 48, "0110000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "1", 49, "0110001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "2", 50, "0110010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "3", 51, "0110011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "4", 52, "0110100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "5", 53, "0110101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "6", 54, "0110110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "7", 55, "0110111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "8", 56, "0111000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "9", 57, "0111001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", ":", 58, "0111010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", ";", 59, "0111011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<", 60, "0111100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "=", 61, "0111101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", ">", 62, "0111110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "?", 63, "0111111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "@", 64, "1000000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "A", 65, "1000001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "B", 66, "1000010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "C", 67, "1000011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "D", 68, "1000100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "E", 69, "1000101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "F", 70, "1000110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "G", 71, "1000111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "H", 72, "1001000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "I", 73, "1001001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "J", 74, "1001010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "K", 75, "1001011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "L", 76, "1001100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "M", 77, "1001101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "N", 78, "1001110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "O", 79, "1001111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "P", 80, "1010000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "Q", 81, "1010001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "R", 82, "1010010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "S", 83, "1010011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "T", 84, "1010100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "U", 85, "1010101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "V", 86, "1010110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "W", 87, "1010111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "X", 88, "1011000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "Y", 89, "1011001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "Z", 90, "1011010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "[", 91, "1011011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "\\", 92, "1011100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "]", 93, "1011101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "^", 94, "1011110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "_", 95, "1011111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "`", 96, "1100000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "a", 97, "1100001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "b", 98, "1100010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "c", 99, "1100011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "d", 100, "1100100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "e", 101, "1100101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "f", 102, "1100110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "g", 103, "1100111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "h", 104, "1101000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "i", 105, "1101001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "j", 106, "1101010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "k", 107, "1101011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "l", 108, "1101100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "m", 109, "1101101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "n", 110, "1101110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "o", 111, "1101111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "p", 112, "1110000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "q", 113, "1110001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "r", 114, "1110010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "s", 115, "1110011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "t", 116, "1110100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "u", 117, "1110101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "v", 118, "1110110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "w", 119, "1110111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "x", 120, "1111000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "y", 121, "1111001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "z", 122, "1111010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "{", 123, "1111011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "|", 124, "1111100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "}", 125, "1111101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "~", 126, "1111110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ASCII", "<DEL>", 127, "1111111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<NUL>", 0, "00000000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<SOH>", 1, "00000001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<STX>", 2, "00000010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<ETX>", 3, "00000011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<EOT>", 4, "00000100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<ENQ>", 5, "00000101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<ACK>", 6, "00000110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<BEL>", 7, "00000111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<BS>", 8, "00001000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<HT>", 9, "00001001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "\n", 10, "00001010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<VT>", 11, "00001011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<FF>", 12, "00001100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "\r", 13, "00001101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<SO>", 14, "00001110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<SI>", 15, "00001111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<DLE>", 16, "00010000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<DC1>", 17, "00010001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<DC2>", 18, "00010010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<DC3>", 19, "00010011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<DC4>", 20, "00010100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<NAK>", 21, "00010101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<SYN>", 22, "00010110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<ETB>", 23, "00010111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<CAN>", 24, "00011000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<EM>", 25, "00011001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<SUB>", 26, "00011010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<ESC>", 27, "00011011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<FS>", 28, "00011100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<GS>", 29, "00011101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<RS>", 30, "00011110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<US>", 31, "00011111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", " ", 32, "00100000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "!", 33, "00100001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "\"", 34, "00100010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "#", 35, "00100011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "$", 36, "00100100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "%", 37, "00100101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "&", 38, "00100110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "", 39, "00100111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "(", 40, "00101000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", ")", 41, "00101001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "*", 42, "00101010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "+", 43, "00101011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", ",", 44, "00101100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "-", 45, "00101101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", ".", 46, "00101110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "/", 47, "00101111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "0", 48, "00110000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "1", 49, "00110001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "2", 50, "00110010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "3", 51, "00110011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "4", 52, "00110100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "5", 53, "00110101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "6", 54, "00110110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "7", 55, "00110111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "8", 56, "00111000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "9", 57, "00111001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", ":", 58, "00111010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", ";", 59, "00111011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<", 60, "00111100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "=", 61, "00111101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", ">", 62, "00111110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "?", 63, "00111111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "@", 64, "01000000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "A", 65, "01000001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "B", 66, "01000010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "C", 67, "01000011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "D", 68, "01000100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "E", 69, "01000101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "F", 70, "01000110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "G", 71, "01000111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "H", 72, "01001000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "I", 73, "01001001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "J", 74, "01001010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "K", 75, "01001011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "L", 76, "01001100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "M", 77, "01001101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "N", 78, "01001110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "O", 79, "01001111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "P", 80, "01010000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Q", 81, "01010001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "R", 82, "01010010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "S", 83, "01010011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "T", 84, "01010100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "U", 85, "01010101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "V", 86, "01010110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "W", 87, "01010111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "X", 88, "01011000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Y", 89, "01011001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Z", 90, "01011010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "[", 91, "01011011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "\\", 92, "01011100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "]", 93, "01011101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "^", 94, "01011110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "_", 95, "01011111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "`", 96, "01100000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "a", 97, "01100001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "b", 98, "01100010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "c", 99, "01100011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "d", 100, "01100100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "e", 101, "01100101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "f", 102, "01100110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "g", 103, "01100111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "h", 104, "01101000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "i", 105, "01101001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "j", 106, "01101010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "k", 107, "01101011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "l", 108, "01101100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "m", 109, "01101101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "n", 110, "01101110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "o", 111, "01101111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "p", 112, "01110000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "q", 113, "01110001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "r", 114, "01110010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "s", 115, "01110011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "t", 116, "01110100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "u", 117, "01110101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "v", 118, "01110110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "w", 119, "01110111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "x", 120, "01111000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "y", 121, "01111001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "z", 122, "01111010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "{", 123, "01111011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "|", 124, "01111100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "}", 125, "01111101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "~", 126, "01111110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "<DEL>", 127, "01111111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "�", 128, "10000000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "", 129, "10000001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "‚", 130, "10000010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ƒ", 131, "10000011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "„", 132, "10000100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "…", 133, "10000101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "†", 134, "10000110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "‡", 135, "10000111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ˆ", 136, "10001000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "‰", 137, "10001001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Š", 138, "10001010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "‹", 139, "10001011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Œ", 140, "10001100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "", 141, "10001101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ž", 142, "10001110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "", 143, "10001111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "", 144, "10010000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "‘", 145, "10010001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "’", 146, "10010010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "“", 147, "10010011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "”", 148, "10010100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "•", 149, "10010101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "–", 150, "10010110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "—", 151, "10010111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "˜", 152, "10011000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "™", 153, "10011001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "š", 154, "10011010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "›", 155, "10011011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "œ", 156, "10011100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "", 157, "10011101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ž", 158, "10011110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ÿ", 159, "10011111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "", 160, "10100000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¡", 161, "10100001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¢", 162, "10100010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "£", 163, "10100011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¤", 164, "10100100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¥", 165, "10100101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¦", 166, "10100110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "§", 167, "10100111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¨", 168, "10101000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "©", 169, "10101001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ª", 170, "10101010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "«", 171, "10101011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¬", 172, "10101100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "", 173, "10101101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "®", 174, "10101110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¯", 175, "10101111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "°", 176, "10110000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "±", 177, "10110001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "²", 178, "10110010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "³", 179, "10110011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "´", 180, "10110100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "µ", 181, "10110101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¶", 182, "10110110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "·", 183, "10110111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¸", 184, "10111000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¹", 185, "10111001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "º", 186, "10111010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "»", 187, "10111011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¼", 188, "10111100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "½", 189, "10111101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¾", 190, "10111110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "¿", 191, "10111111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "v", 192, "11000000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Á", 193, "11000001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Â", 194, "11000010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ã", 195, "11000011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ä", 196, "11000100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Å", 197, "11000101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Æ", 198, "11000110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ç", 199, "11000111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "È", 200, "11001000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "É", 201, "11001001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ê", 202, "11001010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ë", 203, "11001011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ì", 204, "11001100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Í", 205, "11001101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Î", 206, "11001110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ï", 207, "11001111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ð", 208, "11010000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ñ", 209, "11010001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ò", 210, "11010010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ó", 211, "11010011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ô", 212, "11010100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Õ", 213, "11010101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ö", 214, "11010110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "×", 215, "11010111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ø", 216, "11011000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ù", 217, "11011001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ú", 218, "11011010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Û", 219, "11011011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ü", 220, "11011100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Ý", 221, "11011101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "Þ", 222, "11011110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ß", 223, "11011111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "à", 224, "11100000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "á", 225, "11100001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "â", 226, "11100010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ã", 227, "11100011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ä", 228, "11100100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "å", 229, "11100101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "æ", 230, "11100110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ç", 231, "11100111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "è", 232, "11101000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "é", 233, "11101001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ê", 234, "11101010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ë", 235, "11101011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ì", 236, "11101100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "í", 237, "11101101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "î", 238, "11101110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ï", 239, "11101111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ð", 240, "11110000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ñ", 241, "11110001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ò", 242, "11110010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ó", 243, "11110011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ô", 244, "11110100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "õ", 245, "11110101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ö", 246, "11110110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "÷", 247, "11110111"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ø", 248, "11111000"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ù", 249, "11111001"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ú", 250, "11111010"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "û", 251, "11111011"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ü", 252, "11111100"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ý", 253, "11111101"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "þ", 254, "11111110"));
            _asciiAnsiItems.Add(new Tuple<string, string, int, string>("ANSI", "ÿ", 255, "11111111"));

            #endregion ANSI
        }

        /// <summary>
        /// Gets the ANSI character from bin.
        /// </summary>
        /// <param name="bin">The bin.</param>
        /// <param name="includeControlCharacters"></param>
        /// <returns></returns>
        public string GetANSICharFromBin(string bin, bool includeControlCharacters)
        {
            if (_asciiAnsiItems.Any(x => x.Item4 == bin && x.Item1 == "ANSI"))
            {
                var value = _asciiAnsiItems.Where(x => x.Item4 == bin && x.Item1 == "ANSI").Select(x => x.Item2).FirstOrDefault();
                if (value != null && value.StartsWith("<") && value.EndsWith(">") && !includeControlCharacters)
                {
                    return string.Empty;
                }
                return value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the ANSI character from decimal.
        /// </summary>
        /// <param name="dec">The decimal.</param>
        /// <returns></returns>
        public string GetANSICharFromDec(int dec, bool includeControlCharacters)
        {
            var value = _asciiAnsiItems.Where(x => x.Item3 == dec && x.Item1 == "ANSI").Select(x => x.Item2).FirstOrDefault();
            if (value != null && value.StartsWith("<") && value.EndsWith(">") && !includeControlCharacters)
            {
                return string.Empty;
            }
            return value;
        }

        /// <summary>
        /// Gets the ASCII character from bin.
        /// </summary>
        /// <param name="bin">The bin.</param>
        /// <param name="includeControlCharacters"></param>
        /// <returns></returns>
        public string GetASCIICharFromBin(string bin, bool includeControlCharacters)
        {
            if (_asciiAnsiItems.Any(x => x.Item4 == bin && x.Item1 == "ASCII"))
            {
                var value = _asciiAnsiItems.Where(x => x.Item4 == bin && x.Item1 == "ASCII").Select(x => x.Item2).FirstOrDefault();
                if (value != null && value.StartsWith("<") && value.EndsWith(">") && !includeControlCharacters)
                {
                    return string.Empty;
                }
                return value;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the ASCII character from decimal.
        /// </summary>
        /// <param name="dec">The decimal.</param>
        /// <returns></returns>
        public string GetASCIICharFromDec(int dec, bool includeControlCharacters)
        {
            var value = _asciiAnsiItems.Where(x => x.Item3 == dec && x.Item1 == "ASCII").Select(x => x.Item2).FirstOrDefault();
            if (value != null && value.StartsWith('<') && value.EndsWith('>') && !includeControlCharacters)
            {
                return string.Empty;
            }
            return value;
        }

        /// <summary>
        /// Gets the gematria strings Cicada Solver Style.
        /// </summary>
        /// <returns></returns>
        public string[] GetGematriaRunes()
        {
            return new string[] {
                "ᛝ", //ING
                "ᛟ", //OE
                "ᛇ", //EO
                "ᛡ", //IO
                "ᛠ", //EA
                "ᚫ", //AE
                "ᚦ", //TH
                "ᚠ", //F
                "ᚢ", //U
                "ᚩ", //O
                "ᚱ", //R
                "ᚳ", //C/K
                "ᚷ", //G
                "ᚹ", //W
                "ᚻ", //H
                "ᚾ", //N
                "ᛁ", //I
                "ᛄ", //J
                "ᛈ", //P
                "ᛉ", //X
                "ᛋ", //S/Z
                "ᛏ", //T
                "ᛒ", //B
                "ᛖ", //E
                "ᛗ", //M
                "ᛚ", //L
                "ᛞ", //D
                "ᚪ", //A
                "ᚣ" //Y
            };
        }

        /// <summary>
        /// Gets the character from rune.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string GetCharFromRune(string value)
        {
            string retval = string.Empty;
            switch (value)
            {
                case "ᛝ":
                    retval = "ING";
                    break;

                case "ᛟ":
                    retval = "OE";
                    break;

                case "ᛇ":
                    retval = "EO";
                    break;

                case "ᛡ":
                    retval = "IO";
                    break;

                case "ᛠ":
                    retval = "EA";
                    break;

                case "ᚫ":
                    retval = "AE";
                    break;

                case "ᚦ":
                    retval = "TH";
                    break;

                case "ᚠ":
                    retval = "F";
                    break;

                case "ᚢ":
                    retval = "U";
                    break;

                case "ᚩ":
                    retval = "O";
                    break;

                case "ᚱ":
                    retval = "R";
                    break;

                case "ᚳ":
                    retval = "C";
                    break;

                case "ᚷ":
                    retval = "G";
                    break;

                case "ᚹ":
                    retval = "W";
                    break;

                case "ᚻ":
                    retval = "H";
                    break;

                case "ᚾ":
                    retval = "N";
                    break;

                case "ᛁ":
                    retval = "I";
                    break;

                case "ᛄ":
                    retval = "J";
                    break;

                case "ᛈ":
                    retval = "P";
                    break;

                case "ᛉ":
                    retval = "X";
                    break;

                case "ᛋ":
                    retval = "S";
                    break;

                case "ᛏ":
                    retval = "T";
                    break;

                case "ᛒ":
                    retval = "B";
                    break;

                case "ᛖ":
                    retval = "E";
                    break;

                case "ᛗ":
                    retval = "M";
                    break;

                case "ᛚ":
                    retval = "L";
                    break;

                case "ᛞ":
                    retval = "D";
                    break;

                case "ᚪ":
                    retval = "A";
                    break;

                case "ᚣ":
                    retval = "Y";
                    break;

                default:
                    retval = value;
                    break;
            }

            return retval;
        }

        /// <summary>
        /// Gets the rune from value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string GetRuneFromValue(int value)
        {
            string retval;
            switch (value)
            {
                case 2:
                    retval = "ᚠ";
                    break;

                case 3:
                    retval = "ᚢ";
                    break;

                case 5:
                    retval = "ᚦ";
                    break;

                case 7:
                    retval = "ᚩ";
                    break;

                case 11:
                    retval = "ᚱ";
                    break;

                case 13:
                    retval = "ᚳ";
                    break;

                case 17:
                    retval = "ᚷ";
                    break;

                case 19:
                    retval = "ᚹ";
                    break;

                case 23:
                    retval = "ᚻ";
                    break;

                case 29:
                    retval = "ᚾ";
                    break;

                case 31:
                    retval = "ᛁ";
                    break;

                case 37:
                    retval = "ᛄ";
                    break;

                case 41:
                    retval = "ᛇ";
                    break;

                case 43:
                    retval = "ᛈ";
                    break;

                case 47:
                    retval = "ᛉ";
                    break;

                case 53:
                    retval = "ᛋ";
                    break;

                case 59:
                    retval = "ᛏ";
                    break;

                case 61:
                    retval = "ᛒ";
                    break;

                case 67:
                    retval = "ᛖ";
                    break;

                case 71:
                    retval = "ᛗ";
                    break;

                case 73:
                    retval = "ᛚ";
                    break;

                case 79:
                    retval = "ᛝ";
                    break;

                case 83:
                    retval = "ᛟ";
                    break;

                case 89:
                    retval = "ᛞ";
                    break;

                case 97:
                    retval = "ᚪ";
                    break;

                case 101:
                    retval = "ᚫ";
                    break;

                case 103:
                    retval = "ᚣ";
                    break;

                case 107:
                    retval = "ᛡ";
                    break;

                case 109:
                    retval = "ᛠ";
                    break;

                default:
                    retval = string.Empty;
                    break;
            }

            return retval;
        }

        /// <summary>
        /// Gets the permutations.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        public IEnumerable<string[]> GetPermutations(string[] array)
        {
            foreach (var permutation in Permute(array, 0, array.Length - 1))
            {
                yield return permutation;
            }
        }

        /// <summary>
        /// Permutes the specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="l">The l.</param>
        /// <param name="r">The r.</param>
        /// <returns></returns>
        private IEnumerable<string[]> Permute(string[] array, int l, int r)
        {
            if (l == r)
            {
                yield return (string[])array.Clone();
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    Swap(ref array[l], ref array[i]);
                    foreach (var permutation in Permute(array, l + 1, r))
                    {
                        yield return permutation;
                    }
                    Swap(ref array[l], ref array[i]); // backtrack
                }
            }
        }

        /// <summary>
        /// Swaps the specified a.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        private void Swap(ref string a, ref string b)
        {
            var temp = a;
            a = b;
            b = temp;
        }
    }
}