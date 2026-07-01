# Revit MEP Coordination Plan - NZ Residential MVHR / ERV

This note records the Revit/BIM coordination layer added to the NZ residential MVHR / ERV ventilation project. It is a portfolio-level BIM workflow and documentation package, not a consent design or construction issue drawing.

## Purpose

The original ventilation project already included SolidWorks geometry, a DXF plan, airflow schedule, pressure-loss estimate, equipment selection and commissioning notes. The Revit upgrade adds the part that New Zealand building-services consultancies are more likely to recognise: model coordination, schedules, tagged views, drawing issue control and clash-review thinking.

## Revit Model Scope

Model discipline: Mechanical / HVAC

Level of development: portfolio coordination model, approximately LOD 200-300 depending on element type.

Main model elements to be represented in Revit MEP:

- MVHR / ERV unit envelope with maintenance-clearance zone.
- Supply air, extract air, outdoor air and exhaust air duct systems.
- Supply diffusers for bedrooms and living spaces.
- Extract terminals for kitchen, bathroom and laundry.
- Outdoor intake and exhaust terminals.
- Balancing dampers, access notes and system tags.
- Basic ceiling/service-zone coordination against a simple architectural layout.

## Views and Sheets

Recommended Revit browser structure:

- `M-101 Ventilation Plan - Level 1`
- `M-201 MVHR Equipment and Duct Coordination`
- `M-301 Airflow Schedule`
- `M-401 Commissioning and Validation Notes`
- `3D-MEP Coordination View`

Recommended sheet package:

- `M-101`: MVHR / ERV ventilation layout with system colours, duct sizes and airflow tags.
- `M-201`: equipment location, service access and route coordination.
- `M-301`: airflow and terminal schedule exported from the model.
- `M-401`: commissioning sensors, CO2/RH/pressure/power checks and evidence-gap notes.

## Naming and System Conventions

System names:

- `SA-01 Supply Air`
- `EA-01 Extract Air`
- `OA-01 Outdoor Air`
- `EX-01 Exhaust Air`

Element naming:

- `MVHR-01`
- `SA-DIF-01` to `SA-DIF-04`
- `EA-GRL-KIT-01`
- `EA-GRL-BATH-01`
- `EA-GRL-LDY-01`
- `OA-TERM-01`
- `EX-TERM-01`

## Schedule Fields

The attached CSV schedule is intended to be recreated as a Revit duct-terminal/equipment schedule:

- Mark
- System
- Element type
- Room / zone
- Design airflow L/s
- Duct diameter mm
- Design velocity m/s
- Design pressure note
- Commissioning check

## Clash and Coordination Checks

Minimum model review checklist:

- Maintain access clearance around MVHR filters, drain and service panel.
- Avoid crossing duct routes through structural walls or beams without an opening note.
- Keep outdoor intake and exhaust separation visible in plan.
- Keep branch duct velocities in a reasonable residential comfort range.
- Check diffuser placement against likely furniture and room use.
- Keep balancing and measuring points accessible for commissioning.
- Flag acoustic risk where duct velocity, fan duty or grille location may affect bedrooms.

## Design Boundary

Completed in the public portfolio:

- Revit MEP coordination workflow.
- Revit-ready schedule and naming convention.
- Drawing/sheet index and coordination checklist.
- DXF and SolidWorks outputs that can be used as model references.

Still requiring a native Revit session and professional review:

- Native `.rvt` model with real Revit duct elements and families.
- Architectural background model based on an actual dwelling.
- Revit schedule export from modelled MEP elements.
- Clash detection against structural and architectural models.
- Construction documentation and consent-ready compliance review.

## Resume-Safe Wording

Use:

> Added a Revit MEP coordination workflow to a NZ residential MVHR / ERV project, including BIM naming conventions, sheet/view plan, duct-terminal schedule fields, access-clearance checks and commissioning handoff notes.

Avoid:

> Completed a construction-ready Revit model.

The second statement is not safe until a native Revit file, screenshots and schedule exports are available.
