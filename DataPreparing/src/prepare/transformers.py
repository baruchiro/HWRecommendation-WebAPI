import pandas as pd
import re


def __capacity_to_byte_value(capacity: pd.Series) -> int:
    if capacity[0] is not pd.np.nan:
        convection_relation = {
            'tb': 1024 * 1024 * 1024,
            'gb': 1024 * 1024,
            'm': 1024,
            'mb': 1024,
            'kb': 1
        }
        capacity = capacity[0].lower()
        groups = re.search('(\\d+)((?:M|GB|TB))', capacity, re.IGNORECASE)
        count = groups.group(1)
        unit = groups.group(2)
        return int(count) * convection_relation[unit]


def extract_ddr_from_gpu_processor(df: pd.DataFrame) -> pd.DataFrame:
    df["Gpu_Processor_ddr"] = df["Gpu_Processor"].str.extract('(G?DDR\\dX?)', re.IGNORECASE)

    capacity: pd.DataFrame = df["Gpu_Processor"].str.extract('(\\d+(?:M|GB))', re.IGNORECASE)
    capacity_in_kb = capacity.apply(__capacity_to_byte_value, axis=1)
    df["Gpu_Processor_ddr_capacity"] = capacity_in_kb
    del df["Gpu_Processor"]
    return df
