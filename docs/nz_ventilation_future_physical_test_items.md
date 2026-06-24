# Future Physical Test Items

This project currently includes simulated commissioning data and a SolidWorks validation kit. Physical sensor testing is the strongest future upgrade, but it is intentionally listed as future work until the required hardware is available.

## Why This Is Future Work

The portfolio already shows the design basis, CAD layout, test plan, dashboard and validation-kit model. Real measured data should only be added after sensors are connected, calibration assumptions are recorded and the raw CSV is saved without manual editing.

## Required Hardware

| Item | Purpose | Example |
| --- | --- | --- |
| CO2 / RH / temperature sensor | Indoor air-quality and moisture trend | Sensirion SCD40/SCD41 or equivalent |
| ESP32 or similar logger | Timestamped sensor logging | ESP32 DevKit |
| Differential pressure sensor or manometer | Duct/filter pressure trend | SDP810/SDP31 class sensor or handheld meter |
| Power meter | Fan power comparison | Plug-in power meter |
| Airflow meter or flow hood | Terminal balancing check | Anemometer or borrowed flow hood |

## Proposed Test Sequence

1. Baseline logging: record at least 30 minutes of indoor CO2, RH and temperature in normal conditions.
2. CO2 recovery event: record occupancy increase and recovery after ventilation or window opening.
3. Moisture event: record bathroom or kitchen RH spike and clearing trend.
4. Duct pressure check: compare upstream/downstream pressure in normal and boost modes.
5. Fan power check: record power draw in normal and boost.
6. Dashboard update: replace the simulated CSV with measured CSV and keep the original raw log.

## Acceptance Evidence

- Raw CSV log with timestamps and sensor values.
- Calibration note or sensor assumption note.
- Dashboard screenshot using measured data.
- Short result summary: what changed, what failed, what would be redesigned.

## Resume-Safe Wording

Until hardware data is collected, the safe wording is:

> Developed a sensor-validation workflow and SolidWorks validation-kit concept for future CO2, humidity, pressure and fan-power testing.

After physical testing is complete, the stronger wording becomes:

> Collected and analysed CO2, humidity, pressure and fan-power data to validate ventilation control response and commissioning assumptions.
