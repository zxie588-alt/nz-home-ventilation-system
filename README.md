# NZ Home MVHR / ERV Ventilation System

Preliminary New Zealand residential ventilation case study combining 3D CAD layout, SolidWorks API automation, airflow scheduling, duct velocity checks and first-pass pressure-loss estimation.

This repository is structured as a portfolio engineering project, not a consent design or professional compliance statement.

## Engineering Scope

- MVHR / ERV unit layout for a concept New Zealand three-bedroom home.
- Routed supply, extract, outdoor intake and exhaust systems.
- Diffusers, grilles, collars, hangers, outdoor terminals and airflow markers.
- C# SolidWorks API model generation workflow.
- Normal balanced airflow case: 60 L/s supply and 60 L/s extract.
- Boost / extractor check case: 85 L/s extract target.
- Preliminary fan-duty targets: about 80 Pa normal mode and 120 Pa boost mode.

## Repository Structure

- `source/solidworks/` - native SolidWorks part file.
- `source/scripts/` - C# SolidWorks API generation script and PowerShell build helper.
- `exports/` - STEP and STL model exports.
- `docs/` - component list, airflow schedule, pressure estimate and design notes.
- `images/` - rendered isometric system view.

## Key Files

- `source/solidworks/NZ_Home_Ventilation_System.SLDPRT`
- `source/scripts/BuildNzHomeVentilationSystem.cs`
- `docs/nz_ventilation_airflow_schedule.csv`
- `docs/nz_ventilation_duct_pressure_estimate.csv`
- `docs/nz_ventilation_preliminary_design_note.md`

## Preview

![NZ home ventilation system isometric view](images/NZ_Home_Ventilation_System_Isometric.png)

## Notes

The model uses New Zealand building-services context from NZBC G4 ventilation, H1 energy efficiency and Healthy Homes ventilation guidance as project references. The current limitations are no real architectural plan, no selected MVHR/ERV manufacturer fan curve, no diffuser/grille data, no acoustic check and no professional compliance review.
