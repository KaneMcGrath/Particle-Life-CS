using KaneUI7;
using KaneUI7.Foundation;
using ParticleLife.Game;
using Raylib_cs;
using System.Numerics;
using System.Text;

namespace KaneText7
{
    /// <summary>
    /// Framework for text editing to integrate with KaneUI7 and be as immediate mode as possible
    /// offers absolutly no benifit over any other text editing solution other than the miniscule 
    /// prestige of having made it myself.
    /// Designed specifically for raylib_cs
    /// </summary>
    public static unsafe class KaneText
    {
        //Im going to use the select area rect to identify which text box we are currently editing
        //otherwise I would have to have an ID or use an object for the text boxes
        public static Rect EditedTextBox = new Rect();
        public static bool IsEdititingText = false;

        public static bool TextDebugVis = false;

        public static int CursorIndex = 0;
        public static bool insert = true;           //toggle insert mode to replace text

        public static bool IsSelection = true;      //if text is curently highlighted, but now is just a toggle for if you can select text
        public static int SelectionEndIndex = 0;    //if highlighting text highlight the range from CursorIndex to SelectionEndIndex

        public static float[] CurrentStringSpacing = new float[3];

        private static char lastChar = ' ';

        private static float CursorBlinkTimer = 0f;
        private static float CursorBlinkTime = 0.3f;
        private static bool DrawCursor = true;

        /// <summary>
        /// 
        /// </summary>
        public static string EditableText(Rect selectArea, Vector2 textOrigin, string text, int fontSize)
        {
            Vector2 textSize = Raylib.MeasureTextEx(KaneGameManager.DefaultFont, text, fontSize, 0);
            Rect textArea = new Rect((int)textOrigin.X, (int)textOrigin.Y, (int)textSize.X, (int)textSize.Y);
            if (IsEdititingText && EditedTextBox == selectArea)
            {
                if (TextDebugVis)
                {
                    KaneBlocks.DrawOutline(selectArea, 1, DefaultColors.Orange);
                    KaneUI.Label(new Rect(10, 160, 100, 30), "Key:" + lastChar.ToString());
                    KaneUI.Label(new Rect(10, 190, 100, 30), "Raw:" + Raylib.GetKeyPressed().ToString());
                    KaneUI.Label(new Rect(10, 220, 100, 30), "Cursor Index:" + CursorIndex.ToString());
                    KaneUI.Label(new Rect(10, 250, 100, 30), "Selection End Index:" + SelectionEndIndex.ToString());
                    KaneUI.Label(new Rect(10, 280, 100, 30), "Text Length:" + text.Length);
                }


                if (TextDebugVis)
                {
                    KaneBlocks.DrawOutline(textArea, 1, DefaultColors.Yellow);
                }

                if (Tools.timer(ref CursorBlinkTimer, CursorBlinkTime))
                {
                    DrawCursor = !DrawCursor;
                }
                //draw Selection Rectangle behind text
                if (IsSelection)
                {
                    //KaneFoundation.DrawRect(new Rect(textArea.x + (int)(widthSum), textArea.y, 2, textArea.height), Color.BLACK);
                    //find start pixel and width
                    int startPixel = -1;
                    int width = -1;
                    float selectionWidthSum = 0f;
                    for (int c = 0; c < text.Length + 1; c++)
                    {
                        if (c == CursorIndex)
                        {
                            startPixel = textArea.X + (int)(selectionWidthSum);
                        }
                        if (c == SelectionEndIndex)
                        {
                            width = (textArea.X + (int)(selectionWidthSum)) - startPixel;
                        }
                        if (c < text.Length)
                        {
                            float charwidth = Raylib.MeasureTextEx(KaneGameManager.DefaultFont, text.Substring(c, 1), fontSize, 0).X;
                            selectionWidthSum += charwidth;
                        }
                    }
                    //create a rect from the startPixel and width
                    if (startPixel >= 0 && width > 0)
                    {
                        Rect selectionArea = new Rect(startPixel, textArea.Y, width, textArea.Height);
                        KaneFoundation.DrawRect(selectionArea, DefaultColors.TextSelectionColor);
                    }
                }

                //handle keyboard inputs
                text = KeyboardInputs(text);

                //Sanity Checks in case I suck
                //if the string is modified elsewhere keep our cursor inbounds
                if (CursorIndex > text.Length)
                {
                    CursorIndex = text.Length;
                }
                if (SelectionEndIndex > text.Length)
                {
                    SelectionEndIndex = text.Length;
                }
                if (SelectionEndIndex < CursorIndex)
                {
                    SelectionEndIndex = CursorIndex;
                }
                if (CursorIndex < 0) //not sure how this could ever happen
                {
                    CursorIndex = 0;
                }
                //draw cursor
                float widthSum = 0f;
                for (int c = 0; c < text.Length + 1; c++)
                {
                    if (c == CursorIndex)
                    {
                        if (DrawCursor)
                        {
                            KaneFoundation.DrawRect(new Rect(textArea.X + (int)(widthSum), textArea.Y, 2, textArea.Height), DefaultColors.Black);
                        }

                        break;
                    }
                    if (c < text.Length)
                    {
                        float charwidth = Raylib.MeasureTextEx(KaneGameManager.DefaultFont, text.Substring(c, 1), fontSize, 0).X;
                        widthSum += charwidth;
                    }
                }

                //click the text to put the cursor somewhere
                if (KaneFoundation.IsLeftClickDown())
                {
                    if (PanelManager.IsMouseInActivePanelAndRect(selectArea))
                    {
                        if (PanelManager.IsMouseInActivePanelAndRect(textArea))
                        {

                            float widthSum3 = 0f;
                            for (int c = 0; c < text.Length; c++)
                            {
                                float charwidth = Raylib.MeasureTextEx(KaneGameManager.DefaultFont, text.Substring(c, 1), fontSize, 0).X;
                                float p = Raylib.GetMouseX() - textArea.X;

                                if (p > widthSum3 && p < widthSum3 + charwidth)
                                {
                                    if (p < widthSum3 + (charwidth / 2f))
                                    {
                                        CursorIndex = c;
                                    }
                                    else
                                    {
                                        CursorIndex = c + 1;
                                    }
                                }
                                widthSum3 += charwidth;
                            }
                        }
                        else
                        {
                            CursorIndex = text.Length;
                            SelectionEndIndex = text.Length;
                        }
                    }
                    else
                    {
                        IsEdititingText = false;
                    }
                }

                //Drag the text to select an area
                if (KaneFoundation.IsLeftClick())
                {
                    if (PanelManager.IsMouseInActivePanelAndRect(selectArea))
                    {
                        if (PanelManager.IsMouseInActivePanelAndRect(textArea))
                        {
                            float widthSum3 = 0f;
                            for (int c = 0; c < text.Length; c++)
                            {
                                float charwidth = Raylib.MeasureTextEx(KaneGameManager.DefaultFont, text.Substring(c, 1), fontSize, 0).X;
                                float p = Raylib.GetMouseX() - textArea.X;
                                if (c >= CursorIndex)
                                {
                                    if (p > widthSum3 && p < widthSum3 + charwidth)
                                    {
                                        if (p < widthSum3 + (charwidth / 2f))
                                        {
                                            SelectionEndIndex = c;
                                        }
                                        else
                                        {
                                            SelectionEndIndex = c + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    SelectionEndIndex = CursorIndex;
                                }

                                widthSum3 += charwidth;
                            }
                        }
                    }
                }


                if (TextDebugVis)
                {
                    if (PanelManager.IsMouseInActivePanelAndRect(textArea))
                    {

                        float widthSum2 = 0f;
                        for (int c = 0; c < text.Length; c++)
                        {
                            float charwidth = Raylib.MeasureTextEx(KaneGameManager.DefaultFont, text.Substring(c, 1), fontSize, 0).X;

                            float p = Raylib.GetMouseX() - textArea.X;

                            if (p > widthSum2 && p < widthSum2 + charwidth)
                            {
                                KaneUI.Label(new Rect(10, 10, 100, 30), "CharWidth:" + charwidth.ToString());
                                KaneUI.Label(new Rect(10, 40, 100, 30), "widthSum2:" + widthSum2.ToString());
                                KaneUI.Label(new Rect(10, 70, 100, 30), "P:" + p.ToString());
                                KaneUI.Label(new Rect(10, 100, 100, 30), "TX:" + textArea.X.ToString());
                                KaneUI.Label(new Rect(10, 130, 100, 30), "I:" + c.ToString());
                                KaneBlocks.DrawOutline(new Rect(textArea.X + (int)(widthSum2), textArea.Y, (int)charwidth, textArea.Height), 1, DefaultColors.Blue);
                                if (p < widthSum2 + (charwidth / 2f))
                                {
                                    KaneFoundation.DrawRect(new Rect(textArea.X + (int)(widthSum2), textArea.Y, (int)(charwidth / 2f), textArea.Height), DefaultColors.Green);
                                }
                                else
                                {
                                    KaneFoundation.DrawRect(new Rect(textArea.X + (int)(widthSum2 + charwidth / 2f), textArea.Y, (int)(charwidth / 2f), textArea.Height), DefaultColors.Green);
                                }
                            }
                            widthSum2 += charwidth;
                        }
                    }
                }

            }
            else
            {
                if (PanelManager.IsMouseInActivePanelAndRect(selectArea) && KaneFoundation.IsLeftClickDown())
                {
                    IsEdititingText = true;
                    EditedTextBox = selectArea;
                    //Vector2 textSize = Raylib.MeasureTextEx(KaneGameManager.DefaultFont, text, fontSize, 0);
                    //Rect textArea = new Rect((int)textOrigin.X, (int)textOrigin.Y, (int)textSize.X, (int)textSize.Y);
                    if (KaneUtils.IsMouseInRect(textArea))
                    {
                        float widthSum3 = 0f;
                        for (int c = 0; c < text.Length; c++)
                        {
                            float charwidth = Raylib.MeasureTextEx(KaneGameManager.DefaultFont, text.Substring(c, 1), fontSize, 0).X;
                            float p = Raylib.GetMouseX() - textArea.X;

                            if (p > widthSum3 && p < widthSum3 + charwidth)
                            {
                                if (p < widthSum3 + (charwidth / 2f))
                                {
                                    CursorIndex = c;
                                }
                                else
                                {
                                    CursorIndex = c + 1;
                                }
                            }
                            widthSum3 += charwidth;
                        }
                    }
                    else
                    {
                        CursorIndex = text.Length;
                    }
                }
            }
            KaneUI.Label(textArea, text, DefaultColors.White, fontSize, 0);
            return text;
        }

        private static string KeyboardInputs(string text)
        {
            if (InputManager.IsInputs)
            {
                for (int i = 0; i < InputManager.InputQueue.Count; i++)
                {
                    int key = InputManager.InputQueue[i];
                    if (!InputManager.IsModifierKey())
                    {
                        if (InputManager.IsChar(key))
                        {
                            if (SelectionEndIndex - CursorIndex > 0)
                            {
                                text = text.Remove(CursorIndex, SelectionEndIndex - CursorIndex);
                                text = text.Insert(CursorIndex, InputManager.KeyboardKeyToChar(key).ToString());
                                CursorIndex++;
                                SelectionEndIndex = CursorIndex;
                            }
                            else
                            {
                                text = text.Insert(CursorIndex, InputManager.KeyboardKeyToChar(key).ToString());
                                CursorIndex++;
                                SelectionEndIndex = CursorIndex;
                            }
                            continue;
                        }
                        if (key == (int)KeyboardKey.Left)
                        {

                            if (CursorIndex > 0)
                            {
                                CursorIndex--;
                                if (!InputManager.IsShiftKey())
                                {
                                    SelectionEndIndex = CursorIndex;
                                }
                            }
                        }
                        if (key == (int)KeyboardKey.Right)
                        {
                            if (CursorIndex < text.Length)
                            {
                                if (!InputManager.IsShiftKey())
                                {
                                    CursorIndex++;
                                    SelectionEndIndex = CursorIndex;
                                }
                                else
                                {
                                    if (SelectionEndIndex < text.Length)
                                    {
                                        SelectionEndIndex++;
                                    }
                                }
                            }
                        }
                        if (key == (int)KeyboardKey.Backspace)
                        {

                            if (CursorIndex <= text.Length)
                            {
                                if (SelectionEndIndex - CursorIndex > 0)
                                {
                                    text = text.Remove(CursorIndex, SelectionEndIndex - CursorIndex);
                                    SelectionEndIndex = CursorIndex;
                                }
                                else if (CursorIndex > 0)
                                {
                                    CursorIndex--;
                                    SelectionEndIndex = CursorIndex;
                                    text = text.Remove(CursorIndex, 1);
                                }
                            }
                        }
                        if (key == (int)KeyboardKey.Delete)
                        {
                            if (CursorIndex < text.Length)
                            {
                                if (SelectionEndIndex - CursorIndex > 0)
                                {
                                    text = text.Remove(CursorIndex, SelectionEndIndex - CursorIndex);
                                    SelectionEndIndex = CursorIndex;
                                }
                                else
                                {
                                    text = text.Remove(CursorIndex, 1);
                                }
                            }
                        }
                        if (key == (int)KeyboardKey.Home)
                        {
                            CursorIndex = 0;
                            if (!InputManager.IsShiftKey())
                            {
                                SelectionEndIndex = CursorIndex;
                            }
                        }
                        if (key == (int)KeyboardKey.End)
                        {
                            if (!InputManager.IsShiftKey())
                            {
                                CursorIndex = text.Length;
                                SelectionEndIndex = CursorIndex;
                            }
                            else
                            {
                                SelectionEndIndex = text.Length;
                            }
                        }
                    }
                    else
                    {
                        if (InputManager.GetKey(KeyboardKey.LeftControl))
                        {
                            if (key == 86) //KEY_V = 86
                            {
                                try
                                {
                                    if (SelectionEndIndex - CursorIndex > 0)
                                    {
                                        text = text.Remove(CursorIndex, SelectionEndIndex - CursorIndex);
                                    }
                                    sbyte* s = Raylib.GetClipboardText();
                                    string clip = new string(s);
                                    text = text.Insert(CursorIndex, clip);
                                    CursorIndex += clip.Length;
                                    SelectionEndIndex = CursorIndex;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (key == 67) //KEY_C = 67
                            {
                                try
                                {
                                    byte[] bytes = Encoding.ASCII.GetBytes(text.Substring(CursorIndex, SelectionEndIndex - CursorIndex));
                                    fixed (byte* b = bytes)
                                    {
                                        sbyte* sb = (sbyte*)b;
                                        Raylib.SetClipboardText(sb);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            //Control + Left
                            //jump to the previous word seperated by a space
                            if (key == 263)//KEY_LEFT = 263
                            {
                                if (CursorIndex > 0)
                                {
                                    int space = text.LastIndexOf(' ', CursorIndex - 2, CursorIndex - 2);
                                    if (space != -1)
                                    {
                                        CursorIndex = space + 1;
                                    }
                                    else
                                    {
                                        CursorIndex = 0;
                                    }
                                    SelectionEndIndex = CursorIndex;
                                }
                            }
                            //Control + Right
                            //jump to the next word seperated by a space
                            if (key == 262)//KEY_RIGHT = 262
                            {
                                if (CursorIndex < text.Length)
                                {
                                    int space = text.IndexOf(' ', CursorIndex);
                                    if (space != -1)
                                    {
                                        CursorIndex = space + 1;
                                    }
                                    else
                                    {
                                        CursorIndex = text.Length;
                                    }
                                    SelectionEndIndex = CursorIndex;
                                }
                            }
                            //Control + A
                            //Select All
                            if (key == (int)(KeyboardKey.A))
                            {
                                CursorIndex = 0;
                                SelectionEndIndex = text.Length;
                            }
                        }
                    }
                }
            }

            return text;
        }
    }
}
