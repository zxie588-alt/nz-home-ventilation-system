# Smart NZ Ventilation System - Digital Twin and Control Method

The dashboard is a lightweight digital-twin prototype for the portfolio project. It does not claim to simulate a full building. Its purpose is to connect design assumptions, sensor data and operating decisions in one reviewable workflow.

## Inputs

- CO2 concentration, ppm.
- Relative humidity, %RH.
- Indoor temperature, degC.
- Outdoor temperature, degC.
- Fan mode: normal or boost.
- Supply and extract airflow targets, L/s.
- Fan power, W.
- Heat-recovery effectiveness assumption.

## Derived Values

### Air Change Rate

The dwelling volume is estimated as:

`96 m2 x 2.4 m = 230 m3`

Normal mode:

`60 L/s = 216 m3/h`

`216 / 230 = 0.94 ACH`

Boost mode:

`85 L/s = 306 m3/h`

`306 / 230 = 1.33 ACH`

### Control Thresholds

The dashboard uses simple thresholds for demonstrator logic:

- CO2 above 1000 ppm: recommend boost.
- CO2 below 850 ppm after recovery: return to normal.
- Relative humidity above 65%: recommend boost.
- Relative humidity below 60% after clearing: return to normal.

These thresholds are portfolio controls assumptions, not statutory limits.

### Heat-Recovery Estimate

The dashboard estimates avoided heating/cooling load using:

`Q = rho x cp x airflow x deltaT x effectiveness`

Where:

- `rho = 1.2 kg/m3`
- `cp = 1.006 kJ/kg.K`
- `airflow` is the supply flow in m3/s
- `deltaT` is the indoor/outdoor temperature difference
- `effectiveness` is adjustable in the dashboard

The result is only an engineering sense check. A real design would use manufacturer performance tables, seasonal weather data, fan heat, defrost logic and building thermal modelling.

## Dashboard Pages / Blocks

- Summary cards: current mode, CO2, RH, airflow and estimated ACH.
- Time-series chart: CO2 and RH trend from the sample commissioning log.
- Control decision block: normal / boost / purge recommendation.
- Energy estimate block: heat-recovery load estimate.
- Design-basis block: links back to airflow schedule, pressure estimate, equipment selection and commissioning plan.

## Validation Path

1. Start with simulated sample data to prove the data structure.
2. Connect ESP32 logger to CO2 and RH sensors.
3. Export CSV from serial monitor or Wi-Fi endpoint.
4. Replace sample CSV with measured CSV.
5. Compare dashboard control decisions against expected test-plan outcomes.
6. Update thresholds and limitations based on measured behaviour.

## Why This Matters for Employers

For building-services consultancies, the digital twin shows a bridge from HVAC sizing to commissioning and stakeholder explanation.

For hardware/product companies, it shows practical data logging, validation logic, sensor-driven decisions and a willingness to test assumptions instead of only drawing models.
