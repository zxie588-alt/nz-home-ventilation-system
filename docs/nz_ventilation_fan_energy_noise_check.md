# Fan Curve, Energy and Noise Sense Check

This note strengthens the preliminary MVHR / ERV equipment selection by linking airflow targets, external static pressure, estimated fan power and acoustic risk. It is a first-pass portfolio check, not a final manufacturer selection or acoustic design.

## Design Duties

| Mode | Airflow target | External static pressure target | Use case |
| --- | ---: | ---: | --- |
| Normal balanced | 60 L/s supply and 60 L/s extract | about 80 Pa | Continuous or regular low-noise ventilation |
| Boost / clearing | 85 L/s extract-equivalent target | about 120 Pa | Cooking, shower or high-CO2 event |

## Concept Unit Check

Preferred concept unit: Mitsubishi Electric Lossnay LGH-35RVX3-E.

Relevant published points used in the project:

- 75% default point: 73 L/s at 90 Pa.
- 100% default point: 97 L/s at 160 Pa.

Reasoning:

- Normal duty of 60 L/s at about 80 Pa sits below the 75% default point, leaving commissioning margin.
- Boost duty of 85 L/s at about 120 Pa sits below the 100% default point, leaving headroom.
- LGH-25RVX3-E is close to normal duty but does not have enough boost headroom.
- LGH-50RVX3-E is capable but likely oversized for the concept dwelling unless real pressure losses increase.

## Energy Sense Check

The dashboard estimates recovered heating/cooling load using:

`Q = rho x cp x airflow x deltaT x effectiveness`

Assumptions:

- Air density: 1.2 kg/m3.
- Specific heat of air: 1.006 kJ/kg.K.
- Heat-recovery effectiveness placeholder: 77%.
- Normal airflow: 60 L/s.
- Boost airflow: 85 L/s.
- Example indoor/outdoor deltaT: 10 K.

Indicative recovered thermal power:

| Mode | Airflow | DeltaT | Effectiveness | Recovered load estimate |
| --- | ---: | ---: | ---: | ---: |
| Normal | 60 L/s | 10 K | 77% | about 558 W |
| Boost | 85 L/s | 10 K | 77% | about 790 W |

This is not an annual energy model. It is a communication check showing why heat recovery matters in a ventilation system.

## Fan Power Sense Check

The sample commissioning log uses placeholder fan powers:

| Mode | Fan power placeholder | Comment |
| --- | ---: | --- |
| Normal | 42 W | Represents low-speed continuous operation for dashboard logic |
| Boost | 120 W | Represents higher clearing mode and acoustic/energy trade-off |

These values should be replaced by measured data or final manufacturer power values before making any energy-performance claim.

## Acoustic Risk Note

Noise risk increases in boost mode because:

- Kitchen boost branch velocity is higher than normal operation.
- Grille/diffuser pressure drops are not yet manufacturer-specific.
- Flexible duct, elbows, dampers and filters may increase fan speed requirement.
- Occupants may disable a noisy ventilation system, which is a real usability risk.

Preliminary mitigation options:

- Keep normal operation at lower velocity and fan duty.
- Use boost as a timed clearing mode, not the default state.
- Increase branch/grille size if measured noise or pressure loss is high.
- Add acoustic lining or silencers only after pressure and maintenance impacts are checked.

## Interview Explanation

> I did not stop at "the unit can move enough air". I checked normal and boost duty points against the selected product family, added an energy-recovery sense check, and identified boost-mode noise as a design risk to be verified during commissioning.
