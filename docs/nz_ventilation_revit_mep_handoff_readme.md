# Revit MEP Handoff README

Folder purpose: this public portfolio package gives a Revit user enough structure to rebuild the MVHR / ERV design as a native Revit MEP coordination model.

## Inputs

- `../cad/nz_home_ventilation_layout.dxf` - 2D plan reference.
- `../models/nz_home_ventilation_system.step` - SolidWorks geometry reference.
- `nz_ventilation_revit_mep_schedule.csv` - duct-terminal and equipment schedule seed.
- `nz_ventilation_revit_mep_coordination_plan.md` - view/sheet/naming/checklist plan.

## Suggested Revit Steps

1. Start a mechanical project from the local Revit 2026 template.
2. Link/import the DXF as a plan reference on Level 1.
3. Create four duct systems: `SA-01`, `EA-01`, `OA-01`, `EX-01`.
4. Place the MVHR unit envelope as `MVHR-01`.
5. Route main ducts and branches using the schedule diameters and airflow targets.
6. Place duct terminals and assign marks from the schedule.
7. Create a duct-terminal schedule with Mark, System, Room, Airflow, Diameter and Commissioning Check.
8. Create `M-101`, `M-201`, `M-301` and `M-401` sheets.
9. Export schedule CSV and screenshots after a native Revit file is created.

## Current Boundary

This folder is a Revit-ready BIM coordination package. It does not yet contain a native `.rvt` file because the public portfolio should not claim a construction-ready Revit model until the model has been opened, checked, screenshot and exported from Revit.
