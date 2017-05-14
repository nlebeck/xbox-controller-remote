﻿using System.Drawing;
using XboxControllerRemote.AppMenuItems;

namespace XboxControllerRemote
{
    public class AppMenu : Menu
    {
        private AppMenuItem[] menuItems = {
            new WebsiteItem("Netflix", "https://www.netflix.com"),
            new WebsiteItem("Hulu", "https://www.hulu.com"),
            new ControllerProgramItem("Steam", "Steam", "C:\\Program Files (x86)\\Steam\\steam.exe", "-bigPicture", "steam://open/bigpicture"),
            new ProgramItem("Skype (Classic)", "Skype", "C:\\Program Files (x86)\\Skype\\Phone\\Skype.exe", "", ""),
            new MouseEmulatorItem(),
            new ShutdownItem(),
            new QuitItem()
        };

        private int selectedIndex = 0;

        public AppMenu(MainForm form, int width, int height) : base(form, width, height) { }

        public override void Draw(Graphics graphics)
        {
            graphics.Clear(Color.LightGray);

            int menuItemHeight = height / menuItems.Length;

            for (int i = 0; i < menuItems.Length; i++)
            {
                int vOffset = i * height / menuItems.Length;
                Rectangle rect = new Rectangle(0, vOffset, width, menuItemHeight);
                Font font = new Font(MENU_FONT, 16);
                if (i == selectedIndex)
                {
                    graphics.FillRectangle(Brushes.Black, rect);
                    graphics.DrawString(menuItems[i].Name, font, Brushes.White, 0, vOffset);
                }
                else
                {
                    graphics.DrawRectangle(Pens.Black, rect);
                    graphics.DrawString(menuItems[i].Name, font, Brushes.Black, 0, vOffset);
                }
            }
        }

        public override void OnDownButton()
        {
            selectedIndex++;
            if (selectedIndex >= menuItems.Length)
            {
                selectedIndex = 0;
            }
        }

        public override void OnUpButton()
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = menuItems.Length - 1;
            }
        }

        public override void OnAButton()
        {
            AppMenuItem selectedItem = menuItems[selectedIndex];
            mainForm.StartApp(selectedItem);
        }
    }
}
