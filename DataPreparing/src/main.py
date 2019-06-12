"""Prepare Data

Usage:
    main.py <input> <output>
"""
import sys
from docopt import docopt

import pandas as pd

from transformers import *


def parse_arguments() -> dict:
    if len(sys.argv) == 1:
        sys.argv.append('../data/fake-data-orig.csv')
        sys.argv.append('../data/fake-data-out.csv')
    return docopt(__doc__, version="Prepare Data 0.1")


def read_data(source_path: str) -> pd.DataFrame:
    return pd.read_csv(source_path)


if __name__ == '__main__':
    arguments = parse_arguments()
    df = read_data(arguments['<input>'])

    df = extract_ddr_from_gpu_processor(df)
