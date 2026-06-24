from pathlib import Path

import pandas as pd
import streamlit as st


DATA_PATH = Path(__file__).resolve().parents[1] / "data" / "nz_ventilation_commissioning_sample.csv"
DWELLING_VOLUME_M3 = 96 * 2.4
AIR_DENSITY_KG_M3 = 1.2
CP_AIR_KJ_KG_K = 1.006


@st.cache_data
def load_data() -> pd.DataFrame:
    data = pd.read_csv(DATA_PATH, parse_dates=["timestamp_nz"])
    return data.sort_values("timestamp_nz")


def recovered_power_w(flow_lps: float, indoor_temp_c: float, outdoor_temp_c: float, effectiveness: float) -> float:
    flow_m3_s = flow_lps / 1000
    delta_t = abs(indoor_temp_c - outdoor_temp_c)
    return AIR_DENSITY_KG_M3 * CP_AIR_KJ_K_K * 1000 * flow_m3_s * delta_t * effectiveness


st.set_page_config(page_title="Smart NZ Ventilation Digital Twin", layout="wide")
st.title("Smart NZ Ventilation Digital Twin")
st.caption("Portfolio prototype: sensor validation, commissioning log review and demand-control logic.")

df = load_data()
latest = df.iloc[-1]

co2_boost_threshold = st.sidebar.slider("CO2 boost threshold (ppm)", 800, 1400, 1000, 25)
co2_return_threshold = st.sidebar.slider("CO2 return threshold (ppm)", 650, 1000, 850, 25)
rh_boost_threshold = st.sidebar.slider("RH boost threshold (%)", 55, 80, 65, 1)
rh_return_threshold = st.sidebar.slider("RH return threshold (%)", 45, 70, 60, 1)
heat_recovery_effectiveness = st.sidebar.slider("Heat-recovery effectiveness", 0.50, 0.90, 0.77, 0.01)

recommended_mode = "normal"
decision_reason = "CO2 and RH are below boost thresholds."
if latest["co2_ppm"] >= co2_boost_threshold or latest["rh_percent"] >= rh_boost_threshold:
    recommended_mode = "boost"
    decision_reason = "CO2 or RH is above the boost threshold."
elif latest["co2_ppm"] > co2_return_threshold or latest["rh_percent"] > rh_return_threshold:
    recommended_mode = "recovery"
    decision_reason = "Values are recovering; keep monitoring before returning fully to normal."

ach = latest["supply_flow_Lps"] * 3.6 / DWELLING_VOLUME_M3
recovered_w = recovered_power_w(
    latest["supply_flow_Lps"],
    latest["indoor_temp_C"],
    latest["outdoor_temp_C"],
    heat_recovery_effectiveness,
)

col1, col2, col3, col4 = st.columns(4)
col1.metric("Recommended mode", recommended_mode.upper())
col2.metric("Latest CO2", f"{latest['co2_ppm']:.0f} ppm")
col3.metric("Latest RH", f"{latest['rh_percent']:.0f}%")
col4.metric("Estimated ACH", f"{ach:.2f}")

st.subheader("Commissioning Sample Log")
chart_data = df.set_index("timestamp_nz")[["co2_ppm", "rh_percent", "supply_flow_Lps"]]
st.line_chart(chart_data)

col_a, col_b = st.columns(2)
with col_a:
    st.subheader("Control Decision")
    st.write(f"Current sample event: **{latest['event']}**")
    st.write(f"Dashboard decision: **{recommended_mode}**")
    st.write(decision_reason)
    st.write("Normal target: 60 L/s supply and 60 L/s extract.")
    st.write("Boost target: 85 L/s supply/extract equivalent.")

with col_b:
    st.subheader("Heat-Recovery Sense Check")
    st.write(f"Assumed effectiveness: **{heat_recovery_effectiveness:.0%}**")
    st.write(f"Latest indoor/outdoor temperature difference: **{abs(latest['indoor_temp_C'] - latest['outdoor_temp_C']):.1f} K**")
    st.write(f"Estimated recovered thermal power at latest flow: **{recovered_w:.0f} W**")
    st.write("This is a first-pass estimate for engineering communication, not an energy-compliance model.")

st.subheader("Raw Data")
st.dataframe(df, use_container_width=True)
