# NZ Residential MVHR / ERV Ventilation Layout - Preliminary Design Note

This note upgrades the SolidWorks residential ventilation layout from a visual CAD concept into a preliminary building-services design exercise. It is written for portfolio use and is not a building consent design, producer statement, compliance certificate or professional engineering statement.

## Base Model

- Concept dwelling footprint: 12 m x 8 m, nominal plan area 96 m2.
- Nominal internal volume used for a first-pass ventilation sense check: 96 m2 x 2.4 m = 230 m3.
- Existing CAD deliverables: SolidWorks multi-body model, STEP, STL, PNG render and component-list CSV.
- Added drawing deliverable: AutoCAD-readable DXF ventilation plan and SVG plan schematic.
- System architecture: ceiling-mounted MVHR / ERV unit with supply, extract, outdoor intake and exhaust routes.
- Terminals in model: 4 supply diffusers, 3 extract grilles, 1 outdoor intake and 1 exhaust terminal.
- Automation method: C# SolidWorks API generation with 151 named CAD bodies/features.

## Local Design Context Checked

- NZ Building Code G4 - Ventilation: used as the main code context because it requires ventilation to occupied spaces and covers outdoor air and extract ventilation.
- NZ Building Code H1 - Energy Efficiency: used as an energy-context reminder because heat-recovery ventilation and uncontrolled airflow affect HVAC and energy performance.
- Healthy Homes ventilation standard: used as a practical residential reference point for openable-window area and kitchen/bathroom extraction checks.

Sources checked on 24 June 2026:

- https://www.building.govt.nz/building-code-compliance/g-services-and-facilities/g4-ventilation
- https://www.building.govt.nz/building-code-compliance/h-energy-efficiency/h1-energy-efficiency
- https://www.tenancy.govt.nz/healthy-homes/ventilation-standard/

## Proposed Normal Operating Mode

The normal mode is a balanced MVHR / ERV case:

- Total supply: 60 L/s, or 216 m3/h.
- Total extract: 60 L/s, or 216 m3/h.
- Nominal whole-dwelling air-change sense check: 216 m3/h / 230 m3 = 0.94 ACH.
- Supply distribution: four terminals at 15 L/s each.
- Extract distribution: kitchen 30 L/s, bathroom 20 L/s, laundry 10 L/s.

This mode is intentionally balanced so that the model reads as a heat-recovery ventilation system rather than a purely extract-only system.

## Boost / Extractor Check Mode

Healthy Homes distinguishes extractor-fan checks from some continuous mechanical ventilation checks. To avoid mixing these two ideas, the project keeps a separate boost mode:

- Kitchen boost extract: 50 L/s.
- Bathroom boost extract: 25 L/s.
- Laundry extract retained: 10 L/s.
- Boost total extract: 85 L/s.

This boost mode is a design target only. It needs a selected MVHR/fan curve, acoustic check, diffuser/grille data and makeup-air strategy before it can be treated as a real design.

## Duct Velocity Results

Normal mode:

- 180 mm supply main at 60 L/s: 2.36 m/s.
- 130 mm supply branches at 15 L/s: 1.13 m/s.
- 170 mm extract main at 60 L/s: 2.64 m/s.
- 120 mm kitchen extract branch at 30 L/s: 2.65 m/s.
- 120 mm bathroom extract branch at 20 L/s: 1.77 m/s.
- 180 mm outdoor intake/exhaust at 60 L/s: 2.36 m/s.

Boost mode:

- 120 mm kitchen extract branch at 50 L/s: 4.42 m/s.
- 120 mm bathroom extract branch at 25 L/s: 2.21 m/s.
- 180 mm outdoor intake/exhaust at 85 L/s: 3.34 m/s.

The kitchen boost velocity is acceptable for a preliminary calculation but should be checked for noise and pressure loss if the project is developed further.

## Pressure-Loss Method

The pressure estimate uses a simple Darcy-Weisbach first pass:

- Air density: 1.2 kg/m3.
- Duct friction factor: 0.02.
- Circular duct velocity from Q/A.
- Straight-duct pressure loss: f x L/D x rho x V2 / 2.
- Fitting and terminal losses: simplified K allowances plus a 10 Pa terminal placeholder.

This is a preliminary path comparison. It does not include manufacturer data for the MVHR core, filters, silencers, flexible duct roughness, balancing dampers, real diffuser pressure drops or acoustic requirements.

## Estimated Fan Duty Targets

The calculated path losses are low because the CAD model uses short, smooth concept ductwork and simplified fittings:

- Highest normal-mode path estimate: kitchen extract path, about 34 Pa.
- Highest supply path estimate: about 19 Pa.
- Highest boost-mode path estimate: kitchen boost path, about 72 Pa.

For a portfolio-level design target, the project should state:

- Normal mode fan target: 60 L/s at about 80 Pa external static pressure.
- Boost mode fan target: 85 L/s at about 120 Pa external static pressure.

These targets include margin for filters, grilles, flexible connections, balancing and real equipment pressure drops. A manufacturer-based concept check was added against Mitsubishi Electric Lossnay LGH-RVX3-E data, with LGH-35RVX3-E selected as the preferred concept unit because it covers the 85 L/s boost target with headroom.

## Current Limitations / Blockers

- No real architectural plan, window schedule or confirmed room areas.
- Concept MVHR unit selected, but no full manufacturer fan-curve overlay, filter pressure-drop schedule or final heat-recovery commissioning data.
- No manufacturer diffuser, grille, damper or weather-hood pressure-loss data.
- No acoustic calculation or noise criteria.
- No formal AS/NZS 1668.2 design table extraction.
- No Revit MEP coordination model yet.
- No building-consent pathway or professional review.

## Recommended Next Upgrade

The strongest next portfolio upgrade is to redraw this in Revit MEP using the same airflow schedule if licensed access is available, then export a plan sheet with ducts, terminals, schedule tags and a simple fan-duty note. The current AutoCAD-readable DXF plan is the interim 2D drawing deliverable.
