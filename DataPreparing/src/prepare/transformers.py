import pandas as pd
import re


def extract_ddr_from_gpu_processor(df: pd.DataFrame) -> pd.DataFrame:
    df["Gpu_Processor_ddr"] = df["Gpu_Processor"].str.extract('G?DDR\\dX?', re.IGNORECASE)
    return df
