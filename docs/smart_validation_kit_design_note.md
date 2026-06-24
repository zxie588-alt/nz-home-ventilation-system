# Smart Ventilation Validation Kit - SolidWorks Design Note

This note documents the SolidWorks validation kit added to the NZ residential MVHR / ERV project. The model is a portfolio-level mechanical test-rig concept, not a certified product design.

## Purpose

The original project already showed a New Zealand residential ventilation layout, airflow schedule, pressure estimate and equipment selection. The validation kit adds the physical testing layer:

- How a CO2/RH/temperature sensor package could be mounted.
- How duct pressure could be sampled.
- How normal and boost airflow modes could be represented.
- How an ESP32-based logger and dashboard could connect to the mechanical system.
- How the project could move from drawing/calculation to commissioning evidence.

## SolidWorks Deliverables

- Native SolidWorks part: `source/solidworks/Smart_Ventilation_Validation_Kit.SLDPRT`
- STEP export: `exports/Smart_Ventilation_Validation_Kit.step`
- STL export: `exports/Smart_Ventilation_Validation_Kit.STL`
- Isometric preview: `images/Smart_Ventilation_Validation_Kit_Isometric.png`
- Component list: `docs/smart_validation_kit_component_list.csv`
- Generation script: `source/scripts/BuildSmartVentilationValidationKit.cs`

The model contains 91 named bodies/features generated through the SolidWorks API.

## Main Subsystems

| Subsystem | What the model shows | Engineering value |
| --- | --- | --- |
| Transparent 150 mm test duct | A short duct section with inlet/outlet flanges, hatch and support brackets | Represents a controlled path for airflow and sensor validation |
| Sensor pod enclosure | CO2/RH sensor enclosure with lid, mounting ears, cable gland and probe route | Shows mechanical packaging for electronics and service access |
| Pressure taps and silicone tubes | Upstream/downstream taps routed to a differential pressure sensor | Connects duct design to commissioning measurement |
| Electronics tray | ESP32 board, differential pressure module, power monitor, OLED display, button and LEDs | Shows hardware integration and test-data workflow |
| Variable-speed fan module | Fan box, inlet/outlet rings, simplified blades and hub | Represents normal/boost airflow generation |
| Flow conditioning and outlet grille | Honeycomb visual and outlet grille | Shows awareness of measurement stability and outlet effects |
| Base frame | Rails, brackets and rubber feet | Shows that the model is treated as a bench test rig rather than only a diagram |

## How It Supports the Portfolio Story

For building-services consultancies, the kit shows that the design could be commissioned: airflow targets, sensor locations, pressure taps and data logs are all thought through.

For hardware and product companies, the kit shows practical mechanical packaging, electronics integration, test-rig design and iterative validation thinking.

For systems-engineering interviews, the kit connects requirements, FMEA, sensor data, dashboard logic and physical geometry into one traceable workflow.

## Current Limitations

- The duct and sensor components are concept geometry, not production-ready detailed parts.
- Circular holes are represented visually rather than cut with full manufacturing feature history.
- Sensor dimensions are indicative and should be updated after selecting exact hardware.
- The fan module is a functional visual placeholder; real fan mounting, guards, wiring and safety checks are not finalised.
- Physical testing still requires real sensors, calibration records and airflow/pressure measurement equipment.

## Interview Explanation

A concise way to explain this addition:

> I extended the MVHR/ERV design into a SolidWorks validation kit so the project does not stop at layout and sizing. The kit includes a 150 mm test duct, sensor pod, ESP32 electronics tray, pressure taps, tubing, fan module and flow-conditioning features. It shows how I would validate CO2, humidity, duct pressure, airflow mode and fan-power data before feeding the results into a commissioning log and dashboard.
