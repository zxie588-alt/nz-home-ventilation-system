# Smart Validation Kit Manufacturing Package

This package turns the SolidWorks validation kit into a practical build discussion. It is not a final fabrication release, but it shows how the concept could be purchased, printed, cut, assembled and tested.

## Build Strategy

| Subassembly | Build method | Notes |
| --- | --- | --- |
| Base plate | Laser-cut plywood, acrylic, aluminium sheet or offcut panel | 900 mm x 420 mm concept base |
| Transparent duct | Purchased 150 mm clear duct or acrylic tube | Used for visualising sensor and tap locations |
| Sensor pod | 3D printed enclosure | Holds CO2/RH sensor and cable gland |
| Electronics tray | Laser-cut acrylic/aluminium or 3D printed tray | Holds ESP32, pressure sensor, display and wiring |
| Pressure taps | Purchased barb fittings or printed tap bosses | Connect upstream/downstream pressure points |
| Tubing | Purchased silicone tube | Connects taps to pressure sensor |
| Fan module | Purchased inline fan or printed placeholder enclosure | Needs guard, wiring and safe power supply |
| Flow straightener | 3D printed honeycomb or purchased mesh | Reduces swirl before measurement point |
| Outlet grille | Purchased grille or printed placeholder | Represents terminal resistance and outlet condition |

## Indicative BOM and Cost

| Item | Qty | Build / buy | Indicative NZD | Comment |
| --- | ---: | --- | ---: | --- |
| ESP32 DevKit | 1 | Buy | 12 | Data logger / controller |
| SCD40/SCD41 CO2 sensor module | 1 | Buy | 45 | CO2, temperature and RH |
| Differential pressure sensor or basic manometer | 1 | Buy | 35 | Duct pressure trend |
| OLED display | 1 | Buy | 8 | Local status display |
| Push button and LEDs | 1 set | Buy | 5 | Manual boost and status indicators |
| 150 mm duct section | 1 | Buy | 25 | Test duct |
| Inline fan or spare PC blower concept | 1 | Buy | 30 | Airflow source for bench test |
| Silicone tubing and barb fittings | 1 set | Buy | 12 | Pressure tap connection |
| 3D printed sensor pod | 1 | Make | 8 | PLA/PETG concept cost |
| Base plate and brackets | 1 set | Make | 25 | Plywood/acrylic/aluminium |
| Fasteners and cable glands | 1 set | Buy | 15 | Assembly hardware |
| Estimated total | - | Mixed | 220 | Rough planning value only |

## Assembly Sequence

1. Cut base plate and mark duct centreline.
2. Mount U-brackets and secure 150 mm duct section.
3. Install fan module and outlet grille.
4. Install pressure taps upstream and downstream of the measurement section.
5. Mount sensor pod near duct access hatch and route probe into duct.
6. Mount electronics tray and wire ESP32, CO2/RH sensor, pressure sensor, display and LEDs.
7. Connect silicone tubes to differential pressure sensor.
8. Load logger firmware and confirm serial CSV output.
9. Run normal/boost fan states and check dashboard import.

## Safety and Practical Risks

- Use a fan guard and avoid exposed moving blades.
- Keep low-voltage electronics isolated from mains power.
- Do not place sensors in a wet duct without enclosure and condensation planning.
- Label tubing so upstream/downstream pressure readings are not reversed.
- Record calibration assumptions before using sensor data in the dashboard.

## Manufacturing Evidence Value

This package is useful for internship applications because it shows:

- CAD-to-build thinking.
- Practical material and purchasing decisions.
- Assembly sequence planning.
- Test hardware design.
- Safety and validation awareness.

## Resume-Safe Wording

> Developed a SolidWorks validation kit and preliminary manufacturing package for a ventilation test rig, including sensor pod packaging, pressure-tap layout, electronics tray, BOM, assembly sequence and safety considerations.
