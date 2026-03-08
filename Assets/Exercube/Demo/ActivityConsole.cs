using System;
using System.Text;
using UnityEngine;
using TMPro;

namespace Sphery.ExerCube
{
    public class ActivityConsole
    {
        protected const int MaxLines = 48;
        protected const float MonoSpacing = 0.6f;

        protected TMP_Text console;
        protected StringBuilder sb = new StringBuilder(MaxLines * 256);
        private int lineCount = 0;

        public ActivityConsole(TMP_Text console)
        {
            this.console = console;
        }

        public string LastLine { get; protected set; } = String.Empty;

        protected void CleanupRoutine()
        {
            if (lineCount < MaxLines)
            {
                lineCount++;
            }
            else
            {
                int index = sb.ToString().IndexOf('\n');

                if (index >= 0)
                    sb.Remove(0, index + 1);
            }
        }

        public void Log(string msg, Color color)
        {
            sb.Append("\n<color=#");
            sb.Append(ColorUtility.ToHtmlStringRGB(color));
            sb.Append(">");
            sb.Append(msg);
            sb.Append("</color>");

            CleanupRoutine();

            console.text = $"<mspace={MonoSpacing:F2}em>{sb.ToString()}</mspace>";
            LastLine = msg;
        }

        public void Log(string msg)
        {
            sb.Append($"\n{msg}");

            CleanupRoutine();

            console.text = $"<mspace={MonoSpacing:F2}em>{sb.ToString()}</mspace>";
            LastLine = msg;
        }

        public void Clear()
        {
            lineCount = 0;
            sb.Clear();
            console.text = sb.ToString();
            LastLine = String.Empty;
        }
    }
}
