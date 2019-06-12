import pandas as pd
import re


def __series_apply_capacity_to_byte_value(capacity: str) -> int:
    if capacity is not pd.np.nan:
        convection_relation = {
            'tb': 1024 * 1024 * 1024,
            'gb': 1024 * 1024,
            'm': 1024,
            'mb': 1024,
            'kb': 1
        }
        capacity = capacity.lower()
        groups = re.search('(\\d+)((?:M|GB|TB))', capacity, re.IGNORECASE)
        count = groups.group(1)
        unit = groups.group(2)
        return int(count) * convection_relation[unit]


def __dataframe_apply_capacity_to_byte_value(capacity: pd.Series) -> int:
    return __series_apply_capacity_to_byte_value(capacity[0])


def extract_ddr_from_gpu_processor(df: pd.DataFrame) -> pd.DataFrame:
    df["gpu_processor_ddr"] = df["gpu_processor"].str.extract('(G?DDR\\dX?)', re.IGNORECASE)

    capacity: pd.DataFrame = df["gpu_processor"].str.extract('(\\d+(?:M|GB))', re.IGNORECASE)
    df["gpu_processor_ddr_capacity"] = capacity.apply(__dataframe_apply_capacity_to_byte_value, axis=1)
    del df["gpu_processor"]
    return df


def convert_disk_capacity_to_byte(df: pd.DataFrame) -> pd.DataFrame:
    df['disk_capacity_as_kb'] = df["disk_capacity"].apply(__series_apply_capacity_to_byte_value)
    del df["disk_capacity"]
    return df

def convert_memory_capacity_to_byte(df: pd.DataFrame) -> pd.DataFrame:
    df['memory_capacity_as_kb'] = df["memory_capacity"].apply(__series_apply_capacity_to_byte_value)
    del df["memory_capacity"]
    return df

def convert_processor_ghz_to_mhz(df: pd.DataFrame) -> pd.DataFrame:
    df['processor_mhz'] = df['processor_ghz'] * 1000
    del df['processor_ghz']
    return df
