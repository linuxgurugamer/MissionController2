﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Reflection;
using Contracts;
using Contracts.Parameters;
using System.Text.RegularExpressions;

namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        # region initializers
        public bool comSatwin = false;
        public bool supplywin = false;
        public int count = 0;
        public int prCount = 1;
        private int DictCount = 7;
        private string resourceAmountString;
        #endregion
        #region Resources Dictionary
        public Dictionary<int, ResourceNames> resourceNamesInfo = new Dictionary<int, ResourceNames>();
        ResourceRefs rf = new ResourceRefs();
        public void LoadResourceDictionary()
        {
            resourceNamesInfo.Add(rf.ResourceName1.ID, rf.ResourceName1);
            resourceNamesInfo.Add(rf.ResourceName2.ID, rf.ResourceName2);
            resourceNamesInfo.Add(rf.ResourceName3.ID, rf.ResourceName3);
            resourceNamesInfo.Add(rf.ResourceName4.ID, rf.ResourceName4);
            resourceNamesInfo.Add(rf.ResourceName5.ID, rf.ResourceName5);
            resourceNamesInfo.Add(rf.ResourceName6.ID, rf.ResourceName6);
            resourceNamesInfo.Add(rf.ResourceName7.ID, rf.ResourceName7);
            //Debug.Log("MCE Loaded Resource Dictionary");
        }
        #endregion
        #region EditorWindow Ship Values
        public void drawEditorwindow(int id)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            MCE_ScenarioStartup.scrollPosition = GUILayout.BeginScrollView(MCE_ScenarioStartup.scrollPosition, GUILayout.Width(408));
            GetPartsCost();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            showFuel = (GUILayout.Toggle(showFuel, "Toggle Fuel Type Visible"));
            showTons = (GUILayout.Toggle(showTons, "Show Mass Vessel"));
            if (showTons)
                showMiniTons = (GUILayout.Toggle(showMiniTons, "Show Individual Mass(Kg) Parts"));
            
            if (GUILayout.Button("Exit Window And Save"))
            {
                MCE_ScenarioStartup.ShowEditorWindow = false;
            }

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
        #endregion
        #region finance Window
        public void drawFinanceWind(int id)
        {            
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();

            GUILayout.Space(2);
            GUILayout.BeginHorizontal();
            GUILayout.Box(MCE_ScenarioStartup.mainWindowTitle + " " + MCE_ScenarioStartup.versionCode, GUILayout.Width(400));

            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Spent Kerbals",GUILayout.Width(200));
            GUILayout.Box("" + (int)SaveInfo.TotalSpentKerbals, GUILayout.Width(200));
            GUILayout.EndHorizontal();            

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Total Spent RocketTest (Reverts)", GUILayout.Width(200));
            GUILayout.Box("" + (int)SaveInfo.TotalSpentOnRocketTest, GUILayout.Width(200));
            GUILayout.EndHorizontal();
                                            
            GUILayout.EndVertical();
            if (settings.DebugMenu && GUILayout.Button("Debug Menu"))
            {
               MCE_ScenarioStartup.ShowMainWindow = true;
            }
            if (GUILayout.Button("Custom Contracts"))
            {
                MCE_ScenarioStartup.ShowCustomWindow = true;
                getSupplyList();
            }
            if (GUILayout.Button("Settings Menu"))
            {
                MCE_ScenarioStartup.ShowSettingsWindow = true;
            }
            if (GUILayout.Button("Exit Window And Save"))
            {
                MCE_ScenarioStartup.ShowfinanaceWindow = false;
                settings.Save();
                settings.Load();
            }

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
        # endregion
        # region Custom GUI Window
        public void drawCustomGUI(int id)
        {
            CelestialBody targetbody = null;
            targetbody = FlightGlobals.Bodies[settings.bodyNumber];           
           
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            comSatwin = GUILayout.Toggle(comSatwin, "Edit ComSat Contract Values");
            supplywin = GUILayout.Toggle(supplywin, "Edit Supply Contract Values");
            
            #region ComSatValues
            if (comSatwin)
            {
                supplywin = false;
                GUILayout.Label("ComSat Network Information",MCE_ScenarioStartup.styleGreenBold);

                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                GUILayout.Box("ComSat Network Contracts On", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                GUILayout.Box("" + settings.StartBuilding,MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Turn On Contracts"))
                {
                    settings.StartBuilding = true;
                }
                if (GUILayout.Button("Turn Off Contracts"))
                {
                    settings.StartBuilding = false;
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("ComSat Max Orbital Period", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                GUILayout.Box("" + Tools.formatTime(settings.maxOrbP), MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("- Hour", GUILayout.Width(75))) { settings.maxOrbP -= 3600; }
                if (GUILayout.Button("+ Hour", GUILayout.Width(75))) { settings.maxOrbP += 3600; }
                if (GUILayout.Button("- Minute", GUILayout.Width(75))) { settings.maxOrbP -= 60; }
                if (GUILayout.Button("+ Minute", GUILayout.Width(75))) { settings.maxOrbP += 60; }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("ComSat Min Orbital Period", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                GUILayout.Box("" + Tools.formatTime(settings.minOrbP), MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("- Hour", GUILayout.Width(75))) { settings.minOrbP -= 3600; }
                if (GUILayout.Button("+ Hour", GUILayout.Width(75))) { settings.minOrbP += 3600; }
                if (GUILayout.Button("- Minute", GUILayout.Width(75))) { settings.minOrbP -= 60; }
                if (GUILayout.Button("+ Minute", GUILayout.Width(75))) { settings.minOrbP += 60; }
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("ComSat Body", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                GUILayout.Box("" + targetbody.theName, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("- Previous"))
                {
                    settings.bodyNumber--;
                    if (settings.bodyNumber < 1 || settings.bodyNumber > 16)
                    {
                        settings.bodyNumber = 1;
                    }
                }

                if (GUILayout.Button("+ Next"))
                {
                    settings.bodyNumber++;
                    if (settings.bodyNumber > 16 || settings.bodyNumber < 1)
                    {
                        settings.bodyNumber = 1;
                    }

                }

                GUILayout.EndHorizontal();
                
                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Box("Contract Name", MCE_ScenarioStartup.StyleBold, GUILayout.Width(150), GUILayout.Height(30));
                settings.contractName = GUILayout.TextField(settings.contractName, 50);
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
            }
            #endregion
            #region Supply Contract Values
            if (supplywin)
            {
                comSatwin = false;            
                GUILayout.Label("Supply Contract Information", MCE_ScenarioStartup.styleGreenBold);

                if (supplyCount > 0)
                {
                    SaveInfo.SupplyVesName = SupVes[count].vesselName; 
                    SaveInfo.SupplyVesId = SupVes[count].vesselId.ToString();
                    SaveInfo.SupplyBodyIDX = SupVes[count].body.flightGlobalsIndex;

                    ResourceNames Rname = resourceNamesInfo[prCount];
                    SaveInfo.ResourceName = Rname.Name;

                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Supply Contracts On", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + SaveInfo.supplyContractOn, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Turn On Contracts"))
                    {
                        SaveInfo.supplyContractOn = true;
                    }
                    if (GUILayout.Button("Turn Off Contracts"))
                    {
                        SaveInfo.supplyContractOn = false;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Select Vessel To Supply", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + SaveInfo.SupplyVesName, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("- Previous"))
                    {
                        count--;
                        if (count < 1 || count > (supplyCount - 1))
                        {
                            count = 0;
                        }
                    }
                    if (GUILayout.Button("+ Next"))
                    {
                        count++;
                        if (count > (supplyCount - 1) || count < 1)
                        {
                            count = 0;
                        }
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);


                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Select Resource To Supply", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + SaveInfo.ResourceName, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("- Previous"))
                    {
                        prCount--;
                        if (prCount < 1 || prCount > DictCount)
                        {
                            prCount = 1;
                        }
                    }

                    if (GUILayout.Button("+ Next"))
                    {
                        prCount++;
                        if (prCount > DictCount || prCount < 1)
                        {
                            prCount = 1;
                        }
                    }
                    GUILayout.Space(15);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Set Amount Of " + SaveInfo.ResourceName, MCE_ScenarioStartup.StyleBold, GUILayout.Width(300));
                    resourceAmountString = SaveInfo.supplyAmount.ToString();
                    resourceAmountString = Regex.Replace(GUILayout.TextField(resourceAmountString), "[^.0-9]", "");
                    SaveInfo.supplyAmount = double.Parse(resourceAmountString);
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Vessel Is Located At Body", MCE_ScenarioStartup.StyleBold, GUILayout.Width(200));
                    GUILayout.Box("" + FlightGlobals.Bodies[SaveInfo.SupplyBodyIDX].theName, MCE_ScenarioStartup.styleBlueBold, GUILayout.Width(200));
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                    GUILayout.BeginHorizontal();
                    GUILayout.Box("Contract Name", MCE_ScenarioStartup.StyleBold, GUILayout.Width(150), GUILayout.Height(30));
                    SaveInfo.SupplyContractName = GUILayout.TextField(SaveInfo.SupplyContractName, 50);
                    GUILayout.EndHorizontal();

                    GUILayout.Space(15);
                }
                else
                    GUILayout.Label("You have no Vessels of Type Station or Base in space This contract Only works On those Types Vessels!",MCE_ScenarioStartup.StyleBold);

            }
            # endregion
            GUILayout.EndVertical();
            if (GUILayout.Button("Exit Window"))
            {
                MCE_ScenarioStartup.ShowCustomWindow = false;              
            }

            if (!Input.GetMouseButtonDown(1))
            {
                GUI.DragWindow();
            }
        }
        #endregion
    }
    #region extra Resources Classes
    public class ResourceNames
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    class ResourceRefs
    {
        public ResourceNames ResourceName1 = new ResourceNames()
        {
            ID = 1,
            Name = "LiquidFuel",
        };
        public ResourceNames ResourceName2 = new ResourceNames()
        {
            ID = 2,
            Name = "Oxidizer",
        };
        public ResourceNames ResourceName3 = new ResourceNames()
        {
            ID = 3,
            Name = "MonoPropellant",
        };
        public ResourceNames ResourceName4 = new ResourceNames()
        {
            ID = 4,
            Name = "XenonGas",
        };
        public ResourceNames ResourceName5 = new ResourceNames()
        {
            ID = 5,
            Name = "Food",
        };
        public ResourceNames ResourceName6 = new ResourceNames()
        {
            ID = 6,
            Name = "Water",
        };
        public ResourceNames ResourceName7 = new ResourceNames()
        {
            ID = 7,
            Name = "Oxygen",
        };
    }
# endregion
}
