﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace XboxControllerRemote
{
    public class Keyboard
    {
        public enum Direction { Left, Right, Up, Down };
        public enum KeySet { Lowercase, Uppercase, Symbols };

        private static string FONT = "Arial";

        private Dictionary<KeySet, string[][]> keySets = new Dictionary<KeySet, string[][]>();
        private int width;
        private int height;

        private int selectedCol;
        private int selectedRow;
        KeySet currentKeySet;

        public Keyboard(int width, int height)
        {
            this.width = width;
            this.height = height;

            string[][] uppercaseKeys = new string[4][];
            uppercaseKeys[0] = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            uppercaseKeys[1] = new string[] { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" };
            uppercaseKeys[2] = new string[] { "A", "S", "D", "F", "G", "H", "J", "K", "L", "{ENTER}" };
            uppercaseKeys[3] = new string[] { "Z", "X", "C", "V", "B", "N", "M", "@", "." };
            keySets.Add(KeySet.Uppercase, uppercaseKeys);

            string[][] lowercaseKeys = new string[4][];
            lowercaseKeys[0] = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            lowercaseKeys[1] = new string[] { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p" };
            lowercaseKeys[2] = new string[] { "a", "s", "d", "f", "g", "h", "j", "k", "l", "{ENTER}" };
            lowercaseKeys[3] = new string[] { "z", "x", "c", "v", "b", "n", "m", "@", "." };
            keySets.Add(KeySet.Lowercase, lowercaseKeys);

            string[][] symbolKeys = new string[4][];
            symbolKeys[0] = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            symbolKeys[1] = new string[] { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")" };
            symbolKeys[2] = new string[] { "-", "_", "=", "+", "[", "]", "{", "}", "'", "{ENTER}" };
            symbolKeys[3] = new string[] { "\\", "|", "<", ">", ",", ".", "?", "/", "\"" };
            keySets.Add(KeySet.Symbols, symbolKeys);

            selectedCol = 0;
            selectedRow = 0;
            currentKeySet = KeySet.Lowercase;
        }

        private int GetMaxRowLength()
        {
            int maxLength = 0;
            foreach (string[][] keys in keySets.Values)
            {
                foreach (string[] row in keys)
                {
                    if (row.Length > maxLength)
                    {
                        maxLength = row.Length;
                    }
                }
            }
            return maxLength;
        }

        private int GetMaxNumRows()
        {
            int maxNumRows = 0;
            foreach (string[][] keys in keySets.Values)
            {
                if (keys.Length > maxNumRows)
                {
                    maxNumRows = keys.Length;
                }
            }
            return maxNumRows;
        }

        private int GetKeyWidth()
        {
            return width / (GetMaxRowLength() + 1);
        }
        
        private int GetKeyHeight()
        {
            return height / (GetMaxNumRows() + 1);
        }

        public void DrawKeyboard(Graphics graphics)
        {
            graphics.Clear(Color.LightGray);

            for (int row = 0; row < GetCurrentKeys().Length; row++)
            {
                for (int col = 0; col < GetCurrentKeys()[row].Length; col++)
                {
                    string key = GetCurrentKeys()[row][col];

                    int x = GetKeyWidth() * col;
                    int y = GetKeyHeight() * row;

                    Rectangle rect = new Rectangle(x, y, GetKeyWidth(), GetKeyHeight());

                    int fontSize = GetKeyWidth() / 2;
                    int textX = x;
                    int textY = y + GetKeyHeight() / 4;
                    if (key.Length > 1)
                    {
                        fontSize = GetKeyWidth() / 7;
                        textY = y + GetKeyHeight() / 2;
                    }
                    Font font = new Font(FONT, fontSize);

                    if (col == selectedCol && row == selectedRow)
                    {
                        graphics.FillRectangle(Brushes.Black, rect);
                        graphics.DrawString(key, font, Brushes.White, textX, textY);
                    }
                    else
                    {
                        graphics.DrawRectangle(Pens.Black, rect);
                        graphics.DrawString(key, font, Brushes.Black, textX, textY);
                    }

                }
            }
        }

        public void MoveCursor(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    selectedCol -= 1;
                    break;
                case Direction.Right:
                    selectedCol += 1;
                    break;
                case Direction.Up:
                    selectedRow -= 1;
                    break;
                case Direction.Down:
                    selectedRow += 1;
                    break;
            }

            MoveSelectionInsideBounds();
        }

        private void MoveSelectionInsideBounds()
        {
            if (selectedRow < 0)
            {
                selectedRow = 0;
            }
            if (selectedRow >= GetCurrentKeys().Length)
            {
                selectedRow = GetCurrentKeys().Length - 1;
            }
            if (selectedCol < 0)
            {
                selectedCol = 0;
            }
            if (selectedCol >= GetCurrentKeys()[selectedRow].Length)
            {
                selectedCol = GetCurrentKeys()[selectedRow].Length - 1;
            }
        }

        private string[][] GetCurrentKeys()
        {
            return keySets[currentKeySet];
        }

        public string GetSelectedKey()
        {
            return GetCurrentKeys()[selectedRow][selectedCol];
        }

        public KeySet CurrentKeySet()
        {
            return currentKeySet;
        }

        public void SwitchKeySet(KeySet keySet)
        {
            currentKeySet = keySet;
            MoveSelectionInsideBounds();
        }
    }
}