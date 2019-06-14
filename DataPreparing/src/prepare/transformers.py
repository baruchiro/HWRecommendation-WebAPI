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
        groups = re.search(r'(\d+)((?:M|GB|TB))', capacity, re.IGNORECASE)
        count = groups.group(1)
        unit = groups.group(2)
        return int(count) * convection_relation[unit]


def __dataframe_apply_capacity_to_byte_value(capacity: pd.Series) -> int:
    return __series_apply_capacity_to_byte_value(capacity[0])


def extract_ddr_from_gpu_processor(df: pd.DataFrame) -> pd.DataFrame:
    df["gpu_processor_ddr"] = df["gpu_processor"].str.extract(r'(G?DDR\dX?)', re.IGNORECASE)

    capacity: pd.DataFrame = df["gpu_processor"].str.extract(r'(\d+(?:M|GB))', re.IGNORECASE)
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


def __remove_unwanted_chars_in_name(df: pd.DataFrame, column:str) -> pd.DataFrame:
    df[column] = df[column].str.replace('®', '')
    df[column] = df[column].str.replace('™', '')
    df[column] = df[column].str.replace(' Processor', '')
    df[column] = df[column].str.replace(' processor', '')
    return df


def remove_unwanted_chars_in_processor_name(df: pd.DataFrame) -> pd.DataFrame:
    return __remove_unwanted_chars_in_name(df, 'processor_name')


def remove_unwanted_chars_in_gpu_name(df: pd.DataFrame) -> pd.DataFrame:
    return __remove_unwanted_chars_in_name(df, 'gpu_name')


def extract_processor_features(df: pd.DataFrame) -> pd.DataFrame:
    df["processor_manufacturer"] = df["processor_name"].str.extract('((?:intel|amd))', re.IGNORECASE)
    df['processor_name'] = df['processor_name'].str.replace('((?:intel|amd)) *', '', flags=re.IGNORECASE)
    df["processor_model"] = df["processor_name"].str.extract(r'((?:[a-z ]+)\d[- ]?\d{2,4}[a-z]{0,2})', re.IGNORECASE)
    del df["processor_name"]
    return df


def extract_gpu_features(df: pd.DataFrame) -> pd.DataFrame:
    df["gpu_manufacturer"] = df["gpu_name"].str.extract('((?:intel|nvidia|amd|asus))', re.IGNORECASE)
    df["gpu_model"] = df["gpu_name"].str.extract(
            '((?:(?:geforce )*(?:gt|m|rt)x|U?HD Graphics|radeon(?: vega)*|gt|geforce|ROG STRIX-GTX){1})',
            re.IGNORECASE)
    df["gpu_version"] = df["gpu_name"].str.extract(r'(\d{2,})', re.IGNORECASE)
    del df["gpu_name"]
    return df

def fix_disk_type(df: pd.DataFrame) -> pd.DataFrame:
    df.loc[df.disk_type == 'sdd', 'disk_type'] = 'ssd'
    return df
