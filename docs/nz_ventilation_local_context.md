# NZ Residential Ventilation Project - Local Context Notes

This note records the New Zealand code and software context used to upgrade the residential MVHR / ERV ventilation portfolio project. It is not a consent design, compliance certificate or professional engineering statement.

## New Zealand References

### NZ Building Code G4 - Ventilation

MBIE Building Performance states that NZBC clause G4 requires ventilation to all occupied spaces and requires ventilation consistent with maximum occupancy and intended use. It covers outdoor air and extract ventilation, including removal of cooking fumes, moisture and gases.

For current project notes, reference G4/AS1 5th edition and G4/VM1 5th edition, effective 28 July 2025. MBIE also lists the 4th edition as effective from 27 June 2019 until 31 July 2026.

Project use:

- Define occupied zones and wet / pollutant-source zones.
- Record outdoor-air and extract-air intent.
- State that the current portfolio model is a preliminary layout and sizing exercise, not a compliance pathway.

### NZ Building Code H1 - Energy Efficiency

MBIE describes H1 as providing for efficient use of energy and setting physical conditions for energy performance. It includes requirements affecting HVAC systems, hot water, lighting, thermal resistance and uncontrolled airflow.

Project use:

- Mention that MVHR / ERV selection is relevant to ventilation-energy interaction.
- Add a future energy note: compare heat-recovery ventilation with extract-only ventilation.
- Avoid claiming H1 compliance without a formal H1 method or building fabric data.

### Healthy Homes Ventilation Standard

Tenancy Services states that rental homes must have openable windows in living rooms, dining rooms, kitchens and bedrooms, with openable area at least 5% of the floor area of that room. It also gives mechanical ventilation checks for kitchens and bathrooms, including 50 L/s or 150 mm for kitchens and 25 L/s or 120 mm for bathrooms for relevant extractor-fan cases, and 12 L/s kitchen / 10 L/s bathroom extract checks for some continuous mechanical ventilation systems.

Project use:

- Use as a practical residential reference point, especially because the concept is a New Zealand home.
- Label the checks as "Healthy Homes point checks", not full Building Code compliance.
- Include openable-area and extract-rate checks in the sizing spreadsheet.

## Software Stack to Learn

### Priority 1 - Excel or Google Sheets

Use for room schedule, airflow schedule, duct velocity, pressure-loss estimate, fan duty and compliance notes. This is the fastest way to make the project look like engineering rather than only CAD.

### Priority 2 - Revit MEP and AutoCAD

Use Revit MEP for BIM-style duct layout, schedules and coordination. Use AutoCAD for 2D plan markups and schematic layout. Autodesk describes Revit as software used by architects, structural engineers, MEP engineers, civil engineers and construction professionals to design, document and deliver building projects.

### Priority 3 - SolidWorks

Keep the existing SolidWorks model as a visual mechanical / MEP layout demonstration. It is useful for showing geometry, routing and modelling discipline, even if Revit is more common in building services practice.

### Priority 4 - Energy / Simulation Tools

For a later upgrade, consider EnergyPlus / OpenStudio / DesignBuilder or IES VE for building energy and load analysis, depending on access. Use ANSYS Fluent or CFX only after the basic HVAC sizing is complete, for a simple room airflow or diffuser-flow visualisation.

## Completed Portfolio Deliverables

1. Local context note referencing G4, H1 and Healthy Homes.
2. Preliminary airflow schedule for normal balanced operation and boost / extractor checks.
3. Duct diameter and velocity table linked to the SolidWorks model routes.
4. Pressure-loss estimate with straight duct and fitting allowances.
5. Fan duty note: 60 L/s at about 80 Pa for normal mode, 85 L/s at about 120 Pa for boost mode.
6. One-page project summary for the website.
7. Revit MEP coordination package with view/sheet plan, naming convention, duct-terminal schedule seed and handoff notes.

## Remaining Blockers

- Real architectural plan, confirmed room areas and openable-window schedule.
- Manufacturer MVHR/ERV fan curve, filter loss and heat-recovery data.
- Diffuser, grille, damper, flexible-duct, silencer and weather-hood pressure-loss data.
- Acoustic check and real installation constraints.
- Formal compliance pathway and professional review.
- Native Revit `.rvt` model and Revit-exported screenshots from checked MEP elements.
