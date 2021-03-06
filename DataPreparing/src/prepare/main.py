"""Prepare Data

Usage:
    main.py <input> <output> <cpu_cache>
"""
import sys

import pandas as pd
from docopt import docopt

from src.prepare.expand import expand_df_with_similar_processors_from_cpubenchmark, \
    expand_df_with_ssd_for_gamers_programmers, expand_prices_by_fieldinterest_mainuse, expand_ddrsocket_by_computertype
from src.prepare.transformers import extract_ddr_from_gpu_processor, \
    convert_disk_capacity_to_byte, \
    convert_memory_capacity_to_byte, \
    convert_processor_ghz_to_mhz, \
    remove_unwanted_chars_in_processor_name, \
    extract_processor_features, \
    remove_unwanted_chars_in_gpu_name, \
    extract_gpu_features, \
    fix_disk_type, \
    rename_processor_name_to_match_cpubenchmark, minus_rpm_for_ssd, drop_rows_with_nan_by_columns, \
    split_ddr_column_to_type_number


def parse_arguments() -> dict:
    if len(sys.argv) == 1:
        sys.argv.append('data/fake-data-orig.csv')
        sys.argv.append('data/fake-data-out.csv')
        sys.argv.append('data/cpu_cache.csv')
    return docopt(__doc__, version="Prepare Data 0.1")


def read_data(source_path: str, **kwargs) -> pd.DataFrame:
    return pd.read_csv(source_path, **kwargs)


def save_data(df_to_save: pd.DataFrame, output_path: str):
    df_to_save.to_csv(output_path, index=False)
    df_to_save.dtypes.to_csv(output_path.replace('.csv', '.dtypes.csv'), index=True, header=False)


def transpose_data(df: pd.DataFrame, cpu_cache: str) -> pd.DataFrame:
    df.columns = [n.lower() for n in df.columns]

    df = df.drop(['motherboard_name', 'motherboard_sataconnections', 'processor_architecture'], axis=1) \
        .reset_index(drop=True)
    df = drop_rows_with_nan_by_columns(df, 'processor_name')

    # Memory
    df = extract_ddr_from_gpu_processor(df)
    df = convert_memory_capacity_to_byte(df)
    df = split_ddr_column_to_type_number(df, 'memory_type')

    # Disk
    df = convert_disk_capacity_to_byte(df)
    df = fix_disk_type(df)
    df = minus_rpm_for_ssd(df)
    df = expand_df_with_ssd_for_gamers_programmers(df)

    # Processor
    df = convert_processor_ghz_to_mhz(df)
    df = remove_unwanted_chars_in_processor_name(df)
    df = rename_processor_name_to_match_cpubenchmark(df)
    df = expand_df_with_similar_processors_from_cpubenchmark(df, cpu_cache)
    df = extract_processor_features(df)

    # GPU
    df = remove_unwanted_chars_in_gpu_name(df)
    df = extract_gpu_features(df)
    df = split_ddr_column_to_type_number(df, 'gpu_processor_ddr')

    # Mother Board
    df = expand_ddrsocket_by_computertype(df)

    # Price
    df = expand_prices_by_fieldinterest_mainuse(df)

    df.drop_duplicates(inplace=True)
    df = df.reindex(sorted(df.columns), axis=1)

    # Fix dtypes
    df['gpu_processor_ddr_number'] = pd.to_numeric(df['gpu_processor_ddr_number']).astype(float)
    df['gpu_version'] = df.gpu_version.astype(float)
    df['memory_type_number'] = pd.to_numeric(df['memory_type_number']).astype(float)

    df.reset_index(inplace=True, drop=True)

    return df


if __name__ == '__main__':
    arguments = parse_arguments()
    input_df = read_data(arguments['<input>'])

    output = transpose_data(input_df, arguments['<cpu_cache>'])

    save_data(output, arguments['<output>'])
