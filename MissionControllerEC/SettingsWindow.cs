﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Text.RegularExpressions;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        private string difficulties;
        private string hireme;
        private string insuranceme;
        private void drawSettings(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            if (settings.difficutlylevel == 1)
            {
                difficulties = "Easy Mode";
            }
            if (settings.difficutlylevel == 2)
            {
                difficulties = "Medium Mode";
            }
            if (settings.difficutlylevel == 3)
            {
                difficulties = "HardCore Mode";
            }
            if (settings.difficutlylevel > 3 || settings.difficutlylevel < 1)
            {
                difficulties = "Number Not Accepted";
            }

            GUILayout.BeginHorizontal();
            GUILayout.Box("Current Difficulty Settings",MCE_ScenarioStartup.StyleBold);
            GUILayout.Box("" + difficulties, MCE_ScenarioStartup.styleBlueBold);
            GUILayout.EndHorizontal();

            GUILayout.Label("Difficulty Will require Restart If changed!",MCE_ScenarioStartup.styleGreenBold);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Easy Mode"))
            {
                settings.difficutlylevel = 1;
            }
            if (GUILayout.Button("Set Medium Mode"))
            {
                settings.difficutlylevel = 2;
            }
            if (GUILayout.Button("Set Hardcore Mode"))
            {
                settings.difficutlylevel = 3;
            }
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Hire Kerbal Cost",MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
            hireme = settings.HireCost.ToString();
            hireme = Regex.Replace(GUILayout.TextField(hireme), "[^.0-9]", "");                        
            settings.HireCost = double.Parse(hireme);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Kerbal Insurance Cost",MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
            insuranceme = settings.DeathInsurance.ToString();
            insuranceme = Regex.Replace(GUILayout.TextField(insuranceme), "[^.0-9]", "");           
            settings.DeathInsurance = double.Parse(insuranceme);
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Flight Help On (Apa,PeA,Orbital Period)",MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
            GUILayout.Box("" + settings.MessageHelpers, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(75));
            settings.MessageHelpers = GUILayout.Toggle(settings.MessageHelpers, "Set", GUILayout.Width(25));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Rescue Kerbal Contracts Off (Needs Restart)",MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
            GUILayout.Box("" + settings.NoRescueKerbalContracts, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(75));
            settings.NoRescueKerbalContracts = GUILayout.Toggle(settings.NoRescueKerbalContracts, "Set", GUILayout.Width(25));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Set MCE Revert On (Needs Restart)", MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
            GUILayout.Box("" + settings.RevertOn, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(75));
            settings.RevertOn = GUILayout.Toggle(settings.RevertOn, "Set Revert", GUILayout.Width(25));
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Set Debug Menu On", MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
            GUILayout.Box("" + settings.DebugMenu, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(75));
            settings.DebugMenu = GUILayout.Toggle(settings.DebugMenu, "Set Debug", GUILayout.Width(25));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            if (GUILayout.Button("Exit Save Settings"))
            {
                MCE_ScenarioStartup.ShowSettingsWindow = false;
                settings.Save();
                settings.Load();
            }

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
    }
}
