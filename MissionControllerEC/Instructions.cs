﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Reflection;
using Contracts;
using KSP.UI.Screens;
using Contracts.Parameters;
using MissionControllerEC.MCEContracts;


namespace MissionControllerEC
{
    public partial class MissionControllerEC
    {
        public float resourceCost;
        public float vesselPartCost;
        public float vesseltons;
        public bool showFuel = false;
        public bool showTons = false;
        public bool showMiniTons = false;
        public float vesselResourceTons;
        public int currentContractType = 0;

        public int kerbalNumbers;
        public float kerbCost;
        public float KerbalFlatrate;
        public float kerbalMultiplier;

        public static int supplyCount;

        public static List<SupplyVesselList> SupVes = new List<SupplyVesselList>();

        public static List<string> CivName = new List<string>();

        Tools.MC2RandomWieghtSystem.Item<int>[] RandomRepairContractsCheck;
        Tools.MC2RandomWieghtSystem.Item<int>[] RandomSatelliteContractsCheck;
        
        public void loadTextures()
        {
            if (texture == null)
            {
                texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
                texture.LoadImage(File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MCEStockToolbar.png")));
            }
            if (texture2 == null)
            {
                texture2 = new Texture2D(36, 36, TextureFormat.RGBA32, false);
                texture2.LoadImage(File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "MCERevert.png")));
            }
            else { /*Debug.Log("MCE LoadTexture Failure"); */}
            //Debug.Log("MCE Textures Loaded");
        }

        public void loadFiles()
        {           
            if (settings.FileExists) { settings.Load(); settings.Save(); }
            else { settings.Save(); settings.Load(); }
            //Debug.Log("MCE Settings Loaded");
        }       

        public void CreateButtons()
        {
            if (HighLogic.LoadedScene == GameScenes.SPACECENTER && this.MCEButton == null)
            {
                this.MCEButton = ApplicationLauncher.Instance.AddModApplication(
                    this.MCEOn,
                    this.MCEOff,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.SPACECENTER,
                    texture
                    );
                //Debug.Log("Creating MCEButton Buttons");
            }
            
            if (HighLogic.LoadedScene == GameScenes.FLIGHT && this.MCERevert == null && settings.RevertOn)
            {
                this.MCERevert = ApplicationLauncher.Instance.AddModApplication(
                    this.revertOn,
                    this.revertOff,
                    null,
                    null,
                    null,
                    null,
                    ApplicationLauncher.AppScenes.FLIGHT,
                    texture2
                    );
                //Debug.Log("creating MCERevert Buttons");
            }
            else { /*Debug.Log("MCE2 CreateButtons Failed");*/ }
        }

        private void MCEOn()
        {           
            MCE_ScenarioStartup.ShowfinanaceWindow = true;
        }

        private void MCEOff()
        {           
            MCE_ScenarioStartup.ShowfinanaceWindow = false;
        }
       
        private void revertOff()
        {
            MCE_ScenarioStartup.ShowPopUpWindow3 = false;
        }
        private void revertOn()
        {
            MCE_ScenarioStartup.ShowPopUpWindow3 = true;
        }
        public void DestroyButtons()
        {
            if (this.MCEButton != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(this.MCEButton);
            }           
            if (this.MCERevert != null)
            {
                ApplicationLauncher.Instance.RemoveModApplication(this.MCERevert);
            }
            else { /*Debug.Log("MCE destroy buttons failed");*/ }
        }
               
        public void CheckRepairContractTypes(GameScenes gs)
        {
            randomContractsCheck();
            currentContractType = Tools.MC2RandomWieghtSystem.PickOne<int>(RandomRepairContractsCheck);
            Debug.LogWarning("Checking for Random Contracts Now MCE");
            if (currentContractType == 0)
            {
                SaveInfo.RepairContractGeneratedOn = false;               
                SaveInfo.RepairStationContractGeneratedOn = false;
                Debug.Log("No Repair Type contracts at this time");
            }            
            else if (currentContractType == 1)
            {
                SaveInfo.RepairContractGeneratedOn = true;             
                SaveInfo.RepairStationContractGeneratedOn = false;
                Debug.Log("Repair Contract Selected");
            }
            else if (currentContractType == 2)
            {
                SaveInfo.RepairContractGeneratedOn = false;
                SaveInfo.RepairStationContractGeneratedOn = true;
                Debug.Log("RepairStation Contract Selected");
            }           
            else
            {
                Debug.LogWarning("MCE failed to load random contract Check, defaulting");
            }
        }

        public void CheckRandomSatelliteContractTypes()
        {
            randomSatelliteContractsCheck();
            SaveInfo.SatelliteTypeChoice = Tools.MC2RandomWieghtSystem.PickOne<int>(RandomSatelliteContractsCheck); 
            Debug.LogWarning("Satellite Type Chosen Is Number " + SaveInfo.SatelliteTypeChoice);         
        }

        public void randomContractsCheck()
        {
            RandomRepairContractsCheck = new Tools.MC2RandomWieghtSystem.Item<int>[3];
            RandomRepairContractsCheck[0] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomRepairContractsCheck[0].weight = 10;
            RandomRepairContractsCheck[0].value = 0;

            RandomRepairContractsCheck[1] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomRepairContractsCheck[1].weight = 55;
            RandomRepairContractsCheck[1].value = 1;

            RandomRepairContractsCheck[2] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomRepairContractsCheck[2].weight = 35;
            RandomRepairContractsCheck[2].value = 2;
           
        }

        public void randomSatelliteContractsCheck()
        {
            RandomSatelliteContractsCheck = new Tools.MC2RandomWieghtSystem.Item<int>[6];
            RandomSatelliteContractsCheck[0] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[0].weight = 35;
            RandomSatelliteContractsCheck[0].value = 0;

            RandomSatelliteContractsCheck[1] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[1].weight = 35;
            RandomSatelliteContractsCheck[1].value = 1;

            RandomSatelliteContractsCheck[2] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[2].weight = 35;
            RandomSatelliteContractsCheck[2].value = 2;

            RandomSatelliteContractsCheck[3] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[3].weight = 35;
            RandomSatelliteContractsCheck[3].value = 3;

            RandomSatelliteContractsCheck[4] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[4].weight = 5;
            RandomSatelliteContractsCheck[4].value = 4;

            RandomSatelliteContractsCheck[5] = new Tools.MC2RandomWieghtSystem.Item<int>();
            RandomSatelliteContractsCheck[5].weight = 5;
            RandomSatelliteContractsCheck[5].value = 5; 
        }           

        public void onContractLoaded()
        {           
            if (settings.No_Rescue_Kerbal_Contracts && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.RecoverAsset)))          
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.RecoverAsset));
                    Debug.Log("Removed RescueKerbal Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run NoRescueKerbalContracts Returned Null");}
            }
            if (settings.No_Finprint_Satellite_Contracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.SatelliteContract)))
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.SatelliteContract));
                    Debug.Log("Removed FinePrint Satellite Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run FinePrint Satellite Returned Null"); }
            }
            if (settings.No_Fineprint_Base_Contracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.BaseContract)))
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.BaseContract));
                    Debug.Log("Removed FinePrint Satellite Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run FinePrint Satellite Returned Null"); }
            }
            if (settings.No_Fineprint_Station_Contracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.StationContract)))
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.StationContract));
                    Debug.Log("Removed FinePrint Satellite Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run FinePrint Satellite Returned Null"); }
            }
            if (settings.No_Fineprint_ISRU_Contracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.ISRUContract)))
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.ISRUContract));
                    Debug.Log("Removed FinePrint Satellite Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run FinePrint Satellite Returned Null"); }
            }
            if (settings.No_Fineprint_Tourism_Contracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.TourismContract)))
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.TourismContract));
                    Debug.Log("Removed FinePrint Satellite Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run FinePrint Satellite Returned Null"); }
            }
            if (settings.No_Fineprint_Survey_Contracts && ContractSystem.ContractTypes.Contains(typeof(FinePrint.Contracts.SurveyContract)))
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(FinePrint.Contracts.SurveyContract));
                    Debug.Log("Removed FinePrint Survey Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run FinePrint Survey Contracts Returned Null"); }
            }            
            if (settings.No_Part_Test_Contracts && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.PartTest)))
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.PartTest));
                    Debug.Log("Removed PartTest Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run NoPartTest Returned Null"); }
            }
            if (settings.No_GrandTour_Contracts && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.GrandTour)))
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.GrandTour));
                    Debug.Log("Removed PartTest Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run NoPartTest Returned Null"); }
            }
            if (settings.No_Explore_Body && ContractSystem.ContractTypes.Contains(typeof(Contracts.Templates.ExploreBody)))
            {
                try
                {
                    ContractSystem.ContractTypes.Remove(typeof(Contracts.Templates.ExploreBody));
                    Debug.Log("Removed PartTest Type Contracts from Gererating");
                }

                catch { Debug.LogError("could not run NoPartTest Returned Null"); }
            }
            Debug.Log("MCE Remove Contracts loaded"); 
        }    
                                   
        public void getSupplyList(bool stationOnly)
        {
            bool boolStation = stationOnly;
            supplyCount = 0;
            SupVes.Clear();

            if (!stationOnly)
            {
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    if (v.vesselType == VesselType.Station || v.vesselType == VesselType.Base)
                    {
                        SupVes.Add(new SupplyVesselList(v.name.Replace("(unloaded)", ""), v.id.ToString(), v.mainBody));
                        supplyCount++;
                        //Debug.Log("Found Vessel " + v.name + " " + v.vesselType + " Count is: " + supplyCount);
                    }
                    else { }
                }
            }
            else
            {
                foreach (Vessel v in FlightGlobals.Vessels)
                {
                    if (v.vesselType == VesselType.Station)
                    {
                        SupVes.Add(new SupplyVesselList(v.name.Replace("(unloaded)", ""), v.id.ToString(), v.mainBody));
                        supplyCount++;
                        //Debug.Log("Found Vessel " + v.name + " " + v.vesselType + " Count is: " + supplyCount);
                    }
                    else { }
                }
            }
        }
              
        public void GetLatandLonDefault(Vessel vessel)
        {
            double LatValue;
            LatValue = vessel.latitude;
            double LonValue;
            LonValue = vessel.longitude;

            SaveInfo.apolloLandingLat = LatValue;

            SaveInfo.apolloLandingLon = LonValue;
        }
        public void GetEvaTypeKerbal()
        {
            List<ProtoCrewMember> protoCrewMembers = FlightGlobals.ActiveVessel.GetVesselCrew();
            foreach (Experience.ExperienceEffect exp in protoCrewMembers[0].experienceTrait.Effects)
            {
                if (exp.ToString() == "Experience.Effects.RepairSkill")
                {
                    Debug.Log("Current kerbal is a Engineer you have passed");
                }
                else
                {
                    Debug.Log("Current kerbal is NOT an Engineer you don't pass... Bad boy!");
                }
            }
        }
        public void SetVesselLaunchCurrentTime()
        {
            Debug.Log("Old LaunchTime was: " + Tools.ConvertDays(FlightGlobals.ActiveVessel.launchTime));
            double currentTime = Planetarium.GetUniversalTime();
            FlightGlobals.ActiveVessel.launchTime = currentTime;
            Debug.Log("New LaunchTime Is: " + Tools.ConvertDays(FlightGlobals.ActiveVessel.launchTime));
        }
        //public void GetContractList()
        //{
        //    SaveInfo.KspContractList.Clear();
        //    if (ContractSystem.Instance != null)
        //    {
        //        foreach(Contract c in Contracts.ContractSystem.Instance.Contracts)
        //        {
        //            Type ContractType = c.GetType();
        //            if (c.ContractState == Contract.State.Offered || c.ContractState == Contract.State.Active)
        //            {
        //                SaveInfo.KspContractList.Add(new McContractList(ContractType.Name, false));
        //            }
        //            else { }
        //        }
                
        //    }
        //    else
        //    {
        //        Debug.Log("Contract Instance Not found, list not loading");
        //    }
        //    if (ContractSystem.Instance != false)
        //    {
        //        foreach (McContractList t in SaveInfo.KspContractList)
        //        {
        //            Debug.LogError("ContractList = " + t.ContractName + t.ContractDisabled);
        //        }
        //    }
        //    else { }
        //}
        public void GetLongAndLat(Vessel v)
        {
            double longitude;
            double latitude;
            longitude = v.longitude;
            latitude = v.latitude;
            Debug.LogError("Current longitude is " + longitude + " Current latitude is " + latitude);
        }
        
    }
   
    public class RepairVesselsList
    {
        public string vesselName;
        public string vesselId;
        public int bodyidx;
        public double MaxApA;
        public RepairVesselsList()
        {
        }
        public RepairVesselsList(string name, string id,double maxAP,int Vbody)
        {
            this.vesselName = name;
            this.vesselId = id;
            this.MaxApA = maxAP;
            this.bodyidx = Vbody;
        }


    }

    public class SupplyVesselList
    {
        public string vesselName;
        public string vesselId;
        public CelestialBody body;
        public SupplyVesselList()
        {
        }
        public SupplyVesselList(string name, string vId, CelestialBody bod)
        {
            this.vesselName = name;
            this.vesselId = vId;
            this.body = bod;
        }
    }
    public class McContractList
    {
        public string ContractName;
        public bool ContractDisabled = false;
        public McContractList() { }
        public McContractList(string ContName, bool ContDisabled)
        {
            this.ContractName = ContName;
            this.ContractDisabled = ContDisabled;
        }
        
    }
}
