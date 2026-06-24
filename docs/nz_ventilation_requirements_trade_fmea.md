# Smart NZ Ventilation System - Requirements, Trade Study and FMEA

This document shows the systems-engineering layer behind the ventilation concept. It is designed to make the project read like a complete engineering workflow, not a standalone CAD model.

## Stakeholders

| Stakeholder | Need | Design response |
| --- | --- | --- |
| Occupants | Fresh air, moisture control, low noise and comfort | Balanced normal ventilation, boost logic and future acoustic check |
| Homeowner / landlord | Practical, maintainable and justifiable system | Equipment selection note, commissioning checklist and service access assumptions |
| Installer | Clear duct routes, terminal labels and commissioning targets | DXF plan, airflow schedule and test plan |
| Building-services reviewer | Traceable assumptions and limitations | NZ local context note, pressure estimate and limitation list |
| Product or hardware engineer | Testability, sensors, repeatability and data | ESP32 logger concept, dashboard and sensor BOM |
| Energy-conscious user | Ventilation without unnecessary heating/cooling penalty | MVHR concept and heat-recovery estimate in dashboard |

## System Requirements

| ID | Requirement | Rationale | Verification method |
| --- | --- | --- | --- |
| R-01 | Provide balanced normal ventilation at 60 L/s supply and 60 L/s extract. | Keep MVHR concept balanced and easy to commission. | Airflow schedule plus commissioning test T-01 |
| R-02 | Provide a boost target of 85 L/s for moisture or high-CO2 events. | Cover kitchen/bathroom point-check logic and demand response. | Dashboard state change plus test T-02/T-03/T-04 |
| R-03 | Keep main duct velocity below about 3 m/s in normal operation. | Reduce pressure loss and noise risk at preliminary stage. | Duct velocity schedule |
| R-04 | Select a concept MVHR unit with headroom above normal and boost targets. | Avoid undersizing the system. | Equipment selection note |
| R-05 | Record CO2, RH, temperature, airflow target, fan power and mode. | Make the system measurable and explainable. | CSV commissioning log |
| R-06 | Provide a dashboard that explains control decisions. | Convert raw sensor data into engineering judgement. | Digital twin dashboard |
| R-07 | Keep drawing outputs reviewable in common tools. | Support AutoCAD/SolidWorks review. | DXF, STEP and STL exports |
| R-08 | State limitations clearly. | Avoid overstating compliance or professional design authority. | Design boundary section and documentation review |

## Equipment Trade Study

| Option | Strength | Weakness | Decision |
| --- | --- | --- | --- |
| LGH-25RVX3-E | Compact and close to normal duty. | Not enough headroom for 85 L/s boost target. | Rejected for this concept. |
| LGH-35RVX3-E | Covers 60 L/s normal and 85 L/s boost with headroom. Uses 150 mm duct connection. | Needs transition logic from 180 mm concept trunks to 150 mm unit ports. | Preferred concept selection. |
| LGH-50RVX3-E | More airflow headroom. | Oversized for the concept dwelling; may increase cost, noise and commissioning risk. | Backup only if real duct losses increase. |

## Control-State Logic

| State | Entry trigger | Exit trigger | Action |
| --- | --- | --- | --- |
| Normal | Default state and all inputs below thresholds. | CO2 or RH exceeds threshold. | 60 L/s supply and 60 L/s extract. |
| Boost | CO2 above 1000 ppm, RH above 65%, or kitchen/bathroom event. | CO2 below 850 ppm and RH below 60% for a timed period. | 85 L/s target and higher fan duty. |
| Purge | Severe CO2 or humidity event, or post-shower timer. | Timed clearing complete or values recovered. | Keep boost target and flag user/commissioning note. |
| Fault safe | Sensor missing, out of range or stale data. | Sensor returns valid data. | Hold normal mode or boost if moisture source is known. |

## FMEA

| Failure mode | Effect | Cause | Detection | Mitigation | Severity | Occurrence | Detection score | RPN |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: |
| Outdoor intake too close to exhaust | Short-circuiting stale air back into supply | Poor terminal placement | Drawing review and CO2 trend | Maintain separation and label terminals | 7 | 3 | 4 | 84 |
| Filter clogging | Reduced airflow and higher fan power | Maintenance missed | Pressure trend or airflow check | Add filter inspection interval | 6 | 5 | 5 | 150 |
| Kitchen boost noise | Occupant disables system | High branch velocity or grille pressure drop | User feedback and acoustic check | Larger branch/grille or lower boost ramp | 5 | 4 | 5 | 100 |
| Sensor drift | Wrong control decisions | Ageing or poor calibration | Calibration check against reference | Record calibration date and offset | 5 | 4 | 6 | 120 |
| Condensation in duct | Moisture damage or hygiene issue | Cold duct route or poor insulation | Inspection and RH trend | Insulate cold-side duct and provide fall/drain strategy | 7 | 3 | 6 | 126 |
| Unbalanced supply/extract | Pressure imbalance and poor heat recovery | Damper setting or duct resistance mismatch | Commissioning airflow check | Balance terminals and update schedule | 6 | 4 | 4 | 96 |
| Controller stuck in normal | Poor IAQ during cooking/shower/occupancy peak | Firmware or sensor failure | Dashboard alarm and data log | Fault-safe boost timer and manual override | 7 | 2 | 5 | 70 |

## Design Review Questions

- Does the system still read as balanced MVHR rather than extract-only ventilation?
- Does the selected unit have enough external static pressure margin after real grilles, filters and silencers are added?
- Are the Healthy Homes kitchen/bathroom checks treated as point checks rather than a blanket compliance claim?
- Can the installer understand the airflow schedule without reading the full project narrative?
- Does the dashboard show decisions in a way a non-specialist stakeholder can follow?
