using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using SolidWorks.Interop.sldworks;

public static class BuildSmartVentilationValidationKit
{
    private const string PartTemplatePath = @"C:\ProgramData\SOLIDWORKS\SOLIDWORKS 2018\templates\gb_part.prtdot";

    private static SldWorks _swApp;
    private static ModelDoc2 _model;
    private static PartDoc _part;
    private static Modeler _modeler;
    private static readonly List<ComponentRecord> _components = new List<ComponentRecord>();

    private static readonly Material Base = new Material("Base plate - matte black", 0.05, 0.06, 0.06, 0.00);
    private static readonly Material Duct = new Material("Clear test duct", 0.72, 0.86, 0.96, 0.55);
    private static readonly Material Galv = new Material("Galvanised bracket", 0.60, 0.64, 0.66, 0.00);
    private static readonly Material Dark = new Material("Dark hardware", 0.10, 0.11, 0.12, 0.00);
    private static readonly Material Green = new Material("PCB green", 0.03, 0.42, 0.20, 0.00);
    private static readonly Material Blue = new Material("Sensor blue", 0.04, 0.28, 0.82, 0.00);
    private static readonly Material Orange = new Material("Boost air marker", 0.94, 0.42, 0.08, 0.00);
    private static readonly Material Cyan = new Material("Normal air marker", 0.04, 0.58, 0.88, 0.00);
    private static readonly Material White = new Material("Label white", 0.96, 0.96, 0.92, 0.00);
    private static readonly Material Tube = new Material("Silicone tube", 0.90, 0.93, 0.96, 0.20);

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
        string repoDir = new DirectoryInfo(scriptDir).Parent.Parent.FullName;
        string swDir = Path.Combine(repoDir, "source", "solidworks");
        string exportDir = Path.Combine(repoDir, "exports");
        string docDir = Path.Combine(repoDir, "docs");
        string imageDir = Path.Combine(repoDir, "images");
        Directory.CreateDirectory(swDir);
        Directory.CreateDirectory(exportDir);
        Directory.CreateDirectory(docDir);
        Directory.CreateDirectory(imageDir);

        string partPath = Path.Combine(swDir, "Smart_Ventilation_Validation_Kit.SLDPRT");
        string stepPath = Path.Combine(exportDir, "Smart_Ventilation_Validation_Kit.step");
        string stlPath = Path.Combine(exportDir, "Smart_Ventilation_Validation_Kit.stl");
        string pngPath = Path.Combine(imageDir, "Smart_Ventilation_Validation_Kit_Isometric.png");
        string csvPath = Path.Combine(docDir, "smart_validation_kit_component_list.csv");

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

        Console.WriteLine("Building smart validation kit geometry...");
        BuildBaseAndFrame();
        BuildDuctTestRig();
        BuildSensorPod();
        BuildElectronicsTray();
        BuildPressureTapsAndTubes();
        BuildFanAndFlowConditioning();
        BuildCalloutBlocks();

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

    private static void BuildBaseAndFrame()
    {
        AddBox("Base_Plate_900x420mm", "Base", "Validation rig", V(0, 0, -0.025), V(0.90, 0.42, 0.05), Base, "Benchtop base plate for sensor validation kit");
        AddBox("Left_Aluminium_Rail", "Frame", "Validation rig", V(-0.36, -0.17, 0.045), V(0.08, 0.035, 0.09), Galv, "Aluminium rail support");
        AddBox("Right_Aluminium_Rail", "Frame", "Validation rig", V(0.36, -0.17, 0.045), V(0.08, 0.035, 0.09), Galv, "Aluminium rail support");
        AddBox("Rear_Aluminium_Rail", "Frame", "Validation rig", V(0, 0.17, 0.045), V(0.82, 0.035, 0.09), Galv, "Rear rail for electronics and duct supports");

        AddCylinder("Rubber_Foot_FL", "Foot", "Validation rig", V(-0.40, -0.17, -0.075), V(-0.40, -0.17, -0.025), 0.035, Dark, "Rubber foot");
        AddCylinder("Rubber_Foot_FR", "Foot", "Validation rig", V(0.40, -0.17, -0.075), V(0.40, -0.17, -0.025), 0.035, Dark, "Rubber foot");
        AddCylinder("Rubber_Foot_RL", "Foot", "Validation rig", V(-0.40, 0.17, -0.075), V(-0.40, 0.17, -0.025), 0.035, Dark, "Rubber foot");
        AddCylinder("Rubber_Foot_RR", "Foot", "Validation rig", V(0.40, 0.17, -0.075), V(0.40, 0.17, -0.025), 0.035, Dark, "Rubber foot");
    }

    private static void BuildDuctTestRig()
    {
        AddDuct("Transparent_150mm_Test_Duct", V(-0.34, 0, 0.20), V(0.34, 0, 0.20), 0.075, Duct, "150 mm clear duct section for airflow, CO2 and RH validation");
        AddCollar("Duct_Inlet_Flange", V(-0.39, 0, 0.20), V(1, 0, 0), 0.092, 0.055, Galv, "Inlet flange");
        AddCollar("Duct_Outlet_Flange", V(0.39, 0, 0.20), V(1, 0, 0), 0.092, 0.055, Galv, "Outlet flange");
        AddBox("Sensor_Access_Hatch", "Duct access", "Validation rig", V(0.02, -0.082, 0.225), V(0.19, 0.018, 0.08), Galv, "Removable hatch for sensor probe access");
        AddBox("Hatch_Gasket_Visual", "Duct access", "Validation rig", V(0.02, -0.093, 0.225), V(0.21, 0.006, 0.095), Dark, "Dark gasket line around sensor access hatch");

        AddBox("Front_U_Bracket_Left", "Duct support", "Validation rig", V(-0.28, -0.085, 0.105), V(0.032, 0.035, 0.19), Galv, "U-bracket leg");
        AddBox("Front_U_Bracket_Right", "Duct support", "Validation rig", V(-0.28, 0.085, 0.105), V(0.032, 0.035, 0.19), Galv, "U-bracket leg");
        AddBox("Front_U_Bracket_Cross", "Duct support", "Validation rig", V(-0.28, 0, 0.015), V(0.045, 0.22, 0.030), Galv, "U-bracket base");
        AddBox("Rear_U_Bracket_Left", "Duct support", "Validation rig", V(0.28, -0.085, 0.105), V(0.032, 0.035, 0.19), Galv, "U-bracket leg");
        AddBox("Rear_U_Bracket_Right", "Duct support", "Validation rig", V(0.28, 0.085, 0.105), V(0.032, 0.035, 0.19), Galv, "U-bracket leg");
        AddBox("Rear_U_Bracket_Cross", "Duct support", "Validation rig", V(0.28, 0, 0.015), V(0.045, 0.22, 0.030), Galv, "U-bracket base");

        AddArrow("Normal_Flow_Arrow", V(-0.30, 0, 0.33), V(-0.06, 0, 0.33), Cyan, "Normal 60 L/s airflow marker");
        AddArrow("Boost_Flow_Arrow", V(0.03, 0, 0.33), V(0.30, 0, 0.33), Orange, "Boost 85 L/s airflow marker");
    }

    private static void BuildSensorPod()
    {
        AddBox("Sensor_Pod_Enclosure_Base", "Sensor pod", "Controls", V(0.02, -0.185, 0.235), V(0.18, 0.12, 0.075), White, "3D-printable enclosure for CO2/RH/temperature sensor package");
        AddBox("Sensor_Pod_Lid", "Sensor pod", "Controls", V(0.02, -0.185, 0.282), V(0.19, 0.13, 0.016), Galv, "Removable lid with four screw positions");
        AddBox("Sensor_Pod_Label_CO2_RH", "Sensor pod", "Controls", V(0.02, -0.252, 0.292), V(0.12, 0.006, 0.026), Blue, "Face label representing CO2/RH sensor window");
        AddCylinder("Sensor_Pod_Cable_Gland", "Cable gland", "Controls", V(0.125, -0.185, 0.235), V(0.175, -0.185, 0.235), 0.018, Dark, "Cable gland for USB/sensor wiring");

        AddBox("Sensor_Pod_Mounting_Ear_Left", "Sensor pod", "Controls", V(-0.095, -0.185, 0.220), V(0.030, 0.050, 0.018), White, "Mounting ear with M4 clearance visual");
        AddBox("Sensor_Pod_Mounting_Ear_Right", "Sensor pod", "Controls", V(0.135, -0.185, 0.220), V(0.030, 0.050, 0.018), White, "Mounting ear with M4 clearance visual");
        AddCylinder("Sensor_Pod_M4_Hole_Left_Visual", "Fastener hole visual", "Controls", V(-0.095, -0.185, 0.210), V(-0.095, -0.185, 0.232), 0.010, Dark, "M4 clearance hole represented as dark insert");
        AddCylinder("Sensor_Pod_M4_Hole_Right_Visual", "Fastener hole visual", "Controls", V(0.135, -0.185, 0.210), V(0.135, -0.185, 0.232), 0.010, Dark, "M4 clearance hole represented as dark insert");

        AddBox("SCD41_CO2_RH_Sensor_Module", "Sensor module", "Controls", V(-0.025, -0.185, 0.245), V(0.045, 0.030, 0.010), Blue, "CO2/RH/temperature sensor module position");
        AddBox("SHT31_Secondary_RH_Module", "Sensor module", "Controls", V(0.050, -0.185, 0.245), V(0.040, 0.026, 0.010), Blue, "Secondary humidity sensor module position");
        AddCylinder("Sensor_Probe_Stem_To_Duct", "Sensor probe", "Controls", V(0.02, -0.128, 0.235), V(0.02, -0.074, 0.215), 0.010, Tube, "Short probe route from enclosure into duct access hatch");
    }

    private static void BuildElectronicsTray()
    {
        AddBox("Electronics_Tray", "Electronics", "Controls", V(-0.21, 0.205, 0.055), V(0.30, 0.14, 0.025), Galv, "Removable electronics tray for ESP32 and sensors");
        AddBox("ESP32_Dev_Board", "Electronics", "Controls", V(-0.25, 0.205, 0.087), V(0.072, 0.030, 0.009), Green, "ESP32 controller board");
        AddBox("USB_Connector_Visual", "Electronics", "Controls", V(-0.302, 0.205, 0.095), V(0.018, 0.020, 0.014), Dark, "USB connector visual");
        AddBox("Differential_Pressure_Sensor_Module", "Electronics", "Controls", V(-0.14, 0.205, 0.088), V(0.060, 0.042, 0.010), Blue, "Differential pressure sensor module");
        AddBox("Power_Monitor_Module", "Electronics", "Controls", V(-0.045, 0.205, 0.088), V(0.065, 0.040, 0.010), Blue, "Fan power monitoring placeholder");

        AddBox("OLED_Status_Display", "Electronics", "Controls", V(0.115, 0.205, 0.095), V(0.090, 0.038, 0.010), Dark, "Small display for mode and readings");
        AddBox("Display_Active_Area", "Electronics", "Controls", V(0.115, 0.205, 0.102), V(0.076, 0.026, 0.006), Cyan, "Display active area visual");
        AddCylinder("Manual_Boost_Button", "Controls", "Controls", V(0.205, 0.205, 0.080), V(0.205, 0.205, 0.112), 0.018, Orange, "Manual boost button");
        AddCylinder("Status_LED_Normal", "Controls", "Controls", V(0.250, 0.175, 0.080), V(0.250, 0.175, 0.110), 0.010, Cyan, "Normal mode status LED");
        AddCylinder("Status_LED_Boost", "Controls", "Controls", V(0.250, 0.235, 0.080), V(0.250, 0.235, 0.110), 0.010, Orange, "Boost mode status LED");
    }

    private static void BuildPressureTapsAndTubes()
    {
        AddCylinder("Upstream_Static_Pressure_Tap", "Pressure tap", "Validation rig", V(-0.12, 0, 0.275), V(-0.12, 0, 0.350), 0.010, Galv, "Upstream duct pressure tap");
        AddCylinder("Downstream_Static_Pressure_Tap", "Pressure tap", "Validation rig", V(0.16, 0, 0.275), V(0.16, 0, 0.350), 0.010, Galv, "Downstream duct pressure tap");
        AddCylinder("Upstream_Silicone_Tube", "Pressure tube", "Controls", V(-0.12, 0, 0.350), V(-0.145, 0.185, 0.105), 0.006, Tube, "Silicone tube to differential pressure sensor");
        AddCylinder("Downstream_Silicone_Tube", "Pressure tube", "Controls", V(0.16, 0, 0.350), V(-0.125, 0.215, 0.105), 0.006, Tube, "Silicone tube to differential pressure sensor");
        AddBox("Pressure_Tap_Saddle_Plate", "Pressure tap", "Validation rig", V(0.02, 0.081, 0.235), V(0.35, 0.012, 0.050), Galv, "Saddle plate showing measurement location on duct");
    }

    private static void BuildFanAndFlowConditioning()
    {
        AddBox("Variable_Speed_Fan_Box", "Fan", "Validation rig", V(-0.49, 0, 0.20), V(0.15, 0.22, 0.22), Dark, "Variable-speed fan module for normal/boost validation");
        AddCylinder("Fan_Inlet_Ring", "Fan", "Validation rig", V(-0.575, 0, 0.20), V(-0.535, 0, 0.20), 0.075, Galv, "Fan inlet ring");
        AddCylinder("Fan_Outlet_Ring", "Fan", "Validation rig", V(-0.445, 0, 0.20), V(-0.405, 0, 0.20), 0.075, Galv, "Fan outlet ring");
        AddBox("Fan_Blade_A", "Fan blade", "Validation rig", V(-0.49, 0, 0.20), V(0.012, 0.140, 0.020), Orange, "Simplified fan blade");
        AddBox("Fan_Blade_B", "Fan blade", "Validation rig", V(-0.49, 0, 0.20), V(0.012, 0.020, 0.140), Orange, "Simplified fan blade");
        AddCylinder("Fan_Hub", "Fan", "Validation rig", V(-0.505, 0, 0.20), V(-0.475, 0, 0.20), 0.026, Galv, "Fan hub");

        AddBox("Honeycomb_Flow_Straightener_Block", "Flow conditioner", "Validation rig", V(-0.31, 0, 0.20), V(0.035, 0.145, 0.145), Galv, "Flow straightener plane");
        for (int y = -2; y <= 2; y++)
        {
            for (int z = -2; z <= 2; z++)
            {
                AddCylinder("Flow_Straightener_Cell_" + y.ToString("+#;-#;0") + "_" + z.ToString("+#;-#;0"), "Flow conditioner cell", "Validation rig", V(-0.335, y * 0.024, 0.20 + z * 0.024), V(-0.285, y * 0.024, 0.20 + z * 0.024), 0.006, Dark, "Visual honeycomb cell");
            }
        }

        AddBox("Outlet_Grille_Frame", "Outlet grille", "Validation rig", V(0.49, 0, 0.20), V(0.035, 0.19, 0.19), Galv, "Outlet grille frame");
        AddBox("Outlet_Grille_Louvre_1", "Outlet grille", "Validation rig", V(0.512, -0.055, 0.20), V(0.018, 0.010, 0.16), Dark, "Outlet grille louvre");
        AddBox("Outlet_Grille_Louvre_2", "Outlet grille", "Validation rig", V(0.512, 0.000, 0.20), V(0.018, 0.010, 0.16), Dark, "Outlet grille louvre");
        AddBox("Outlet_Grille_Louvre_3", "Outlet grille", "Validation rig", V(0.512, 0.055, 0.20), V(0.018, 0.010, 0.16), Dark, "Outlet grille louvre");
    }

    private static void BuildCalloutBlocks()
    {
        AddBox("Callout_Normal_60Lps", "Callout", "Documentation", V(-0.18, -0.265, 0.050), V(0.20, 0.012, 0.055), Cyan, "Normal balanced target: 60 L/s");
        AddBox("Callout_Boost_85Lps", "Callout", "Documentation", V(0.12, -0.265, 0.050), V(0.20, 0.012, 0.055), Orange, "Boost validation target: 85 L/s");
        AddBox("Callout_CSV_Dashboard", "Callout", "Documentation", V(0.34, 0.205, 0.115), V(0.19, 0.012, 0.050), White, "CSV log to dashboard workflow");
        AddBox("Callout_Commissioning", "Callout", "Documentation", V(-0.39, 0.205, 0.115), V(0.18, 0.012, 0.050), White, "Commissioning test rig evidence");
    }

    private static void AddDuct(string name, Vec start, Vec end, double radius, Material material, string notes)
    {
        AddCylinder(name, "Round duct", material.Name, start, end, radius, material, notes);
        Vec dir = (end - start).Normalized();
        AddCollar(name + "_Start_Collar", start + dir * 0.04, dir, radius * 1.12, 0.055, Galv, "Joint collar");
        AddCollar(name + "_End_Collar", end - dir * 0.04, dir, radius * 1.12, 0.055, Galv, "Joint collar");
    }

    private static void AddCollar(string name, Vec center, Vec direction, double radius, double length, Material material, string notes)
    {
        Vec dir = direction.Normalized();
        AddCylinder(name, "Collar/flange", material.Name, center - dir * (length / 2.0), center + dir * (length / 2.0), radius, material, notes);
    }

    private static void AddArrow(string name, Vec start, Vec end, Material material, string notes)
    {
        Vec delta = end - start;
        double length = delta.Length();
        if (length < 0.06) return;
        Vec dir = delta.Normalized();
        double headLength = 0.055;
        Vec shaftEnd = end - dir * headLength;
        AddCylinder(name + "_Shaft", "Airflow arrow", material.Name, start, shaftEnd, 0.012, material, notes);
        AddCone(name + "_Head", shaftEnd, dir, 0.035, 0.004, headLength, material, notes);
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
