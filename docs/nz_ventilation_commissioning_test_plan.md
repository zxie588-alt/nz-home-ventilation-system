# Smart NZ Ventilation System - Commissioning and Sensor Validation Test Plan

This document upgrades the MVHR / ERV case study from a CAD and sizing exercise into a testable engineering workflow. It is written for portfolio evidence only. It is not a building consent document, producer statement or professional commissioning certificate.

## Test Objective

Validate that the concept system can be explained as a controllable, measurable residential ventilation system:

- Normal balanced operation at 60 L/s supply and 60 L/s extract.
- Boost operation at 85 L/s supply/extract equivalent for moisture or CO2 events.
- Data capture for CO2, relative humidity, indoor temperature, outdoor temperature, fan mode and indicative airflow.
- Control logic that moves between normal, boost and purge states based on measurable inputs.
- A commissioning record that can be reviewed by a building-services, product-development or manufacturing engineering interviewer.

## Design Basis

- Concept dwelling: 96 m2 plan area and about 230 m3 internal volume.
- Normal mode: 216 m3/h, about 0.94 ACH.
- Boost mode: 306 m3/h, about 1.33 ACH.
- Concept MVHR unit: Mitsubishi Electric Lossnay LGH-35RVX3-E.
- Local context references: NZBC G4 ventilation, NZBC H1 energy efficiency and Healthy Homes ventilation standard point checks.

## Instrumentation Plan

| Item | Measurement | Suggested tool | Purpose |
| --- | --- | --- | --- |
| CO2 sensor | CO2 ppm | SCD41/SCD40 or equivalent NDIR sensor | Occupancy and indoor air-quality response |
| Temperature/RH sensor | degC and %RH | SHT31, SCD41 internal RH, or equivalent | Moisture and comfort response |
| Differential pressure sensor | Pa | SDP810/SDP31 class sensor or manometer | Duct/static pressure trend |
| Airflow check | L/s at terminal | Hood/anemometer if available | Balancing verification |
| Power meter | W | Plug-in power meter or inline monitor | Fan-energy estimate |
| ESP32 logger | CSV data | Serial/Wi-Fi logging | Repeatable data trail |

## Pre-Start Checks

- Confirm outside-air and exhaust-air paths are separated in the drawing and labelled clearly.
- Confirm all supply and extract terminals have airflow targets.
- Confirm duct sizes in the airflow schedule match the latest plan.
- Confirm filters, access space and inspection notes are included in the equipment selection note.
- Record sensor serial numbers, calibration assumptions and date of test.
- Check that the dashboard uses the same normal and boost airflow values as the design note.

## Test Cases

| Test ID | Test | Procedure | Acceptance criterion |
| --- | --- | --- | --- |
| T-01 | Normal balanced airflow | Set system to normal mode and record supply/extract targets. | Supply and extract targets are both 60 L/s; balancing error target within +/-10% when real measurements are available. |
| T-02 | Kitchen boost response | Simulate cooking event or raise kitchen RH input above threshold. | System changes to boost mode and records 85 L/s target until RH returns below threshold. |
| T-03 | Bathroom boost response | Simulate shower event with RH above threshold. | Boost mode starts and remains active for a timed clearing period. |
| T-04 | CO2 demand response | Raise CO2 above 1000 ppm in dashboard or sensor stream. | System recommends boost or purge and logs decision reason. |
| T-05 | Energy recovery estimate | Compare outside and indoor temperature with heat-recovery efficiency assumption. | Dashboard reports recovered heating/cooling load as an estimate, not a compliance claim. |
| T-06 | Data export | Export CSV log from logger or dashboard sample data. | CSV includes timestamp, mode, CO2, RH, temperature, airflow and control action. |
| T-07 | Failure-state review | Simulate sensor missing/drift/out-of-range values. | Dashboard flags data quality issue and defaults to safe normal/boost logic. |

## Data Fields

Minimum commissioning log:

- `timestamp_nz`
- `mode`
- `event`
- `co2_ppm`
- `rh_percent`
- `indoor_temp_C`
- `outdoor_temp_C`
- `supply_flow_Lps`
- `extract_flow_Lps`
- `fan_power_W`
- `estimated_ach`
- `control_action`

## Acceptance Summary

This project is successful as a portfolio engineering upgrade when it can show:

- A clear design basis.
- A visible relationship between CAD, airflow schedule, equipment selection and controls.
- A repeatable test plan with pass/fail criteria.
- A sample or real data log.
- A dashboard that turns data into engineering decisions.
- A limitation list that separates portfolio evidence from professional compliance.

## Current Limitations

- Sample log is simulated until physical sensors are connected.
- Airflow balancing requires real test equipment.
- Acoustic performance is not yet measured.
- Manufacturer fan-curve overlay is still simplified.
- The commissioning plan is not a substitute for a qualified installer or professional review.
