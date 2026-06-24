# MVHR / ERV Equipment Selection Note

This note adds a manufacturer-based equipment check to the preliminary NZ home MVHR / ERV ventilation project. It is a concept selection only, not a final procurement schedule.

## Design Duty From Project Calculation

- Normal balanced operation: 60 L/s supply and 60 L/s extract.
- Normal fan-duty target after design allowance: about 80 Pa external static pressure.
- Boost / extractor check: 85 L/s extract target.
- Boost fan-duty target after design allowance: about 120 Pa external static pressure.

## Candidate Product Family

Mitsubishi Electric New Zealand lists the Lossnay LGH-RVX3-E Fresh Air Energy Recovery Ventilation range for commercial and residential buildings. The product range covers model sizes from LGH-15RVX3-E to LGH-200RVX3-E, with supply and exhaust fan speeds adjustable in 5% increments.

Relevant model data from Mitsubishi Electric NZ:

| Model | Airflow range | 75% default point | 100% default point | Duct size |
| --- | ---: | ---: | ---: | ---: |
| LGH-25RVX3-E | 17-69 L/s | 52 L/s at 68 Pa | 69 L/s at 120 Pa | 150 mm |
| LGH-35RVX3-E | 24-97 L/s | 73 L/s at 90 Pa | 97 L/s at 160 Pa | 150 mm |
| LGH-50RVX3-E | 35-139 L/s | 104 L/s at 85 Pa | 139 L/s at 150 Pa | 200 mm |

## Selection Logic

LGH-25RVX3-E is close to the normal design duty but does not provide enough headroom for the 85 L/s boost extract target.

LGH-35RVX3-E is the preferred concept selection because:

- It covers the 60 L/s normal balanced case with reasonable allowance.
- Its 75% default point is 73 L/s at 90 Pa, close to the normal design target.
- Its 100% default point is 97 L/s at 160 Pa, above the boost target of 85 L/s at about 120 Pa.
- It uses 150 mm duct connections, which is compatible with the current concept once the 180 mm main duct is treated as a low-velocity distribution trunk with transitions at the unit.

LGH-50RVX3-E is technically capable but oversized for the current concept dwelling and would need stronger justification on noise, cost and commissioning.

## Concept Selection

Preferred concept unit: Mitsubishi Electric Lossnay LGH-35RVX3-E.

Design note: final selection still requires a real duct take-off, manufacturer fan curve check, filter pressure drop, grille/diffuser pressure drop, acoustic criteria, controls and installer review.

## Sources

- Mitsubishi Electric NZ, Lossnay LGH-RVX3 Energy Recovery Ventilation publication: https://www.mitsubishi-electric.co.nz/publications/ventilation/Lossnay-LGH-RVX3-Energy-Recovery-Ventilation/
- Mitsubishi Electric NZ, Fresh Air Home Ventilation and Heat Recovery product page: https://www.mitsubishi-electric.co.nz/ventilation/c/8364/fresh-air-home-ventilation-and-heat-recovery
