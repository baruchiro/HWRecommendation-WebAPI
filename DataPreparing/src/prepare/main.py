"""Prepare Data

Usage:
    main.py <input> <output>
"""
import sys
from docopt import docopt

import pandas as pd

from src.prepare.transformers import extract_ddr_from_gpu_processor, \
    convert_disk_capacity_to_byte, \
    convert_memory_capacity_to_byte, \
    convert_processor_ghz_to_mhz, \
    remove_unwanted_chars_in_processor_name, \
    extract_processor_features, \
    remove_unwanted_chars_in_gpu_name, \
    extract_gpu_features, \
    fix_disk_type


def parse_arguments() -> dict:
    if len(sys.argv) == 1:
        sys.argv.append('data/fake-data-orig.csv')
        sys.argv.append('data/fake-data-out.csv')
    return docopt(__doc__, version="Prepare Data 0.1")


def read_data(source_path: str) -> pd.DataFrame:
    return pd.read_csv(source_path)


def save_data(df_to_save: pd.DataFrame, output_path: str):
    df_to_save.to_csv(output_path, index=False)


if __name__ == '__main__':
    arguments = parse_arguments()
    df = read_data(arguments['<input>'])

    df.columns = [n.lower() for n in df.columns]

    df = extract_ddr_from_gpu_processor(df)
    df = convert_disk_capacity_to_byte(df)
    df = convert_memory_capacity_to_byte(df)
    df = convert_processor_ghz_to_mhz(df)
    df = remove_unwanted_chars_in_processor_name(df)
    df = extract_processor_features(df)
    df = remove_unwanted_chars_in_gpu_name(df)
    df = extract_gpu_features(df)
    df = fix_disk_type(df)

    df = df.reindex(sorted(df.columns), axis=1)

    save_data(df, arguments['<output>'])
