"""Prepare Data

Usage:
    main.py <input> <output>
"""
import sys
from docopt import docopt

import pandas as pd

from transformers import extract_ddr_from_gpu_processor, convert_disk_capacity_to_byte


def parse_arguments() -> dict:
    if len(sys.argv) == 1:
        sys.argv.append('../../data/fake-data-orig.csv')
        sys.argv.append('../../data/fake-data-out.csv')
    return docopt(__doc__, version="Prepare Data 0.1")


def read_data(source_path: str) -> pd.DataFrame:
    return pd.read_csv(source_path)


if __name__ == '__main__':
    arguments = parse_arguments()
    df = read_data(arguments['<input>'])

    df = extract_ddr_from_gpu_processor(df)
    df = convert_disk_capacity_to_byte(df)

    df.columns = [n.lower() for n in df.columns]
    df = df.reindex(sorted(df.columns), axis=1)
