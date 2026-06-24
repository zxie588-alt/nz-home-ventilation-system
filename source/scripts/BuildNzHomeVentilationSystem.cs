using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using SolidWorks.Interop.sldworks;

public static class BuildNzHomeVentilationSystem
{
    private const string PartTemplatePath = @"C:\ProgramData\SOLIDWORKS\SOLIDWORKS 2018\templates\gb_part.prtdot";
    private const double DuctZ = 0.65;
    private const double DiffuserZ = 0.43;
    private const double CeilingZ = 0.95;

    private static SldWorks _swApp;
    private static ModelDoc2 _model;
    private static PartDoc _part;
    private static Modeler _modeler;
    private static readonly List<ComponentRecord> _components = new List<ComponentRecord>();

    private static readonly Material Floor = new Material("Floor", 0.82, 0.84, 0.83, 0.10);
    private static readonly Material Wall = new Material("Walls", 0.88, 0.88, 0.84, 0.00);
    private static readonly Material CeilingRef = new Material("Ceiling reference", 0.70, 0.88, 0.96, 0.45);
    private static readonly Material Steel = new Material("Galvanised steel", 0.60, 0.64, 0.66, 0.00);
    private static readonly Material DarkSteel = new Material("Dark steel", 0.15, 0.17, 0.18, 0.00);
    private static readonly Material Supply = new Material("Supply air - blue", 0.02, 0.33, 0.86, 0.00);
    private static readonly Material Extract = new Material("Extract air - orange", 0.95, 0.40, 0.08, 0.00);
    private static readonly Material FreshAir = new Material("Outdoor intake - green", 0.08, 0.62, 0.30, 0.00);
    private static readonly Material Exhaust = new Material("Exhaust air - grey", 0.46, 0.50, 0.54, 0.00);
    private static readonly Material Hanger = new Material("Hangers", 0.08, 0.09, 0.10, 0.00);

    public static int Main()
    {
        AppDomain.CurrentDomain.AssemblyResolve += ResolveSolidWorksInterop;
        try
        {
            return Run();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("ExceptionType=" + ex.GetType().FullName);
            Console.Error.WriteLine("HResult=0x" + ex.HResult.ToString("X8"));
            Console.Error.WriteLine("Message=" + ex.Message);
            return 99;
        }
    }

    private static Assembly ResolveSolidWorksInterop(object sender, ResolveEventArgs args)
    {
        string name = new AssemblyName(args.Name).Name + ".dll";
        string path = Path.Combine(@"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\api\redist", name);
        return File.Exists(path) ? Assembly.LoadFrom(path) : null;
    }

    private static int Run()
    {
        string scriptDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectDir = new DirectoryInfo(scriptDir).Parent.FullName;
        string swDir = Path.Combine(projectDir, "01_SolidWorks");
        string exportDir = Path.Combine(projectDir, "02_Exports");
        string docDir = Path.Combine(projectDir, "04_Docs");
        string imageDir = Path.Combine(projectDir, "05_Images");
        Directory.CreateDirectory(swDir);
        Directory.CreateDirectory(exportDir);
        Directory.CreateDirectory(docDir);
        Directory.CreateDirectory(imageDir);

        string partPath = Path.Combine(swDir, "NZ_Home_Ventilation_System.SLDPRT");
        string stepPath = Path.Combine(exportDir, "NZ_Home_Ventilation_System.step");
        string stlPath = Path.Combine(exportDir, "NZ_Home_Ventilation_System.stl");
        string pngPath = Path.Combine(imageDir, "NZ_Home_Ventilation_System_Isometric.png");
        string csvPath = Path.Combine(docDir, "Component_List.csv");

        DeleteIfExists(partPath);
        DeleteIfExists(stepPath);
        DeleteIfExists(stlPath);
        DeleteIfExists(pngPath);
        DeleteIfExists(csvPath);

        Console.WriteLine("Starting SOLIDWORKS...");
        Type swType = Type.GetTypeFromProgID("SldWorks.Application");
        _swApp = (SldWorks)Activator.CreateInstance(swType);
        _swApp.Visible = true;

        _model = (ModelDoc2)_swApp.NewDocument(PartTemplatePath, 0, 0, 0);
        if (_model == null)
        {
            Console.Error.WriteLine("Could not create SOLIDWORKS part document.");
            return 1;
        }
        _part = (PartDoc)_model;
        _modeler = (Modeler)_swApp.GetModeler();

        Console.WriteLine("Building model geometry...");
        BuildHouseReference();
        BuildMvhrUnit();
        BuildSupplySystem();
        BuildExtractSystem();
        BuildOutdoorTerminals();
        BuildCeilingReference();

        _model.ForceRebuild3(false);
        TrySetView();

        Console.WriteLine("Saving CAD files...");
        bool partOk = SaveAs(partPath, "SLDPRT");
        bool stepOk = SaveAs(stepPath, "STEP");
        bool stlOk = SaveAs(stlPath, "STL");
        bool pngOk = SaveAs(pngPath, "PNG");
        WriteComponentList(csvPath);

        _swApp.CloseDoc(_model.GetTitle());
        _swApp.ExitApp();

        Console.WriteLine("Part=" + partOk + " " + partPath);
        Console.WriteLine("STEP=" + stepOk + " " + stepPath);
        Console.WriteLine("STL=" + stlOk + " " + stlPath);
        Console.WriteLine("PNG=" + pngOk + " " + pngPath);
        Console.WriteLine("Components=" + _components.Count);

        return partOk ? 0 : 2;
    }

    private static void BuildHouseReference()
    {
        AddBox("Floor_Slab_12m_x_8m", "Architectural reference", "House", V(6.0, 4.0, -0.025), V(12.0, 8.0, 0.05), Floor, "Low-rise floor plate, 12 m x 8 m");

        AddWall("Exterior_Wall_South", 0, 0, 12, 0, 0.12, 0.16);
        AddWall("Exterior_Wall_North", 0, 8, 12, 8, 0.12, 0.16);
        AddWall("Exterior_Wall_West", 0, 0, 0, 8, 0.12, 0.16);
        AddWall("Exterior_Wall_East", 12, 0, 12, 8, 0.12, 0.16);

        AddWall("Interior_Wall_Living_To_Service_A", 7.2, 0.0, 7.2, 3.0, 0.08, 0.14);
        AddWall("Interior_Wall_Living_To_Service_B", 7.2, 4.0, 7.2, 8.0, 0.08, 0.14);
        AddWall("Interior_Wall_Living_Kitchen", 0.0, 4.6, 5.0, 4.6, 0.08, 0.14);
        AddWall("Interior_Wall_Kitchen_Service", 6.0, 4.6, 7.2, 4.6, 0.08, 0.14);
        AddWall("Interior_Wall_Bedroom_1", 7.2, 2.7, 12.0, 2.7, 0.08, 0.14);
        AddWall("Interior_Wall_Bath_Laundry", 7.2, 5.6, 12.0, 5.6, 0.08, 0.14);
        AddWall("Interior_Wall_Bedroom_Divider", 9.6, 0.0, 9.6, 5.6, 0.08, 0.14);
        AddWall("Interior_Wall_Service_Divider", 10.4, 5.6, 10.4, 8.0, 0.08, 0.14);
    }

    private static void BuildMvhrUnit()
    {
        AddBox("MVHR_Main_Unit_Casing", "MVHR unit", "Equipment", V(10.65, 6.75, 0.62), V(1.15, 0.75, 0.35), Steel, "Compact MVHR/ERV unit in ceiling service zone");
        AddBox("MVHR_Service_Access_Panel", "MVHR unit", "Equipment", V(10.65, 6.75, 0.805), V(1.02, 0.62, 0.025), DarkSteel, "Removable filter/service access panel");
        AddBox("MVHR_Filter_Drawer_1", "MVHR unit", "Equipment", V(10.36, 6.75, 0.82), V(0.08, 0.60, 0.04), DarkSteel, "Filter cassette detail");
        AddBox("MVHR_Filter_Drawer_2", "MVHR unit", "Equipment", V(10.94, 6.75, 0.82), V(0.08, 0.60, 0.04), DarkSteel, "Filter cassette detail");

        AddCollar("MVHR_Supply_Port", V(10.05, 6.45, DuctZ), V(-1, 0, 0), 0.12, 0.28, Supply, "Round supply outlet on MVHR unit");
        AddCollar("MVHR_Extract_Port", V(10.05, 7.05, DuctZ), V(-1, 0, 0), 0.12, 0.28, Extract, "Round extract inlet on MVHR unit");
        AddCollar("MVHR_Fresh_Air_Port", V(11.25, 7.05, DuctZ), V(1, 0, 0), 0.12, 0.28, FreshAir, "Outdoor intake inlet");
        AddCollar("MVHR_Exhaust_Port", V(11.25, 6.45, DuctZ), V(1, 0, 0), 0.12, 0.28, Exhaust, "Outdoor exhaust outlet");
    }

    private static void BuildSupplySystem()
    {
        double mainR = 0.09;
        double branchR = 0.065;

        AddDuct("SUP_Main_Trunk_01_Unit_To_Hall", V(10.05, 6.45, DuctZ), V(7.20, 6.45, DuctZ), mainR, Supply, "Main supply trunk from MVHR to hallway");
        AddDuct("SUP_Main_Trunk_02_Hall_Run", V(7.20, 6.45, DuctZ), V(7.20, 1.10, DuctZ), mainR, Supply, "Main supply trunk along central hallway");

        AddHub("SUP_Elbow_Unit_Hall", V(7.20, 6.45, DuctZ), 0.12, Supply, "90 degree elbow with inspection collar");
        AddHub("SUP_Tee_Living_North", V(7.20, 6.00, DuctZ), 0.105, Supply, "T-branch to living area diffuser");
        AddHub("SUP_Tee_Living_South", V(7.20, 2.00, DuctZ), 0.105, Supply, "T-branch to second living area diffuser");
        AddHub("SUP_Tee_Bedroom_2", V(7.20, 3.50, DuctZ), 0.105, Supply, "T-branch to bedroom diffuser");
        AddHub("SUP_Tee_Bedroom_1", V(7.20, 1.10, DuctZ), 0.105, Supply, "T-branch to bedroom diffuser");

        AddDuct("SUP_Branch_Living_North", V(7.20, 6.00, DuctZ), V(3.20, 6.00, DuctZ), branchR, Supply, "Branch to open-plan living/dining");
        AddDuct("SUP_Branch_Living_South", V(7.20, 2.00, DuctZ), V(3.00, 2.00, DuctZ), branchR, Supply, "Branch to lounge zone");
        AddDuct("SUP_Branch_Bedroom_2", V(7.20, 3.50, DuctZ), V(10.40, 3.50, DuctZ), branchR, Supply, "Branch to bedroom 2");
        AddDuct("SUP_Branch_Bedroom_1", V(7.20, 1.10, DuctZ), V(10.60, 1.10, DuctZ), branchR, Supply, "Branch to bedroom 1");

        AddRoundDiffuser("SUP_Diffuser_Living_North", V(3.20, 6.00, DiffuserZ), Supply, "Ceiling supply diffuser for living/dining");
        AddRoundDiffuser("SUP_Diffuser_Living_South", V(3.00, 2.00, DiffuserZ), Supply, "Ceiling supply diffuser for lounge");
        AddRoundDiffuser("SUP_Diffuser_Bedroom_2", V(10.40, 3.50, DiffuserZ), Supply, "Ceiling supply diffuser for bedroom 2");
        AddRoundDiffuser("SUP_Diffuser_Bedroom_1", V(10.60, 1.10, DiffuserZ), Supply, "Ceiling supply diffuser for bedroom 1");

        AddArrow("SUP_Flow_Arrow_Main", V(9.15, 6.45, DuctZ + 0.16), V(8.40, 6.45, DuctZ + 0.16), Supply);
        AddArrow("SUP_Flow_Arrow_Hall", V(7.20, 5.40, DuctZ + 0.16), V(7.20, 4.55, DuctZ + 0.16), Supply);
        AddArrow("SUP_Flow_Arrow_Living", V(5.80, 2.00, DuctZ + 0.13), V(4.95, 2.00, DuctZ + 0.13), Supply);
        AddArrow("SUP_Flow_Arrow_Bedroom", V(8.70, 1.10, DuctZ + 0.13), V(9.55, 1.10, DuctZ + 0.13), Supply);

        AddHanger("SUP_Hanger_Main_01", V(8.80, 6.45, DuctZ), true);
        AddHanger("SUP_Hanger_Main_02", V(7.20, 4.80, DuctZ), false);
        AddHanger("SUP_Hanger_Main_03", V(7.20, 2.70, DuctZ), false);
        AddHanger("SUP_Hanger_Living", V(5.20, 2.00, DuctZ), true);
    }

    private static void BuildExtractSystem()
    {
        double mainR = 0.085;
        double branchR = 0.06;

        AddDuct("EXT_Main_Trunk_01_Unit_To_Hall", V(10.05, 7.05, DuctZ), V(7.20, 7.05, DuctZ), mainR, Extract, "Main extract trunk back to MVHR");
        AddDuct("EXT_Main_Trunk_02_Service_Run", V(7.20, 7.05, DuctZ), V(7.20, 4.10, DuctZ), mainR, Extract, "Extract trunk through service zone");

        AddHub("EXT_Elbow_Unit_Hall", V(7.20, 7.05, DuctZ), 0.115, Extract, "90 degree elbow with inspection collar");
        AddHub("EXT_Tee_Kitchen", V(7.20, 5.40, DuctZ), 0.10, Extract, "T-branch to kitchen extract grille");
        AddHub("EXT_Tee_Bathroom", V(7.20, 4.10, DuctZ), 0.10, Extract, "T-branch to bathroom extract grille");
        AddHub("EXT_Tee_Laundry", V(10.05, 6.25, DuctZ), 0.10, Extract, "Short laundry extract branch");

        AddDuct("EXT_Branch_Kitchen", V(7.20, 5.40, DuctZ), V(4.80, 5.40, DuctZ), branchR, Extract, "Branch to kitchen extract point");
        AddDuct("EXT_Branch_Bathroom", V(7.20, 4.10, DuctZ), V(9.05, 4.10, DuctZ), branchR, Extract, "Branch to bathroom extract grille");
        AddDuct("EXT_Branch_Laundry", V(10.05, 6.25, DuctZ), V(10.95, 6.25, DuctZ), branchR, Extract, "Short branch to laundry extract");

        AddRectGrille("EXT_Grille_Kitchen", V(4.80, 5.40, DiffuserZ), Extract, "Rectangular kitchen extract grille");
        AddRectGrille("EXT_Grille_Bathroom", V(9.05, 4.10, DiffuserZ), Extract, "Bathroom extract grille");
        AddRectGrille("EXT_Grille_Laundry", V(10.95, 6.25, DiffuserZ), Extract, "Laundry extract grille");

        AddArrow("EXT_Flow_Arrow_Kitchen", V(5.25, 5.40, DuctZ + 0.13), V(6.10, 5.40, DuctZ + 0.13), Extract);
        AddArrow("EXT_Flow_Arrow_Service", V(7.20, 4.70, DuctZ + 0.16), V(7.20, 5.55, DuctZ + 0.16), Extract);
        AddArrow("EXT_Flow_Arrow_Main", V(8.25, 7.05, DuctZ + 0.16), V(9.10, 7.05, DuctZ + 0.16), Extract);

        AddHanger("EXT_Hanger_Main_01", V(8.80, 7.05, DuctZ), true);
        AddHanger("EXT_Hanger_Main_02", V(7.20, 5.35, DuctZ), false);
        AddHanger("EXT_Hanger_Kitchen", V(5.60, 5.40, DuctZ), true);
    }

    private static void BuildOutdoorTerminals()
    {
        AddDuct("OA_Fresh_Air_Intake_Duct", V(11.25, 7.05, DuctZ), V(12.30, 7.05, DuctZ), 0.09, FreshAir, "Outdoor air intake through exterior wall");
        AddDuct("EA_Exhaust_Air_Duct", V(11.25, 6.45, DuctZ), V(12.30, 6.45, DuctZ), 0.09, Exhaust, "Exhaust air through exterior wall");

        AddCollar("OA_Intake_Wall_Flange", V(12.10, 7.05, DuctZ), V(1, 0, 0), 0.13, 0.12, FreshAir, "Exterior wall intake flange");
        AddCollar("EA_Exhaust_Wall_Flange", V(12.10, 6.45, DuctZ), V(1, 0, 0), 0.13, 0.12, Exhaust, "Exterior wall exhaust flange");
        AddBox("OA_Intake_Rain_Hood", "Outdoor terminal", "Fresh air", V(12.43, 7.05, DuctZ + 0.02), V(0.18, 0.42, 0.28), FreshAir, "Simplified louvred weather hood");
        AddBox("EA_Exhaust_Rain_Hood", "Outdoor terminal", "Exhaust air", V(12.43, 6.45, DuctZ + 0.02), V(0.18, 0.42, 0.28), Exhaust, "Simplified exhaust weather hood");

        AddArrow("OA_Flow_Arrow_Intake", V(12.55, 7.05, DuctZ + 0.16), V(11.85, 7.05, DuctZ + 0.16), FreshAir);
        AddArrow("EA_Flow_Arrow_Exhaust", V(11.85, 6.45, DuctZ + 0.16), V(12.55, 6.45, DuctZ + 0.16), Exhaust);
    }

    private static void BuildCeilingReference()
    {
        AddBox("Ceiling_Service_Zone_North_Rail", "Ceiling reference", "Reference", V(6.0, 8.12, CeilingZ), V(12.0, 0.035, 0.035), CeilingRef, "Ceiling service-zone reference rail");
        AddBox("Ceiling_Service_Zone_South_Rail", "Ceiling reference", "Reference", V(6.0, -0.12, CeilingZ), V(12.0, 0.035, 0.035), CeilingRef, "Ceiling service-zone reference rail");
        AddBox("Ceiling_Service_Zone_West_Rail", "Ceiling reference", "Reference", V(-0.12, 4.0, CeilingZ), V(0.035, 8.0, 0.035), CeilingRef, "Ceiling service-zone reference rail");
        AddBox("Ceiling_Service_Zone_East_Rail", "Ceiling reference", "Reference", V(12.12, 4.0, CeilingZ), V(0.035, 8.0, 0.035), CeilingRef, "Ceiling service-zone reference rail");
    }

    private static void AddWall(string name, double x1, double y1, double x2, double y2, double thickness, double height)
    {
        double cx = (x1 + x2) / 2.0;
        double cy = (y1 + y2) / 2.0;
        double dx = Math.Abs(x2 - x1);
        double dy = Math.Abs(y2 - y1);
        if (dx < 0.001) dx = thickness;
        if (dy < 0.001) dy = thickness;
        AddBox(name, "Wall", "Architectural reference", V(cx, cy, height / 2.0), V(dx, dy, height), Wall, "Low wall reference segment");
    }

    private static void AddDuct(string name, Vec start, Vec end, double radius, Material material, string notes)
    {
        AddCylinder(name, "Round duct", material.Name, start, end, radius, material, notes);
        Vec dir = (end - start).Normalized();
        AddCollar(name + "_Start_Collar", start + dir * 0.07, dir, radius * 1.15, 0.08, material, "Joint collar");
        AddCollar(name + "_End_Collar", end - dir * 0.07, dir, radius * 1.15, 0.08, material, "Joint collar");
    }

    private static void AddHub(string name, Vec center, double radius, Material material, string notes)
    {
        AddCylinder(name, "Duct fitting", material.Name, V(center.X, center.Y, center.Z - 0.045), V(center.X, center.Y, center.Z + 0.045), radius, material, notes);
    }

    private static void AddCollar(string name, Vec center, Vec direction, double radius, double length, Material material, string notes)
    {
        Vec dir = direction.Normalized();
        AddCylinder(name, "Collar/flange", material.Name, center - dir * (length / 2.0), center + dir * (length / 2.0), radius, material, notes);
    }

    private static void AddRoundDiffuser(string name, Vec center, Material material, string notes)
    {
        AddCylinder(name + "_Face", "Round diffuser", material.Name, V(center.X, center.Y, center.Z - 0.02), V(center.X, center.Y, center.Z + 0.02), 0.22, material, notes);
        AddCylinder(name + "_Neck", "Diffuser neck", material.Name, V(center.X, center.Y, center.Z + 0.02), V(center.X, center.Y, DuctZ), 0.065, material, "Vertical drop from duct to diffuser");
        AddBox(name + "_Slot_A", "Diffuser grille detail", material.Name, V(center.X, center.Y - 0.07, center.Z + 0.035), V(0.32, 0.018, 0.018), DarkSteel, "Linear grille slot");
        AddBox(name + "_Slot_B", "Diffuser grille detail", material.Name, V(center.X, center.Y, center.Z + 0.035), V(0.34, 0.018, 0.018), DarkSteel, "Linear grille slot");
        AddBox(name + "_Slot_C", "Diffuser grille detail", material.Name, V(center.X, center.Y + 0.07, center.Z + 0.035), V(0.32, 0.018, 0.018), DarkSteel, "Linear grille slot");
    }

    private static void AddRectGrille(string name, Vec center, Material material, string notes)
    {
        AddBox(name + "_Face", "Rectangular extract grille", material.Name, V(center.X, center.Y, center.Z), V(0.52, 0.34, 0.04), material, notes);
        AddCylinder(name + "_Neck", "Grille neck", material.Name, V(center.X, center.Y, center.Z + 0.02), V(center.X, center.Y, DuctZ), 0.06, material, "Vertical drop from duct to grille");
        AddBox(name + "_Louvre_1", "Grille louvre detail", material.Name, V(center.X, center.Y - 0.09, center.Z + 0.04), V(0.44, 0.018, 0.025), DarkSteel, "Extract grille louvre");
        AddBox(name + "_Louvre_2", "Grille louvre detail", material.Name, V(center.X, center.Y, center.Z + 0.04), V(0.44, 0.018, 0.025), DarkSteel, "Extract grille louvre");
        AddBox(name + "_Louvre_3", "Grille louvre detail", material.Name, V(center.X, center.Y + 0.09, center.Z + 0.04), V(0.44, 0.018, 0.025), DarkSteel, "Extract grille louvre");
    }

    private static void AddHanger(string name, Vec ductCenter, bool ductRunsAlongX)
    {
        double offset = 0.16;
        if (ductRunsAlongX)
        {
            AddCylinder(name + "_Rod_A", "Duct hanger", "Support", V(ductCenter.X, ductCenter.Y - offset, ductCenter.Z + 0.10), V(ductCenter.X, ductCenter.Y - offset, CeilingZ), 0.012, Hanger, "Threaded hanger rod");
            AddCylinder(name + "_Rod_B", "Duct hanger", "Support", V(ductCenter.X, ductCenter.Y + offset, ductCenter.Z + 0.10), V(ductCenter.X, ductCenter.Y + offset, CeilingZ), 0.012, Hanger, "Threaded hanger rod");
            AddBox(name + "_Cross_Bracket", "Duct hanger", "Support", V(ductCenter.X, ductCenter.Y, ductCenter.Z - 0.11), V(0.035, 0.42, 0.035), Hanger, "Duct support cross bracket");
        }
        else
        {
            AddCylinder(name + "_Rod_A", "Duct hanger", "Support", V(ductCenter.X - offset, ductCenter.Y, ductCenter.Z + 0.10), V(ductCenter.X - offset, ductCenter.Y, CeilingZ), 0.012, Hanger, "Threaded hanger rod");
            AddCylinder(name + "_Rod_B", "Duct hanger", "Support", V(ductCenter.X + offset, ductCenter.Y, ductCenter.Z + 0.10), V(ductCenter.X + offset, ductCenter.Y, CeilingZ), 0.012, Hanger, "Threaded hanger rod");
            AddBox(name + "_Cross_Bracket", "Duct hanger", "Support", V(ductCenter.X, ductCenter.Y, ductCenter.Z - 0.11), V(0.42, 0.035, 0.035), Hanger, "Duct support cross bracket");
        }
    }

    private static void AddArrow(string name, Vec start, Vec end, Material material)
    {
        Vec delta = end - start;
        double length = delta.Length();
        if (length < 0.25) return;
        Vec dir = delta.Normalized();
        double headLength = 0.16;
        Vec shaftEnd = end - dir * headLength;
        AddCylinder(name + "_Shaft", "Airflow arrow", material.Name, start, shaftEnd, 0.025, material, "Flow direction arrow shaft");
        AddCone(name + "_Head", shaftEnd, dir, 0.075, 0.006, headLength, material, "Flow direction arrow head");
    }

    private static void AddBox(string name, string category, string system, Vec center, Vec size, Material material, string notes)
    {
        double[] def = { center.X, center.Y, center.Z - size.Z / 2.0, 0, 0, 1, size.X, size.Y, size.Z };
        Body2 body = (Body2)_modeler.CreateBodyFromBox((object)def);
        AddBody(body, name, category, system, material, notes);
    }

    private static void AddCylinder(string name, string category, string system, Vec start, Vec end, double radius, Material material, string notes)
    {
        Vec delta = end - start;
        double length = delta.Length();
        if (length <= 0.0001) return;
        Vec axis = delta.Normalized();
        double[] def = { start.X, start.Y, start.Z, axis.X, axis.Y, axis.Z, radius, length };
        Body2 body = (Body2)_modeler.CreateBodyFromCyl((object)def);
        AddBody(body, name, category, system, material, notes);
    }

    private static void AddCone(string name, Vec baseCenter, Vec direction, double baseRadius, double topRadius, double height, Material material, string notes)
    {
        Vec axis = direction.Normalized();
        double[] def = { baseCenter.X, baseCenter.Y, baseCenter.Z, axis.X, axis.Y, axis.Z, baseRadius, topRadius, height };
        Body2 body = (Body2)_modeler.CreateBodyFromCone((object)def);
        AddBody(body, name, "Airflow arrow", material.Name, material, notes);
    }

    private static void AddBody(Body2 body, string name, string category, string system, Material material, string notes)
    {
        if (body == null)
        {
            throw new InvalidOperationException("Failed to create body: " + name);
        }

        body.Name = name;
        if (material != null)
        {
            body.MaterialPropertyValues2 = material.Values;
        }

        Feature feature = (Feature)_part.CreateFeatureFromBody3(body, false, 0);
        if (feature == null)
        {
            throw new InvalidOperationException("Failed to insert body as feature: " + name);
        }
        feature.Name = name;
        if (material != null)
        {
            try
            {
                feature.SetMaterialPropertyValues2(material.Values, 2, null);
            }
            catch
            {
                // Body-level color is already set; feature-level color is a display enhancement.
            }
        }
        _components.Add(new ComponentRecord(name, category, system, material == null ? "" : material.Name, notes));
    }

    private static bool SaveAs(string path, string label)
    {
        int errors = 0;
        int warnings = 0;
        bool ok = _model.Extension.SaveAs(path, 0, 1, null, ref errors, ref warnings);
        bool exists = File.Exists(path);
        Console.WriteLine(label + " SaveOk=" + ok + " Errors=" + errors + " Warnings=" + warnings + " Exists=" + exists);
        return ok && exists;
    }

    private static void TrySetView()
    {
        try
        {
            _model.ShowNamedView2("*Isometric", 7);
            ModelView view = (ModelView)_model.ActiveView;
            if (view != null)
            {
                view.DisplayMode = 5;
                view.EnableGraphicsUpdate = true;
            }
            _model.ViewZoomtofit2();
            _model.GraphicsRedraw2();
        }
        catch
        {
            // View setup is helpful for preview exports, but the model is valid without it.
        }
    }

    private static void WriteComponentList(string path)
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("FeatureName,Category,System,Material,Notes");
            foreach (ComponentRecord item in _components)
            {
                writer.WriteLine(Csv(item.FeatureName) + "," + Csv(item.Category) + "," + Csv(item.System) + "," + Csv(item.Material) + "," + Csv(item.Notes));
            }
        }
    }

    private static string Csv(string value)
    {
        if (value == null) value = "";
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }

    private static void DeleteIfExists(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private static Vec V(double x, double y, double z)
    {
        return new Vec(x, y, z);
    }

    private sealed class Material
    {
        public readonly string Name;
        public readonly double[] Values;

        public Material(string name, double r, double g, double b, double transparency)
        {
            Name = name;
            Values = new[] { r, g, b, 0.35, 0.80, 0.25, 0.25, transparency, 0.0 };
        }
    }

    private sealed class ComponentRecord
    {
        public readonly string FeatureName;
        public readonly string Category;
        public readonly string System;
        public readonly string Material;
        public readonly string Notes;

        public ComponentRecord(string featureName, string category, string system, string material, string notes)
        {
            FeatureName = featureName;
            Category = category;
            System = system;
            Material = material;
            Notes = notes;
        }
    }

    private struct Vec
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Vec(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vec Normalized()
        {
            double length = Length();
            if (length <= 0.000001)
            {
                return new Vec(0, 0, 1);
            }
            return new Vec(X / length, Y / length, Z / length);
        }

        public static Vec operator +(Vec a, Vec b)
        {
            return new Vec(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec operator -(Vec a, Vec b)
        {
            return new Vec(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec operator *(Vec a, double s)
        {
            return new Vec(a.X * s, a.Y * s, a.Z * s);
        }
    }
}
