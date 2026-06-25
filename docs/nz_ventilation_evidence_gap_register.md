# Evidence Gap Register

This register keeps the portfolio project honest. It separates completed evidence from future validation work so the website does not imply that physical testing or compliance certification has already been completed.

## Current Evidence

| Evidence item | Current status | Where it is shown |
| --- | --- | --- |
| Residential MVHR / ERV layout | Completed as SolidWorks concept model | `source/solidworks/NZ_Home_Ventilation_System.SLDPRT` |
| Airflow schedule | Completed as preliminary sizing schedule | `docs/nz_ventilation_airflow_schedule.csv` |
| Duct velocity and pressure estimate | Completed as first-pass calculation | `docs/nz_ventilation_duct_pressure_estimate.csv` |
| Equipment selection | Completed as concept manufacturer check | `docs/nz_ventilation_equipment_selection.md` |
| NZ reference mapping | Completed as reference check, not compliance claim | `docs/nz_ventilation_reference_check_matrix.md` |
| Commissioning plan | Completed as proposed test workflow | `docs/nz_ventilation_commissioning_test_plan.md` |
| Digital dashboard | Completed with simulated sample data | `dashboard/ventilation_dashboard.py` and website dashboard |
| SolidWorks validation kit | Completed as concept test-rig model | `source/solidworks/Smart_Ventilation_Validation_Kit.SLDPRT` |
| Manufacturing package | Completed as preliminary build package | `docs/smart_validation_kit_manufacturing_package.md` |
| M-101 review sheet | Completed as portfolio review sheet | `drawings/nz_ventilation_m101_design_review_sheet.svg` |

## Evidence Gaps

| Gap | Why it matters | How to close it | Resume wording before closure |
| --- | --- | --- | --- |
| Physical CO2/RH sensor data | Proves real indoor air-quality response instead of only simulated data | Connect SCD40/SCD41 or equivalent sensor and log raw CSV | Developed sensor-validation workflow for future CO2/RH testing |
| Measured terminal airflow | Confirms supply/extract balance and diffuser assumptions | Use an anemometer or flow hood at terminals | Prepared commissioning airflow test plan |
| Measured duct pressure | Confirms pressure-loss assumptions and fan duty | Use pressure taps and differential pressure sensor/manometer | Designed duct pressure-tap validation layout |
| Fan power measurement | Replaces dashboard placeholder power values | Use plug-in power meter or inline current measurement | Added fan-energy sense check and power-measurement plan |
| Acoustic measurement | Checks whether boost mode is usable for occupants | Record dBA near terminals or use manufacturer acoustic data | Identified boost-mode acoustic risk and mitigation options |
| Professional compliance review | Needed before any real building claim | Review with qualified building-services professional | Treated NZBC/Healthy Homes as reference context only |

## Claims to Avoid

Do not claim:

- The design is code compliant.
- The design is ready for building consent.
- The dashboard uses measured sensor data.
- The fan/noise/energy results are final.
- The validation kit is a production-ready product.

## Resume-Safe Claims

Safe wording:

> Developed a smart NZ residential MVHR / ERV portfolio case study covering preliminary HVAC sizing, equipment selection, SolidWorks validation-kit modelling, commissioning planning, NZ reference mapping, manufacturing packaging and a dashboard using simulated commissioning data.

Stronger wording after hardware testing:

> Collected and analysed CO2, humidity, duct pressure and fan-power data to validate ventilation control response and commissioning assumptions.

## Practical Next Step

The highest-value next upgrade is one measured CSV log from a real CO2/RH sensor. Until then, the project should keep the current simulated-data disclaimer visible.
