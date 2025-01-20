using Raylib_cs;

namespace ParticleLife.Game
{
    /// <summary>
    /// Handle The Inputbuffer and and add helpful functions to get keys and ignore when typing
    /// </summary>
    public static class InputManager
    {
        public static bool[] inputs = new bool[400]; //one for every key defined in input.cs
        public static List<int> InputQueue = new List<int>();
        public static Dictionary<int, char> charDict = new Dictionary<int, char>();
        public static bool IsInputs = false;
        public static void Init()
        {

            //  Regular Keys                Shift Modified Keys
            //                              +10000
            // Alphanumeric keys
            charDict[39] = '\''; charDict[10039] = '\"'; //KEY_APOSTROPHE = 39,
            charDict[44] = ','; charDict[10044] = '<';  //KEY_COMMA = 44,
            charDict[45] = '-'; charDict[10045] = '_';  //KEY_MINUS = 45,
            charDict[46] = '.'; charDict[10046] = '>';  //KEY_PERIOD = 46,
            charDict[47] = '/'; charDict[10047] = '?';  //KEY_SLASH = 47,
            charDict[48] = '0'; charDict[10048] = ')';  //KEY_ZERO = 48,
            charDict[49] = '1'; charDict[10049] = '!';  //KEY_ONE = 49,
            charDict[50] = '2'; charDict[10050] = '@';  //KEY_TWO = 50,
            charDict[51] = '3'; charDict[10051] = '#';  //KEY_THREE = 51,
            charDict[52] = '4'; charDict[10052] = '$';  //KEY_FOUR = 52,
            charDict[53] = '5'; charDict[10053] = '%';  //KEY_FIVE = 53,
            charDict[54] = '6'; charDict[10054] = '^';  //KEY_SIX = 54,
            charDict[55] = '7'; charDict[10055] = '&';  //KEY_SEVEN = 55,
            charDict[56] = '8'; charDict[10056] = '*';  //KEY_EIGHT = 56,
            charDict[57] = '9'; charDict[10057] = '(';  //KEY_NINE = 57,
            charDict[59] = ';'; charDict[10059] = ':';  //KEY_SEMICOLON = 59,
            charDict[61] = '='; charDict[10061] = '+';  //KEY_EQUAL = 61,
            charDict[65] = 'a'; charDict[10065] = 'A';  //KEY_A = 65,
            charDict[66] = 'b'; charDict[10066] = 'B';  //KEY_B = 66,
            charDict[67] = 'c'; charDict[10067] = 'C';  //KEY_C = 67,
            charDict[68] = 'd'; charDict[10068] = 'D';  //KEY_D = 68,
            charDict[69] = 'e'; charDict[10069] = 'E';  //KEY_E = 69,
            charDict[70] = 'f'; charDict[10070] = 'F';  //KEY_F = 70,
            charDict[71] = 'g'; charDict[10071] = 'G';  //KEY_G = 71,
            charDict[72] = 'h'; charDict[10072] = 'H';  //KEY_H = 72,
            charDict[73] = 'i'; charDict[10073] = 'I';  //KEY_I = 73,
            charDict[74] = 'j'; charDict[10074] = 'J';  //KEY_J = 74,
            charDict[75] = 'k'; charDict[10075] = 'K';  //KEY_K = 75,
            charDict[76] = 'l'; charDict[10076] = 'L';  //KEY_L = 76,
            charDict[77] = 'm'; charDict[10077] = 'M';  //KEY_M = 77,
            charDict[78] = 'n'; charDict[10078] = 'N';  //KEY_N = 78,
            charDict[79] = 'o'; charDict[10079] = 'O';  //KEY_O = 79,
            charDict[80] = 'p'; charDict[10080] = 'P';  //KEY_P = 80,
            charDict[81] = 'q'; charDict[10081] = 'Q';  //KEY_Q = 81,
            charDict[82] = 'r'; charDict[10082] = 'R';  //KEY_R = 82,
            charDict[83] = 's'; charDict[10083] = 'S';  //KEY_S = 83,
            charDict[84] = 't'; charDict[10084] = 'T';  //KEY_T = 84,
            charDict[85] = 'u'; charDict[10085] = 'U';  //KEY_U = 85,
            charDict[86] = 'v'; charDict[10086] = 'V';  //KEY_V = 86,
            charDict[87] = 'w'; charDict[10087] = 'W';  //KEY_W = 87,
            charDict[88] = 'x'; charDict[10088] = 'X';  //KEY_X = 88,
            charDict[89] = 'y'; charDict[10089] = 'Y';  //KEY_Y = 89,
            charDict[90] = 'z'; charDict[10090] = 'Z';  //KEY_Z = 90,

            // Function keys
            charDict[32] = ' ';                                 //KEY_SPACE = 32,
                                                                //KEY_ESCAPE = 256,
                                                                //KEY_ENTER = 257,
                                                                //KEY_TAB = 258,
                                                                //KEY_BACKSPACE = 259,
                                                                //KEY_INSERT = 260,
                                                                //KEY_DELETE = 261,
                                                                //KEY_RIGHT = 262,
                                                                //KEY_LEFT = 263,
                                                                //KEY_DOWN = 264,
                                                                //KEY_UP = 265,
                                                                //KEY_PAGE_UP = 266,
                                                                //KEY_PAGE_DOWN = 267,
                                                                //KEY_HOME = 268,
                                                                //KEY_END = 269,
                                                                //KEY_CAPS_LOCK = 280,
                                                                //KEY_SCROLL_LOCK = 281,
                                                                //KEY_NUM_LOCK = 282,
                                                                //KEY_PRINT_SCREEN = 283,
                                                                //KEY_PAUSE = 284,
                                                                //KEY_F1 = 290,
                                                                //KEY_F2 = 291,
                                                                //KEY_F3 = 292,
                                                                //KEY_F4 = 293,
                                                                //KEY_F5 = 294,
                                                                //KEY_F6 = 295,
                                                                //KEY_F7 = 296,
                                                                //KEY_F8 = 297,
                                                                //KEY_F9 = 298,
                                                                //KEY_F10 = 299,
                                                                //KEY_F11 =              300,
                                                                //KEY_F12 =              301,
                                                                //KEY_LEFT_SHIFT =       340,
                                                                //KEY_LEFT_CONTROL =     341,
                                                                //KEY_LEFT_ALT =         342,
                                                                //KEY_LEFT_SUPER =       343,
                                                                //KEY_RIGHT_SHIFT =      344,
                                                                //KEY_RIGHT_CONTROL =    345,
                                                                //KEY_RIGHT_ALT =        346,
                                                                //KEY_RIGHT_SUPER =      347,
                                                                //KEY_KB_MENU =          348,
            charDict[91] = '['; charDict[10091] = '{';  //KEY_LEFT_BRACKET =      91,
            charDict[92] = '\\'; charDict[10092] = '|';  //KEY_BACKSLASH = 92,
            charDict[93] = ']'; charDict[10093] = '}';  //KEY_RIGHT_BRACKET = 93,
            charDict[96] = '`'; charDict[10096] = '~';  //KEY_GRAVE = 96,

            // Keypad keys      dont have a numpad so I dont know what goes here
            charDict[320] = '0';                                //KEY_KP_0 = 320,
            charDict[321] = '1';                                //KEY_KP_1 = 321,
            charDict[322] = '2';                                //KEY_KP_2 = 322,
            charDict[323] = '3';                                //KEY_KP_3 = 323,
            charDict[324] = '4';                                //KEY_KP_4 = 324,
            charDict[325] = '5';                                //KEY_KP_5 = 325,
            charDict[326] = '6';                                //KEY_KP_6 = 326,
            charDict[327] = '7';                                //KEY_KP_7 = 327,
            charDict[328] = '8';                                //KEY_KP_8 = 328,
            charDict[329] = '9';                                //KEY_KP_9 = 329,
            charDict[330] = '.';                                //KEY_KP_DECIMAL =   330,
            charDict[331] = '/';                                //KEY_KP_DIVIDE =    331,
            charDict[332] = '*';                                //KEY_KP_MULTIPLY =  332,
            charDict[333] = '-';                                //KEY_KP_SUBTRACT =  333,
            charDict[334] = '+';                                //KEY_KP_ADD =       334,
                                                                //KEY_KP_ENTER =     335,
            charDict[336] = '=';                                //KEY_KP_EQUAL =     336,
        }


        public static void Update()
        {
            InputQueue.Clear();
            IsInputs = false;
            while (true)
            {
                int k = Raylib.GetKeyPressed();
                if (k != 0)
                {
                    k += IsCAPS(k);
                    InputQueue.Add(k);
                    IsInputs = true;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Should Our Inputs be Shifted(Caps and special chars) by the shift keys or caps lock
        /// if caps lock, only shift alphabetical keys a-z
        /// if caps lock and shift, return lowercase
        /// </summary>
        /// <returns></returns>
        private static int IsCAPS(int key)
        {
            if (charDict.ContainsKey(key))
            {
                if (IsShiftKey())
                {
                    if (GetKey(KeyboardKey.CapsLock))
                    {
                        return 0;
                    }

                    return 10000;
                }
                if (GetKey(KeyboardKey.CapsLock) && key >= 65 && key <= 90)
                {
                    return 10000;
                }
            }
            return 0;
        }

        private static readonly KeyboardKey[] modifierKeys =
        {
            KeyboardKey.LeftControl,
            KeyboardKey.RightControl,
            KeyboardKey.LeftAlt,
            KeyboardKey.RightAlt,
            KeyboardKey.LeftSuper,
            KeyboardKey.RightSuper
        };

        public static bool IsModifierKey()
        {
            foreach (KeyboardKey modifier in modifierKeys)
            {
                if (GetKey(modifier))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsShiftKey()
        {
            return (InputManager.GetKey(KeyboardKey.LeftShift) || InputManager.GetKey(KeyboardKey.RightShift));
        }

        public static char[] GetChars()
        {
            char[] result = new char[InputQueue.Count];
            for (int i = 0; i < InputQueue.Count; i++)
            {
                char c = KeyboardKeyToChar(InputQueue[i]);
                result[i] = c;
            }
            return result;
        }

        public static bool IsChar(int key)
        {
            return (charDict.ContainsKey(key));

        }

        public static char KeyboardKeyToChar(int key)
        {
            if (charDict.ContainsKey(key))
            {
                return charDict[key];
            }
            else
            {
                return char.MinValue;
            }
        }

        public static bool GetKey(KeyboardKey key)
        {
            return Raylib.IsKeyDown(key);
        }
    }
}
