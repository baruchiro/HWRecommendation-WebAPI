import pandas as pd
import numpy as np


def get_correlation_matrix(df: pd.DataFrame) -> pd.DataFrame:
    df = df.copy()
    types = dict(df.dtypes)
    string_columns = [c for c in types if not np.issubdtype(types[c], np.number)]
    for column in string_columns:
        df[column] = pd.factorize(df[column])[0]

    return df.corr()


def correlation_matrix_to_sorted_pairs(corr: pd.DataFrame):
    df = corr.abs().stack().reset_index()
    df = df.loc[(df['level_0'] != 'level_0') & (df['level_1'] != 'level_0')]
    df = df.loc[df['level_0'] != df['level_1']]
    return df.sort_values([0]).iloc[::2].reset_index(drop=True)
